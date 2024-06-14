/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Exchange Credits/Tokens
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     30-Mar-2021    Abhishek           Created for POS UI Redesign 
 *2.130.9     28-Jun-2022    Abhishek           Modified:Added Technician Card Check.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI
{
    public class TaskExchangeCreditTokenVM : TaskBaseViewModel
    {
        #region Members
        private string moduleName;
        private TaskExchangeCreditTokenView taskExchangeCreditTokenView;
        private TaskType taskType;
        private ICommand loaded;
        private ICommand navigationClickCommand;
        private ICommand cardAddedCommand;
        private CardDetailsVM cardDetailsVM;
        private string credits;
        private string tokens;
        private string tokensHeading;
        private string creditsHeading;
        private string fromEntitlement;
        private string toEntitlement;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
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
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref moduleName, value);
                }
            }
        }

        public string FromEntitlement
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(fromEntitlement);
                return fromEntitlement;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref fromEntitlement, value);
                }
            }
        }

        public string ToEntitlement
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(toEntitlement);
                return toEntitlement;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref toEntitlement, value);
                }
            }
        }

        public string TokensHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tokensHeading);
                return tokensHeading;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref tokensHeading, value);
                }
            }
        }


        public string CreditsHeading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(creditsHeading);
                return creditsHeading;
            }

            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref creditsHeading, value);
                }
            }
        }

        public string Credits
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return credits;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref credits, value);
                }
                //if (credits != value)
                //{
                //    this.credits = value;
                //    this.OnTextChanged(credits);
                //}
            }
        }

        public string Tokens
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return tokens;
            }
            set
            {

                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref tokens, value);
                    this.OnTextChanged();
                }
            }
        }

        public ICommand NavigationClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return navigationClickCommand;
            }
            set
            {
                log.LogMethodEntry();
                navigationClickCommand = value;
                log.LogMethodExit();
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

        public CardDetailsVM CardDetailsVM
        {
            get { return cardDetailsVM; }
            set { SetProperty(ref cardDetailsVM, value); }
        }

        #endregion

        #region Methods
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            taskExchangeCreditTokenView = parameter as TaskExchangeCreditTokenView;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            if (!ManagerApprovalCheck(taskType,parameter))
            {
                ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 268);
                PerformClose(parameter);
            }
            log.LogMethodExit();
        }

        private void OnNavigationClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                PerformClose(parameter);
            }
            log.LogMethodExit();
        }

        private bool CheckCreditBalance()
        {
            bool result = true;
            log.LogMethodEntry();
            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.AccountId >= 0)
            {
                decimal? availableCredits = (CardDetailsVM.AccountDTO.Credits == null ? 0 : CardDetailsVM.AccountDTO.Credits);
                if (CardDetailsVM.AccountDTO.AccountSummaryDTO != null && CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusCardBalance != null)
                {
                    availableCredits += CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusCardBalance;
                }
                if (Convert.ToDecimal(Credits) > availableCredits)
                {
                    result = false;
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 49, Credits, availableCredits), MessageType.Error);
                }
            }
            log.LogMethodExit(result);
            return result;
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
                if (TappedAccountDTO.TechnicianCard == "Y")
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 197, TappedAccountDTO.TagNumber), MessageType.Warning);
                    return;
                }
                CardDetailsVM.AccountDTO = TappedAccountDTO;
            }
            if (CardDetailsVM.AccountDTO == null)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 257, null), MessageType.Warning);
                return;
            }
            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.AccountId >= 0)
            {
                SetFooterContent(string.Empty, MessageType.None);
            }
            else
            {
                if (taskType == TaskType.EXCHANGETOKENFORCREDIT)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 256, null), MessageType.Error);
                }
                else if (taskType == TaskType.EXCHANGECREDITFORTOKEN)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 258, null), MessageType.Error);
                }
            }
            log.LogMethodExit();
        }

        private async void OnOkButtonClicked(object parameter)
        {
            log.LogMethodEntry();
            if (CardDetailsVM.AccountDTO == null)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 257, null), MessageType.Warning);
                return;
            }
            if (CardDetailsVM.AccountDTO.AccountId < 0)
            {
                if (taskType == TaskType.EXCHANGETOKENFORCREDIT)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 256, null), MessageType.Error);
                }
                else if (taskType == TaskType.EXCHANGECREDITFORTOKEN)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 258, null), MessageType.Error);
                }
                return;
            }
            if (taskType == TaskType.EXCHANGETOKENFORCREDIT)
            {
                if (!ManagerApprovalLimitCheck(taskType, Convert.ToInt32(Tokens),parameter))
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268), MessageType.Error);
                    return;
                }
            }
            if (Convert.ToDecimal(Tokens) < 1)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 70), MessageType.Error);
                return;
            }
            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.AccountId > -1)
            {
                ITaskUseCases taskUseCase = TaskUseCaseFactory.GetTaskUseCases(ExecutionContext);
                ExchangeTokenDTO exchangeTokenDTO = null;
                if (taskType == TaskType.EXCHANGETOKENFORCREDIT)
                {
                    exchangeTokenDTO = new ExchangeTokenDTO(CardDetailsVM.AccountDTO.AccountId, ManagerId, ExchangeTokenDTO.FromTypeEnum.TOKEN,
                                                            ExchangeTokenDTO.FromTypeEnum.CREDITS, Convert.ToDecimal(Tokens), Remarks);
                }
                else
                {
                    if (!CheckCreditBalance())
                    {
                        return;
                    }
                    exchangeTokenDTO = new ExchangeTokenDTO(CardDetailsVM.AccountDTO.AccountId, ManagerId, ExchangeTokenDTO.FromTypeEnum.CREDITS,
                                                            ExchangeTokenDTO.FromTypeEnum.TOKEN, Convert.ToDecimal(Tokens), Remarks);
                }
                try
                {
                    IsLoadingVisible = true;
                    ExchangeTokenDTO exchangeTokensDTO = await taskUseCase.ExchangeTokens(exchangeTokenDTO);
                    IsLoadingVisible = false;
                    if (taskType == TaskType.EXCHANGECREDITFORTOKEN)
                    {
                        SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 40);
                    }
                    else
                    {
                        SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 39);
                    }
                    SetFooterContent(SuccessMessage, MessageType.None);
                    PerformClose(taskExchangeCreditTokenView);
                    PoleDisplay.executionContext = ExecutionContext;
                    PoleDisplay.writeSecondLine(Tokens.ToString() + " Tokens = " + Credits.ToString() + " Credits");
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

        private void OnClearCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            CardDetailsVM.AccountDTO = null;
            Remarks = string.Empty;
            Tokens = "0";
            Credits = "0";
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
            SetFooterContent(string.Empty, MessageType.None);
            log.LogMethodExit();
        }

        private void OnTextChanged()
        {
            log.LogMethodEntry();
            int tokens = Convert.ToInt32(Tokens);
            double result = ParafaitDefaultViewContainerList.GetParafaitDefault<double>(ExecutionContext, "TOKEN_PRICE");
            Credits = Convert.ToString(tokens * result);
            if (taskType == TaskType.EXCHANGECREDITFORTOKEN)
            {
                CheckCreditBalance();
            }
            log.LogMethodExit();
        }
        private void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            (NavigationClickCommand as DelegateCommand).RaiseCanExecuteChanged();
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
        #region Constructor
        public TaskExchangeCreditTokenVM(ExecutionContext executionContext, TaskType taskType, DeviceClass cardReader) : base(executionContext, cardReader)
        {
            log.Info("TaskExchangeCreditToken screen is opened");
            log.LogMethodEntry();
            this.ExecutionContext = executionContext;
            this.taskType = taskType;
            Loaded = new DelegateCommand(OnLoaded);
            CardAddedCommand = new DelegateCommand(OnCardAdded);
            base.CardTappedEvent += HandleCardRead;
            ClearCommand = new DelegateCommand(OnClearCommand, ButtonEnable);
            PropertyChanged += OnPropertyChanged;
            CardDetailsVM = new CardDetailsVM(ExecutionContext);
            CardDetailsVM.PropertyChanged += OnPropertyChanged;
            if (taskType == TaskType.EXCHANGETOKENFORCREDIT)
            {
                ModuleName = MessageViewContainerList.GetMessage(executionContext, "Exchange", null);//" Exchange "; /*+ Environment.NewLine + " Tokens  Credits ";*/
                FromEntitlement = MessageViewContainerList.GetMessage(executionContext, "Tokens", null);//"Tokens";
                ToEntitlement = MessageViewContainerList.GetMessage(executionContext, "Credits", null);//"Credits";
                tokensHeading = MessageViewContainerList.GetMessage(executionContext, "Tokens Tendered", null);//"Tokens Tendered";
                creditsHeading = MessageViewContainerList.GetMessage(executionContext, "Credits Exchanged", null);//"Credits Exchanged";
            }
            else if (taskType == TaskType.EXCHANGECREDITFORTOKEN)
            {
                ModuleName = MessageViewContainerList.GetMessage(executionContext, "Exchange", null);//" Exchange "; /*+ Environment.NewLine + " Credits  Tokens ";*/
                FromEntitlement = MessageViewContainerList.GetMessage(executionContext, "Credits", null);//"Credits";
                ToEntitlement = MessageViewContainerList.GetMessage(executionContext, "Tokens", null);//"Tokens";
                tokensHeading = MessageViewContainerList.GetMessage(executionContext, "Tokens To Buy", null);//"Tokens To Buy";
                creditsHeading = MessageViewContainerList.GetMessage(executionContext, "Credits Required", null);//"Credits Required";
            }
            NavigationClickCommand = new DelegateCommand(OnNavigationClick, ButtonEnable);
            OkCommand = new DelegateCommand(OnOkButtonClicked,ButtonEnable);
            Credits = "0";
            Tokens = "0";
        }
        #endregion
    }
}

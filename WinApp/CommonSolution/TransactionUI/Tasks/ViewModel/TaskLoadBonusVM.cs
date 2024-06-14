/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - TaskLoadBonus view model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     10-May-2021   Fiona                   Created for POS UI Redesign 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Drawing;
using System.Collections.ObjectModel;

using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.AccountsUI;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Promotions;
using Semnox.Parafait.Printer;
namespace Semnox.Parafait.TransactionUI
{
    public class TaskLoadBonusVM : TaskBaseViewModel
    {
        #region Members
        private TaskLoadBonusView taskLoadBonusView;
        private ICommand loaded;
        private ICommand cardAddedCommand;
        private ICommand navigationClickCommand;
        private string moduleName;
        private CardDetailsVM cardDetailsVM;
        private GenericToggleButtonsVM genericToggleButtonsVM;
        private ObservableCollection<CustomToggleButtonItem> toggleButtonItems;
        private string bonusToLoad;
        private int trxId;
        private object _parameter;
        private bool bonusToLoadReadOnly;
        private int gamePlayId;
        private AccountDTO accountDTO;
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
        public ObservableCollection<CustomToggleButtonItem> ToggleButtonItems
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return toggleButtonItems;

            }
            set
            {
                log.LogMethodEntry();
                toggleButtonItems = value;
                SetProperty(ref toggleButtonItems, value);
                log.LogMethodExit();
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
                log.LogMethodExit();
            }
        }

        public bool BonusToLoadReadOnly
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(bonusToLoadReadOnly);
                return bonusToLoadReadOnly;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref bonusToLoadReadOnly, value);
                log.LogMethodExit();
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
                log.LogMethodEntry();
                SetProperty(ref cardDetailsVM, value);
                log.LogMethodExit();
            }
        }
        public GenericToggleButtonsVM GenericToggleButtonsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericToggleButtonsVM);
                return genericToggleButtonsVM;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref genericToggleButtonsVM, value);
                log.LogMethodExit();
            }
        }
        public string BonusToLoad
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(bonusToLoad);
                return bonusToLoad;
            }
            set
            {
                log.LogMethodEntry();
                if (value != null)
                {
                    SetProperty(ref bonusToLoad, value);
                    OnTextChanged(null);
                }
                log.LogMethodExit();
            }
        }
        #endregion
        #region Methods
        private async void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            BonusToLoadReadOnly = false;
            taskLoadBonusView = parameter as TaskLoadBonusView;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            Remarks = string.Empty;
            if (!ManagerApprovalCheck(TaskType.LOADBONUS, parameter))
            {
                ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 268);
                PerformClose(parameter);
                return;
            }
            else
            {
                if (_parameter != null && ManagerId == -1)
                {
                    if (!ShowManagerApproval(parameter))
                    {
                        ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 268);
                        PerformClose(parameter);
                        return;
                    }
                    else
                    {
                        taskLoadBonusView.Focus();
                    }
                }
            }
            string loadBonusType = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "LOAD_BONUS_DEFAULT_ENT_TYPE");
            if (string.IsNullOrEmpty(loadBonusType))
            {
                loadBonusType = "B";
            }
            if (!loadBonusType.ToLower().Equals("none"))
            {
                foreach (CustomToggleButtonItem customToggleButtonItem in GenericToggleButtonsVM.ToggleButtonItems)
                {
                    if (customToggleButtonItem.Key == loadBonusType)
                    {
                        GenericToggleButtonsVM.SelectedToggleButtonItem = customToggleButtonItem;
                        customToggleButtonItem.IsChecked = true;
                    }
                    if (customToggleButtonItem.Key != loadBonusType)
                    {
                        customToggleButtonItem.IsEnabled = false;
                    }
                }
            }
            
            
            if (_parameter != null)
            {
                object[] pars = _parameter as object[];
                AddCardDetails(_parameter);
                string amountFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_FORMAT");
                BonusToLoad = Convert.ToDecimal(pars[1]).ToString(amountFormat);
                BonusToLoadReadOnly = true;
                Remarks = "Refund Gameplay :" + pars[3];
                gamePlayId = Convert.ToInt32(pars[2]);
            }
            log.LogMethodExit();
        }
        private void OnCardAdded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (_parameter != null)
            {
                return;
            }
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

            if (CardDetailsVM.AccountDTO.TechnicianCard == "Y")
            {
                ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 197, CardDetailsVM.AccountDTO.TagNumber);
                PerformClose(taskLoadBonusView);
                return;
            }
            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.AccountId >= 0)
            {
                SetFooterContent(string.Empty, MessageType.None);
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 263, null), MessageType.Error);
                log.Error(MessageViewContainerList.GetMessage(ExecutionContext, 263, null));
                return;
            }
            log.LogMethodExit();
        }
        private void OnTextChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty, MessageType.None);
            if (string.IsNullOrEmpty(BonusToLoad))
            {
                return;
            }
            if (CardDetailsVM.AccountDTO != null && Convert.ToDouble(BonusToLoad) == 0)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5151), MessageType.Warning);
                return;
            }
            string amountFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_FORMAT");
            bonusToLoad = Convert.ToDouble(BonusToLoad).ToString(amountFormat);
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
        private async void OnOkButtonClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (CardDetailsVM.AccountDTO == null)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 257, null), MessageType.Warning);
                return;
            }
            if (CardDetailsVM.AccountDTO.AccountId < 1)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 263, null), MessageType.Warning);
                return;
            }
            if (GenericToggleButtonsVM != null && GenericToggleButtonsVM.SelectedToggleButtonItem == null)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5181), MessageType.Warning);
                return;
            }
            if (BonusToLoad == string.Empty || Convert.ToDouble(BonusToLoad) == 0)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5151), MessageType.Warning);
                return;
            }
            double bonus = Convert.ToDouble(BonusToLoad);
            if (bonus > 0)
            {
                double loadBonusLimit = ParafaitDefaultViewContainerList.GetParafaitDefault<double>(ExecutionContext, "LOAD_BONUS_LIMIT");
                if (bonus > loadBonusLimit)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 43, loadBonusLimit.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_FORMAT"))), MessageType.Warning);
                    return;
                }
            }
            else if (bonus < 0)
            {
                //deduction limit
                double loadBonusLimit = ParafaitDefaultViewContainerList.GetParafaitDefault<double>(ExecutionContext, "LOAD_BONUS_DEDUCTION_LIMIT");
                if ((-1 * bonus) > loadBonusLimit)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5099, loadBonusLimit.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_FORMAT"))), MessageType.Warning);
                    return;
                }
            }
            if (string.IsNullOrEmpty(Remarks) && ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "LOAD_BONUS_REMARKS_MANDATORY") == "Y")
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 201, null), MessageType.Warning);
                return;
            }

            BonusDTO.BonusTypeEnum bonusType = BonusDTO.BonusTypeEnum.NONE;
            if (GenericToggleButtonsVM != null && GenericToggleButtonsVM.SelectedToggleButtonItem != null)
            {
                switch (GenericToggleButtonsVM.SelectedToggleButtonItem.Key)
                {
                    case "A":
                        bonusType = BonusDTO.BonusTypeEnum.CARD_BALANCE;
                        break;
                    case "L":
                        bonusType = BonusDTO.BonusTypeEnum.LOYALTY_POINT;
                        break;
                    case "G":
                        bonusType = BonusDTO.BonusTypeEnum.GAME_PLAY_CREDIT;
                        break;
                    case "B":
                        bonusType = BonusDTO.BonusTypeEnum.GAME_PLAY_BONUS;
                        break;
                }
            }
            if (!bonusType.Equals(BonusDTO.BonusTypeEnum.NONE))
            {
                if (bonus < 0)
                {
                    decimal creditsAvailable = 0;
                    if (bonusType.Equals(BonusDTO.BonusTypeEnum.CARD_BALANCE))
                    {
                        if (CardDetailsVM.AccountDTO.Credits != null)
                        {
                            creditsAvailable = creditsAvailable + (decimal)CardDetailsVM.AccountDTO.Credits;
                        }
                        if (CardDetailsVM.AccountDTO.AccountSummaryDTO != null)
                        {
                            if (CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusCardBalance != null)
                            {
                                creditsAvailable = creditsAvailable + (decimal)CardDetailsVM.AccountDTO.AccountSummaryDTO.CreditPlusCardBalance;
                            }
                        }
                    }
                    else if (bonusType.Equals(BonusDTO.BonusTypeEnum.LOYALTY_POINT))
                    {
                        if (CardDetailsVM.AccountDTO.AccountSummaryDTO != null)
                        {
                            if (CardDetailsVM.AccountDTO.AccountSummaryDTO.TotalLoyaltyPointBalance != null)
                            {
                                creditsAvailable = creditsAvailable + (decimal)CardDetailsVM.AccountDTO.AccountSummaryDTO.TotalLoyaltyPointBalance;
                            }
                        }
                    }
                    else if (bonusType.Equals(BonusDTO.BonusTypeEnum.GAME_PLAY_CREDIT))
                    {
                        if (CardDetailsVM.AccountDTO.AccountSummaryDTO != null)
                        {
                            if (CardDetailsVM.AccountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance != null)
                            {
                                creditsAvailable = creditsAvailable + (decimal)CardDetailsVM.AccountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance;
                            }
                        }
                    }
                    else if (bonusType.Equals(BonusDTO.BonusTypeEnum.GAME_PLAY_BONUS))
                    {
                        if (CardDetailsVM.AccountDTO.AccountSummaryDTO != null)
                        {
                            if (CardDetailsVM.AccountDTO.AccountSummaryDTO.TotalBonusBalance != null)
                            {
                                creditsAvailable = creditsAvailable + (decimal)CardDetailsVM.AccountDTO.AccountSummaryDTO.TotalBonusBalance;
                            }
                        }
                    }
                    if ((bonus + Convert.ToDouble(creditsAvailable)) < 0)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 49, (bonus * -1), creditsAvailable), MessageType.Warning);
                        return;
                    }
                }
            }
            if (!CheckManagerApprovalLimit(Convert.ToDecimal(bonus)))
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268), MessageType.Error);
                return;
            }
            //deduction manager limit
            BonusDTO bonusDTO = new BonusDTO(CardDetailsVM.AccountDTO.AccountId, bonusType, Convert.ToDecimal(bonus), Remarks, ManagerId, gamePlayId, -1);
            ITaskUseCases taskUseCases = TaskUseCaseFactory.GetTaskUseCases(ExecutionContext);

            try
            {
                IsLoadingVisible = true;
                BonusDTO result = await taskUseCases.LoadBonus(bonusDTO);
                IsLoadingVisible = false;
                trxId = result.TrxId;
                if (_parameter == null)
                {
                    SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 44);
                }
                else
                {
                    SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 523);
                }
                SetFooterContent(SuccessMessage, MessageType.Info);

                PoleDisplay.executionContext = ExecutionContext;
                PoleDisplay.writeSecondLine(BonusToLoad + " Bonus Loaded");
                if (ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AUTO_PRINT_LOAD_BONUS") == "Y")
                {
                    IsLoadingVisible = false;
                    ReceiptClass receiptClass = await taskUseCases.GetBonusReceipt(trxId);
                    IsLoadingVisible = false;
                    POSPrintHelper.PrintReceiptToPrinter(ExecutionContext, receiptClass, "Load Bonus Receipt", string.Empty);
                    ManagerId = -1;
                }
                if (ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AUTO_PRINT_LOAD_BONUS") == "A")
                {
                    GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                    GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                    {
                        Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "PRINT TRANSACTION", null),
                        Content = MessageViewContainerList.GetMessage(ExecutionContext, 484),
                        OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "Yes", null),
                        CancelButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "No", null),
                        MessageButtonsType = MessageButtonsType.OkCancel
                    };
                    messagePopupView.DataContext = messagePopupVM;
                    if (taskLoadBonusView != null)
                    {
                        messagePopupView.Owner = taskLoadBonusView;
                    }
                    messagePopupView.ShowDialog();
                    if (messagePopupVM.ButtonClickType == ButtonClickType.Ok)
                    {
                        IsLoadingVisible = true;
                        ReceiptClass receiptClass = await taskUseCases.GetBonusReceipt(trxId);
                        IsLoadingVisible = false;
                        POSPrintHelper.PrintReceiptToPrinter(ExecutionContext, receiptClass, "Load Bonus Receipt", string.Empty);
                        ManagerId = -1;
                    }
                    ManagerId = -1;
                }
                IsLoadingVisible = false;
                taskLoadBonusView.DialogResult = true;
                PerformClose(taskLoadBonusView);
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                IsLoadingVisible = false;
                SetFooterContent(vex.Message, MessageType.Error);
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
                IsLoadingVisible = false;
                SetFooterContent(pax.ToString(), MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
            }
            log.LogMethodExit();
        }
        
        int GetManagerApprovalLimit(decimal bonusValue)
        {
            log.LogMethodEntry(bonusValue);
            int limit = 0;
            if (bonusValue > 0)
            {
                limit = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(this.ExecutionContext, "LOAD_BONUS_LIMIT_FOR_MANAGER_APPROVAL", 0);
            }
            else if (bonusValue < 0)
            {
                limit = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(this.ExecutionContext, "LOAD_BONUS_DEDUCTION_LIMIT_FOR_MANAGER_APPROVAL", 0);
            }
            log.LogMethodExit(limit);
            return limit;
        }
        bool CheckManagerApprovalLimit(decimal bonusValue)
        {
            bool result = false;
            log.LogMethodEntry(bonusValue);
            int mgtLimitValue = GetManagerApprovalLimit(bonusValue);
            if (bonusValue > 0)
            {
                if (mgtLimitValue > 0 && bonusValue > mgtLimitValue)
                {
                    ManagerId = -1;
                    result = ShowManagerApproval(taskLoadBonusView);
                }
                else
                {
                    result = true;
                }
            }
            else if (bonusValue < 0)
            {
                if (mgtLimitValue > 0 && ((-1 * bonusValue) > mgtLimitValue))
                {
                    ManagerId = -1;
                    result = ShowManagerApproval(taskLoadBonusView);
                }
                else
                {
                    result = true;
                }
            }
            
            log.LogMethodExit(result);
            if (!result)
            {
                this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268), MessageType.Error);
            }
            return result;
        }
        private async void AddCardDetails(object param)
        {
            log.LogMethodEntry(param);
            try
            {
                IsLoadingVisible = true;
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(ExecutionContext);
                object[] pars = _parameter as object[];
                string cardNumber = pars[0].ToString();
                log.Debug("cardNumber" + cardNumber);
                AccountDTOCollection accountDTOCollection = await accountUseCases.GetAccounts(accountNumber: cardNumber, tagSiteId: ExecutionContext.GetSiteId(), buildChildRecords: true, activeRecordsOnly: true);
                if (accountDTOCollection != null && accountDTOCollection.data != null)
                {
                    accountDTO = accountDTOCollection.data[0];
                    CardDetailsVM.AccountDTO = accountDTO;
                    CardDetailsVM.EnableManualEntry = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(ex.Message, MessageType.Error);
            }
            finally
            {
                IsLoadingVisible = false;
            }
            log.LogMethodExit();
        }
        private void OnClearCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            CardDetailsVM.AccountDTO = null;
            Remarks = string.Empty;
            BonusToLoad = "0";
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
            SetFooterContent(string.Empty, MessageType.None);
            log.LogMethodExit();
        }
        private void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            if (genericToggleButtonsVM != null)
            {
                genericToggleButtonsVM.IsLoadingVisible = IsLoadingVisible;
                genericToggleButtonsVM.RaiseCanExecuteChanged();
            }
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
        private bool ClearButtonEnable(object state)
        {
            log.LogMethodEntry(_parameter);
            if (_parameter!=null)
            {
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                log.LogMethodExit(!IsLoadingVisible);
                return !IsLoadingVisible;
            }
        }
        #endregion
        #region Constructor
        public TaskLoadBonusVM(ExecutionContext executionContext, DeviceClass cardReader, object parameter = null) : base(executionContext, cardReader)
        {
            log.Info("TaskLoadBonus screen is opened");
            log.LogMethodEntry(executionContext, cardReader, parameter);
            this.ExecutionContext = executionContext;
            this._parameter = parameter;
            this.gamePlayId = -1;
            Loaded = new DelegateCommand(OnLoaded);
            NavigationClickCommand = new DelegateCommand(OnNavigationClick, ButtonEnable);
            OkCommand = new DelegateCommand(OnOkButtonClicked, ButtonEnable);
            ClearCommand = new DelegateCommand(OnClearCommand, ClearButtonEnable);
            PropertyChanged += OnPropertyChanged;
            CardDetailsVM = new CardDetailsVM(ExecutionContext);
            CardDetailsVM.PropertyChanged += OnPropertyChanged;
            CardDetailsVM.EnableManualEntry = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ALLOW_MANUAL_CARD_IN_LOAD_BONUS", false);
            if (parameter == null)
            {
                base.CardTappedEvent += HandleCardRead;                
            }

            CardAddedCommand = new DelegateCommand(OnCardAdded);
            ModuleName = MessageViewContainerList.GetMessage(executionContext, "Load Bonus", null);


            ToggleButtonItems = new ObservableCollection<CustomToggleButtonItem>();
            ToggleButtonItems.Add(new CustomToggleButtonItem()
            {
                DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(ExecutionContext,"Card Balance"),
                            FontWeight = FontWeights.Bold,
                            TextSize = TextSize.Small,
                            HorizontalAlignment = HorizontalAlignment.Center
                        },
                    },
                Key = CreditPlusTypeConverter.ToString(CreditPlusType.CARD_BALANCE)
            });
            ToggleButtonItems.Add(new CustomToggleButtonItem()
            {
                DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(this.ExecutionContext,"Loyalty Points",null),
                            FontWeight = FontWeights.Bold,
                            TextSize = TextSize.Small,
                            HorizontalAlignment = HorizontalAlignment.Center
                        }
                    },
                Key = CreditPlusTypeConverter.ToString(CreditPlusType.LOYALTY_POINT)
            });
            ToggleButtonItems.Add(new CustomToggleButtonItem()
            {
                DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(this.ExecutionContext,"Game Play Credits",null),
                            FontWeight = FontWeights.Bold,
                            TextSize = TextSize.Small,
                            HorizontalAlignment = HorizontalAlignment.Center
                        }
                    },
                Key = CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_CREDIT)
            });
            ToggleButtonItems.Add(new CustomToggleButtonItem()
            {
                DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(this.ExecutionContext,"Game Play Bonus",null),
                            FontWeight = FontWeights.Bold,
                            TextSize = TextSize.Small,
                            HorizontalAlignment = HorizontalAlignment.Center
                        }
                    },
                Key = CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_BONUS)
            });
            genericToggleButtonsVM = new GenericToggleButtonsVM()
            {
                ToggleButtonItems = ToggleButtonItems,
                IsVerticalOrientation = true,
                Columns = 2,
                IsDefaultSelectionNeeded = false
            };
            log.LogMethodExit();
        }
        #endregion
    }
}

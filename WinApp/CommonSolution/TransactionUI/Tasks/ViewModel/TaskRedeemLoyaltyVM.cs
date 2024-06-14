/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - TaskRedeem View model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-July-2021  Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Promotions;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.ViewContainer;
namespace Semnox.Parafait.TransactionUI
{
    public class TaskRedeemLoyaltyVM : TaskBaseViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ICommand cardAddedCommand;
        private ICommand isSelectedCommand;
        private CustomDataGridVM customDataGridVM;
        private string enteredCardNumber;
        private string availablePoints;
        private CardDetailsVM cardDetailsVM;
        private bool showRedeemLoyaltyScreen;
        private string redeemPoints;
        private string amountFormat;
        private ICommand loaded;
        List<LoyaltyAttributeContainerDTO> loyaltyAttributeContainerDTOs;
        List<LoyaltyRedemptionRuleContainerDTO> loyaltyRedemptionRuleContainerDTOs;
        private LoyaltyRedemptionRuleContainerDTO selectedLoyaltyRedemptionRule;
        private TaskRedeemLoyaltyView taskRedeemLoyaltyView;
        private ICommand redeemLoyaltyChanged;
        private ICommand backButtonCommand;
        #endregion

        #region Properties
        public ICommand RedeemLoyaltyChanged
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redeemLoyaltyChanged);
                return redeemLoyaltyChanged;
            }
            set
            {
                log.LogMethodEntry(redeemLoyaltyChanged, value);
                SetProperty(ref redeemLoyaltyChanged, value);
            }
        }
        public ICommand IsSelectedCommand
        {
            set
            {
                log.LogMethodEntry(isSelectedCommand, value);
                SetProperty(ref isSelectedCommand, value);
                log.LogMethodExit(isSelectedCommand);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isSelectedCommand);
                return isSelectedCommand;
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

        public string AvailablePoints
        {
            set
            {
                log.LogMethodEntry(availablePoints, value);
                SetProperty(ref availablePoints, value);
                log.LogMethodExit(availablePoints);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(availablePoints);
                return availablePoints;
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

        public bool ShowRedeemLoyaltyScreen
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showRedeemLoyaltyScreen);
                return showRedeemLoyaltyScreen;
            }
            set
            {
                log.LogMethodEntry(showRedeemLoyaltyScreen, value);
                SetProperty(ref showRedeemLoyaltyScreen, value);
                log.LogMethodExit(showRedeemLoyaltyScreen);
            }
        }

        public string EnteredCardNumber
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enteredCardNumber);
                return enteredCardNumber;

            }
            set
            {
                log.LogMethodEntry(enteredCardNumber, value);
                SetProperty(ref enteredCardNumber, value);
                log.LogMethodExit(enteredCardNumber);
            }
        }

        public CustomDataGridVM CustomDataGridVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(customDataGridVM);
                return customDataGridVM;
            }
            set
            {
                log.LogMethodEntry(customDataGridVM, value);
                SetProperty(ref customDataGridVM, value);
                log.LogMethodExit(customDataGridVM);
            }
        }

        public string RedeemPoints
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redeemPoints);
                return redeemPoints;
            }
            set
            {
                log.LogMethodEntry(redeemPoints, value);
                SetProperty(ref redeemPoints, value);
                log.LogMethodExit(redeemPoints);
            }
        }



        #endregion

        #region Constructor
        public TaskRedeemLoyaltyVM(ExecutionContext executionContext, DeviceClass cardReader) : base(executionContext, cardReader)
        {
            log.LogMethodEntry(executionContext);
            ExecutionContext = executionContext;
            ShowRedeemLoyaltyScreen = false;
            PropertyChanged += OnPropertyChanged;
            CardDetailsVM = new CardDetailsVM(ExecutionContext);
            CardDetailsVM.PropertyChanged += OnPropertyChanged;
            CardDetailsVM.EnableManualEntry = true;
            CardTappedEvent += HandleCardRead;
            CustomDataGridVM = new CustomDataGridVM(ExecutionContext)
            {
                IsComboAndSearchVisible = false,
                VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto
            };
            Loaded = new DelegateCommand(OnLoaded);
            OkCommand = new DelegateCommand(OnOkCommand, ButtonEnable);
            ClearCommand = new DelegateCommand(OnClearCommand, ButtonEnable);
            cardAddedCommand = new DelegateCommand(OnCardAdded);
            backButtonCommand = new DelegateCommand(OnBackButtonCommand, ButtonEnable);
            IsSelectedCommand = new DelegateCommand(OnSelectionChanged);
            RedeemLoyaltyChanged = new DelegateCommand(LoyaltyRedeemPointsValidating);
            loyaltyAttributeContainerDTOs = LoyaltyAttributeViewContainerList.GetLoyaltyAttributeContainerDTOList(ExecutionContext);
            loyaltyRedemptionRuleContainerDTOs = new List<LoyaltyRedemptionRuleContainerDTO>();
            List<LoyaltyRedemptionRuleContainerDTO> loyaltyRedemptionRuleList = LoyaltyRedemptionRuleViewContainerList.GetLoyaltyRedemptionRuleContainerDTOList(ExecutionContext);
            if (loyaltyRedemptionRuleList != null && loyaltyRedemptionRuleList.Any())
            {
                loyaltyRedemptionRuleContainerDTOs.AddRange(loyaltyRedemptionRuleList.Where(x => x.ActiveFlag && (x.ExpiryDate.Equals(DateTime.MinValue) || x.ExpiryDate >= DateTime.Now)).ToList());
            }
            CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                {
                    {"Attribute", new CustomDataGridColumnElement(){Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Choose an attribute")} },
                    {"Rule", new CustomDataGridColumnElement(){Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Rule")} },
                    {"RedemptionRuleId", new CustomDataGridColumnElement(){Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Redemption Value")} },
                    {"MinimumPoints", new CustomDataGridColumnElement(){Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Min Points")} },
                    {"MultiplesOnly", new CustomDataGridColumnElement(){Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Multiples Only") } }
                };
            log.LogMethodExit();
        }
        #endregion

        #region methods
        private void OnSelectionChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(CustomDataGridVM != null && CustomDataGridVM.SelectedItem != null)
            {
                selectedLoyaltyRedemptionRule = CustomDataGridVM.SelectedItem as LoyaltyRedemptionRuleContainerDTO;                
            }
            log.LogMethodExit();
        }
        private void LoyaltyRedeemPointsValidating(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty, MessageType.None);
            try
            {
                decimal redeemedPoints = Convert.ToDecimal(RedeemPoints);
                if ( redeemedPoints > (CardDetailsVM.AccountDTO.LoyaltyPoints + CardDetailsVM.AccountDTO.AccountSummaryDTO.RedeemableCreditPlusLoyaltyPoints))
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 79), MessageType.Warning);
                    log.Info("Ends-LoyaltyRedeemPointsValidating() as Redemption Points cannot be more than available points");
                    log.LogMethodExit();
                    return;
                }
            }
            catch (Exception ex)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 80), MessageType.Error);
                log.LogMethodExit();
                return;
            }
            PopulateRedemptionRule();
            log.LogMethodExit();
        }
        private void OnBackButtonCommand(object parameter)
        {
            log.LogMethodEntry();
            PerformClose(parameter);
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
            if(TappedAccountDTO != null)
            {
                CardDetailsVM.AccountDTO = TappedAccountDTO;
            }
            if (CardDetailsVM.AccountDTO == null)
            {
                ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 257, null);
                PerformClose(taskRedeemLoyaltyView);
                return;
            }
            if (CardDetailsVM.AccountDTO.TechnicianCard == "Y")
            {
                ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 197, CardDetailsVM.AccountDTO.TagNumber);
                PerformClose(taskRedeemLoyaltyView);
                return;
            }
            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.AccountId >= 0)
            {
                amountFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_FORMAT");
                decimal availableLoyaltyPoints = 0;
                if (CardDetailsVM.AccountDTO.LoyaltyPoints != null && CardDetailsVM.AccountDTO.AccountSummaryDTO.RedeemableCreditPlusLoyaltyPoints != null)
                {
                    availableLoyaltyPoints = Convert.ToDecimal(CardDetailsVM.AccountDTO.LoyaltyPoints + CardDetailsVM.AccountDTO.AccountSummaryDTO.RedeemableCreditPlusLoyaltyPoints);
                }
                else if(CardDetailsVM.AccountDTO.LoyaltyPoints != null)
                {
                    availableLoyaltyPoints = Convert.ToDecimal(CardDetailsVM.AccountDTO.LoyaltyPoints);
                }
                else if (CardDetailsVM.AccountDTO.AccountSummaryDTO.RedeemableCreditPlusLoyaltyPoints != null)
                {
                    availableLoyaltyPoints = Convert.ToDecimal(CardDetailsVM.AccountDTO.AccountSummaryDTO.RedeemableCreditPlusLoyaltyPoints);
                }
                else
                {
                    availableLoyaltyPoints = 0;
                }
                AvailablePoints = availableLoyaltyPoints.ToString(amountFormat);
                RedeemPoints = availableLoyaltyPoints.ToString(amountFormat);
                PopulateRedemptionRule();
                ShowRedeemLoyaltyScreen = true;
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 459), MessageType.Warning);
            }
            log.LogMethodExit();
        }

        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                taskRedeemLoyaltyView = parameter as TaskRedeemLoyaltyView;
                ErrorMessage = string.Empty;
                SuccessMessage = string.Empty;
                if (!ManagerApprovalCheck(TaskType.REDEEMLOYALTY, parameter))
                {
                    ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 268);
                    PerformClose(parameter);
                }
            }
            log.LogMethodExit();
        }
        void PopulateRedemptionRule()
        {
            log.LogMethodEntry();            
            if (loyaltyRedemptionRuleContainerDTOs != null && loyaltyRedemptionRuleContainerDTOs.Any())
            {
                List<LoyaltyRedemptionRuleModel> loyaltyRedemptionRuleModels = new List<LoyaltyRedemptionRuleModel>();
                foreach (LoyaltyRedemptionRuleContainerDTO loyaltyRedemptionRuleContainerDTO in loyaltyRedemptionRuleContainerDTOs)
                {
                    loyaltyRedemptionRuleModels.Add(new LoyaltyRedemptionRuleModel(amountFormat, loyaltyRedemptionRuleContainerDTO.RedemptionRuleId, loyaltyRedemptionRuleContainerDTO.LoyaltyPoints, loyaltyRedemptionRuleContainerDTO.RedemptionValue));
                }
                CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(loyaltyRedemptionRuleContainerDTOs);

                CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                {
                    {"Attribute", new CustomDataGridColumnElement(){Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Choose an attribute"), SourcePropertyName = "LoyaltyAttributeId", ChildOrSecondarySourcePropertyName = "LoyaltyAttributeId", SecondarySource = new ObservableCollection<object>(loyaltyAttributeContainerDTOs) } },
                    {"Rule", new CustomDataGridColumnElement(){Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Rule"), SourcePropertyName = "RedemptionRuleId", ChildOrSecondarySourcePropertyName = "RuleId", SecondarySource = new ObservableCollection<object>(loyaltyRedemptionRuleModels) } },
                    {"RedemptionRuleId", new CustomDataGridColumnElement(){Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Redemption Value") , Converter = new RedemptionValueConverter(), ConverterParameter = new List<object>(){ExecutionContext, redeemPoints, loyaltyRedemptionRuleContainerDTOs} } },
                    {"MinimumPoints", new CustomDataGridColumnElement(){Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Min Points"),DataGridColumnStringFormat = amountFormat } },
                    {"MultiplesOnly", new CustomDataGridColumnElement(){Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Multiples Only") } }
                };
                if (selectedLoyaltyRedemptionRule != null)
                {
                    CustomDataGridVM.SelectedItem = selectedLoyaltyRedemptionRule;
                }
                else
                {
                    CustomDataGridVM.SelectedItem = CustomDataGridVM.UICollectionToBeRendered[0] as LoyaltyRedemptionRuleContainerDTO;
                }
                taskRedeemLoyaltyView.loyaltyRedemptionRulesCustomDataGridUserControl.dataGrid.ScrollIntoView(CustomDataGridVM.SelectedItem);
                OnSelectionChanged(null);
            }

            log.LogMethodExit();
        }

        private async void OnOkCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (CardDetailsVM.AccountDTO != null && CardDetailsVM.AccountDTO.AccountId > -1)
            {
                if (selectedLoyaltyRedemptionRule == null)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4212), MessageType.Error);
                    return;
                }
                try
                {
                    Convert.ToDecimal(RedeemPoints);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    return;
                }
                decimal redeemedPoints;
                decimal.TryParse(redeemPoints, out redeemedPoints);
                if (redeemedPoints <= 0)
                {
                    PerformClose(parameter);
                    return;
                }
                else if (redeemedPoints > (CardDetailsVM.AccountDTO.LoyaltyPoints + CardDetailsVM.AccountDTO.AccountSummaryDTO.RedeemableCreditPlusLoyaltyPoints))
                {
                    return;
                }
                else
                {
                    bool result = ManagerApprovalLimitCheck(TaskType.REDEEMLOYALTY, Convert.ToInt32(redeemedPoints), parameter);
                    if (!result)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1215), MessageType.Error);
                        return;
                    }
                    else
                    {
                        SetFooterContent(string.Empty, MessageType.None);
                    }
                    IsLoadingVisible = true;
                    ITaskUseCases taskUseCases = TaskUseCaseFactory.GetTaskUseCases(ExecutionContext);
                    LoyaltyRedeemDTO loyaltyRedeemDTO = new LoyaltyRedeemDTO()
                    {
                        CardId = CardDetailsVM.AccountDTO.AccountId,
                        LoyaltyRedeemPoints = redeemedPoints,
                        ManagerId = ManagerId,
                        Remarks = string.IsNullOrEmpty(Remarks) ? string.Empty : Remarks,
                        RuleId = selectedLoyaltyRedemptionRule.RedemptionRuleId
                    };
                    try
                    {
                        await taskUseCases.RedeemLoyalty(loyaltyRedeemDTO);
                        SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 46);
                        PerformClose(taskRedeemLoyaltyView);
                        PoleDisplay.executionContext = ExecutionContext;
                        PoleDisplay.writeSecondLine(redeemPoints.ToString() + " points redeemed");
                    }
                    catch (ValidationException vex)
                    {
                        log.Error(vex);
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
                        SetFooterContent(pax.ToString(), MessageType.Error);
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
        }

        private void OnClearCommand(object parameter)
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);
            CardDetailsVM.AccountDTO = null;
            TappedAccountDTO = null;
            Remarks = string.Empty;
            ShowRedeemLoyaltyScreen = false;
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
    }
}

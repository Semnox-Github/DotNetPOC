/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Link cards View model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-June-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.ViewContainer;
namespace Semnox.Parafait.TransactionUI
{
    public class LinkCardsVM : TaskBaseViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private LinkCardsView linkCardsView;
        private ICommand backButtonCommand;
        private ICommand cardAddedCommand;
        private ICommand cardDetailsClearCommand;
        private ICommand enterCardNoClickedCommand;
        private ICommand loaded;
        private ICommand toggleButtonCheckedCommand;
        private CardDetailsVM parentDetailsVM;
        private GenericToggleButtonsVM genericToggleButtonsVM;
        private AccountDTO childCardDTO;
        private ObservableCollection<AccountRelationShipModel> childCardsCollection;
        private List<AccountRelationshipDTO> activeAccountRelationshipDTOList;
        private List<AccountRelationShipModel> childCardDeletionlist;
        private List<AccountRelationshipDTO> inactiveAccountRelationshipDTOList;
        private List<AccountRelationshipDTO> trxAccountRelationShipDTOList;
        private Visibility showCustomNumericUpDown;
        private int totalDailyLimitPercentage;
        private int customFirstCardValue;
        private int totalCardsToBeLinked;
        private string parentCard;
        private bool isParentCardTapped;
        private CustomToggleButtonItem selectedSplitAction;
        private bool showDailyLimitText;
        #endregion

        #region Properties
        public bool ShowDailyLimitText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showDailyLimitText);
                return showDailyLimitText;
            }
            set
            {
                log.LogMethodEntry(showDailyLimitText, value);
                SetProperty(ref showDailyLimitText, value);
                log.LogMethodExit(showDailyLimitText);
            }
        }
        public ICommand ToggleButtonCheckedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(toggleButtonCheckedCommand);
                return toggleButtonCheckedCommand;
            }
            set
            {
                log.LogMethodEntry(toggleButtonCheckedCommand, value);
                SetProperty(ref toggleButtonCheckedCommand, value);
                log.LogMethodExit(toggleButtonCheckedCommand);
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
                log.LogMethodEntry(genericToggleButtonsVM, value);
                SetProperty(ref genericToggleButtonsVM, value);
                log.LogMethodExit(genericToggleButtonsVM);
            }
        }
        public bool IsParentCardTapped
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isParentCardTapped);
                return isParentCardTapped;
            }
            set
            {
                log.LogMethodEntry(isParentCardTapped, value);
                SetProperty(ref isParentCardTapped, value);
                log.LogMethodExit(isParentCardTapped);
            }
        }
        public int TotalCardsToBeLinked
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(totalCardsToBeLinked);
                return totalCardsToBeLinked;
            }
            set
            {
                log.LogMethodEntry(totalCardsToBeLinked,value);
                SetProperty(ref totalCardsToBeLinked, value);
                log.LogMethodExit(totalCardsToBeLinked);
            }
        }
        public int CustomFirstCardValue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(customFirstCardValue);
                return customFirstCardValue;
            }
            set
            {
                log.LogMethodEntry(customFirstCardValue, value);
                SetProperty(ref customFirstCardValue, value);
                splitDailyLimitPercentage();
                log.LogMethodExit(customFirstCardValue);
            }
        }
        public Visibility ShowCustomNumericUpDown
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showCustomNumericUpDown);
                return showCustomNumericUpDown;
            }
            set
            {
                log.LogMethodEntry(showCustomNumericUpDown, value);
                SetProperty(ref showCustomNumericUpDown, value);
                log.LogMethodExit(showCustomNumericUpDown);
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
                log.LogMethodExit();
                return loaded;
            }
        }
        public ICommand EnterCardNoClickedCommand
        {
            set
            {
                log.LogMethodEntry(enterCardNoClickedCommand, value);
                SetProperty(ref enterCardNoClickedCommand, value);
                log.LogMethodExit(enterCardNoClickedCommand);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enterCardNoClickedCommand);
                return enterCardNoClickedCommand;
            }
        }
        public ObservableCollection<AccountRelationShipModel> ChildCardsCollection
        {
            set
            {
                log.LogMethodEntry(childCardsCollection, value);
                SetProperty(ref childCardsCollection, value);
                log.LogMethodExit(childCardsCollection);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(childCardsCollection);
                return childCardsCollection;
            }
        }


        public CardDetailsVM ParentDetailsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(parentDetailsVM);
                return parentDetailsVM;
            }
            set
            {
                log.LogMethodEntry(parentDetailsVM, value);
                SetProperty(ref parentDetailsVM, value);
                log.LogMethodExit(parentDetailsVM);
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

        #region Contructors
        public LinkCardsVM(ExecutionContext executionContext, DeviceClass cardReader) : base(executionContext, cardReader)
        {
            log.LogMethodEntry(executionContext, cardReader);
            IsParentCardTapped = false;
            showDailyLimitText = false;
            activeAccountRelationshipDTOList = null;
            inactiveAccountRelationshipDTOList = null;
            trxAccountRelationShipDTOList = null;
            TotalCardsToBeLinked = 0;
            parentCard = string.Empty;
            backButtonCommand = new DelegateCommand(OnBackButtonClicked, ButtonEnable);
            loaded = new DelegateCommand(OnLoaded);
            ShowRemark = false;
            ShowCustomNumericUpDown = Visibility.Visible;
            CustomFirstCardValue = 10;
            parentCard = string.Empty;
            totalDailyLimitPercentage = 100;
            PropertyChanged += OnPropertyChanged;
            parentDetailsVM = new CardDetailsVM(ExecutionContext);
            parentDetailsVM.PropertyChanged += OnPropertyChanged;
            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext,"Tap Parent Card"), MessageType.Info);
            cardDetailsClearCommand = new DelegateCommand(OnCardDetailsClearClicked, ButtonEnable);
            cardAddedCommand = new DelegateCommand(OnCardAdded);
            base.CardTappedEvent += HandleCardRead;
            childCardsCollection = new ObservableCollection<AccountRelationShipModel>();
            childCardDeletionlist = new List<AccountRelationShipModel>();
            enterCardNoClickedCommand = new DelegateCommand(OnEnterCardNoClickedCommand, ButtonEnable);
            ClearCommand = new DelegateCommand(OnClearClicked, ButtonEnable);
            OkCommand = new DelegateCommand(OnOkClicked, ButtonEnable);
            GenericToggleButtonsVM = new GenericToggleButtonsVM();
            ObservableCollection<CustomToggleButtonItem> splitActionButtons = GetCustomToggleButtonItems();
            GenericToggleButtonsVM.ToggleButtonItems = splitActionButtons;
            GenericToggleButtonsVM.IsVerticalOrientation = true;
            ToggleButtonCheckedCommand = new DelegateCommand(OnToggleButtonCheckedCommand);
            log.LogMethodExit();
        }
        public LinkCardsVM(ExecutionContext executionContext, DeviceClass cardReader, string parentCard): this(executionContext,cardReader)
        {
            log.LogMethodEntry(parentCard);
            this.parentCard = parentCard;
            log.LogMethodExit();
        }
        public LinkCardsVM(ExecutionContext executionContext, DeviceClass cardReader, List<AccountRelationshipDTO> accountRelationshipDTOs):this(executionContext, cardReader)
        {
            log.LogMethodEntry(accountRelationshipDTOs);
            this.trxAccountRelationShipDTOList = accountRelationshipDTOs;
            log.LogMethodExit();
        }
        #endregion

        #region Methods
        internal ObservableCollection<CustomToggleButtonItem> GetCustomToggleButtonItems()
        {
            ObservableCollection<CustomToggleButtonItem> actionButtons = new ObservableCollection<CustomToggleButtonItem>()
            {
                new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(ExecutionContext, "CLEAR SPLIT"),
                            TextSize = TextSize.Small,
                            HorizontalAlignment = HorizontalAlignment.Center
                        }
                    },
                    Key = "clearsplit",
                    IsChecked = false
                },
                new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(ExecutionContext, "SPLIT EQUAL"),
                            TextSize = TextSize.Small,
                            HorizontalAlignment = HorizontalAlignment.Center
                        }
                    },
                    Key = "splitequal",
                    IsChecked = false
                },
                new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(ExecutionContext, "50% FIRST CARD"),
                            TextSize = TextSize.Small,
                            HorizontalAlignment = HorizontalAlignment.Center
                        }
                    },
                    Key = "50firstcard",
                    IsChecked = false
                },
                new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(ExecutionContext, "CUSTOM FIRST CARD"),
                            TextSize = TextSize.Small
                        }
                    },
                    Key = "customfirstcard",
                    IsChecked = false
                },
            };
            return actionButtons;
        }
        private void OnToggleButtonCheckedCommand(object parameter)
        {
            if (parameter != null)
            {
                log.LogMethodEntry(parameter);
                selectedSplitAction = parameter as CustomToggleButtonItem;
                if (selectedSplitAction.Key.Equals("customfirstcard"))
                {
                    CustomFirstCardValue = totalDailyLimitPercentage / 2;
                    ShowCustomNumericUpDown = Visibility.Visible;
                }
                else
                {
                    ShowCustomNumericUpDown = Visibility.Collapsed;
                }
                splitDailyLimitPercentage();
                log.LogMethodExit();
            }
        }

        private async void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                linkCardsView = parameter as LinkCardsView;
                GenericToggleButtonsVM.SelectedToggleButtonItem = GenericToggleButtonsVM.ToggleButtonItems[0];
                selectedSplitAction = GenericToggleButtonsVM.SelectedToggleButtonItem;
                try
                {
                    IsLoadingVisible = true;
                    if (trxAccountRelationShipDTOList != null && trxAccountRelationShipDTOList.Count > 0)
                    {
                        AccountDTO parentAccountDTO;
                        int parentId = -1;
                        AccountRelationshipDTO accountRelationshipDTO = trxAccountRelationShipDTOList.FirstOrDefault(x => x.AccountId != -1);
                        if (accountRelationshipDTO != null)
                        {
                            parentId = accountRelationshipDTO.AccountId;
                        }
                        if (parentId > -1)
                        {
                            AccountDTOCollection accountDTOCollection = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).GetAccounts(accountId: parentId, tagSiteId: ExecutionContext.GetSiteId(), buildChildRecords: true, activeRecordsOnly: true);
                            if (accountDTOCollection != null && accountDTOCollection.data != null)
                            {
                                parentAccountDTO = accountDTOCollection.data[0];
                                ParentDetailsVM.AccountDTO = parentAccountDTO;
                                await HandleParentCardRead();
                            }
                        }

                        foreach (AccountRelationshipDTO trxAccountRelationshipDTO in trxAccountRelationShipDTOList)
                        {
                            int childCardId = trxAccountRelationshipDTO.RelatedAccountId;
                            AccountDTOCollection accountDTOCollection = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).GetAccounts(accountId: childCardId, tagSiteId: ExecutionContext.GetSiteId(), buildChildRecords: true, activeRecordsOnly: true);
                            if (accountDTOCollection != null && accountDTOCollection.data != null)
                            {
                                childCardDTO = accountDTOCollection.data[0];
                                bool result = await HandleChildCardRead();
                                if (result)
                                {
                                    ShowDailyLimitText = true;
                                    ChildCardsCollection.Add(new AccountRelationShipModel()
                                    {
                                        AccountRelationshipId = -1,
                                        DailyLimitPercentage = null,
                                        ChildCardDetailsVM = new CardDetailsVM(this.ExecutionContext)
                                        {
                                            AccountDTO = childCardDTO
                                        }
                                    });
                                    TotalCardsToBeLinked = TotalCardsToBeLinked + 1;
                                }
                            }
                        }
                        if (parentId == -1)
                        {
                            IsParentCardTapped = false;
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Tap Parent Card"), MessageType.Info);
                        }

                    }
                    if (!string.IsNullOrEmpty(parentCard))
                    {
                        AccountDTOCollection accountDTOCollection = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).GetAccounts(accountNumber: parentCard, tagSiteId: ExecutionContext.GetSiteId(), buildChildRecords: true, activeRecordsOnly: true);
                        if (accountDTOCollection != null && accountDTOCollection.data[0] != null)
                        {
                            ParentDetailsVM.AccountDTO = accountDTOCollection.data[0];
                            HandleCardRead();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                finally
                {
                    IsLoadingVisible = false;
                }
                log.LogMethodExit();
            }
        }
        private async void OnOkClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if ((ChildCardsCollection.Count == 0 && childCardDeletionlist.Count == 0) || parentDetailsVM.AccountDTO == null)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 371), MessageType.Warning);
                log.LogMethodExit();
                return;
            }
            List<AccountRelationshipDTO> updateChildCardsList = new List<AccountRelationshipDTO>();
            List<LinkNewCardDTO> saveChildCardsList = new List<LinkNewCardDTO>();
            foreach (AccountRelationShipModel child in ChildCardsCollection)
            {
                if (child.AccountRelationshipId > -1)
                {
                    AccountRelationshipDTO accountRelationshipDTO = null;
                    if (activeAccountRelationshipDTOList != null && activeAccountRelationshipDTOList.Exists(x => x.AccountRelationshipId == child.AccountRelationshipId))
                    {
                        accountRelationshipDTO = activeAccountRelationshipDTOList.Find(x => x.AccountRelationshipId == child.AccountRelationshipId);
                        if (accountRelationshipDTO != null)
                        {
                            accountRelationshipDTO.DailyLimitPercentage = child.DailyLimitPercentage;
                            accountRelationshipDTO.IsActive = true;
                            updateChildCardsList.Add(accountRelationshipDTO);
                        }
                    }
                    else
                    {
                        accountRelationshipDTO = inactiveAccountRelationshipDTOList.Find(x => x.AccountRelationshipId == child.AccountRelationshipId);
                        if (accountRelationshipDTO != null)
                        {
                            accountRelationshipDTO.DailyLimitPercentage = child.DailyLimitPercentage;
                            accountRelationshipDTO.IsActive = true;
                            updateChildCardsList.Add(accountRelationshipDTO);
                        }
                    }
                }
                else
                {
                    AccountRelationshipDTO accountRelationshipDTO = new AccountRelationshipDTO(-1, parentDetailsVM.AccountDTO.AccountId, child.ChildCardDetailsVM.AccountDTO.AccountId, true, child.DailyLimitPercentage);
                    LinkNewCardDTO newAccountRelationshipDTO = new LinkNewCardDTO(child.ChildCardDetailsVM.AccountDTO.TagNumber, accountRelationshipDTO);
                    saveChildCardsList.Add(newAccountRelationshipDTO);
                }
            }
            if (childCardDeletionlist != null && childCardDeletionlist.Count > 0)
            {
                foreach (AccountRelationShipModel accountRelationShipModel in childCardDeletionlist)
                {
                    AccountRelationshipDTO accountRelationshipDTO = null;
                    accountRelationshipDTO = activeAccountRelationshipDTOList.Find(x => x.AccountRelationshipId == accountRelationShipModel.AccountRelationshipId);
                    if (accountRelationshipDTO != null)
                    {
                        accountRelationshipDTO.IsActive = false;
                        updateChildCardsList.Add(accountRelationshipDTO);
                    }
                    else
                    {
                        accountRelationshipDTO = inactiveAccountRelationshipDTOList.Find(x => x.AccountRelationshipId == accountRelationShipModel.AccountRelationshipId);
                        if (accountRelationshipDTO != null)
                        {
                            accountRelationshipDTO.IsActive = false;
                            updateChildCardsList.Add(accountRelationshipDTO);
                        }
                    }
                }
            }
            List<AccountRelationshipDTO> resultFromUpdate = null;
            List<LinkNewCardDTO> resultFromSave = null;
            IsLoadingVisible = true;
            try
            {
                if (updateChildCardsList != null && updateChildCardsList.Count > 0)
                {
                    resultFromUpdate = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).UpdateAccountRelationships(updateChildCardsList);
                }
                if (saveChildCardsList != null && saveChildCardsList.Count > 0)
                {
                    resultFromSave = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).CreateAccountRelationships(saveChildCardsList);
                }

            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                IsLoadingVisible = false;
                SetFooterContent(vex.Message.ToString(), MessageType.Error);
                return;
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
                SetFooterContent(pax.Message.ToString(), MessageType.Error);
                return;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                return;
            }
            finally
            {
                IsLoadingVisible = false;
            }
            if (resultFromSave != null || resultFromUpdate != null)
            {
                SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 122);
                SetFooterContent(SuccessMessage, MessageType.Info);
            }

            log.LogMethodExit();
        }

        private async void PopulateChildCardsCollection()
        {
            log.LogMethodEntry();
            if (activeAccountRelationshipDTOList != null && activeAccountRelationshipDTOList.Count > 0)
            {
                try
                {
                    IsLoadingVisible = true;
                    foreach (AccountRelationshipDTO parentChildRelation in activeAccountRelationshipDTOList)
                    {
                        AccountDTOCollection accountDTOCollection = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).GetAccounts(accountId: parentChildRelation.RelatedAccountId);
                        if (accountDTOCollection != null && accountDTOCollection.data != null)
                        {
                            ShowDailyLimitText = true;
                            ChildCardsCollection.Add(new AccountRelationShipModel()
                            {
                                AccountRelationshipId = parentChildRelation.AccountRelationshipId,
                                DailyLimitPercentage = parentChildRelation.DailyLimitPercentage == -1 ? null: parentChildRelation.DailyLimitPercentage,
                                ChildCardDetailsVM = new CardDetailsVM(this.ExecutionContext)
                                {
                                    AccountDTO = accountDTOCollection.data[0]
                                }
                            });
                            TotalCardsToBeLinked = TotalCardsToBeLinked + 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                finally
                {
                    IsLoadingVisible = false;
                }
            }
            log.LogMethodExit();
        }

        private void splitDailyLimitPercentage()
        {
            log.LogMethodEntry();
            if(childCardsCollection != null && childCardsCollection.Count > 0)
            {
                int childCardsCount = ChildCardsCollection.Count;
                int totalToSplit = Math.Max(100, totalDailyLimitPercentage);
                int each = 0;
                int first = 0;
                int last = 0;

                if (selectedSplitAction.Key.Equals("splitequal"))
                {
                    each = totalToSplit / childCardsCount;
                    first = each + totalToSplit - (each * childCardsCount);
                }
                else if (selectedSplitAction.Key.Equals("50firstcard"))
                {
                    if (childCardsCount == 1)
                        first = totalToSplit;
                    else
                    {
                        first = totalToSplit / 2;
                        each = (totalToSplit - first) / (childCardsCount - 1);
                        last = each + first - each * (childCardsCount - 1);
                    }
                }
                else if (selectedSplitAction.Key.Equals("customfirstcard"))
                {

                    if (childCardsCount == 1)
                        first = Math.Min(customFirstCardValue, totalToSplit);
                    else
                    {
                        first = Math.Min(customFirstCardValue, totalToSplit);
                        int balance = Math.Max(0, totalToSplit - first);
                        each = balance / (childCardsCount - 1);
                        last = each + balance - each * (childCardsCount - 1);
                    }
                }
                else
                {
                    first = 0;
                    each = 0;
                    last = 0;
                }
                ObservableCollection<AccountRelationShipModel> backup = new ObservableCollection<AccountRelationShipModel>(ChildCardsCollection);
                backup[0].DailyLimitPercentage = first;
                for(int i = 1; i < backup.Count; i++)
                {
                    backup[i].DailyLimitPercentage = each;
                }
                if(last > 0)
                {
                    backup[backup.Count - 1].DailyLimitPercentage = last;
                }
                ChildCardsCollection = backup;
            }
            log.LogMethodExit();
        }
        private void OnEnterCardNoClickedCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            linkCardsView = parameter as LinkCardsView;
            SetFooterContent(string.Empty, MessageType.None);
            GenericDataEntryView dataEntryView = new GenericDataEntryView();
            GenericDataEntryVM dataEntryVM = new GenericDataEntryVM(ExecutionContext)
            {
                Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "Enter Card No"),
                DataEntryCollections = new ObservableCollection<DataEntryElement>()
                    {
                        new DataEntryElement()
                        {
                            Heading=MessageViewContainerList.GetMessage(this.ExecutionContext, "Card No"),
                            Type = DataEntryType.TextBox,
                            DefaultValue = MessageViewContainerList.GetMessage(this.ExecutionContext, "Enter Card No."),
                            IsMandatory = true
                        }
                    }
            };
            dataEntryView.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            dataEntryView.Height = SystemParameters.PrimaryScreenHeight;
            dataEntryView.DataContext = dataEntryVM;
            dataEntryView.Owner = linkCardsView;
            dataEntryView.ShowDialog();
            if (dataEntryVM.ButtonClickType == ButtonClickType.Ok)
            {
                string cardNumber = dataEntryVM.DataEntryCollections[0].Text;
                TagNumberViewParser tagNumberViewParser = new TagNumberViewParser(ExecutionContext);
                TagNumberView tagNumberView;
                if (tagNumberViewParser.TryParse(cardNumber, out tagNumberView) == false)
                {
                    string errorMessage = tagNumberViewParser.Validate(cardNumber);
                    SetFooterContent(errorMessage, MessageType.Warning);
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }
                SetAccountsDTO(cardNumber);
            }
            log.LogMethodExit();
        }

        private async void SetAccountsDTO(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            try
            {
                IsLoadingVisible = true;
                AccountDTOCollection accountDTOCollection = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).GetAccounts(accountNumber:cardNumber,tagSiteId:ExecutionContext.GetSiteId(),buildChildRecords:true,activeRecordsOnly:true);
                if(accountDTOCollection != null && accountDTOCollection.data != null)
                {
                    childCardDTO = accountDTOCollection.data[0];
                    TappedAccountDTO = childCardDTO;
                    HandleCardRead();
                }
                else
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 257, null), MessageType.Warning);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                IsLoadingVisible = false;
            }
            log.LogMethodExit();
        }



        private void OnCardAdded(object parameter)
        {
            log.LogMethodEntry(parameter);
            HandleCardRead();
            log.LogMethodExit();
        }
        internal async void HandleCardRead()
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);
            if (!IsParentCardTapped)
            {
                await HandleParentCardRead();
            }
            else
            {
                bool result = await HandleChildCardRead();
                if (result)
                {
                    AddChildCardToCollection();
                }
            }

            log.LogMethodExit();
        }

        internal async Task<bool> HandleParentCardRead()
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);

            if (TappedAccountDTO != null)
            {
                parentDetailsVM.AccountDTO = TappedAccountDTO;
            }
            if (parentDetailsVM.AccountDTO == null)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 257, null), MessageType.Warning);
                ParentDetailsVM.AccountDTO = null;
                return false;
            }
            if (parentDetailsVM.AccountDTO != null && parentDetailsVM.AccountDTO.AccountId == -1)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1514), MessageType.Warning);
                ParentDetailsVM.AccountDTO = null;
                return false;
            }
            if (parentDetailsVM.AccountDTO.TechnicianCard == "Y")
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 197, ParentDetailsVM.AccountDTO.TagNumber), MessageType.Warning);
                ParentDetailsVM.AccountDTO = null;
                return false;
            }

            if (ParentDetailsVM.AccountDTO.AccountRelationshipDTOList != null && ParentDetailsVM.AccountDTO.AccountRelationshipDTOList.Any())
            {
                if(ParentDetailsVM.AccountDTO.AccountRelationshipDTOList.Exists(r => r.RelatedAccountId == ParentDetailsVM.AccountDTO.AccountId))
                {
                    AccountRelationshipDTO parentAsChild = ParentDetailsVM.AccountDTO.AccountRelationshipDTOList.FirstOrDefault(r => r.RelatedAccountId == ParentDetailsVM.AccountDTO.AccountId);
                    if (parentAsChild != null)
                    {
                        AccountDTOCollection accountCollection = null;
                        try
                        {
                            IsLoadingVisible = true;
                            accountCollection = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).GetAccounts(accountId: parentAsChild.AccountId);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                        finally
                        {
                            IsLoadingVisible = false;
                        }
                        if (accountCollection != null && accountCollection.data.Any())
                        {
                            AccountDTO parent = accountCollection.data[0];
                            if (parent != null)
                            {
                                if (parent.ValidFlag)
                                {
                                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1513, parent.TagNumber), MessageType.Warning);
                                }
                                else
                                {
                                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1513, parent.TagNumber + "(Inactive)"), MessageType.Warning);
                                }
                                ParentDetailsVM.AccountDTO = null;
                                return false;
                            }
                        }
                    }
                }
            }

            inactiveAccountRelationshipDTOList = await GetParentChildCards(0);
            activeAccountRelationshipDTOList = ParentDetailsVM.AccountDTO.AccountRelationshipDTOList;
            IsParentCardTapped = true;
            PopulateChildCardsCollection();
            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1515), MessageType.Info);
            return true;
        }

        internal async Task<bool> HandleChildCardRead()
        {
            if (TappedAccountDTO != null)
            {
                childCardDTO = TappedAccountDTO;
            }
            if (childCardDTO != null && childCardDTO.AccountId != -1)
            {
                if (childCardDTO.TechnicianCard == "Y")
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 197, childCardDTO.TagNumber), MessageType.Warning);
                    return false;
                }
                if (ChildCardsCollection.Any(c => c.ChildCardDetailsVM.AccountDTO != null && c.ChildCardDetailsVM.AccountDTO.TagNumber.ToLower() == childCardDTO.TagNumber.ToLower()))
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 1517), MessageType.Warning);
                    return false;
                }
                if (ParentDetailsVM.AccountDTO != null && (ParentDetailsVM.AccountDTO.TagNumber.ToLower() == childCardDTO.TagNumber.ToLower()) ||
                    childCardDTO.AccountRelationshipDTOList != null && childCardDTO.AccountRelationshipDTOList.Count > 0 && childCardDTO.AccountRelationshipDTOList.Exists(c => c.AccountId == childCardDTO.AccountId))
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 1516), MessageType.Warning);
                    return false;
                }
                List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.ACCOUNT_ID, childCardDTO.AccountId.ToString()));
                searchParameters.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.IS_ACTIVE, "0"));
                try
                {
                    IsLoadingVisible = true;
                    List<AccountRelationshipDTO> tappedCardsInactiveAccountRelationshipDTOs = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).GetAccountRelationships(searchParameters);
                    if (tappedCardsInactiveAccountRelationshipDTOs != null && tappedCardsInactiveAccountRelationshipDTOs.Count > 0)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 1516), MessageType.Warning);
                        IsLoadingVisible = false;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                finally
                {
                    IsLoadingVisible = false;
                }
                if(childCardDTO.AccountRelationshipDTOList != null && childCardDTO.AccountRelationshipDTOList.Any())
                {
                    if(childCardDTO.AccountRelationshipDTOList.Exists(r => r.RelatedAccountId == childCardDTO.AccountId) && !childCardDeletionlist.Exists(c => c.ChildCardDetailsVM.AccountDTO.AccountId == childCardDTO.AccountId))
                    {
                        AccountRelationshipDTO parentChild = childCardDTO.AccountRelationshipDTOList.FirstOrDefault(r => r.RelatedAccountId == childCardDTO.AccountId);
                        if(parentChild != null)
                        {
                            AccountDTOCollection accountCollection = null;
                            try
                            {
                                IsLoadingVisible = true;
                                accountCollection = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).GetAccounts(accountId: parentChild.AccountId);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                            finally
                            {
                                IsLoadingVisible = false;
                            }
                            if(accountCollection != null && accountCollection.data.Any())
                            {
                                AccountDTO parent= accountCollection.data[0];
                                if (parent != null)
                                {
                                    if (parent.ValidFlag)
                                    {
                                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1513, parent.TagNumber), MessageType.Warning);
                                    }
                                    else
                                    {
                                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1513, parent.TagNumber + "(Inactive)"), MessageType.Warning);
                                    }
                                    return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }
            else
            {
                if ((ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "ALLOW_NEW_CARDS_FOR_CHILDCARDS")).Equals("N"))
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1518), MessageType.Warning);
                    return false;
                }
                else
                {
                    if (ChildCardsCollection.Any(c => c.ChildCardDetailsVM.AccountDTO != null && c.ChildCardDetailsVM.AccountDTO.TagNumber.ToLower() == childCardDTO.TagNumber.ToLower()))
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 1517), MessageType.Warning);
                        return false;
                    }
                    return true;
                }
            }
        }

        private void AddChildCardToCollection()
        {
            AccountRelationShipModel accountRelationShipModel = null;
            if(childCardDeletionlist != null && childCardDeletionlist.Any())
            {
                accountRelationShipModel = childCardDeletionlist.Find(x => x.ChildCardDetailsVM.AccountDTO.AccountId == childCardDTO.AccountId);
                childCardDeletionlist.Remove(accountRelationShipModel);
                if (accountRelationShipModel != null)
                {
                    ChildCardsCollection.Add(accountRelationShipModel);
                }
            }
            AccountRelationshipDTO inactiveAccountRelationshipDTO = null;
            if (inactiveAccountRelationshipDTOList != null && inactiveAccountRelationshipDTOList.Count > 0)
            {
                inactiveAccountRelationshipDTO = inactiveAccountRelationshipDTOList.Find(x => x.RelatedAccountId == childCardDTO.AccountId);
            }
            if (inactiveAccountRelationshipDTO != null)
            {
                ChildCardsCollection.Add(new AccountRelationShipModel()
                {
                    AccountRelationshipId = inactiveAccountRelationshipDTO.AccountRelationshipId,
                    DailyLimitPercentage = inactiveAccountRelationshipDTO.DailyLimitPercentage,
                    ChildCardDetailsVM = new CardDetailsVM(this.ExecutionContext)
                    {
                        AccountDTO = childCardDTO
                    }
                });
            }
            if(accountRelationShipModel != null || inactiveAccountRelationshipDTO != null)
            {
                selectedSplitAction = GenericToggleButtonsVM.ToggleButtonItems.FirstOrDefault(t => t.Key.Equals("splitequal"));
                GenericToggleButtonsVM.SelectedToggleButtonItem = selectedSplitAction;
                GenericToggleButtonsVM.SelectedToggleButtonItem.IsChecked = true;
                splitDailyLimitPercentage();
            }
            else
            {
                ChildCardsCollection.Add(new AccountRelationShipModel()
                {
                    AccountRelationshipId = -1,
                    DailyLimitPercentage = null,
                    ChildCardDetailsVM = new CardDetailsVM(this.ExecutionContext)
                    {
                        AccountDTO = childCardDTO
                    }
                });
                if (!selectedSplitAction.Key.Equals("splitequal"))
                {
                    selectedSplitAction = GenericToggleButtonsVM.ToggleButtonItems.FirstOrDefault(t => t.Key.Equals("clearsplit"));
                    ShowCustomNumericUpDown = Visibility.Collapsed;
                    GenericToggleButtonsVM.SelectedToggleButtonItem = selectedSplitAction;
                    GenericToggleButtonsVM.SelectedToggleButtonItem.IsChecked = true;
                }

                if (ChildCardsCollection != null && ChildCardsCollection.Count > 1)
                {
                    splitDailyLimitPercentage();
                }
            }
            TotalCardsToBeLinked = TotalCardsToBeLinked + 1;
            ShowDailyLimitText = true;
            if (linkCardsView != null && linkCardsView.scvChildControls != null)
            {
                linkCardsView.scvChildControls.ScrollToBottom();
            }

        }


        private void OnBackButtonClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            PerformClose(parameter);
            log.LogMethodExit();
        }
        private async Task<List<AccountRelationshipDTO>> GetParentChildCards(int isActive)
        {
            log.LogMethodEntry(isActive);
            List<AccountRelationshipDTO> accountRelationshipDTOList = null;
            if (parentDetailsVM.AccountDTO.AccountId > -1)
            {
                List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.ACCOUNT_ID, parentDetailsVM.AccountDTO.AccountId.ToString()));
                searchParameters.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                try
                {
                    IsLoadingVisible = true;
                    accountRelationshipDTOList = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).GetAccountRelationships(searchParameters);
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
            log.LogMethodExit(accountRelationshipDTOList);
            return accountRelationshipDTOList;
        }

        private void ResetValues()
        {
            log.LogMethodEntry();
            ShowDailyLimitText = false;
            if (ParentDetailsVM != null)
            {
                ParentDetailsVM.AccountDTO = null;
            }
            if (ChildCardsCollection != null)
            {
                ChildCardsCollection.Clear();
            }
            if(childCardDeletionlist != null)
            {
                childCardDeletionlist.Clear();
            }
            if (activeAccountRelationshipDTOList != null)
            {
                activeAccountRelationshipDTOList.Clear();
            }
            if(inactiveAccountRelationshipDTOList != null)
            {
                inactiveAccountRelationshipDTOList.Clear();
            }
            if(!string.IsNullOrEmpty(parentCard))
            {
                parentCard = string.Empty;
            }
            if(trxAccountRelationShipDTOList != null)
            {
                trxAccountRelationShipDTOList = null;
            }
            if(GenericToggleButtonsVM != null)
            {
                ObservableCollection<CustomToggleButtonItem> splitActionButtons = GetCustomToggleButtonItems();
                GenericToggleButtonsVM.ToggleButtonItems = splitActionButtons;
                GenericToggleButtonsVM.SelectedToggleButtonItem = GenericToggleButtonsVM.ToggleButtonItems[0];
                selectedSplitAction = GenericToggleButtonsVM.SelectedToggleButtonItem;
            }
            TappedAccountDTO = null;
            TotalCardsToBeLinked = 0;
            isParentCardTapped = false;
            log.LogMethodExit();
        }

        private void OnCardDetailsClearClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty, MessageType.None);
            if (parameter != null)
            {
                AccountRelationShipModel childCardDetails = parameter as AccountRelationShipModel;
                if (childCardDetails != null)
                {
                    AccountRelationShipModel childCard = ChildCardsCollection.First(x => x.Equals(childCardDetails));
                    ChildCardsCollection.Remove(childCard);
                    if(childCard.AccountRelationshipId > -1)
                    {
                        childCardDeletionlist.Add(childCard);
                    }
                    TotalCardsToBeLinked = TotalCardsToBeLinked - 1;

                    splitDailyLimitPercentage();
                }
            }
            log.LogMethodExit();
        }

        private void OnClearClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty, MessageType.None);
            ResetValues();
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
                        IsLoadingVisible = parentDetailsVM.IsLoadingVisible;
                    }
                    else
                    {
                        RaiseCanExecuteChanged();
                    }
                    break;
            }
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
            (EnterCardNoClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (CardDetailsClearCommand as DelegateCommand).RaiseCanExecuteChanged();
            (BackButtonCommand as DelegateCommand).RaiseCanExecuteChanged();
            (OkCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ClearCommand as DelegateCommand).RaiseCanExecuteChanged();
            log.LogMethodExit();
        }
        #endregion

    }
}

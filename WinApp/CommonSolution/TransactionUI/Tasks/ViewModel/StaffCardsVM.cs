/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - staff cards View model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-June-2021   Prashanth                integrated the usecases and checked for UI validation
 ********************************************************************************************/
using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.User;
using Semnox.Parafait.Product;
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.POS;


namespace Semnox.Parafait.TransactionUI
{
    public class StaffCardsVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum StaffCardSelectedType
        {
            New,
            All
        }
        private bool enableManualEntry;
        private bool showNoCardAddedContent;
        private int managerId;
        private string notes;
        private StaffCardSelectedType selectedType;
        public delegate void CardTapped();
        public event CardTapped CardTappedEvent;
        private DeviceClass cardReader;
        private UsersDTO tappedCardUser;
        private TransactionCardDetailsVM transactionCardDetailsVM;
        private List<ProductsContainerDTO> productCollection;
        private List<UsersDTO> userCollection;
        private ObservableCollection<UsersDTO> newSectionUserLOVCollection;
        private List<int> excludedUserRoleIds;
        private List<UserRoleContainerDTO> roleCollection;
        private ObservableCollection<UserRoleContainerDTO> newSectionRoleLOVCollection;
        private UsersDTO selectedUser;
        private UsersDTO newSectionSelectedUser;
        private UserRoleContainerDTO selectedRole;
        private UserRoleContainerDTO newSectionSelectedRole;
        private ProductsContainerDTO selectedProduct;
        private CustomDataGridVM customDataGridVM;
        private StaffCardsView staffCardsView;
        private bool isProductsGridEnabled;
        private bool isNotesEnabled;
        private bool isConfirmButtonEnabled;
        private ICommand deleteClickedCommand;
        private ICommand enterCardCommand;
        private ICommand resetCommand;
        private ICommand confirmCommand;
        private ICommand addCommand;
        private ICommand deactiveCommand;
        private ICommand productClickedCommand;
        private ICommand loadedCommand;
        private ICommand addCommandForNoCardAdded;
        private List<string> displayMemberPathCollection;
        private ObservableCollection<UserRoleContainerDTO> allSectionRoleLovCollection;
        private UserRoleContainerDTO allSectionSelectedRole;
        private ObservableCollection<UsersDTO> allSectionUserLovCollection;
        private UsersDTO allSectionSelectedUser;
        #endregion

        #region Properties
        public ObservableCollection<UserRoleContainerDTO> AllSectionRoleLovCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(allSectionRoleLovCollection);
                return allSectionRoleLovCollection;
            }
            set
            {
                log.LogMethodEntry(allSectionRoleLovCollection, value);
                SetProperty(ref allSectionRoleLovCollection, value);
                log.LogMethodExit(allSectionRoleLovCollection);
            }
        }
        public ObservableCollection<UsersDTO> AllSectionUserLovCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(allSectionUserLovCollection);
                return allSectionUserLovCollection;
            }
            set
            {
                log.LogMethodEntry(allSectionUserLovCollection, value);
                SetProperty(ref allSectionUserLovCollection, value);
                log.LogMethodExit(allSectionUserLovCollection);
            }
        }
        public UsersDTO AllSectionSelectedUser
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(allSectionSelectedUser);
                return allSectionSelectedUser;
            }
            set
            {
                log.LogMethodEntry(allSectionSelectedUser, value);
                SetProperty(ref allSectionSelectedUser, value);
                log.LogMethodExit(allSectionSelectedUser);
            }
        }

        public UserRoleContainerDTO AllSectionSelectedRole
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(allSectionSelectedRole);
                return allSectionSelectedRole;
            }
            set
            {
                log.LogMethodEntry(allSectionSelectedRole, value);
                SetProperty(ref allSectionSelectedRole, value);
                log.LogMethodExit(allSectionSelectedRole);
            }
        }

        public List<string> DisplayMemberPathCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayMemberPathCollection);
                return displayMemberPathCollection;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref displayMemberPathCollection, value);
                log.LogMethodExit();
            }
        }
        

        public bool IsConfirmButtonEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isConfirmButtonEnabled);
                return isConfirmButtonEnabled;
            }
            set
            {
                log.LogMethodEntry(isConfirmButtonEnabled, value);
                SetProperty(ref isConfirmButtonEnabled, value);
                log.LogMethodExit(isConfirmButtonEnabled);
            }
        }

        public bool IsProductsGridEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isProductsGridEnabled);
                return isProductsGridEnabled;
            }
            set
            {
                log.LogMethodEntry(isProductsGridEnabled, value);
                SetProperty(ref isProductsGridEnabled, value);
                log.LogMethodExit(isProductsGridEnabled);
            }
        }
        public bool IsNotesEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isNotesEnabled);
                return isNotesEnabled;
            }
            set
            {
                log.LogMethodEntry(isNotesEnabled, value);
                SetProperty(ref isNotesEnabled, value);
                log.LogMethodExit(isNotesEnabled);
            }
        }

        public string Notes
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(notes);
                return notes;
            }
            set
            {
                log.LogMethodEntry(notes, value);
                SetProperty(ref notes, value);
                log.LogMethodExit(notes);
            }
        }
        public ICommand AddCommandForNoCardAdded
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addCommandForNoCardAdded);
                return addCommandForNoCardAdded;
            }
            set
            {
                log.LogMethodEntry(addCommandForNoCardAdded, value);
                SetProperty(ref addCommandForNoCardAdded, value);
                log.LogMethodExit(addCommandForNoCardAdded);
            }
        }

        public DeviceClass CardReader
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardReader);
                return cardReader;
            }
            set
            {
                log.LogMethodEntry(cardReader, value);
                SetProperty(ref cardReader, value);
                log.LogMethodExit(cardReader);
            }
        }
        public TransactionCardDetailsVM TransactionCardDetailsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(transactionCardDetailsVM);
                if (transactionCardDetailsVM == null)
                {
                    transactionCardDetailsVM = new TransactionCardDetailsVM(this.ExecutionContext) { Expand = true };
                }
                return transactionCardDetailsVM;
            }
            set
            {
                log.LogMethodEntry(transactionCardDetailsVM, value);
                SetProperty(ref transactionCardDetailsVM, value);
                log.LogMethodExit(transactionCardDetailsVM);
            }
        }
        public UsersDTO NewSectionSelectedUser
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(newSectionSelectedUser);
                return newSectionSelectedUser;
            }
            set
            {
                log.LogMethodEntry(newSectionSelectedUser, value);
                SetProperty(ref newSectionSelectedUser, value);           
                log.LogMethodExit(newSectionSelectedUser);
            }
        }
        public ProductsContainerDTO SelectedProduct
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedProduct);
                return selectedProduct;
            }
            set
            {
                log.LogMethodEntry(selectedProduct, value);
                SetProperty(ref selectedProduct, value);
                log.LogMethodExit(selectedProduct);
            }
        }

        public UserRoleContainerDTO NewSectionSelectedRole
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(newSectionSelectedRole);
                return newSectionSelectedRole;
            }
            set
            {
                log.LogMethodEntry(newSectionSelectedRole, value);
                SetProperty(ref newSectionSelectedRole, value);              
                log.LogMethodExit(newSectionSelectedRole);
            }
        }
        public List<ProductsContainerDTO> ProductCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(productCollection);
                return productCollection;
            }
        }
        public List<UsersDTO> UserCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(userCollection);
                return userCollection;
            }
            set
            {
                log.LogMethodEntry(userCollection, value);
                SetProperty(ref userCollection, value);
                log.LogMethodExit(userCollection);
            }
        }
        public ObservableCollection<UsersDTO> NewSectionUserLovCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(newSectionUserLOVCollection);
                return newSectionUserLOVCollection;
            }
            set
            {
                log.LogMethodEntry(newSectionUserLOVCollection, value);
                SetProperty(ref newSectionUserLOVCollection, value);
                log.LogMethodExit(newSectionUserLOVCollection);
            }
        }
        public List<UserRoleContainerDTO> RoleCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(roleCollection);
                return roleCollection;
            }
        }
        public ObservableCollection<UserRoleContainerDTO> NewSectionRoleLovCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(newSectionRoleLOVCollection);
                return newSectionRoleLOVCollection;
            }
            set
            {
                log.LogMethodEntry(newSectionRoleLOVCollection, value);
                SetProperty(ref newSectionRoleLOVCollection, value);
                log.LogMethodExit(newSectionRoleLOVCollection);
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
        public StaffCardSelectedType SelectedType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedType);
                return selectedType;
            }
            set
            {
                log.LogMethodEntry(selectedType, value);
                SetProperty(ref selectedType, value);
                log.LogMethodExit(selectedType);
            }
        }
        public bool ShowNoCardAddedContent
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showNoCardAddedContent);
                return showNoCardAddedContent;
            }
            set
            {
                log.LogMethodEntry(showNoCardAddedContent, value);
                SetProperty(ref showNoCardAddedContent, value);
                log.LogMethodExit(showNoCardAddedContent);
            }
        }
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
        public ICommand EnterCardCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enterCardCommand);
                return enterCardCommand;
            }
            set
            {
                log.LogMethodEntry(enterCardCommand, value);
                SetProperty(ref enterCardCommand, value);
                log.LogMethodExit();
            }
        }
        public ICommand ResetCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(resetCommand);
                return resetCommand;
            }
            set
            {
                log.LogMethodEntry(resetCommand, value);
                SetProperty(ref resetCommand, value);
                log.LogMethodExit(resetCommand);
            }
        }
        public ICommand ConfirmCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(confirmCommand);
                return confirmCommand;
            }
            set
            {
                log.LogMethodEntry(confirmCommand, value);
                SetProperty(ref confirmCommand, value);
                log.LogMethodExit(confirmCommand);
            }
        }
        public ICommand AddCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addCommand);
                return addCommand;
            }
            set
            {
                log.LogMethodEntry(addCommand, value);
                SetProperty(ref addCommand, value);
                log.LogMethodExit(addCommand);
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
        public ICommand ProductClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(productClickedCommand);
                return productClickedCommand;
            }
            set
            {
                log.LogMethodEntry(productClickedCommand, value);
                SetProperty(ref productClickedCommand, value);
                log.LogMethodExit(productClickedCommand);
            }
        }
        public ICommand DeactiveCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(deactiveCommand);
                return deactiveCommand;
            }
            set
            {
                log.LogMethodEntry(deactiveCommand, value);
                SetProperty(ref deactiveCommand, value);
                log.LogMethodExit(deactiveCommand);
            }
        }
        public ICommand DeleteClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(deleteClickedCommand);
                return deleteClickedCommand;
            }
            set
            {
                log.LogMethodEntry(deleteClickedCommand, value);
                SetProperty(ref deleteClickedCommand, value);
                log.LogMethodExit(deleteClickedCommand);
            }
        }
        #endregion

        #region Methods

        private void OnAddCommandForNoCardAdded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (selectedType != StaffCardSelectedType.New)
            {
                LeftPaneVM.SelectedMenuItem = LeftPaneVM.MenuItems[0];
                if (CustomDataGridVM != null)
                {
                    UsersDTO usersDTO = CustomDataGridVM.SelectedItem as UsersDTO;
                    selectedUser = usersDTO;
                    NewSectionSelectedUser = usersDTO;
                }
                IsNotesEnabled = false;
                IsProductsGridEnabled = true;
            }
            log.LogMethodExit();
        }
        internal void UnRegisterReader()
        {
            log.LogMethodEntry();
            if (cardReader != null)
            {
                log.Debug("Card Reader: " + cardReader);
                cardReader.UnRegister();
            }
            log.LogMethodExit();
        }
        internal void RegisterReader()
        {
            log.LogMethodEntry();
            if (this.cardReader != null)
            {
                log.Debug("Card Reader: " + cardReader);
                cardReader.Register(new EventHandler(CardScanCompleteEventHandle));
            }
            log.LogMethodExit();
        }
        private async void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                DeviceClass deviceClass = sender as DeviceClass;
                log.Debug("Scanned Number: " + checkScannedEvent.Message);
                TagNumberViewParser tagNumberViewParser = new TagNumberViewParser(ExecutionContext);
                TagNumberView tagNumberView;
                if (tagNumberViewParser.TryParse(checkScannedEvent.Message, out tagNumberView) == false)
                {
                    string errorMessage = tagNumberViewParser.Validate(checkScannedEvent.Message);
                    SetFooterContent(errorMessage, MessageType.Warning);
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }
                try
                {
                    SetAccountsDTO(tagNumberView.Value, deviceClass.TagSiteId);
                    if (CardTappedEvent != null)
                    {
                        CardTappedEvent.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    SetFooterContent(ex.Message, MessageType.Error);
                }
            }
            log.LogMethodExit();
        }
        private void OnLeftPaneMenuSelected(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty, MessageType.None);
            if (MessageViewContainerList.GetMessage(this.ExecutionContext, "New").ToLower() == LeftPaneVM.SelectedMenuItem.ToLower())
            {
                Clear();
                SelectedType = StaffCardSelectedType.New;
            }
            else if ((MessageViewContainerList.GetMessage(this.ExecutionContext, "All") + "(" + UserCollection.Count.ToString() + ")").ToLower()
                == LeftPaneVM.SelectedMenuItem.ToLower())
            {
                SelectedType = StaffCardSelectedType.All;
                RefreshAllSection();
                if (tappedCardUser != null)
                {
                    AllSectionSelectedUser = AllSectionUserLovCollection.FirstOrDefault(u => u.UserId == tappedCardUser.UserId);
                    tappedCardUser = null;
                }
                else
                {
                    TransactionCardDetailsVM.AccountDTO = null;
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4673), MessageType.Info);
                    CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(new List<UsersDTO>());
                    CustomDataGridVM.SelectedItem = null;
                }
            }
            log.LogMethodExit();
        }
        public void PerformClose(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                Window window = parameter as Window;
                if (window != null)
                {
                    window.Close();
                }
                UnRegisterReader();
            }
            log.LogMethodExit();
        }
        private void OnLeftPaneNavigationClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            UserViewContainerList.Rebuild(ExecutionContext.GetSiteId());
            PerformClose(parameter);
            log.LogMethodExit();
        }
        private void OnSelectionChanged(object parameter)
        {
            log.LogMethodExit(parameter);
            if (customDataGridVM != null && customDataGridVM.SelectedItem != null)
            {
                UserIdentificationTagsDTO userIdentification = (customDataGridVM.SelectedItem as UsersDTO).UserIdentificationTagsDTOList.FirstOrDefault();
                if (userIdentification != null)
                {
                    ShowNoCardAddedContent = false;
                    SetAccountsDTO(userIdentification.CardNumber, ExecutionContext.GetSiteId());
                    selectedUser = (customDataGridVM.SelectedItem as UsersDTO);
                }
                else
                {
                    ShowNoCardAddedContent = true;
                    TransactionCardDetailsVM.AccountDTO = null;
                    selectedUser = (customDataGridVM.SelectedItem as UsersDTO);
                }
            }
            else
            {
                ShowNoCardAddedContent = false;
                TransactionCardDetailsVM.AccountDTO = null;
            }
            log.LogMethodExit();
        }
        private void OnSearchClicked(object parameter)
        {
            log.LogMethodExit(parameter);
            ContextSearchView contextSearchView = new ContextSearchView();
            ContextSearchVM contextSearchVM = new ContextSearchVM(ExecutionContext);
            contextSearchVM.Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Search", null);
            contextSearchVM.SearchIndexes = new ObservableCollection<int>() { 0, 1, 2, 3};
            List<UsersDTO> filteredUsers = customDataGridVM.UICollectionToBeRendered.Cast<UsersDTO>().ToList();
            CardConverter cardConverter = new CardConverter();
            contextSearchVM.SearchParams = new ObservableCollection<DisplayParameters>(filteredUsers.Select(m => new DisplayParameters()
            {
                Id = m.UserId,
                ParameterNames = new ObservableCollection<string> {(string)cardConverter.Convert(m.UserIdentificationTagsDTOList,null,userCollection,null), m.UserName, m.EmpLastName, m.LoginId}
            }).ToList());
            contextSearchView.DataContext = contextSearchVM;
            contextSearchView.Owner = staffCardsView;
            contextSearchView.ShowDialog();
            SetFooterContent(String.Empty, MessageType.None);
            if (contextSearchView != null && contextSearchView.SelectedId != -1 && filteredUsers != null)
            {
                CustomDataGridVM.SelectedItem = filteredUsers.FirstOrDefault(x => x.UserId == contextSearchView.SelectedId);
            }
            log.LogMethodExit();
        }
        private void OnAddClicked(object parameter)
        {
            log.LogMethodExit(parameter);
            if (selectedType != StaffCardSelectedType.New)
            {
                AccountDTO accountDTO = transactionCardDetailsVM.AccountDTO;
                LeftPaneVM.SelectedMenuItem = LeftPaneVM.MenuItems[0];
                transactionCardDetailsVM.AccountDTO = accountDTO;
                selectedUser = CustomDataGridVM.SelectedItem as UsersDTO;
                NewSectionSelectedUser = selectedUser;
                IsNotesEnabled = true;
                IsProductsGridEnabled = true;
            }
            log.LogMethodExit();
        }

        private async void OnDeactiveClicked(object parameter)
        {
            log.LogMethodExit(parameter);
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            GenericMessagePopupVM genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
            {
                OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "CONFIRM", null),
                CancelButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                MessageButtonsType = MessageButtonsType.OkCancel,
                SubHeading = null,
                Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "WARNING", null),
                Content = MessageViewContainerList.GetMessage(this.ExecutionContext, 1159)
            };
            messagePopupView.DataContext = genericMessagePopupVM;
            messagePopupView.Owner = staffCardsView;
            messagePopupView.ShowDialog();
            if (genericMessagePopupVM.ButtonClickType.Equals(ButtonClickType.Cancel))
            {
                return;
            }
            try
            {
                IsLoadingVisible = true;
                List<StaffCardDTO> staffCardDTOs = new List<StaffCardDTO>();
                StaffCardDTO staffCardDTO = new StaffCardDTO(selectedUser.UserId, -1, managerId, -1, TransactionCardDetailsVM.AccountDTO.TagNumber, Notes);
                staffCardDTOs.Add(staffCardDTO);
                await TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext).DeactivateStaffCard(staffCardDTOs);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, ex.Message), MessageType.Error);
                return;
            }
            finally
            {
                IsLoadingVisible = false;
            }
            List<UsersDTO> usersDTOs = await BuildUserCollection();
            UsersDTO updatedUserDTO = null;
            if (usersDTOs != null && usersDTOs.Any())
            {
                updatedUserDTO = usersDTOs[0];
                int index = UserCollection.IndexOf(selectedUser);
                UserCollection.Remove(selectedUser);
                UserCollection.Insert(index, updatedUserDTO);
            }
            SetDisplayTags();
            UpdateCustomDataGridForSelectedUser(updatedUserDTO);
            CustomDataGridVM.SelectedItem = updatedUserDTO;
            TransactionCardDetailsVM.AccountDTO = null;
            IsLoadingVisible = false;
            log.LogMethodExit();
        }

        internal void UpdateCustomDataGridForSelectedUser(UsersDTO usersDTO)
        {
            int index = CustomDataGridVM.CollectionToBeRendered.IndexOf(selectedUser);
            if (index > -1)
            {
                if (usersDTO != null)
                {
                    CustomDataGridVM.CollectionToBeRendered.Remove(selectedUser);
                    CustomDataGridVM.UICollectionToBeRendered.Remove(selectedUser);
                    CustomDataGridVM.CollectionToBeRendered.Insert(index, usersDTO);
                    CustomDataGridVM.UICollectionToBeRendered.Insert(index, usersDTO);
                }
            }
        }
        private void OnResetClicked(object parameter)
        {
            log.LogMethodExit(parameter);
            Clear();
            log.LogMethodExit();
        }

        private void Clear()
        {
            TransactionCardDetailsVM.AccountDTO = null;
            if (staffCardsView.cmbUsers.CustomDataGridVM != null)
            {
                staffCardsView.cmbUsers.CustomDataGridVM.SelectedItem = null;
                staffCardsView.cmbUsers.SelectedItem = null;
            }
            if (staffCardsView.cmbAllSectionUsers.CustomDataGridVM != null)
            {
                staffCardsView.cmbAllSectionUsers.CustomDataGridVM.SelectedItem = null;
                staffCardsView.cmbAllSectionUsers.SelectedItem = null;
            }
            NewSectionUserLovCollection = new ObservableCollection<UsersDTO>(UserCollection);
            NewSectionSelectedUser = null;
            NewSectionSelectedRole = null;
            AllSectionSelectedUser = null;
            AllSectionSelectedRole = null;
            selectedUser = null;
            selectedRole = null;
            SelectedProduct = null;
            IsProductsGridEnabled = true;
            staffCardsView.lstViewProduct.SelectedItem = null;
            Notes = String.Empty;
            IsNotesEnabled = false;
            SetFooterContent("", MessageType.None);
        }
        private async void OnConfirmClicked(object parameter)
        {
            log.LogMethodExit(parameter);
            if (string.IsNullOrEmpty(TransactionCardDetailsVM.AccountDTO.TagNumber))
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 257), MessageType.Error);
                return;
            }
            if (selectedUser == null)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1162), MessageType.Error);
                return;
            }

            if (selectedUser != null)
            {
                foreach (UsersDTO usersDTO in UserCollection)
                {
                    UserIdentificationTagsDTO userIdentificationTagsDTO = usersDTO.UserIdentificationTagsDTOList.Find(x => x.CardNumber == TransactionCardDetailsVM.AccountDTO.TagNumber);
                    if (userIdentificationTagsDTO != null)
                    {
                        if (usersDTO.UserId != selectedUser.UserId)
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1163), MessageType.Warning);
                            return;
                        }
                        break;
                    }
                }
                List<StaffCardDTO> result = null;
                List<StaffCardDTO> staffCardDTOs = new List<StaffCardDTO>();
                int productId = -1;
                if (selectedProduct != null && selectedProduct.ProductId > -1)
                {
                    bool canAddProduct = CheckStaffCreditsLimit(selectedProduct);
                    if (canAddProduct)
                    {
                        productId = selectedProduct.ProductId;
                    }
                    else
                    {
                        return;
                    }
                    
                }
                StaffCardDTO staffCardDTO = new StaffCardDTO(selectedUser.UserId, -1, managerId, productId, TransactionCardDetailsVM.AccountDTO.TagNumber, Notes);
                staffCardDTOs.Add(staffCardDTO);
                try
                {
                    IsLoadingVisible = true;
                    result = await TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext).CreateStaffCard(staffCardDTOs);
                }
                catch (ValidationException vex)
                {
                    log.Error(vex);
                    IsLoadingVisible = false;
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, vex.Message), MessageType.Error);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    IsLoadingVisible = false;
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, ex.Message), MessageType.Error);
                }
                finally
                {
                    IsLoadingVisible = false;
                }
                if (result != null && result.Any())
                {
                    if (selectedProduct == null && selectedUser.UserIdentificationTagsDTOList.Count == 0 || (selectedProduct != null && selectedUser.UserIdentificationTagsDTOList.Count == 0))
                    {
                        List<UsersDTO> usersDTOs = await BuildUserCollection();
                        UsersDTO updatedUserDTO = null;
                        if (usersDTOs != null && usersDTOs.Any())
                        {
                            updatedUserDTO = usersDTOs[0];
                            int index = UserCollection.IndexOf(selectedUser);
                            UserCollection.Remove(selectedUser);
                            UserCollection.Insert(index, updatedUserDTO);
                            tappedCardUser = updatedUserDTO;
                        }
                    }
                    else
                    {
                        tappedCardUser = selectedUser;
                    }

                    SetDisplayTags();
                    IsLoadingVisible = false;
                    LeftPaneVM.SelectedMenuItem = LeftPaneVM.MenuItems[1];
                    TransactionCardDetailsVM.IsDeleteButtonVisible = false;
                    TransactionCardDetailsVM.Expand = true;
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 122), MessageType.Info);
                }
            }
            log.LogMethodExit();
        }

        private async Task<List<UsersDTO>> BuildUserCollection()
        {
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            searchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID, selectedUser.UserId.ToString()));
            List<UsersDTO> result = null;
            try
            {
                IsLoadingVisible = true;
                result = await UserUseCaseFactory.GetUserUseCases(ExecutionContext).GetUserDTOList(searchParams, loadChildRecords: true);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            finally
            {
                IsLoadingVisible = false;
            }
            return result;
        }
        private void OnEnterCardClicked(object parameter)
        {
            log.LogMethodExit(parameter);
            SetFooterContent("", MessageType.None);
            GenericDataEntryView dataEntryView = new GenericDataEntryView();
            GenericDataEntryVM dataEntryVM = new GenericDataEntryVM(ExecutionContext)
            {
                Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Enter Card No"),
                DataEntryCollections = new ObservableCollection<DataEntryElement>()
                {
                    new DataEntryElement()
                    {
                        Heading =MessageViewContainerList.GetMessage(ExecutionContext,"Card No"),
                        Type = DataEntryType.TextBox,
                        DefaultValue = MessageViewContainerList.GetMessage(ExecutionContext,"Enter Card No."),
                        IsMandatory = true
                    }
                }
            };
            dataEntryView.Width = SystemParameters.PrimaryScreenWidth;
            dataEntryView.Height = SystemParameters.PrimaryScreenHeight;
            dataEntryView.Owner = staffCardsView;
            dataEntryView.DataContext = dataEntryVM;
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
                SetAccountsDTO(cardNumber, ExecutionContext.GetSiteId());
            }
            log.LogMethodExit();
        }

        private async void SetAccountsDTO(string cardNumber = null, int cardSiteId = -1)
        {
            log.LogMethodEntry(cardNumber, cardSiteId);
            SetFooterContent(string.Empty, MessageType.None);
            if (cardNumber != null)
            {
                AccountDTOCollection accountDTOCollection = null;
                try
                {
                    IsLoadingVisible = true;
                    if (cardSiteId > -1)
                    {
                        accountDTOCollection = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).GetAccounts(accountNumber: cardNumber, tagSiteId: cardSiteId);
                    }
                    else
                    {
                        accountDTOCollection = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).GetAccounts(accountNumber: cardNumber, tagSiteId: ExecutionContext.GetSiteId());
                    }
                }
                catch (Exception ex)
                {
                    IsLoadingVisible = false;
                    SetFooterContent(ex.Message, MessageType.Error);
                    return;
                }
                finally
                {
                    IsLoadingVisible = false;
                }
                if (accountDTOCollection != null && accountDTOCollection.data != null)
                {
                    if (accountDTOCollection.data[0].AccountId > -1)
                    {
                        if (accountDTOCollection.data[0].TechnicianCard != "Y")
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Not a Staff Card"), MessageType.Warning);
                            return;
                        }
                        TransactionCardDetailsVM.AccountDTO = accountDTOCollection.data[0];
                        TransactionCardDetailsVM.Expand = true;
                        transactionCardDetailsVM.IsDeleteButtonVisible = false;
                        IsNotesEnabled = true;
                        if (selectedType != StaffCardSelectedType.All)
                        {
                            foreach (UsersDTO usersDTO in userCollection)
                            {
                                UserIdentificationTagsDTO userIdentification = usersDTO.UserIdentificationTagsDTOList.FirstOrDefault(s => s.CardNumber == TransactionCardDetailsVM.AccountDTO.TagNumber);
                                if (userIdentification != null)
                                {
                                    transactionCardDetailsVM.IsDeleteButtonVisible = false;
                                    tappedCardUser = usersDTO;
                                    LeftPaneVM.SelectedMenuItem = LeftPaneVM.MenuItems[1];

                                    break;
                                }
                            }
                        }

                    }
                    else
                    {
                        if (selectedUser != null)
                        {
                            if (!selectedUser.UserIdentificationTagsDTOList.Any())
                            {
                                transactionCardDetailsVM.AccountDTO = accountDTOCollection.data[0];
                                transactionCardDetailsVM.IsDeleteButtonVisible = true;
                            }
                            else
                            {
                                transactionCardDetailsVM.IsDeleteButtonVisible = false;
                            }
                        }
                        else
                        {
                            transactionCardDetailsVM.IsDeleteButtonVisible = true;
                        }
                        IsNotesEnabled = true;

                        if (selectedType != StaffCardSelectedType.New)
                        {
                            LeftPaneVM.SelectedMenuItem = LeftPaneVM.MenuItems[0];
                        }
                        else
                        {
                            if (selectedUser != null)
                            {
                                if (selectedUser.UserIdentificationTagsDTOList != null && selectedUser.UserIdentificationTagsDTOList.Any() && accountDTOCollection.data[0] != null && !selectedUser.UserIdentificationTagsDTOList.Exists(x => x.CardNumber == accountDTOCollection.data[0].TagNumber))
                                {
                                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1160), MessageType.Warning);
                                }
                            }
                        }

                        return;
                    }
                }

            }
            log.LogMethodExit();
        }

        private void RefreshAllSection()
        {
            log.LogMethodEntry();
            CustomDataGridVM.SelectedItem = null;
            AllSectionUserLovCollection = new ObservableCollection<UsersDTO>(UserCollection);
            AllSectionRoleLovCollection = new ObservableCollection<UserRoleContainerDTO>(roleCollection.Where(r => r.RoleId != -1));
            AllSectionSelectedUser = null;
            AllSectionSelectedRole = null;
            log.LogMethodExit();
        }


        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetFooterContent(string.Empty, MessageType.None);
            if (!string.IsNullOrWhiteSpace(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case "NewSectionSelectedRole":
                        if (NewSectionSelectedRole != null)
                        {
                            IsConfirmButtonEnabled = false;
                            selectedRole = NewSectionSelectedRole;
                            if (NewSectionSelectedRole.RoleId == -1)
                            {
                                SetFooterContent(string.Empty, MessageType.None);
                                TransactionCardDetailsVM.AccountDTO = null;
                                NewSectionUserLovCollection = new ObservableCollection<UsersDTO>(UserCollection);
                                if (staffCardsView.cmbUsers.CustomDataGridVM != null)
                                {
                                    staffCardsView.cmbUsers.CustomDataGridVM.SelectedItem = null;
                                    staffCardsView.cmbUsers.SelectedItem = null;
                                }
                                selectedUser = null;
                            }
                            else
                            {
                                if (NewSectionSelectedUser != null)
                                {
                                    UsersDTO currentSelectedUser = NewSectionSelectedUser;
                                    if (NewSectionSelectedUser.RoleId != NewSectionSelectedRole.RoleId)
                                    {
                                        SetFooterContent(string.Empty, MessageType.None);
                                        TransactionCardDetailsVM.AccountDTO = null;
                                        NewSectionUserLovCollection = new ObservableCollection<UsersDTO>(UserCollection.Where(user => user.RoleId == NewSectionSelectedRole.RoleId));
                                        selectedUser = null;
                                    }
                                    else
                                    {
                                        NewSectionUserLovCollection = new ObservableCollection<UsersDTO>(UserCollection.Where(user => user.RoleId == NewSectionSelectedRole.RoleId));
                                        NewSectionSelectedUser = NewSectionUserLovCollection.FirstOrDefault(user => user.UserId == currentSelectedUser.UserId);
                                        selectedUser = NewSectionSelectedUser;
                                    }
                                }
                                else
                                {
                                    SetFooterContent(string.Empty, MessageType.None);
                                    TransactionCardDetailsVM.AccountDTO = null;
                                    NewSectionUserLovCollection = new ObservableCollection<UsersDTO>(UserCollection.Where(user => user.RoleId == NewSectionSelectedRole.RoleId));
                                    selectedUser = null;
                                }
                            }
                        }
                        else
                        {
                            selectedRole = null;
                        }
                        break;
                    case "NewSectionSelectedUser":
                        if (NewSectionSelectedUser != null)
                        {
                            selectedUser = NewSectionSelectedUser;
                            if (NewSectionSelectedRole == null || (NewSectionSelectedRole != null && NewSectionSelectedRole.RoleId != NewSectionSelectedUser.RoleId))
                            {
                                NewSectionSelectedRole = NewSectionRoleLovCollection.FirstOrDefault(role => role.RoleId == NewSectionSelectedUser.RoleId);
                                selectedRole = NewSectionSelectedRole;
                            }
                            if (selectedUser.UserIdentificationTagsDTOList.Any())
                            {
                                UpdateRightSectionDetails();
                                if (SelectedProduct == null)
                                {
                                    IsConfirmButtonEnabled = false;
                                }
                                else
                                {
                                    IsConfirmButtonEnabled = true;
                                }
                            }
                            else
                            {
                                TransactionCardDetailsVM.AccountDTO = null;
                                IsConfirmButtonEnabled = true;
                            }
                        }
                        else
                        {
                            selectedUser = null;
                            if (staffCardsView.cmbUsers.CustomDataGridVM != null)
                            {
                                staffCardsView.cmbUsers.CustomDataGridVM.SelectedItem = null;
                                staffCardsView.cmbUsers.SelectedItem = null;
                            }
                        }
                        break;
                    case "IsLoadingVisible":
                        RaiseCanExecuteChanged();
                        break;
                    case "AllSectionSelectedRole":
                        if (AllSectionSelectedRole != null)
                        {
                            if (staffCardsView.cmbAllSectionUsers.CustomDataGridVM != null)
                            {
                                staffCardsView.cmbAllSectionUsers.CustomDataGridVM.SelectedItem = null;
                                staffCardsView.cmbAllSectionUsers.SelectedItem = null;
                            }
                            AllSectionUserLovCollection = new ObservableCollection<UsersDTO>(UserCollection.Where(u => u.RoleId == AllSectionSelectedRole.RoleId));
                            UpdateCustomDataGrid();
                        }
                        break;
                    case "AllSectionSelectedUser":
                        if (AllSectionSelectedUser != null)
                        {
                            UpdateCustomDataGrid();
                        }
                        else
                        {
                            if (staffCardsView.cmbAllSectionUsers.CustomDataGridVM != null)
                            {
                                staffCardsView.cmbAllSectionUsers.CustomDataGridVM.SelectedItem = null;
                                staffCardsView.cmbAllSectionUsers.SelectedItem = null;
                            }
                        }
                        break;
                }
            }
        }

        private async void UpdateRightSectionDetails()
        {
            SetFooterContent(string.Empty, MessageType.None);
            if (selectedUser != null)
            {
                if (selectedUser.UserIdentificationTagsDTOList.Any())
                {
                    AccountDTOCollection accountDTOCollection = null;
                    try
                    {
                        IsLoadingVisible = true;
                        accountDTOCollection = await AccountUseCaseFactory.GetAccountUseCases(ExecutionContext).
                            GetAccounts(accountNumber: selectedUser.UserIdentificationTagsDTOList.FirstOrDefault().CardNumber,
                            tagSiteId: ExecutionContext.GetSiteId());

                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        IsLoadingVisible = false;
                        SetFooterContent(ex.Message, MessageType.Error);
                        return;
                    }
                    finally
                    {
                        IsLoadingVisible = false;
                    }
                    if (accountDTOCollection != null && accountDTOCollection.data[0] != null)
                    {
                        TransactionCardDetailsVM.AccountDTO = accountDTOCollection.data[0];
                        TransactionCardDetailsVM.Expand = true;
                        TransactionCardDetailsVM.IsDeleteButtonVisible = false;
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5120), MessageType.Info);
                    }
                }
            }
        }

        private void UpdateCustomDataGrid()
        {
            List<UsersDTO> filteredUsers = new List<UsersDTO>();
            if (AllSectionSelectedRole != null && AllSectionSelectedUser != null)
            {
                filteredUsers = userCollection.Where(user => user.RoleId == AllSectionSelectedRole.RoleId && user.UserId == AllSectionSelectedUser.UserId).ToList();
            }
            else if (AllSectionSelectedRole != null && AllSectionSelectedUser == null)
            {
                filteredUsers = userCollection.Where(user => user.RoleId == AllSectionSelectedRole.RoleId).ToList();
            }
            else if (AllSectionSelectedRole == null && AllSectionSelectedUser != null)
            {
                filteredUsers = userCollection.Where(user => user.UserId == AllSectionSelectedUser.UserId).ToList();
            }
            CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(filteredUsers);
            CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                {
                    { "UserName", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"USER")} },
                    { "EmpLastName", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"LAST NAME")} },
                    { "LoginId", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"LOGIN ID")} },
                    { "RoleId", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"ROLE"), Converter = new RoleConverter(), ConverterParameter = roleCollection } },
                    { "UserIdentificationTagsDTOList", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext, "CARD NO"), Converter = new CardConverter() } },
                    { "UserId", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext, "ISSUED"), Converter = new IssuedDateConverter(),
                        ConverterParameter = new List<object> { ExecutionContext, userCollection } } },
                };
        }

        private void SetDefaultViewModels()
        {
            log.LogMethodEntry();
            if (productCollection == null)
            {
                productCollection = new List<ProductsContainerDTO>();
            }
            if (userCollection == null)
            {
                userCollection = new List<UsersDTO>();
            }
            if (roleCollection == null)
            {
                roleCollection = new List<UserRoleContainerDTO>();
            }
            LeftPaneVM.ModuleName = MessageViewContainerList.GetMessage(this.ExecutionContext, "Staff Card");
            LeftPaneVM.SearchVisibility = Visibility.Collapsed;
            FooterVM = new FooterVM(this.ExecutionContext)
            {
                Message = string.Empty,
                MessageType = MessageType.None
            };
            log.LogMethodExit();
        }

        private void SetDisplayTags()
        {
            int totalStaffCardsCount = 0;
            foreach (UsersDTO usersDTO in userCollection)
            {
                totalStaffCardsCount += usersDTO.UserIdentificationTagsDTOList.Count;
            }
            DisplayTagsVM.DisplayTags = new ObservableCollection<ObservableCollection<DisplayTag>>()
            {
                new ObservableCollection<DisplayTag>()
                {
                    new DisplayTag()
                    {
                        Text = MessageViewContainerList.GetMessage(ExecutionContext, "Total Staff count")
                    },
                    new DisplayTag()
                    {
                        Text = userCollection.Count.ToString(),
                        FontWeight = FontWeights.Bold,
                        TextSize = TextSize.Medium
                    }
                },

                new ObservableCollection<DisplayTag>()
                {
                    new DisplayTag()
                    {
                        Text = MessageViewContainerList.GetMessage(ExecutionContext, "Total Staff cards count")
                    },
                    new DisplayTag()
                    {
                        Text = totalStaffCardsCount.ToString(),
                        FontWeight = FontWeights.Bold,
                        TextSize = TextSize.Medium
                    }
                }
            };
        }
        private void OnProductClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                ListViewItem listViewItem = parameter as ListViewItem;
                if (listViewItem != null && listViewItem.DataContext != null && listViewItem.IsSelected)
                {
                    if (SelectedProduct != null)
                    {
                        if ((listViewItem.DataContext as ProductsContainerDTO).Equals(SelectedProduct))
                        {
                            listViewItem.IsSelected = false;
                            SelectedProduct = null;
                            IsConfirmButtonEnabled = false;
                            staffCardsView.lstViewProduct.SelectedItem = null;
                            staffCardsView.lstViewProduct.SelectedItems.Clear();
                            if (selectedUser != null && selectedUser.UserIdentificationTagsDTOList != null && selectedUser.UserIdentificationTagsDTOList.Any())
                            {
                                IsConfirmButtonEnabled = false;
                            }
                            else if (selectedUser != null && selectedUser.UserIdentificationTagsDTOList != null && selectedUser.UserIdentificationTagsDTOList.Count == 0)
                            {
                                IsConfirmButtonEnabled = true;
                            }

                        }
                        else
                        {
                            SelectedProduct = (listViewItem.DataContext as ProductsContainerDTO);
                            IsConfirmButtonEnabled = true;
                        }
                    }
                    else
                    {
                        SelectedProduct = (listViewItem.DataContext as ProductsContainerDTO);
                        IsConfirmButtonEnabled = true;
                    }
                }
                else
                {
                    if ((selectedUser != null && selectedUser.UserIdentificationTagsDTOList != null && selectedUser.UserIdentificationTagsDTOList.Any() && SelectedProduct != null) ||
                        (selectedUser != null && selectedUser.UserIdentificationTagsDTOList != null && selectedUser.UserIdentificationTagsDTOList.Count == 0))
                    {
                        IsConfirmButtonEnabled = true;
                    }
                    else
                    {
                        IsConfirmButtonEnabled = false;
                    }
                }
            }
            log.LogMethodExit();
        }

        private bool CheckStaffCreditsLimit(ProductsContainerDTO product)
        {
            log.LogMethodEntry(product);
            bool canAddProduct = true;              
            if (product != null)
            {                
                double staffCreditLmt = 0;
                int staffGameLimit = 200;
                int timeLimit = 30;
                string staffCardCreditsLimitConfig = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "STAFF_CARD_CREDITS_LIMIT");
                if (!string.IsNullOrWhiteSpace(staffCardCreditsLimitConfig))
                {
                    try
                    {
                        staffCreditLmt = Convert.ToDouble(staffCardCreditsLimitConfig);
                    }
                    catch
                    {
                        staffCreditLmt = 0;
                    }                   
                }
                string staffCardGameLimitConfig = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "STAFF_CARD_GAME_LIMIT");
                if (!string.IsNullOrWhiteSpace(staffCardGameLimitConfig))
                {
                    try
                    {
                        staffGameLimit = Convert.ToInt32(staffCardGameLimitConfig);
                    }
                    catch
                    {
                        staffGameLimit = 200;
                    }                   
                }
                string staffCardTimeLimitConfig = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "STAFF_CARD_TIME_LIMIT");
                if (!string.IsNullOrWhiteSpace(staffCardTimeLimitConfig))
                {
                    try
                    {
                        timeLimit = Convert.ToInt32(staffCardTimeLimitConfig);
                    }
                    catch
                    {
                        timeLimit = 30;
                    }                    
                }
                
                
                if (staffCreditLmt > 0)
                {
                    double productCredits = 0;
                    double totalCredits = 0;
                    if (TransactionCardDetailsVM.AccountDTO != null && TransactionCardDetailsVM.AccountDTO.AccountSummaryDTO != null)
                    {         
                        totalCredits = Convert.ToDouble(TransactionCardDetailsVM.AccountDTO.AccountSummaryDTO.TotalGamePlayCreditsBalance);
                    }
                    productCredits = Convert.ToDouble(product.Credits);
                    if (product.ProductCreditPlusContainerDTOList != null)
                    {
                        List<ProductCreditPlusContainerDTO> filteredList = product.ProductCreditPlusContainerDTOList.Where(cp => cp.CreditPlusType.Equals("A") || cp.CreditPlusType.Equals("G")).ToList();
                        if (filteredList != null && filteredList.Count > 0)
                        {
                            productCredits += Convert.ToDouble(filteredList.Sum(x => x.CreditPlus));
                        }
                    }
                    if (totalCredits != 0 || productCredits != 0)
                    {
                        if ((totalCredits +  productCredits) > staffCreditLmt)
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1164), MessageType.Error);
                            canAddProduct = false;
                            log.LogMethodExit(canAddProduct);
                            return canAddProduct;
                        }
                    }
                }
                if (timeLimit > 0)
                {
                    double productTime = 0;
                    double cardTime = 0;
                    if (TransactionCardDetailsVM.AccountDTO != null && TransactionCardDetailsVM.AccountDTO.AccountSummaryDTO != null)
                    {
                        cardTime = Convert.ToDouble(TransactionCardDetailsVM.AccountDTO.AccountSummaryDTO.TotalTimeBalance);      
                    }
                    if (product.ProductCreditPlusContainerDTOList != null)
                    {
                        List<ProductCreditPlusContainerDTO> filteredList = product.ProductCreditPlusContainerDTOList.Where(cp => cp.CreditPlusType.Equals("M")).ToList();
                        if (filteredList != null && filteredList.Count > 0)
                        {
                            productTime = Convert.ToDouble(filteredList.Sum(x => x.CreditPlus));
                        }
                    }
                    if (productTime != 0)
                    {
                        if ((cardTime + productTime) > Convert.ToDouble(timeLimit))
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1385), MessageType.Error);
                            canAddProduct = false;
                            log.LogMethodExit(canAddProduct);
                            return canAddProduct;
                        }
                    }
                }
                if (staffGameLimit > 0)
                {
                    int productGame = 0;
                    int cardGame = 0;
                    if (TransactionCardDetailsVM.AccountDTO != null && TransactionCardDetailsVM.AccountDTO.AccountSummaryDTO != null)
                    {
                        cardGame = Convert.ToInt32(TransactionCardDetailsVM.AccountDTO.AccountSummaryDTO.TotalGamesBalance);
                    }
                    if (product.ProductGamesContainerDTOList != null && product.ProductGamesContainerDTOList.Count > 0)
                    {
                        //List<ProductGamesContainerDTO> filteredList = product.ProductGamesContainerDTOList.Where(g => g.Game_profile_id == -1 && g.Game_id == -1).ToList();
                        //if (filteredList != null && filteredList.Count > 0)
                        //{
                        //    productGame = Convert.ToInt32(filteredList.Sum(x => x.Quantity));
                        //}
                        productGame = Convert.ToInt32(product.ProductGamesContainerDTOList.Sum(x => x.Quantity));
                    }
                    if (productGame != 0)
                    {
                        if ((cardGame + productGame) > Convert.ToDouble(staffGameLimit))
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1444), MessageType.Error);
                            canAddProduct = false;
                            log.LogMethodExit(canAddProduct);
                            return canAddProduct;
                        }
                    }
                }
            }           
            log.LogMethodExit(canAddProduct);
            return canAddProduct;           
        }
        private async void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                staffCardsView = parameter as StaffCardsView;
                if (staffCardsView != null)
                {
                    if (!UserViewContainerList.IsSelfApprovalAllowed(ExecutionContext))
                    {
                        AuthenticateManagerView managerView = new AuthenticateManagerView();
                        AuthenticateManagerVM managerVM = new AuthenticateManagerVM(ExecutionContext, this.cardReader);
                        managerView.DataContext = managerVM;
                        managerView.Owner = staffCardsView;
                        managerView.ShowDialog();
                        if (managerVM.IsValid)
                        {
                            managerId = Convert.ToInt32(managerVM.ManagerId);
                        }
                        else
                        {
                            managerId = -1;
                            PerformClose(parameter);
                        }
                    }
                    else
                    {
                        managerId = Convert.ToInt32(ExecutionContext.GetUserPKId());
                    }
                    UserCollection = await GetAllUsers();
                    roleCollection = UserRoleViewContainerList.GetUserRoleContainerDTOList(ExecutionContext);
                    excludedUserRoleIds = roleCollection.Where(role => role.Role.Equals("Semnox Admin") || role.Role.Equals("System User") || role.Role.Equals("Concurrent User")).Select(r => r.RoleId).ToList();
                    UserCollection = UserCollection.Where(user => !excludedUserRoleIds.Any(id => id == user.RoleId)).ToList();
                    NewSectionUserLovCollection = new ObservableCollection<UsersDTO>(UserCollection);
                    roleCollection = roleCollection.Where(role => !excludedUserRoleIds.Any(id => id == role.RoleId)).ToList();
                    UserRoleContainerDTO defaultUserRole = new UserRoleContainerDTO()
                    {
                        RoleId = -1,
                        Role = MessageViewContainerList.GetMessage(ExecutionContext, "All", null)
                    };
                    roleCollection.Add(defaultUserRole);
                    NewSectionRoleLovCollection = new ObservableCollection<UserRoleContainerDTO>(roleCollection);               
                    LeftPaneVM.MenuItems = new ObservableCollection<string>
                    {
                        MessageViewContainerList.GetMessage(this.ExecutionContext, "New"),
                        MessageViewContainerList.GetMessage(this.ExecutionContext, "All") + "(" + UserCollection.Count.ToString() + ")"
                    };
                    LeftPaneVM.SelectedMenuItem = LeftPaneVM.MenuItems[0];
                    SetDisplayTags();
   
                }
            }
            log.LogMethodExit();
        }

        private void OnDeleteClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            TransactionCardDetailsVM.AccountDTO = null;           
            log.LogMethodExit();
        }
        private void InitilaizeCommands()
        {
            log.LogMethodExit();
            LeftPaneMenuSelectedCommand = new DelegateCommand(OnLeftPaneMenuSelected);
            LeftPaneNavigationClickedCommand = new DelegateCommand(OnLeftPaneNavigationClicked);
            IsSelectedCommand = new DelegateCommand(OnSelectionChanged);
            SearchCommand = new DelegateCommand(OnSearchClicked, ButtonEnable);
            enterCardCommand = new DelegateCommand(OnEnterCardClicked, ButtonEnable);
            addCommand = new DelegateCommand(OnAddClicked, ButtonEnable);
            deactiveCommand = new DelegateCommand(OnDeactiveClicked, ButtonEnable);
            resetCommand = new DelegateCommand(OnResetClicked, ButtonEnable);
            confirmCommand = new DelegateCommand(OnConfirmClicked, ButtonEnable);
            deleteClickedCommand = new DelegateCommand(OnDeleteClicked, ButtonEnable);
            productClickedCommand = new DelegateCommand(OnProductClicked, ButtonEnable);
            loadedCommand = new DelegateCommand(OnLoaded);
            AddCommandForNoCardAdded = new DelegateCommand(OnAddCommandForNoCardAdded, ButtonEnable);
            log.LogMethodExit();
        }

        
        

        internal void GetStaffCardProducts()
        {
            int displayGroupId = -1;
            string staffProductDisplayGroup = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "STAFF_CARD_PRODUCTS_DISPLAY_GROUP");
            if (!string.IsNullOrWhiteSpace(staffProductDisplayGroup))
            {
                displayGroupId = Convert.ToInt32(staffProductDisplayGroup);
            }
            POSMachineContainerDTO posMachineContainerDTO = POSMachineViewContainerList.GetPOSMachineContainerDTO(ExecutionContext);
            productCollection = ProductViewContainerList.GetActiveProductsContainerDTOList(ExecutionContext, ManualProductType.SELLABLE.ToString());
            if (productCollection != null)
            {
                productCollection = productCollection.Where(s => s.ProductsDisplayGroupContainerDTOList != null).ToList();
                if (productCollection != null)
                {

                    productCollection = productCollection.Where(p => p.ProductsDisplayGroupContainerDTOList.Any(d => d.DisplayGroupId == displayGroupId)
                    && p.DisplayInPOS.Equals("Y")
                    && (p.StartTime == null || p.StartTime == DateTime.MinValue || p.StartTime <= DateTime.Now)
                    && (p.ExpiryTime == null || p.ExpiryTime == DateTime.MinValue || p.ExpiryTime >= DateTime.Now)
                    && (p.POSTypeId == posMachineContainerDTO.POSTypeId || p.POSTypeId == -1)).ToList();

                    if (productCollection != null && productCollection.Count > 0)
                    {
                        productCollection = productCollection.OrderBy((x) =>
                        {
                            if (x.ProductType == "CARDSALE")
                            {
                                x.SortOrder = 0;
                                return x.SortOrder;
                            }
                            else if (x.ProductType == "NEW")
                            {
                                x.SortOrder = 1;
                                return x.SortOrder;
                            }
                            else if (x.ProductType == "RECHARGE")
                            {
                                x.SortOrder = 2;
                                return x.SortOrder;
                            }
                            else if (x.ProductType == "VARIABLECARD")
                            {
                                x.SortOrder = 3;
                                return x.SortOrder;
                            }
                            else if (x.ProductType == "GAMETIME")
                            {
                                x.SortOrder = 4;
                                return x.SortOrder;
                            }
                            else if (x.ProductType == "CHECK-IN")
                            {
                                x.SortOrder = 5;
                                return x.SortOrder;
                            }
                            else if (x.ProductType == "CHECK-OUT")
                            {
                                x.SortOrder = 6;
                                return x.SortOrder;
                            }
                            else
                            {
                                x.SortOrder = 7;
                                return x.SortOrder;
                            }
                        }).ToList();
                    }
                }


            }
        }
        #endregion

        #region Constructor
        public StaffCardsVM(ExecutionContext executionContext, DeviceClass cardReader)
        {
            log.LogMethodEntry(executionContext, cardReader);
            ExecutionContext = executionContext;
            this.cardReader = cardReader;
            DisplayMemberPathCollection = new List<string>() { "UserName", "EmpLastName" , "LoginId" };
            LeftPaneVM = new LeftPaneVM(this.ExecutionContext);
            PropertyChanged += OnPropertyChanged;
            showNoCardAddedContent = false;
            IsProductsGridEnabled = true;
            IsNotesEnabled = false;
            enableManualEntry = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ALLOW_MANUAL_CARD_IN_POS", false);
            GetStaffCardProducts();           
            DisplayTagsVM = new DisplayTagsVM();
            customDataGridVM = new CustomDataGridVM(ExecutionContext);
            customDataGridVM.IsComboAndSearchVisible = false;
            RegisterReader();
            userCollection = new List<UsersDTO>();
            NewSectionUserLovCollection = new ObservableCollection<UsersDTO>();
            AllSectionUserLovCollection = new ObservableCollection<UsersDTO>();
            SetDefaultViewModels();
            InitilaizeCommands();
            log.LogMethodExit();
        }

        internal async Task<List<UsersDTO>> GetAllUsers()
        {
            List<UsersDTO> userDTOs = null;
            try
            {
                //IsLoadingVisible = true;
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                userDTOs = await UserUseCaseFactory.GetUserUseCases(ExecutionContext).GetUserDTOList(searchParameters, true);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            finally
            {
                //IsLoadingVisible = false;
            }
            return userDTOs;
        }
        private void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            if (LeftPaneVM != null)
            {
                LeftPaneVM.IsLoadingVisible = IsLoadingVisible;
                LeftPaneVM.RaiseCanExecuteChanged();
            }
            (SearchCommand as DelegateCommand).RaiseCanExecuteChanged();
            (EnterCardCommand as DelegateCommand).RaiseCanExecuteChanged();
            (AddCommand as DelegateCommand).RaiseCanExecuteChanged();
            (DeactiveCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ResetCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ConfirmCommand as DelegateCommand).RaiseCanExecuteChanged();
            (DeleteClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ProductClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (AddCommandForNoCardAdded as DelegateCommand).RaiseCanExecuteChanged();
            log.LogMethodExit();
        }
        #endregion
    }
}

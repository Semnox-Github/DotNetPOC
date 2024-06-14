/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - view model for redemption main view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Interop;

using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.Product;
using Semnox.Parafait.POS;
using Semnox.Parafait.Inventory.Location;
using Semnox.Parafait.Authentication;

namespace Semnox.Parafait.RedemptionUI
{
    public enum RedemptionsType
    {
        New = 0,
        Suspended = 1,
        Completed = 2,
        Voucher = 3,
        Flagged = 4,
    }

    public class RedemptionMainVM : BaseWindowViewModel
    {
        #region Members
        private string oldMode;
        private bool enableEnterTicketNumberManually;
        private bool allowTransactionOnZeroStock;
        private bool allowRedemptionsWithoutCard;
        private bool showAllRedemptions;
        private bool multiuserMultiscreen;
        private bool addUserButtonEnabled;
        private bool singleuserMultiscreen;
        private bool mulitpleUserVisibility;
        private bool multiScreenSwitchVisibility;
        private int columnCount;
        private int rowCount;
        private int userCount;

        private ExecutionContext systemuserExecutioncontext;
        private RedemptionView redemptionView;
        private SortedList<int, int> screenNumberCollection;
        private SortedList<int, int> colorCodeCollection;
        private ObservableCollection<RedemptionMainUserControlVM> redemptionUserControlVMs;
        private RedemptionHeaderTagsVM redemptionHeaderTagsVM;
        private RedemptionDeviceCollection redemptionDeviceCollection;
        private List<Task> loadContainerTasks = new List<Task>();


        private RedemptionsType redemptionsType;
        private Visibility searchFilterVisbility;
        private DeviceClass addUserReader;

        private ICommand loadedCommand;
        private ICommand multipleUserClickedCommand;
        private ICommand addUserClickedCommand;
        private ICommand addScreenClickedCommand;
        private ICommand removeButtonClickedCommand;
        private ICommand redemptionUserControlClickedCommand;
        private ICommand headerTagClickedCommand;
        private ICommand backButtonClickedCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public string OldMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(oldMode);
                return oldMode;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref oldMode, value);
                log.LogMethodExit(oldMode);
            }
        }
        public int UserCount
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(userCount);
                return userCount;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref userCount, value);
                log.LogMethodExit(userCount);
            }
        }
        public SortedList<int, int> ScreenNumberCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(screenNumberCollection);
                return screenNumberCollection;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref screenNumberCollection, value);
                log.LogMethodExit(screenNumberCollection);
            }
        }
        public bool EnableEnterTicketNumberManually
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enableEnterTicketNumberManually);
                return enableEnterTicketNumberManually;
            }
            set
            {
                log.LogMethodEntry(enableEnterTicketNumberManually, value);
                SetProperty(ref enableEnterTicketNumberManually, value);
                log.LogMethodExit(enableEnterTicketNumberManually);
            }
        }

        public bool AllowTransactionOnZeroStock
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(allowTransactionOnZeroStock);
                return allowTransactionOnZeroStock;
            }
            set
            {
                log.LogMethodEntry(allowTransactionOnZeroStock, value);
                SetProperty(ref allowTransactionOnZeroStock, value);
                log.LogMethodExit(allowTransactionOnZeroStock);
            }
        }
        public ObservableCollection<RedemptionMainUserControlVM> RedemptionUserControlVMs
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionUserControlVMs);
                return redemptionUserControlVMs;
            }
            set
            {
                log.LogMethodEntry(redemptionUserControlVMs, value);
                SetProperty(ref redemptionUserControlVMs, value);
                log.LogMethodExit(redemptionUserControlVMs);
            }
        }
        public RedemptionDeviceCollection RedemptionDeviceCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionDeviceCollection);
                return redemptionDeviceCollection;
            }
            set
            {
                log.LogMethodEntry(redemptionDeviceCollection, value);
                SetProperty(ref redemptionDeviceCollection, value);
                log.LogMethodExit(redemptionDeviceCollection);
            }
        }
        public RedemptionHeaderTagsVM RedemptionHeaderTagsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionHeaderTagsVM);
                return redemptionHeaderTagsVM;
            }
            set
            {
                log.LogMethodEntry(redemptionHeaderTagsVM, value);
                SetProperty(ref redemptionHeaderTagsVM, value);
                log.LogMethodExit(redemptionHeaderTagsVM);
            }
        }

        public int ColumnCount
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(columnCount);
                return columnCount;
            }
            set
            {
                log.LogMethodEntry(columnCount, value);
                SetProperty(ref columnCount, value);
                log.LogMethodExit(columnCount);
            }
        }

        public int RowCount
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rowCount);
                return rowCount;
            }
            set
            {
                log.LogMethodEntry(rowCount, value);
                SetProperty(ref rowCount, value);
                log.LogMethodExit(rowCount);
            }
        }

        public bool MulitpleUserVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(mulitpleUserVisibility);
                return mulitpleUserVisibility;
            }
            set
            {
                log.LogMethodEntry(mulitpleUserVisibility, value);
                SetProperty(ref mulitpleUserVisibility, value);
                log.LogMethodExit(mulitpleUserVisibility);
            }
        }
        public Visibility MultiScreenSwitchVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(multiScreenSwitchVisibility);
                if (multiScreenSwitchVisibility)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        public Visibility AddUserButtonVisible
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(singleuserMultiscreen);
                if (singleuserMultiscreen)
                {
                    return Visibility.Hidden;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            set
            {
                //log.LogMethodEntry(addUserButtonVisible, value);
                //SetProperty(ref addUserButtonVisible, value);
                //log.LogMethodExit(addUserButtonVisible);
            }
        }

        public bool AllowRedemptionsWithoutCard
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(allowRedemptionsWithoutCard);
                return allowRedemptionsWithoutCard;
            }
            set
            {
                log.LogMethodEntry(allowRedemptionsWithoutCard, value);
                SetProperty(ref allowRedemptionsWithoutCard, value);
                log.LogMethodExit(allowRedemptionsWithoutCard);
            }
        }

        public bool AddUserButtonEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addUserButtonEnabled);
                return addUserButtonEnabled;
            }
            set
            {
                log.LogMethodEntry(addUserButtonEnabled, value);
                SetProperty(ref addUserButtonEnabled, value);
                log.LogMethodExit(addUserButtonEnabled);
            }
        }
        public bool MultiuserMultiscreen
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(multiuserMultiscreen);
                return multiuserMultiscreen;
            }
            set
            {
                log.LogMethodEntry(multiuserMultiscreen, value);
                SetProperty(ref multiuserMultiscreen, value);
                log.LogMethodExit(multiuserMultiscreen);
            }
        }
        public bool SingleuserMultiscreen
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(singleuserMultiscreen);
                return singleuserMultiscreen;
            }
            set
            {
                log.LogMethodEntry(singleuserMultiscreen, value);
                SetProperty(ref singleuserMultiscreen, value);
                log.LogMethodExit(singleuserMultiscreen);
            }
        }
        public bool ShowAllRedemptions
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showAllRedemptions);
                return showAllRedemptions;
            }
            set
            {
                log.LogMethodEntry(showAllRedemptions, value);
                SetProperty(ref showAllRedemptions, value);
                log.LogMethodExit(showAllRedemptions);
            }
        }
        public Visibility SearchFilterVisbility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchFilterVisbility);
                return searchFilterVisbility;
            }
            set
            {
                log.LogMethodEntry(searchFilterVisbility, value);
                SetProperty(ref searchFilterVisbility, value);
                log.LogMethodExit(searchFilterVisbility);
            }
        }

        public ICommand AddUserClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addUserClickedCommand);
                return addUserClickedCommand;
            }
            set
            {
                log.LogMethodEntry(addUserClickedCommand, value);
                SetProperty(ref addUserClickedCommand, value);
                log.LogMethodExit(addUserClickedCommand);
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
            internal set
            {
                log.LogMethodEntry(value);
                SetProperty(ref loadedCommand, value);
                log.LogMethodExit(loadedCommand);
            }
        }
        public ICommand MultipleUserClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(multipleUserClickedCommand);
                return multipleUserClickedCommand;
            }
            set
            {
                log.LogMethodEntry(multipleUserClickedCommand, value);
                SetProperty(ref multipleUserClickedCommand, value);

                log.LogMethodExit(multipleUserClickedCommand);
            }
        }

        public ICommand BackButtonClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(backButtonClickedCommand);
                return backButtonClickedCommand;
            }
            private set
            {
                log.LogMethodEntry(backButtonClickedCommand, value);
                SetProperty(ref backButtonClickedCommand, value);
                log.LogMethodExit(backButtonClickedCommand);
            }
        }
        public ICommand HeaderTagClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(headerTagClickedCommand);
                return headerTagClickedCommand;
            }
            private set
            {
                log.LogMethodEntry(headerTagClickedCommand, value);
                SetProperty(ref headerTagClickedCommand, value);
                log.LogMethodExit(headerTagClickedCommand);
            }
        }

        public ICommand RedemptionUserControlClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionUserControlClickedCommand);
                return redemptionUserControlClickedCommand;
            }
            private set
            {
                log.LogMethodEntry(redemptionUserControlClickedCommand, value);
                SetProperty(ref redemptionUserControlClickedCommand, value);
                log.LogMethodExit(redemptionUserControlClickedCommand);
            }
        }
        public ICommand AddScreenClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addScreenClickedCommand);
                return addScreenClickedCommand;
            }
            private set
            {
                log.LogMethodEntry(addScreenClickedCommand, value);
                SetProperty(ref addScreenClickedCommand, value);
                log.LogMethodExit(addScreenClickedCommand);
            }
        }

        public ICommand RemoveButtonClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(removeButtonClickedCommand);
                return removeButtonClickedCommand;
            }
            private set
            {
                log.LogMethodEntry(removeButtonClickedCommand, value);
                SetProperty(ref removeButtonClickedCommand, value);
                log.LogMethodExit(removeButtonClickedCommand);
            }
        }
        #endregion

        #region Methods 
        public DateTime? GetLastActivityTime(string loginId)
        {
            log.LogMethodEntry(loginId);
            DateTime? result = null;
            if (RedemptionUserControlVMs != null && RedemptionUserControlVMs.Count() > 0 && RedemptionUserControlVMs.Any(x => x.ExecutionContext != null && x.ExecutionContext.UserId == loginId))
            {
                result = RedemptionUserControlVMs.First(x => x.ExecutionContext != null && x.ExecutionContext.UserId == loginId).LastActivityTime;
            }
            log.LogMethodExit(result);
            return result;
        }
        internal void DisposeAllDevices()
        {
            log.LogMethodEntry();
            if (RedemptionDeviceCollection != null)
            {
                RedemptionDeviceCollection.Dispose();
            }
            log.LogMethodExit();
        }
        internal List<RedemptionCurrencyContainerDTO> GetRedemptionCurrencyContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            List<RedemptionCurrencyContainerDTO> result;
            result = RedemptionCurrencyViewContainerList.GetRedemptionCurrencyContainerDTOList(executionContext).OrderBy(x => (!string.IsNullOrWhiteSpace(x.ShortCutKeys) ? x.ShortCutKeys : "ZZZZZZZZZZ")).ThenBy(x => x.CurrencyName).ToList();
            if (result.Any())
            {
                result = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_REDEMPTION_CURRENCY_ACCESS_CONTROL", false) ?
                                      //  result.Where(x => UserRoleViewContainerList.GetUserRoleContainerDTOList(executionContext).FirstOrDefault(r => r.RoleId == UserViewContainerList.GetUserContainerDTOList(executionContext).FirstOrDefault(z => z.LoginId == executionContext.UserId).RoleId).ManagementFormAccessContainerDTOList.Exists(y => y.AccessAllowed && y.FormName == x.CurrencyName)).ToList() 
                                      result.Where(x => UserRoleViewContainerList.CheckAccess(ExecutionContext.GetSiteId(), UserViewContainerList.GetUserContainerDTOList(executionContext).FirstOrDefault(z => z.LoginId == executionContext.UserId).RoleId, x.CurrencyName)).ToList()
                    : result;
            }
            log.LogMethodExit(result);
            return result;
        }
        internal bool ValidateProductInclusion(ExecutionContext executionContext, ProductsContainerDTO productsContainerDTO)
        {
            log.LogMethodEntry(productsContainerDTO);
            bool result = false;
            if (productsContainerDTO != null)
            {
                HashSet<int> posinclusions = new HashSet<int>(POSMachineViewContainerList.GetPOSMachineContainerDTO(executionContext.GetSiteId(), executionContext.GetMachineId()).IncludedProductIdList);
                if (posinclusions.Contains(productsContainerDTO.ProductId) == false)
                {
                    result = false;
                    log.LogMethodExit(result);
                    return result;
                }
                HashSet<int> userexclusions = new HashSet<int>(UserRoleViewContainerList.GetUserRoleContainerDTO(executionContext.GetSiteId(), UserViewContainerList.GetUserContainerDTO(executionContext.GetSiteId(), executionContext.GetUserId()).RoleId).ExcludedProductIdList);
                if (userexclusions.Contains(productsContainerDTO.ProductId) == true)
                {
                    result = false;
                    log.LogMethodExit(result);
                    return result;
                }
                HashSet<int> currencyExclusions = new HashSet<int>(RedemptionCurrencyViewContainerList.GetRedemptionCurrencyContainerDTOList(executionContext).Where(r => r.ProductId >= 0).Select(r => r.ProductId));
                if (productsContainerDTO.InventoryItemContainerDTO != null && currencyExclusions.Contains(productsContainerDTO.InventoryItemContainerDTO.ProductId) == true)
                {
                    result = false;
                    log.LogMethodExit(result);
                    return result;
                }
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }
        internal List<ProductsContainerDTO> GetProductContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            HashSet<int> posinclusions = new HashSet<int>(POSMachineViewContainerList.GetPOSMachineContainerDTO(executionContext.GetSiteId(), executionContext.GetMachineId()).IncludedProductIdList);
            HashSet<int> currencyExclusions = new HashSet<int>(RedemptionCurrencyViewContainerList.GetRedemptionCurrencyContainerDTOList(executionContext).Where(r => r.ProductId >= 0).Select(r => r.ProductId));
            HashSet<int> userexclusions = new HashSet<int>(UserRoleViewContainerList.GetUserRoleContainerDTO(executionContext.GetSiteId(), UserViewContainerList.GetUserContainerDTO(executionContext.GetSiteId(), executionContext.GetUserId()).RoleId).ExcludedProductIdList);
            posinclusions.ExceptWith(userexclusions);
            List<ProductsContainerDTO> result = new List<ProductsContainerDTO>();
            foreach (var productId in posinclusions)
            {
                ProductsContainerDTO p = ProductViewContainerList.GetProductsContainerDTOOrDefault(executionContext, ManualProductType.REDEEMABLE.ToString(), productId);
                if (p == null || (p.InventoryItemContainerDTO != null &&
                    currencyExclusions.Contains(p.InventoryItemContainerDTO.ProductId)))
                {
                    continue;
                }
                result.Add(p);
            }
            if (result.Any(x => x.InventoryItemContainerDTO != null))
            {
                result = result.Where(x => x.InventoryItemContainerDTO != null).ToList();
                result = result.OrderBy(x => (x.SortOrder == -1 ? 99999999 : x.SortOrder)).ThenBy(x => x.ProductName).ToList();
            }
            else
            {
                result = new List<ProductsContainerDTO>();
            }
            log.LogMethodExit(result);
            return result;
        }
        internal bool CheckInProgressRedemption(bool fromBackButton = false, bool fromcloseButton = false)
        {
            log.LogMethodEntry();
            bool discarded = true;
            if (this.redemptionUserControlVMs != null && this.redemptionUserControlVMs.Any(r => r.RedemptionDTO != null &&
             (r.RedemptionDTO.RedemptionGiftsListDTO.Count > 0 || r.RedemptionDTO.TicketReceiptListDTO.Count > 0
             || r.RedemptionDTO.RedemptionCardsListDTO.Count > 0)) && !ShowDiscardConfirmation())
            {
                discarded = false;
            }
            if (discarded)
            {
                if (!fromBackButton)
                {
                    SetPreviousMode();
                }
                if (redemptionView != null)
                {
                    if (this.redemptionUserControlVMs != null)
                    {
                        foreach (RedemptionMainUserControlVM r in this.redemptionUserControlVMs)
                        {
                            if (r.Timer != null)
                            {
                                r.Timer.Stop();
                            }
                            r.CancellationTokenSource.Cancel();
                            r.ResetRecalculateFlags();
                            r.UnRegisterDevices();
                        }
                    }
                    DisposeAllDevices();
                    if (!fromcloseButton)
                    {
                        redemptionView.Closing -= OnClosingCommand;
                        redemptionView.Close();
                    }
                }
            }
            log.LogMethodExit();
            return discarded;
        }

        internal bool ShowDiscardConfirmation()
        {
            log.LogMethodEntry();
            bool discard = false;
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext)
            {
                Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "DISCARD", null) + " " +
                MessageViewContainerList.GetMessage(this.ExecutionContext, "All", null) + "!",
                Content = MessageViewContainerList.GetMessage(ExecutionContext, 2740, ExecutionContext.UserId) + MessageViewContainerList.GetMessage(ExecutionContext, "Close Redemption"),
                OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "DISCARD", null),
                CancelButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                MessageButtonsType = MessageButtonsType.OkCancel
            };
            messagePopupView.DataContext = messagePopupVM;
            if (redemptionView != null)
            {
                messagePopupView.Owner = redemptionView;
            }
            messagePopupView.ShowDialog();
            if (messagePopupVM.ButtonClickType == ButtonClickType.Ok)
            {
                discard = true;
            }
            log.LogMethodExit();
            return discard;
        }

        internal void SetPreviousMode()
        {
            log.LogMethodEntry();
            OldMode = "Y";
            log.LogMethodEntry("old mode selected");
            log.LogVariableState("old mode selected", OldMode);
            log.LogMethodExit();
        }

        private void OnSearchClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (SearchFilterVisbility == Visibility.Collapsed)
            {
                SearchFilterVisbility = Visibility.Visible;
            }
            else
            {
                SearchFilterVisbility = Visibility.Collapsed;
            }
            log.LogMethodExit();
        }

        private void OnMultipleUserClicked(object param)
        {
            log.LogMethodEntry(param);
            if (param != null)
            {
                RedemptionView redemptionView = param as RedemptionView;
                if (redemptionView != null)
                {
                    if (!showAllRedemptions)
                    {
                        foreach (RedemptionMainUserControlVM mainUserControlVM in RedemptionUserControlVMs)
                        {
                            if (!mainUserControlVM.IsActive)
                            {
                                if (mainUserControlVM.UserView != null || mainUserControlVM.ManagerView != null)
                                {
                                    if (RedemptionUserControlVMs.Any(x => x.UserName == mainUserControlVM.UserName && x.IsActive))
                                    {
                                        string message = MessageViewContainerList.GetMessage(mainUserControlVM.ExecutionContext, 2928);
                                        log.Info(message);
                                        RedemptionUserControlVMs.FirstOrDefault(x => x.UserName == mainUserControlVM.UserName && x.IsActive).SetFooterContent(message, MessageType.Warning);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    ShowAllRedemptions = !ShowAllRedemptions;
                    if (ShowAllRedemptions)
                    {
                        List<RedemptionMainUserControlVM> userControlVMs = new List<RedemptionMainUserControlVM>();
                        List<IGrouping<string, RedemptionMainUserControlVM>> groupedVM = RedemptionUserControlVMs.GroupBy(r => r.UserName).ToList();
                        foreach (IGrouping<string, RedemptionMainUserControlVM> n in groupedVM)
                        {
                            userControlVMs.AddRange(n.OrderBy(r => r.ScreenNumber));
                        }
                        foreach (RedemptionMainUserControlVM mainUserControlVM in userControlVMs)
                        {
                            int oldIndex = RedemptionUserControlVMs.IndexOf(mainUserControlVM);
                            int newIndex = userControlVMs.IndexOf(mainUserControlVM);
                            if (oldIndex != newIndex)
                            {
                                RedemptionUserControlVMs.Move(oldIndex, newIndex);
                            }
                        }
                    }
                    else if (!showAllRedemptions)
                    {
                        List<RedemptionMainUserControlVM> activeRedemptions = RedemptionUserControlVMs.Where(r => r.IsActive == true).ToList();
                        for (int i = 0; i < activeRedemptions.Count; i++)
                        {
                            int oldIndex = RedemptionUserControlVMs.IndexOf(activeRedemptions[i]);
                            if (i != RedemptionUserControlVMs.IndexOf(activeRedemptions[i]))
                            {
                                //RedemptionUserControlVMs.Remove(activeRedemptions[i]);
                                //RedemptionUserControlVMs.Insert(i, activeRedemptions[i]);
                                RedemptionUserControlVMs.Move(oldIndex, i);
                            }
                        }
                    }
                    SetRowAndColumnCount();
                    foreach (RedemptionMainUserControlVM mainUserControlVM in RedemptionUserControlVMs)
                    {
                        mainUserControlVM.OnSizeChanged(mainUserControlVM, null);
                        if (!showAllRedemptions && !mainUserControlVM.IsActive)
                        {
                            mainUserControlVM.CloseChildWindows();
                        }
                        mainUserControlVM.SetOpacityforScreens();
                    }
                }
            }
            log.LogMethodExit();
        }
        internal AuthenticateUserView authenticateUserView;
        internal void OnAddNewUserClicked(object param = null)
        {
            log.LogMethodEntry(param);
            //log.Error("OnAddNewUserClicked begin" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            if (RedemptionUserControlVMs == null)
            {
                RedemptionUserControlVMs = new ObservableCollection<RedemptionMainUserControlVM>();

            }
            if (RedemptionUserControlVMs.Count == 8)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2671, null), MessageType.Error);
                return;
            }
            ExecutionContext userexecutionContext = new ExecutionContext(null, ExecutionContext.SiteId, ExecutionContext.MachineId, -1, ExecutionContext.IsCorporate, ExecutionContext.LanguageId);
            authenticateUserView = new AuthenticateUserView(true);
            try
            {
                addUserReader = redemptionDeviceCollection.GetDevice(DeviceType.CardReader, GetNextDeviceAddress("CARDREADER"));
                if (addUserReader != null)
                {
                    addUserReader.Register(CardScanCompleteEventHandle);
                }
            }
            catch (Exception ex)
            {
                GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                {
                    Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "MESSAGE", null),
                    Content = MessageViewContainerList.GetMessage(ExecutionContext, ex.Message),
                    OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "OK", null),
                    MessageButtonsType = MessageButtonsType.OK
                };
                messagePopupView.DataContext = messagePopupVM;
                if (redemptionView != null)
                {
                    messagePopupView.Owner = redemptionView;
                }
                messagePopupView.ShowDialog();
            }
            AuthenticateUserVM authenticateUserVM = new AuthenticateUserVM(userexecutionContext, "", "PARAFAIT POS", loginStyle.PopUp, false, addUserReader);
            authenticateUserView.DataContext = authenticateUserVM;
            if (param != null)
            {
                authenticateUserView.Owner = param as RedemptionView;
            }
            authenticateUserView.CloseButtonClicked += OnAuthenticateUserViewClosed;
            authenticateUserView.ShowDialog();
            //log.Error("OnAddNewUserClicked after login" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            if (authenticateUserVM.IsValid)
            {
                AddNewUser(authenticateUserVM.ExecutionContext);
            }
            //log.Error("OnAddNewUserClicked end" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            log.LogMethodExit();
        }
        internal void AddNewUser(ExecutionContext localexecutionContext)
        {
            log.LogMethodEntry(localexecutionContext);
            try
            {
                IsLoadingVisible = true;
                if (RedemptionUserControlVMs == null)
                {
                    RedemptionUserControlVMs = new ObservableCollection<RedemptionMainUserControlVM>();

                }
                if (RedemptionUserControlVMs.Count == 8)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(localexecutionContext, 2671, null), MessageType.Error);
                    IsLoadingVisible = false;
                    return;
                }
                if (RedemptionUserControlVMs.Any(x => x.UserName == localexecutionContext.UserId))
                {
                    string message = MessageViewContainerList.GetMessage(localexecutionContext, 2672, localexecutionContext.UserId);
                    log.Info(message);
                    RedemptionUserControlVMs.FirstOrDefault(x => x.UserName == localexecutionContext.UserId && x.IsActive).SetFooterContent(message, MessageType.Warning);
                }
                else
                {
                    DeviceClass cardReader = new DeviceClass();
                    DeviceClass barcodeReader = new DeviceClass();
                    string deviceMessage = string.Empty;
                    int carddeviceaddress = -1;
                    int barcodedeviceaddress = -1;
                    try
                    {
                        barcodedeviceaddress = GetNextDeviceAddress("BARCODEREADER");
                        barcodeReader = redemptionDeviceCollection.GetDevice(DeviceType.BarcodeReader, barcodedeviceaddress);
                    }
                    catch (Exception ex)
                    {
                        deviceMessage = ex.Message;
                    }
                    finally
                    {
                        IsLoadingVisible = false;
                    }
                    try
                    {
                        carddeviceaddress = GetNextDeviceAddress("CARDREADER");
                        cardReader = redemptionDeviceCollection.GetDevice(DeviceType.CardReader, carddeviceaddress);
                    }
                    catch (Exception ex)
                    {
                        deviceMessage = ex.Message;
                    }
                    finally
                    {
                        IsLoadingVisible = false;
                    }
                    SetOtherRedemptionScreenAsInactive(localexecutionContext.UserId, null);
                    RedemptionMainUserControlVM redemptionMainUserControlVM = new RedemptionMainUserControlVM(localexecutionContext,
                                                                                                                true,
                                                                                                                localexecutionContext.UserId,
                                                                                                                screenNumberCollection.First().Key,
                                                                                                                cardReader,
                                                                                                                barcodeReader,
                                                                                                                carddeviceaddress,
                                                                                                                barcodedeviceaddress,
                                                                                                                singleuserMultiscreen,
                                                                                                                multiuserMultiscreen,
                                                                                                                (ColorCode)colorCodeCollection.First().Key);
                    redemptionMainUserControlVM.DeviceErrorMessage = deviceMessage;
                    AddUsertoCollection(redemptionMainUserControlVM);
                    screenNumberCollection.Remove(screenNumberCollection.First().Key);
                    colorCodeCollection.Remove(colorCodeCollection.First().Key);
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
        private int GetNextDeviceAddress(string deviceType)
        {
            log.LogMethodEntry(deviceType);
            int deviceAddress = -1;
            if (RedemptionUserControlVMs == null || RedemptionUserControlVMs.Count == 0)
            {
                deviceAddress = 0;
            }
            else
            {
                if (deviceType == "CARDREADER")
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (RedemptionUserControlVMs.Any(x => x.CardDeviceAddress == i))
                        {
                            continue;
                        }
                        else
                        {
                            deviceAddress = i;
                            break;
                        }
                    }
                }
                if (deviceType == "BARCODEREADER")
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (RedemptionUserControlVMs.Any(x => x.BarCodeDeviceAddress == i))
                        {
                            continue;
                        }
                        else
                        {
                            deviceAddress = i;
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit(deviceAddress);
            return deviceAddress;
        }
        private void OnAuthenticateUserViewClosed(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            AuthenticateUserView authenticateUserView = sender as AuthenticateUserView;
            AuthenticateUserVM authenticateUserVM = authenticateUserView.DataContext as AuthenticateUserVM;
            if (authenticateUserVM.CardReader != null)
            {
                authenticateUserVM.CardReader.UnRegister();
            }
            authenticateUserView.Close();

            if (redemptionUserControlVMs.Count <= 0 && redemptionView != null)
            {
                DisposeAllDevices();
                redemptionView.Closing -= OnClosingCommand;
                redemptionView.Close();
            }
            log.LogMethodExit();
        }
        private void AddUsertoCollection(RedemptionMainUserControlVM redemptionMainUserControlVM)
        {
            log.LogMethodEntry(redemptionMainUserControlVM);
            Dictionary<string, string> currencyPropertyAndValueCollection = new Dictionary<string, string>();
            currencyPropertyAndValueCollection.Add("CurrencyName", string.Empty);
            currencyPropertyAndValueCollection.Add("ValueInTickets", MessageViewContainerList.GetMessage(redemptionMainUserControlVM.ExecutionContext, (ParafaitDefaultViewContainerList.GetParafaitDefault(redemptionMainUserControlVM.ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"))));
            Dictionary<string, string> productPropertyAndValueCollection = new Dictionary<string, string>();
            productPropertyAndValueCollection.Add("ProductName", string.Empty);
            productPropertyAndValueCollection.Add("InventoryItemContainerDTO.PriceInTickets", MessageViewContainerList.GetMessage(redemptionMainUserControlVM.ExecutionContext, (ParafaitDefaultViewContainerList.GetParafaitDefault(redemptionMainUserControlVM.ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"))));
            Dictionary<string, string> turnInPropertyAndValueCollection = new Dictionary<string, string>();
            turnInPropertyAndValueCollection.Add("ProductName", string.Empty);
            turnInPropertyAndValueCollection.Add("InventoryItemContainerDTO.TurnInPriceInTickets", MessageViewContainerList.GetMessage(redemptionMainUserControlVM.ExecutionContext, (ParafaitDefaultViewContainerList.GetParafaitDefault(redemptionMainUserControlVM.ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"))));
            if (redemptionMainUserControlVM != null)
            {
                List<ProductsContainerDTO> productsContainers = GetProductContainerDTOList(redemptionMainUserControlVM.ExecutionContext);
                if (productsContainers != null && productsContainers.Count > 0)
                {
                    if (redemptionMainUserControlVM.RedemptionUserControlVM != null)
                    {
                        string numberFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT");
                        string ticketText = MessageViewContainerList.GetMessage(redemptionMainUserControlVM.ExecutionContext, ParafaitDefaultViewContainerList.GetParafaitDefault(redemptionMainUserControlVM.ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"));
                        redemptionMainUserControlVM.RedemptionUserControlVM.GenericDisplayItemsVM = new GenericDisplayItemsVM(redemptionMainUserControlVM.ExecutionContext)
                        {
                            PropertyAndValueCollection = productPropertyAndValueCollection,
                            ButtonType = ButtonType.Info,
                            ShowDisabledItems = true
                        };
                    }
                    if (redemptionMainUserControlVM.TurnInUserControlVM != null)
                    {
                        redemptionMainUserControlVM.TurnInUserControlVM.GenericDisplayItemsVM = new GenericDisplayItemsVM(redemptionMainUserControlVM.ExecutionContext)
                        {
                            PropertyAndValueCollection = turnInPropertyAndValueCollection,
                            ButtonType = ButtonType.Info
                        };
                    }
                }
                if (redemptionMainUserControlVM.LoadTicketRedemptionUserControlVM != null)
                {
                    redemptionMainUserControlVM.LoadTicketRedemptionUserControlVM.GenericDisplayItemsVM = new GenericDisplayItemsVM(redemptionMainUserControlVM.ExecutionContext)
                    {
                        DisplayItemModels = new ObservableCollection<object>(GetRedemptionCurrencyContainerDTOList(redemptionMainUserControlVM.ExecutionContext).Cast<object>()),
                        PropertyAndValueCollection = currencyPropertyAndValueCollection
                    };
                }
            }
            if (redemptionMainUserControlVM != null && redemptionMainUserControlVM.RedemptionUserControlVM != null)
            {
                redemptionMainUserControlVM.RedemptionUserControlVM.OnToggleChecked(null);
            }
            if (!showAllRedemptions)
            {
                RedemptionMainUserControlVM sameUserControlVM = RedemptionUserControlVMs.FirstOrDefault(r => r.UserName == redemptionMainUserControlVM.UserName);
                if (sameUserControlVM != null)
                {
                    RedemptionUserControlVMs.Add(redemptionMainUserControlVM);
                    int firstIndex = RedemptionUserControlVMs.IndexOf(sameUserControlVM);
                    int lastIndex = RedemptionUserControlVMs.IndexOf(redemptionMainUserControlVM);
                    RedemptionUserControlVMs.Move(lastIndex, firstIndex);
                    firstIndex = RedemptionUserControlVMs.IndexOf(sameUserControlVM);
                    RedemptionUserControlVMs.Move(firstIndex, lastIndex);
                }
                else
                {
                    int count = RedemptionUserControlVMs.Select(r => r.UserName).Distinct().ToList().Count;
                    RedemptionUserControlVMs.Insert(count, redemptionMainUserControlVM);
                }
            }
            else
            {
                int position = 0;
                //if (RedemptionUserControlVMs.Where(r => r.UserName == redemptionMainUserControlVM.UserName).Distinct().ToList().Count > 0)
                //{
                //    int maxscreenforuser = RedemptionUserControlVMs.Where(r => r.UserName == redemptionMainUserControlVM.UserName).Select(x => x.ScreenNumber).Max();
                //    position = RedemptionUserControlVMs.IndexOf(RedemptionUserControlVMs.FirstOrDefault(r => r.UserName == redemptionMainUserControlVM.UserName && r.ScreenNumber== maxscreenforuser)) + 1;
                //}
                //else
                //{
                //    position = RedemptionUserControlVMs.Count();
                //}
                if (RedemptionUserControlVMs.Where(r => r.UserName == redemptionMainUserControlVM.UserName).Distinct().ToList().Count > 0)
                {
                    RedemptionMainUserControlVM beforeUserControl = null;
                    foreach (RedemptionMainUserControlVM usercontrolVM in RedemptionUserControlVMs.Where(r => r.UserName == redemptionMainUserControlVM.UserName).OrderBy(x => x.ScreenNumber))
                    {
                        if (usercontrolVM.ScreenNumber > redemptionMainUserControlVM.ScreenNumber)
                            continue;
                        beforeUserControl = usercontrolVM;
                    }
                    if (beforeUserControl == null)
                    {
                        position = RedemptionUserControlVMs.IndexOf(RedemptionUserControlVMs.Where(r => r.UserName == redemptionMainUserControlVM.UserName).OrderBy(x => x.ScreenNumber).FirstOrDefault());
                    }
                    else
                    {
                        position = RedemptionUserControlVMs.IndexOf(beforeUserControl) + 1;
                    }
                }
                else
                {
                    position = RedemptionUserControlVMs.Count();
                }
                RedemptionUserControlVMs.Insert(position, redemptionMainUserControlVM);
            }
            SetRowAndColumnCount();
            SetHeaderGroups(redemptionMainUserControlVM);
            SetAddUserButtonEnabled();
            foreach (RedemptionMainUserControlVM mainUserControlVM in RedemptionUserControlVMs)
            {
                if (RedemptionUserControlVMs.Count == 8 && mainUserControlVM.LeftPaneVM != null && mainUserControlVM.LeftPaneVM.AddButtonVisiblity)
                {
                    mainUserControlVM.LeftPaneVM.AddButtonVisiblity = false;
                }
                mainUserControlVM.OnSizeChanged(mainUserControlVM, null);
            }
            log.LogMethodExit();
        }
        private void SetOtherRedemptionScreenAsInactive(string UserName, int? ScreenNumber = null)
        {
            log.LogMethodEntry(UserName, ScreenNumber);
            foreach (RedemptionMainUserControlVM redemptionUserControlVM in RedemptionUserControlVMs)
            {
                if (redemptionUserControlVM.UserName.ToLower() == UserName.ToLower())
                {
                    if (redemptionUserControlVM.IsActive)
                    {
                        if (ScreenNumber == null || ScreenNumber != redemptionUserControlVM.ScreenNumber)
                        {
                            redemptionUserControlVM.IsActive = false;
                        }
                    }
                    if (RedemptionHeaderTagsVM.HeaderGroups.Any(r => r.UserName.ToLower() == UserName.ToLower()))
                    {
                        RedemptionHeaderGroup redemptionHeaderGroup = RedemptionHeaderTagsVM.HeaderGroups.First(r => r.UserName.ToLower() == UserName.ToLower());
                        if (redemptionHeaderGroup != null)
                        {
                            foreach (RedemptionHeaderTag redemptionHeaderTag in redemptionHeaderGroup.RedemptionHeaderTags)
                            {
                                if (redemptionHeaderTag != null)
                                {
                                    if (ScreenNumber != null)
                                    {
                                        if (redemptionHeaderTag.ScreenNumber == ScreenNumber)
                                        {
                                            redemptionHeaderTag.IsActive = true;
                                        }
                                        else
                                        {
                                            redemptionHeaderTag.IsActive = false;
                                        }
                                    }
                                    else
                                    {
                                        redemptionHeaderTag.IsActive = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        internal void SetRowAndColumnCount()
        {
            log.LogMethodEntry();
            int count = RedemptionUserControlVMs.Count;
            if (!showAllRedemptions)
            {
                count = RedemptionUserControlVMs.Select(r => r.UserName).Distinct().ToList().Count;
            }
            switch (count)
            {
                case 1:
                    {
                        ColumnCount = 1;
                        RowCount = 1;
                    }
                    break;
                case 2:
                    {
                        ColumnCount = 2;
                        RowCount = 1;
                    }
                    break;
                case 3:
                case 4:
                    {
                        ColumnCount = 2;
                        RowCount = 2;
                    }
                    break;
                case 5:
                case 6:
                    {
                        ColumnCount = 3;
                        RowCount = 2;
                    }
                    break;
                case 7:
                case 8:
                    {
                        ColumnCount = 4;
                        RowCount = 2;
                    }
                    break;
            }
            bool userControlMultiScreen = false;
            if ((showAllRedemptions && ColumnCount > 1) || (!showAllRedemptions && count > 1))
            {
                userControlMultiScreen = true;
            }
            SetToggleButtonHeaderSize(userControlMultiScreen);
            SetUserControlMultiScreenMode(userControlMultiScreen);
            log.LogMethodExit();
        }
        private void SetToggleButtonHeaderSize(bool multiScreen)
        {
            log.LogMethodEntry();
            TextSize textSize = TextSize.XSmall;
            if (ColumnCount > 2 && multiScreen)
            {
                textSize = TextSize.Medium;
            }
            foreach (RedemptionMainUserControlVM userControlVM in RedemptionUserControlVMs)
            {
                if (userControlVM.RedemptionUserControlVM.GenericToggleButtonsVM.ToggleButtonItems[1].DisplayTags[0].TextSize != textSize)
                {
                    userControlVM.RedemptionUserControlVM.GenericToggleButtonsVM.ToggleButtonItems[1].DisplayTags[0].TextSize = textSize;
                    if (userControlVM.RedemptionUserControlVM.GenericToggleButtonsVM.ToggleButtonItems.Count > 2)
                    {
                        userControlVM.RedemptionUserControlVM.GenericToggleButtonsVM.ToggleButtonItems[2].DisplayTags[0].TextSize = textSize;
                    }
                }
            }
            log.LogMethodExit();
        }
        private void SetUserControlMultiScreenMode(bool multiScreen)
        {
            log.LogMethodEntry(multiScreen);
            foreach (RedemptionMainUserControlVM userControlVM in RedemptionUserControlVMs)
            {
                userControlVM.MultiScreenMode = multiScreen;
                if (string.IsNullOrEmpty(userControlVM.LeftPaneVM.SelectedMenuItem))
                {
                    userControlVM.LeftPaneVM.SelectedMenuItem = MessageViewContainerList.GetMessage(ExecutionContext, "Redemption");
                }
                if (!multiScreen && userControlVM.FooterVM.SideBarContent != MessageViewContainerList.GetMessage(ExecutionContext, "Hide Sidebar"))
                {
                    userControlVM.FooterVM.OnHideSideBarClicked(null);
                    if (userControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption)
                    {
                        if (userControlVM.RedemptionUserControlVM.RedemptionsType != RedemptionsType.New &&
                        userControlVM.RedemptionUserControlVM.GenericRightSectionContentVM != null
                        && userControlVM.RedemptionUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble)
                        {
                            userControlVM.RedemptionUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble = false;
                        }
                        if (userControlVM.RedemptionUserControlVM.RedemptionsType == RedemptionsType.New &&
                               userControlVM.RedemptionUserControlVM.GenericTransactionListVM != null
                               && !userControlVM.RedemptionUserControlVM.GenericTransactionListVM.ScreenUserAreaVisible)
                        {
                            userControlVM.RedemptionUserControlVM.GenericTransactionListVM.ScreenUserAreaVisible = false;
                        }
                    }
                    else if (userControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket
                        && userControlVM.LoadTicketRedemptionUserControlVM.RedemptionsType == RedemptionsType.New &&
                           userControlVM.LoadTicketRedemptionUserControlVM.GenericTransactionListVM != null
                           && !userControlVM.LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ScreenUserAreaVisible)
                    {
                        userControlVM.LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ScreenUserAreaVisible = false;
                    }
                    else if (userControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.TurnIn)
                    {
                        if (userControlVM.TurnInUserControlVM.RedemptionsType != RedemptionsType.New &&
                        userControlVM.TurnInUserControlVM.GenericRightSectionContentVM != null
                        && userControlVM.TurnInUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble)
                        {
                            userControlVM.TurnInUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble = false;
                        }
                        if (userControlVM.TurnInUserControlVM.RedemptionsType == RedemptionsType.New &&
                               userControlVM.TurnInUserControlVM.GenericTransactionListVM != null
                               && !userControlVM.TurnInUserControlVM.GenericTransactionListVM.ScreenUserAreaVisible)
                        {
                            userControlVM.TurnInUserControlVM.GenericTransactionListVM.ScreenUserAreaVisible = false;
                        }
                    }
                    else if (userControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Voucher
                        && userControlVM.VoucherUserControlVM.GenericRightSectionContentVM != null
                        && userControlVM.VoucherUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble)
                    {
                        userControlVM.VoucherUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble = false;
                    }
                }
                else if (multiScreen && !string.IsNullOrEmpty(userControlVM.LeftPaneVM.SelectedMenuItem)
                    && userControlVM.FooterVM.SideBarContent != MessageViewContainerList.GetMessage(ExecutionContext, "Show Sidebar"))
                {
                    userControlVM.FooterVM.OnHideSideBarClicked(null);
                    if (userControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption)
                    {
                        if (userControlVM.RedemptionUserControlVM.RedemptionsType != RedemptionsType.New &&
                            userControlVM.RedemptionUserControlVM.GenericRightSectionContentVM != null
                            && !userControlVM.RedemptionUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble)
                        {
                            userControlVM.RedemptionUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble = true;
                        }
                        if (userControlVM.RedemptionUserControlVM.RedemptionsType == RedemptionsType.New &&
                           userControlVM.RedemptionUserControlVM.GenericTransactionListVM != null
                           && !userControlVM.RedemptionUserControlVM.GenericTransactionListVM.ScreenUserAreaVisible)
                        {
                            userControlVM.RedemptionUserControlVM.GenericTransactionListVM.ScreenUserAreaVisible = true;
                        }
                    }
                    else if (userControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket
                        && userControlVM.LoadTicketRedemptionUserControlVM.RedemptionsType == RedemptionsType.New &&
                           userControlVM.LoadTicketRedemptionUserControlVM.GenericTransactionListVM != null
                           && !userControlVM.LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ScreenUserAreaVisible)
                    {
                        userControlVM.LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ScreenUserAreaVisible = true;
                    }
                    else if (userControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.TurnIn)
                    {
                        if (userControlVM.TurnInUserControlVM.RedemptionsType != RedemptionsType.New &&
                            userControlVM.TurnInUserControlVM.GenericRightSectionContentVM != null
                            && !userControlVM.TurnInUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble)
                        {
                            userControlVM.TurnInUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble = true;
                        }
                        if (userControlVM.TurnInUserControlVM.RedemptionsType == RedemptionsType.New &&
                           userControlVM.TurnInUserControlVM.GenericTransactionListVM != null
                           && !userControlVM.TurnInUserControlVM.GenericTransactionListVM.ScreenUserAreaVisible)
                        {
                            userControlVM.TurnInUserControlVM.GenericTransactionListVM.ScreenUserAreaVisible = true;
                        }
                    }
                    else if (userControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Voucher
                        && userControlVM.VoucherUserControlVM.GenericRightSectionContentVM != null
                            && !userControlVM.VoucherUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble)
                    {
                        userControlVM.VoucherUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble = true;
                    }
                }
                if (ColumnCount == 2 && RowCount == 2)
                {
                    userControlVM.FooterVM.SpanHideSideBar = false;
                }
                else
                {
                    userControlVM.FooterVM.SpanHideSideBar = true;
                }
                if (multiScreen && userControlVM.RedemptionUserControlVM != null &&
                    userControlVM.RedemptionUserControlVM.GenericRightSectionContentVM != null)
                {
                    if (RowCount == 2)
                    {
                        userControlVM.RedemptionUserControlVM.GenericRightSectionContentVM.IsMultiScreenRowTwo = true;
                    }
                    else
                    {
                        if (userControlVM.RedemptionUserControlVM.GenericRightSectionContentVM.IsMultiScreenRowTwo)
                        {
                            userControlVM.RedemptionUserControlVM.GenericRightSectionContentVM.IsMultiScreenRowTwo = false;
                        }
                    }
                }
                if (!multiuserMultiscreen && !singleuserMultiscreen
                    && userControlVM.LeftPaneVM != null && userControlVM.LeftPaneVM.AddButtonVisiblity)
                {
                    userControlVM.LeftPaneVM.AddButtonVisiblity = false;
                }
                userControlVM.SetProductMenuDisplay(this);
            }
            log.LogMethodExit();
        }
        private void SetHeaderGroups(RedemptionMainUserControlVM redemptionMainUserControlVM)
        {
            log.LogMethodEntry(redemptionMainUserControlVM);
            if (RedemptionHeaderTagsVM == null)
            {
                RedemptionHeaderTagsVM = new RedemptionHeaderTagsVM();
            }
            if (RedemptionHeaderTagsVM.HeaderGroups == null)
            {
                RedemptionHeaderTagsVM.HeaderGroups = new ObservableCollection<RedemptionHeaderGroup>();
            }
            if (RedemptionHeaderTagsVM.HeaderGroups.Any(r => r.UserName.ToLower() == redemptionMainUserControlVM.UserName.ToLower()))
            {
                RedemptionHeaderGroup redemptionHeaderGroup = RedemptionHeaderTagsVM.HeaderGroups.First(r => r.UserName.ToLower() == redemptionMainUserControlVM.UserName.ToLower());
                if (redemptionHeaderGroup != null)
                {
                    redemptionHeaderGroup.RedemptionHeaderTags = new ObservableCollection<RedemptionHeaderTag>(redemptionHeaderGroup.RedemptionHeaderTags.Select(r => { r.IsActive = false; return r; }));
                    int position = 0;
                    if (redemptionHeaderGroup.RedemptionHeaderTags.Any())
                    {
                        RedemptionHeaderTag beforetag = null;
                        foreach (RedemptionHeaderTag headertags in redemptionHeaderGroup.RedemptionHeaderTags.OrderBy(x => x.ScreenNumber))
                        {
                            if (headertags.ScreenNumber > redemptionMainUserControlVM.ScreenNumber)
                                continue;
                            beforetag = headertags;
                        }
                        position = redemptionHeaderGroup.RedemptionHeaderTags.IndexOf(beforetag) + 1;
                    }
                    redemptionHeaderGroup.RedemptionHeaderTags.Insert(position, new RedemptionHeaderTag(ExecutionContext)
                    {
                        BalanceTicket = 0,
                        ScreenNumber = redemptionMainUserControlVM.ScreenNumber,
                        IsActive = true
                    });
                    //redemptionHeaderGroup.RedemptionHeaderTags.Add(new RedemptionHeaderTag(ExecutionContext)
                    //{
                    //    BalanceTicket = 0,
                    //    ScreenNumber = redemptionMainUserControlVM.ScreenNumber,
                    //    IsActive = true
                    //});
                }
                //redemptionMainUserControlVM.ColorCode = redemptionHeaderGroup.ColorCode;
            }
            else
            {
                //redemptionMainUserControlVM.ColorCode = (ColorCode)RedemptionHeaderTagsVM.HeaderGroups.Count;
                RedemptionHeaderTagsVM.HeaderGroups.Add(new RedemptionHeaderGroup()
                {
                    UserName = redemptionMainUserControlVM.UserName,
                    RedemptionHeaderTags = new ObservableCollection<RedemptionHeaderTag>()
                    {
                        new RedemptionHeaderTag(ExecutionContext)
                        {
                            BalanceTicket = 0,
                            ScreenNumber = redemptionMainUserControlVM.ScreenNumber,
                            IsActive = true
                        }
                    },
                    ColorCode = redemptionMainUserControlVM.ColorCode
                });
            }
            SetApplyColorCode();
            log.LogMethodExit();
        }
        private void SetApplyColorCode()
        {
            log.LogMethodEntry();
            foreach (RedemptionMainUserControlVM mainUserControlVM in RedemptionUserControlVMs)
            {
                if (RedemptionHeaderTagsVM != null && RedemptionHeaderTagsVM.HeaderGroups != null &&
                    RedemptionHeaderTagsVM.HeaderGroups.Count > 1)
                {
                    mainUserControlVM.ApplyColorCode = true;
                }
                else if (mainUserControlVM.ApplyColorCode)
                {
                    mainUserControlVM.ApplyColorCode = false;
                }
            }
            log.LogMethodExit();
        }
        private void OnAddScreenClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            try
            {
                IsLoadingVisible = true;
                if (parameter != null)
                {
                    RedemptionMainUserControlVM redemptionMainUserControlVM = parameter as RedemptionMainUserControlVM;
                    if (redemptionMainUserControlVM != null)
                    {
                        if (RedemptionUserControlVMs.Count == 8)
                        {
                            redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2671, null), MessageType.Error);
                            IsLoadingVisible = false;
                            return;
                        }
                        SetOtherRedemptionScreenAsInactive(redemptionMainUserControlVM.UserName, null);
                        RedemptionMainUserControlVM copiedRedemptionMainUserControlVM = new RedemptionMainUserControlVM
                            (redemptionMainUserControlVM.ExecutionContext,
                            true,
                            redemptionMainUserControlVM.UserName,
                            screenNumberCollection.First().Key,
                           redemptionMainUserControlVM.CardReader,
                           redemptionMainUserControlVM.BarCodeReader,
                           redemptionMainUserControlVM.CardDeviceAddress,
                           redemptionMainUserControlVM.BarCodeDeviceAddress,
                           singleuserMultiscreen,
                           multiuserMultiscreen,
                           redemptionMainUserControlVM.ColorCode
                            );
                        //if ( copiedRedemptionMainUserControlVM.LeftPaneVM != null &&
                        //   string.IsNullOrEmpty(copiedRedemptionMainUserControlVM.LeftPaneVM.SelectedMenuItem))
                        //{
                        //    copiedRedemptionMainUserControlVM.LeftPaneVM.SelectedMenuItem = copiedRedemptionMainUserControlVM.LeftPaneVM.MenuItems[0];
                        //}
                        if (copiedRedemptionMainUserControlVM.RedemptionUserControlVM != null && redemptionMainUserControlVM.RedemptionUserControlVM != null
        && redemptionMainUserControlVM.RedemptionUserControlVM.SuspendedRedemptions != null && copiedRedemptionMainUserControlVM.RedemptionUserControlVM.SuspendedRedemptions != null &&
        redemptionMainUserControlVM.RedemptionUserControlVM.SuspendedRedemptions.Count != copiedRedemptionMainUserControlVM.RedemptionUserControlVM.SuspendedRedemptions.Count)
                        {
                            copiedRedemptionMainUserControlVM.RedemptionUserControlVM.SuspendedRedemptions = redemptionMainUserControlVM.RedemptionUserControlVM.SuspendedRedemptions.ToList();
                        }
                        copiedRedemptionMainUserControlVM.ColorCode = redemptionMainUserControlVM.ColorCode;
                        screenNumberCollection.Remove(screenNumberCollection.First().Key);
                        AddUsertoCollection(copiedRedemptionMainUserControlVM);
                        if (!showAllRedemptions)
                        {
                            // redemptionMainUserControlVM.UnWireupEvents();
                        }
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
        private void OnRemoveClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            try
            {
                IsLoadingVisible = true;
                if (parameter != null)
                {
                    bool isMaximumScreens = RedemptionUserControlVMs.Count == 8 ? true : false;

                    RedemptionMainUserControlVM redemptionMainUserControlVM = parameter as RedemptionMainUserControlVM;
                    if (redemptionMainUserControlVM != null && RedemptionUserControlVMs.Contains(redemptionMainUserControlVM))
                    {
                        RemoveScreen(redemptionMainUserControlVM);
                    }
                    SetAddUserButtonEnabled();
                    foreach (RedemptionMainUserControlVM mainUserControlVM in RedemptionUserControlVMs)
                    {
                        if (isMaximumScreens && mainUserControlVM.IsActive && !mainUserControlVM.LeftPaneVM.AddButtonVisiblity)
                        {
                            mainUserControlVM.LeftPaneVM.AddButtonVisiblity = true;
                        }
                        mainUserControlVM.OnSizeChanged(mainUserControlVM, null);
                    }
                }
                if (redemptionUserControlVMs.Count <= 0 && redemptionView != null)
                {
                    DisposeAllDevices();
                    redemptionView.Closing -= OnClosingCommand;
                    redemptionView.Close();
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
        internal void RemoveScreen(RedemptionMainUserControlVM redemptionMainUserControlVM, bool deleteAll = false)
        {
            log.LogMethodEntry(redemptionMainUserControlVM);
            if (redemptionMainUserControlVM.Timer != null)
            {
                redemptionMainUserControlVM.Timer.Stop();
            }
            redemptionMainUserControlVM.UnRegisterDevices();
            // if last screen is getting closed then dispose the reader to avoid orphan events
            if (!RedemptionUserControlVMs.Any(x => (x.UserName == redemptionMainUserControlVM.UserName && x.ScreenNumber != redemptionMainUserControlVM.ScreenNumber)))
            {
                if (redemptionMainUserControlVM.CardReader != null)
                {
                    redemptionMainUserControlVM.CardReader.Dispose();
                }
                if (redemptionMainUserControlVM.BarCodeReader != null)
                {
                    redemptionMainUserControlVM.BarCodeReader.Dispose();
                }
            }
            int index = RedemptionUserControlVMs.IndexOf(redemptionMainUserControlVM);
            redemptionMainUserControlVM.CancellationTokenSource.Cancel();
            redemptionMainUserControlVM.ResetRecalculateFlags();
            RedemptionUserControlVMs.Remove(redemptionMainUserControlVM);
            redemptionMainUserControlVM.CloseChildWindows();
            screenNumberCollection.Add(redemptionMainUserControlVM.ScreenNumber, redemptionMainUserControlVM.ScreenNumber);
            RedemptionHeaderGroup removableHeaderGroup = RedemptionHeaderTagsVM.HeaderGroups.FirstOrDefault(h => h.UserName.ToLower() == redemptionMainUserControlVM.UserName.ToLower());
            if (removableHeaderGroup != null)
            {
                RedemptionHeaderTag redemptionHeaderTag = removableHeaderGroup.RedemptionHeaderTags.FirstOrDefault(r => r.ScreenNumber.ToString() == redemptionMainUserControlVM.ScreenNumber.ToString().ToLower());
                if (redemptionHeaderTag != null)
                {
                    removableHeaderGroup.RedemptionHeaderTags.Remove(redemptionHeaderTag);
                    if (removableHeaderGroup.RedemptionHeaderTags.Count == 0)
                    {
                        RedemptionHeaderTagsVM.HeaderGroups.Remove(removableHeaderGroup);
                        colorCodeCollection.Add((int)redemptionMainUserControlVM.ColorCode, (int)redemptionMainUserControlVM.ColorCode);
                    }
                }
                if (!deleteAll)
                {
                    RedemptionMainUserControlVM firstUserControlVM;
                    RedemptionHeaderTag firstHeaderTag = removableHeaderGroup.RedemptionHeaderTags.FirstOrDefault();
                    if (!showAllRedemptions && firstHeaderTag != null)
                    {
                        firstUserControlVM = RedemptionUserControlVMs.FirstOrDefault(r => r.ScreenNumber == firstHeaderTag.ScreenNumber);
                        if (firstUserControlVM != null)
                        {
                            firstUserControlVM.IsActive = true;
                            firstHeaderTag.IsActive = true;
                            //RedemptionUserControlVMs.Remove(firstUserControlVM);
                            if (index < 0)
                            {
                                index = 0;
                            }
                            RedemptionUserControlVMs.Move(RedemptionUserControlVMs.IndexOf(firstUserControlVM), index);
                            //RedemptionUserControlVMs.Insert(index, firstUserControlVM);
                        }
                    }
                    else
                    {
                        firstUserControlVM = RedemptionUserControlVMs.FirstOrDefault(r => r.UserName == redemptionMainUserControlVM.UserName);
                        if (firstUserControlVM != null && removableHeaderGroup != null &&
                            removableHeaderGroup.RedemptionHeaderTags.Count > 0)
                        {
                            if (redemptionHeaderTag != null)
                            {
                                firstUserControlVM.IsActive = true;
                                if (firstHeaderTag != null)
                                {
                                    firstHeaderTag.IsActive = true;
                                }
                            }
                        }
                    }
                    if (firstUserControlVM != null)
                    {
                        firstUserControlVM.SetUserControlFocus();
                    }
                    if (firstUserControlVM == null && !showAllRedemptions)
                    {
                        RedemptionMainUserControlVM nextuserControlVM = RedemptionUserControlVMs.FirstOrDefault();
                        if (nextuserControlVM != null)
                        {
                            nextuserControlVM.IsActive = true;
                            RedemptionUserControlVMs.Move(RedemptionUserControlVMs.IndexOf(nextuserControlVM), 0);
                            //RedemptionUserControlVMs.Remove(nextuserControlVM);
                            //RedemptionUserControlVMs.Insert(0, nextuserControlVM);
                            if (nextuserControlVM != null)
                            {
                                nextuserControlVM.SetUserControlFocus();
                            }
                        }
                    }
                }
            }
            SetRowAndColumnCount();
            SetApplyColorCode();
            log.LogMethodExit();
        }
        internal void SetAddUserButtonEnabled()
        {
            if (RedemptionUserControlVMs.Count == 8)
            {
                AddUserButtonEnabled = false;
            }
            else if (!AddUserButtonEnabled)
            {
                AddUserButtonEnabled = true;
            }
            UserCount = redemptionUserControlVMs.Select(x => x.UserName).Distinct().Count();
        }
        internal void OnRedemptionMouseClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                RedemptionMainUserControlVM redemptionMainUserControl = parameter as RedemptionMainUserControlVM;
                if (redemptionMainUserControl != null && !redemptionMainUserControl.ReloginInitiated)
                {
                    SetOtherRedemptionScreenAsInactive(redemptionMainUserControl.UserName, redemptionMainUserControl.ScreenNumber);
                    redemptionMainUserControl.IsActive = true;
                }
            }
            log.LogMethodExit();
        }
        private void OnBackButtonClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            CheckInProgressRedemption(true);
            log.LogMethodExit();
        }
        private void OnClosingCommand(object sender, System.ComponentModel.CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!CheckInProgressRedemption(true, true))
            {
                e.Cancel = true;
            }
            log.LogMethodExit();
        }
        private void OnHeaderTagClicked(object parameter)
        {
            log.LogMethodEntry();
            try
            {
                IsLoadingVisible = true;
                if (RedemptionHeaderTagsVM != null && RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup != null
                   && RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.SelectedRedemptionHeaderTag != null)
                {
                    if (redemptionUserControlVMs != null && redemptionUserControlVMs.Any(x => x.UserName == RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.UserName && x.ScreenNumber == RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.SelectedRedemptionHeaderTag.ScreenNumber))
                    {
                        if (redemptionUserControlVMs.FirstOrDefault(x => x.UserName == RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.UserName && x.ScreenNumber == RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.SelectedRedemptionHeaderTag.ScreenNumber).ReloginInitiated)
                        {
                            if (RedemptionHeaderTagsVM.HeaderGroups != null &&
                                RedemptionHeaderTagsVM.HeaderGroups.Any(x => x.UserName == RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.UserName) &&
                                RedemptionHeaderTagsVM.HeaderGroups.FirstOrDefault(x => x.UserName == RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.UserName).RedemptionHeaderTags != null &&
                                RedemptionHeaderTagsVM.HeaderGroups.FirstOrDefault(x => x.UserName == RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.UserName).RedemptionHeaderTags.Any(y => y.IsActive))
                            {
                                RedemptionHeaderTagsVM.HeaderGroups.FirstOrDefault(x => x.UserName == RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.UserName).SelectedRedemptionHeaderTag =
                                RedemptionHeaderTagsVM.HeaderGroups.FirstOrDefault(x => x.UserName == RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.UserName).RedemptionHeaderTags.FirstOrDefault(z => z.ScreenNumber ==
                                RedemptionHeaderTagsVM.HeaderGroups.FirstOrDefault(x => x.UserName == RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.UserName).RedemptionHeaderTags.FirstOrDefault(y => y.IsActive).ScreenNumber);
                            }
                            IsLoadingVisible = false;
                            return;
                        }
                    }
                    RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.SelectedRedemptionHeaderTag.IsActive = true;
                    foreach (RedemptionHeaderTag tag in RedemptionHeaderTagsVM.HeaderGroups.FirstOrDefault(x => x.UserName == RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.UserName).RedemptionHeaderTags.Where(y => y.ScreenNumber != RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.SelectedRedemptionHeaderTag.ScreenNumber))
                    {
                        tag.IsActive = false;
                    }
                    SetasActiveScreen(RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.UserName, RedemptionHeaderTagsVM.SelectedRedemptionHeaderGroup.SelectedRedemptionHeaderTag.ScreenNumber);
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
        internal void SetasActiveScreen(string userName, int screenNumber)
        {
            RedemptionMainUserControlVM redemptionMainUserControlVM = RedemptionUserControlVMs.First(r => r.ScreenNumber == screenNumber);
            if (redemptionMainUserControlVM != null)
            {
                foreach (RedemptionMainUserControlVM redemptionUserControlVM in RedemptionUserControlVMs)
                {
                    if (redemptionUserControlVM.UserName == userName)
                    {
                        if (redemptionUserControlVM != redemptionMainUserControlVM)
                        {
                            if (redemptionUserControlVM.IsActive)
                            {
                                redemptionUserControlVM.IsActive = false;
                            }
                            if (!showAllRedemptions && redemptionUserControlVM.ScreenNumber != screenNumber)
                            {
                                redemptionUserControlVM.CloseChildWindows();
                                //redemptionUserControlVM.UnWireupEvents();
                            }
                        }
                    }
                }
                if (!redemptionMainUserControlVM.IsActive)
                {
                    redemptionMainUserControlVM.IsActive = true;
                }
                if (!showAllRedemptions)
                {
                    RedemptionMainUserControlVM sameUserControlVM = RedemptionUserControlVMs.FirstOrDefault(r => r.UserName == redemptionMainUserControlVM.UserName);
                    if (sameUserControlVM != null)
                    {
                        int firstIndex = RedemptionUserControlVMs.IndexOf(sameUserControlVM);
                        int secondIndex = RedemptionUserControlVMs.IndexOf(redemptionMainUserControlVM);
                        RedemptionUserControlVMs.Move(secondIndex, firstIndex);
                        firstIndex = RedemptionUserControlVMs.IndexOf(sameUserControlVM);
                        RedemptionUserControlVMs.Move(firstIndex, secondIndex);
                    }
                }
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetUserControlFocus();
                }
            }
        }
        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            redemptionView = parameter as RedemptionView;
            redemptionView.Closing += OnClosingCommand;
            IntPtr handle = (new WindowInteropHelper(redemptionView)).Handle;
            redemptionDeviceCollection = new RedemptionDeviceCollection(ExecutionContext, handle);

            if (ExecutionContext != null && !string.IsNullOrWhiteSpace(ExecutionContext.WebApiToken))
            {
                AddNewUser(ExecutionContext);
            }
            else
            {
                OnAddNewUserClicked(parameter);
            }
        }
        internal new void ExecuteAction(Action method)
        {
            log.LogMethodEntry();
            try
            {
                method();
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                OnAddNewUserClicked();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructors
        public RedemptionMainVM(ExecutionContext executionContext)
        {
            log.Info("Redemption screen is opened");
            log.LogMethodEntry();
            ExecuteAction(() =>
            {
                loadedCommand = new DelegateCommand(OnLoaded);
                this.ExecutionContext = executionContext;
                oldMode = "N";
            });
            try
            {
                systemuserExecutioncontext = SystemUserExecutionContextBuilder.GetSystemUserExecutionContext();
                //this.ExecutionContext = systemuserExecutioncontext; // uncomment for HQ test
                //this.ExecutionContext.SetIsCorporate(true);// uncomment for HQ test
            }
            catch (UserAuthenticationException ue)
            {
                log.Error(ue);
                DisposeAllDevices();
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisposeAllDevices();
                if ((ex.InnerException as UserAuthenticationException).UserAuthenticationErrorType == UserAuthenticationErrorType.CHANGE_PASSWORD)
                {
                    throw new UserAuthenticationException(ex.InnerException.Message, UserAuthenticationErrorType.CHANGE_PASSWORD);
                }
                else
                {
                    throw;
                }
            }
            ExecuteAction(() =>
            {
                //log.Error("Start Container" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                loadContainerTasks.Add(Task.Factory.StartNew(() => { List<ProductsContainerDTO> productsContainerDTOList = ProductViewContainerList.GetActiveProductsContainerDTOList(systemuserExecutioncontext, ManualProductType.REDEEMABLE.ToString()); }));
                loadContainerTasks.Add(Task.Factory.StartNew(() => {
                    List<RedemptionCurrencyContainerDTO> redemptionCurrencyContainerDTOList = RedemptionCurrencyViewContainerList.GetRedemptionCurrencyContainerDTOList(systemuserExecutioncontext);
                }));
                loadContainerTasks.Add(Task.Factory.StartNew(() => {
                    List<RedemptionCurrencyRuleContainerDTO> redemptionCurrencyRuleContainerDTOList = RedemptionCurrencyRuleViewContainerList.GetRedemptionCurrencyRuleContainerDTOList(systemuserExecutioncontext);
                }));
                loadContainerTasks.Add(Task.Factory.StartNew(() => {
                    List<UserRoleContainerDTO> userRolesDTOList = UserRoleViewContainerList.GetUserRoleContainerDTOList(systemuserExecutioncontext);
                }));
                loadContainerTasks.Add(Task.Factory.StartNew(() => { List<UserContainerDTO> userDTOList = UserViewContainerList.GetUserContainerDTOList(systemuserExecutioncontext); }));
                loadContainerTasks.Add(Task.Factory.StartNew(() => {
                    List<POSMachineContainerDTO> pOSMachineContainerDTOs = POSMachineViewContainerList.GetPOSMachineContainerDTOList(systemuserExecutioncontext);
                }));
                loadContainerTasks.Add(Task.Factory.StartNew(() => {
                    List<LocationTypeContainerDTO> locationtypeContainerDTOList = LocationTypeViewContainerList.GetLocationTypeContainerDTOList(systemuserExecutioncontext);
                }));
                loadContainerTasks.Add(Task.Factory.StartNew(() => {
                    List<LocationContainerDTO> locationContainerDTOList = LocationViewContainerList.GetLocationContainerDTOList(systemuserExecutioncontext);
                }));
                loadContainerTasks.Add(Task.Factory.StartNew(() =>
                {
                    if (ProductViewContainerList.GetActiveProductsContainerDTOList(systemuserExecutioncontext, ManualProductType.REDEEMABLE.ToString()).FirstOrDefault(x => x.InventoryItemContainerDTO != null && x.InventoryItemContainerDTO.ProductId >= 0) != null)
                    {
                        decimal discountedPrice = RedemptionPriceViewContainerList.GetLeastPriceInTickets(systemuserExecutioncontext.SiteId, ProductViewContainerList.GetActiveProductsContainerDTOList(systemuserExecutioncontext, ManualProductType.REDEEMABLE.ToString()).FirstOrDefault(x => x.InventoryItemContainerDTO != null && x.InventoryItemContainerDTO.ProductId >= 0).ProductId, new List<int>());
                    }
                }));

            });
            ExecuteAction(() =>
            {
                singleuserMultiscreen = ParafaitDefaultViewContainerList.GetParafaitDefault(this.ExecutionContext, "ENABLE_SINGLE_USER_MULTI_SCREEN") == "Y" ? true : false;
                multiuserMultiscreen = ParafaitDefaultViewContainerList.GetParafaitDefault(this.ExecutionContext, "ENABLE_MULTI_USER_MULTI_SCREEN") == "Y" ? true : false;
                showAllRedemptions = ParafaitDefaultViewContainerList.GetParafaitDefault(this.ExecutionContext, "SHOW_ALL_REDEMPTION_SCREENS") == "Y" ? true : false;
                allowRedemptionsWithoutCard = ParafaitDefaultViewContainerList.GetParafaitDefault(this.ExecutionContext, "ALLOW_REDEMPTION_WITHOUT_CARD") == "Y" ? true : false;
                allowRedemptionsWithoutCard = true;
                enableEnterTicketNumberManually = ParafaitDefaultViewContainerList.GetParafaitDefault(this.ExecutionContext, "ENABLE_MANUAL_TICKET_ENTRY_IN_REDEMPTION") == "Y" ? true : false;
                allowTransactionOnZeroStock = ParafaitDefaultViewContainerList.GetParafaitDefault(this.ExecutionContext, "ALLOW_TRANSACTION_ON_ZERO_STOCK") == "Y" ? true : false;
                if (!singleuserMultiscreen && !multiuserMultiscreen)
                {
                    multiScreenSwitchVisibility = false;
                }
                else
                {
                    multiScreenSwitchVisibility = true;
                }
                rowCount = 1;
                columnCount = 1;
                SearchCommand = new DelegateCommand(OnSearchClicked);
                addUserClickedCommand = new DelegateCommand(OnAddNewUserClicked);
                multipleUserClickedCommand = new DelegateCommand(OnMultipleUserClicked);
                addScreenClickedCommand = new DelegateCommand(OnAddScreenClicked);
                removeButtonClickedCommand = new DelegateCommand(OnRemoveClicked);
                redemptionUserControlClickedCommand = new DelegateCommand(OnRedemptionMouseClicked);
                headerTagClickedCommand = new DelegateCommand(OnHeaderTagClicked);
                backButtonClickedCommand = new DelegateCommand(OnBackButtonClicked);
                searchFilterVisbility = Visibility.Collapsed;
            });
            ExecuteAction(() =>
            {
                Task.WaitAll(loadContainerTasks.ToArray());
                //log.Error("End Container" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            });
            ExecuteAction(() =>
            {
                redemptionUserControlVMs = new ObservableCollection<RedemptionMainUserControlVM>();
                redemptionHeaderTagsVM = new RedemptionHeaderTagsVM();
                screenNumberCollection = new SortedList<int, int>();
                for (int i = 1; i <= 8; i++)
                {
                    screenNumberCollection.Add(i, i);
                }
                colorCodeCollection = new SortedList<int, int>();
                for (int i = 0; i <= 7; i++)
                {
                    colorCodeCollection.Add(i, i);
                }
            });
            log.LogMethodExit();
        }
        #endregion
    }
}
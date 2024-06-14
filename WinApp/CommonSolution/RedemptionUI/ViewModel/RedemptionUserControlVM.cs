/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - view model for redemption user control
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 *2.130.10    26-Aug-2022   Amitha Joy            Removed turn in from location  
 ********************************************************************************************/
using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Globalization;
using System.Reflection;

using Semnox.Parafait.CommonUI;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Product;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.Inventory.Location;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Inventory;

namespace Semnox.Parafait.RedemptionUI
{
    public class RedemptionUserControlVM : ViewModelBase
    {
        #region Members
        public enum ActionType
        {
            None,
            Suspend,
            Retrieve,
            Delete,
            SuspendRetrieve,
            Complete,
            CompleteSearch,
            VoucherSearch
        }
        private CustomTextBoxDatePicker selectedDatePicker;
        private TicketReceiptDTO receiptDTO;
        private RedemptionUserControl redemptionuserControl;
        private DateTime? from = null;
        private DateTime? to = null;
        private ObservableCollection<LocationContainerDTO> targetLocations;
        private LocationContainerDTO selectedTargetLocation;
        private ProductsContainerDTO selectedProductContainerDTO;
        private bool uiClicked;
        private bool multiScreenMode;
        private bool iscontentAreaVisible;
        private bool isLoadTicket;
        private bool isTurnIn;
        private bool isVoucher;
        private bool showNoCurrencyTextBlock;
        private bool stayInTransactionMode;
        private bool showSearchCloseIcon;
        private bool showAll;
        private bool isChangeStatusEnable;
        private bool enableNextNavigaion;
        private bool enablePreviousNavigation;
        private RedemptionDTO selectedRedemptionDTO;
        private TicketReceiptDTO flaggedticketReceiptDTO;

        private string defaultTransactionID = "RO-";
        private string transactionID;
        private string userName;
        private string screenNumber;
        private string searchedReceiptNo;
        private string searchedCardNo;
        private string searchedBalanceTicketFromOrOrderNo;
        private string searchedBalanceTicketToOrPrdCodeDesBarCode;
        private string searchedIssuedDateFrom;
        private string searchedIssuedDateTo;
        private string printErrorMessage;
        private string loadTotatlTicketText;
        private int loadTotatlTicketCount;
        private int addedId = -1;
        //private RedemptionDTO NewRedemptionDTO;
        private int ticketsToLoad = 0;
        private int pageSize;
        private int pageNumber;
        private int totalPageCount;


        private string selectedStatus;
        private List<RedemptionDTO> suspendedRedemptions;
        private List<RedemptionDTO> todayCompletedRedemptions;
        private ObservableCollection<string> statusCollection;
        private GenericTransactionListVM genericTransactionListVM;
        private GenericToggleButtonsVM genericToggleButtonsVM;
        private DisplayTagsVM rightSectionDisplayTagsVM;
        private GenericDisplayItemsVM genericDisplayItemsVM;
        private RedemptionMainUserControl redemptionMainUserControl;
        private RedemptionMainUserControlVM redemptionMainUserControlVM;
        private RedemptionView redemptionMainView;
        private RedemptionMainVM mainVM;
        private CustomDataGridVM customDataGridVM;
        private GenericContentVM genericContentVM;
        private GenericRightSectionContentVM genericRightSectionContentVM;
        private List<RedemptionDTO> redemptionDTOList;
        private List<TicketReceiptDTO> ticketReceiptDTOList;
        private List<TicketReceiptDTO> currentDayTicketReceiptList;

        private RedemptionsType redemptionsType;
        private Visibility searchFilterVisbility;

        private ICommand scrollChangedCommand;
        private ICommand contentRenderedCommand;
        private ICommand showAllClickedCommand;
        private ICommand datePickerLoadedCommand;
        private ICommand toggleCheckedCommand;
        private ICommand toggleUncheckedCommand;
        private ICommand itemClickedCommand;
        private ICommand itemOfferOrInfoClickedCommand;
        private ICommand searchCommand;
        private ICommand showContentAreaClickedCommand;
        private ICommand showTransactionAreaClickedCommand;
        private ICommand deleteCommand;
        private ICommand resetCommand;
        private ICommand transactionItemClickedCommand;
        private ICommand loadedCommand;
        private ICommand transactionActionsCommand;
        private ICommand isSelectedCommand;
        private ICommand previousNavigationCommand;
        private ICommand nextNavigationCommand;
        private ICommand suspendCompleteActionsCommand;
        private ICommand totalTicketClickedCommand;
        private ICommand searchActionsCommand;
        private ICommand scanEnterCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public List<KeyValuePair<int, string>> Actions
        {
            get
            {
                return new List<KeyValuePair<int, string>>()
                {
                    new KeyValuePair<int, string>(1, "PreviousNavigationClicked"),
                    new KeyValuePair<int, string>(2, "NextNavigationClicked"),
                };
            }
        }
        public bool AutoShowSearchSection { get; private set; }
        public ICommand ActionsCommand { get; private set; }
        public bool EnableNextNavigation
        {
            get { return enableNextNavigaion; }
            set { SetProperty(ref enableNextNavigaion, value); }
        }
        public bool EnablePreviousNavigation
        {
            get { return enablePreviousNavigation; }
            set { SetProperty(ref enablePreviousNavigation, value); }
        }
        public RedemptionUserControl RedemptionUserControl
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionuserControl);
                return redemptionuserControl;
            }
        }
        public TicketReceiptDTO ReceiptDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(receiptDTO);
                return receiptDTO;
            }
            set
            {
                log.LogMethodEntry(receiptDTO, value);
                SetProperty(ref receiptDTO, value);
                log.LogMethodExit(receiptDTO);
            }
        }
        public ObservableCollection<LocationContainerDTO> TargetLocations
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(targetLocations);
                return targetLocations;
            }
            set
            {
                log.LogMethodEntry(targetLocations, value);
                SetProperty(ref targetLocations, value);
                log.LogMethodExit(targetLocations);
            }
        }
        public LocationContainerDTO SelectedTargetLocation
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedTargetLocation);
                return selectedTargetLocation;
            }
            set
            {
                log.LogMethodEntry(selectedTargetLocation, value);
                SetProperty(ref selectedTargetLocation, value);
                log.LogMethodExit(selectedTargetLocation);
            }
        }
        public List<RedemptionDTO> SuspendedRedemptions
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(suspendedRedemptions);
                return suspendedRedemptions;
            }
            set
            {
                log.LogMethodEntry(suspendedRedemptions, value);
                SetProperty(ref suspendedRedemptions, value);
                log.LogMethodExit(suspendedRedemptions);
            }
        }
        public List<RedemptionDTO> TodayCompletedRedemptions
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(todayCompletedRedemptions);
                return todayCompletedRedemptions;
            }
            set
            {
                log.LogMethodEntry(todayCompletedRedemptions, value);
                SetProperty(ref todayCompletedRedemptions, value);
                log.LogMethodExit(todayCompletedRedemptions);
            }
        }

        public List<RedemptionDTO> RedemptionDTOList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionDTOList);
                return redemptionDTOList;
            }
            set
            {
                log.LogMethodEntry(redemptionDTOList, value);
                SetProperty(ref redemptionDTOList, value);
                log.LogMethodExit(redemptionDTOList);
            }
        }

        public RedemptionDTO SelectedRedemptionDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedRedemptionDTO);
                return selectedRedemptionDTO;
            }
            set
            {
                log.LogMethodEntry(selectedRedemptionDTO, value);
                //if (value != null)
                //{
                //    value.ETickets = redemptionMainUserControlVM.GetETickets(value);
                //    value.ReceiptTickets = redemptionMainUserControlVM.GetReceiptTickets(value);
                //    value.CurrencyTickets = redemptionMainUserControlVM.GetCurrencyTickets(value);
                //}
                SetProperty(ref selectedRedemptionDTO, value);
                log.LogMethodExit(selectedRedemptionDTO);
            }
        }
        public TicketReceiptDTO FlaggedTicketReceiptDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(flaggedticketReceiptDTO);
                return flaggedticketReceiptDTO;
            }
            set
            {
                log.LogMethodEntry(flaggedticketReceiptDTO, value);
                SetProperty(ref flaggedticketReceiptDTO, value);
                if (value != null && !GenericToggleButtonsVM.ToggleButtonItems[1].IsChecked)
                {
                    if (GenericToggleButtonsVM.ToggleButtonItems[0].IsChecked)
                    {
                        GenericToggleButtonsVM.ToggleButtonItems[0].IsChecked = false;
                    }
                    GenericToggleButtonsVM.ToggleButtonItems[1].IsChecked = true;
                    //OnToggleChecked(null);
                }
                log.LogMethodExit(flaggedticketReceiptDTO);
            }
        }
        public bool ShowAll
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showAll);
                return showAll;
            }
            set
            {
                log.LogMethodEntry(showAll, value);
                SetProperty(ref showAll, value);
                log.LogMethodExit(showAll);
            }
        }

        public bool ShowSearchCloseIcon
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showSearchCloseIcon);
                return showSearchCloseIcon;
            }
            set
            {
                log.LogMethodEntry(showSearchCloseIcon, value);
                SetProperty(ref showSearchCloseIcon, value);
                log.LogMethodExit(showSearchCloseIcon);
            }
        }
        public bool StayInTransactionMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(stayInTransactionMode);
                return stayInTransactionMode;
            }
            set
            {
                log.LogMethodEntry(stayInTransactionMode, value);
                SetProperty(ref stayInTransactionMode, value);
                log.LogMethodExit(stayInTransactionMode);
            }
        }

        public GenericTransactionListVM GenericTransactionListVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericTransactionListVM);
                return genericTransactionListVM;
            }
            set
            {
                log.LogMethodEntry(genericTransactionListVM, value);
                SetProperty(ref genericTransactionListVM, value);
                log.LogMethodExit(genericTransactionListVM);
            }
        }
        public string LoadTotatlTicketText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loadTotatlTicketText);
                return loadTotatlTicketText;
            }
            set
            {
                log.LogMethodEntry(loadTotatlTicketText, value);
                SetProperty(ref loadTotatlTicketText, value);
                log.LogMethodExit(loadTotatlTicketText);
            }
        }
        public int LoadTotatlTicketCount
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loadTotatlTicketCount);
                return loadTotatlTicketCount;
            }
            set
            {
                log.LogMethodEntry(loadTotatlTicketCount, value);
                SetProperty(ref loadTotatlTicketCount, value);
                LoadTotatlTicketText = GetNumberFormattedString(loadTotatlTicketCount);
                log.LogMethodExit(loadTotatlTicketCount);
            }
        }

        public bool ShowNoCurrencyTextBlock
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showNoCurrencyTextBlock);
                return showNoCurrencyTextBlock;
            }
            set
            {
                log.LogMethodEntry(showNoCurrencyTextBlock, value);
                SetProperty(ref showNoCurrencyTextBlock, value);
                log.LogMethodExit(showNoCurrencyTextBlock);
            }
        }

        public bool IsVoucher
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isVoucher);
                return isVoucher;
            }
            set
            {
                log.LogMethodEntry(isVoucher, value);
                SetProperty(ref isVoucher, value);
                log.LogMethodExit(isVoucher);
            }
        }

        public bool IsTurnIn
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isTurnIn);
                return isTurnIn;
            }
            set
            {
                log.LogMethodEntry(isTurnIn, value);
                SetProperty(ref isTurnIn, value);
                log.LogMethodExit(isTurnIn);
            }
        }

        public bool IsLoadTicket
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isLoadTicket);
                return isLoadTicket;
            }
            set
            {
                log.LogMethodEntry(isLoadTicket, value);
                SetProperty(ref isLoadTicket, value);
                log.LogMethodExit(isLoadTicket);
            }
        }

        public string UserName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(userName);
                return userName;
            }
            set
            {
                log.LogMethodEntry(userName, value);
                SetProperty(ref userName, value);
                if (GenericRightSectionContentVM != null)
                {
                    GenericRightSectionContentVM.UserName = userName;
                }
                if (GenericTransactionListVM != null)
                {
                    GenericRightSectionContentVM.UserName = userName;
                }
                log.LogMethodExit(userName);
            }
        }

        public string ScreenNumber
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(screenNumber);
                return screenNumber;
            }
            set
            {
                log.LogMethodEntry(screenNumber, value);
                SetProperty(ref screenNumber, value);
                if (GenericRightSectionContentVM != null)
                {
                    GenericRightSectionContentVM.ScreenNumber = screenNumber;
                }
                if (GenericTransactionListVM != null)
                {
                    GenericRightSectionContentVM.ScreenNumber = screenNumber;
                }
                log.LogMethodExit(screenNumber);
            }
        }
        public ObservableCollection<string> StatusCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(statusCollection);
                return statusCollection;
            }
        }

        public string SelectedStatus
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedStatus);
                return selectedStatus;
            }
            set
            {
                log.LogMethodEntry(selectedStatus, value);
                SetProperty(ref selectedStatus, value);
                log.LogMethodExit(selectedStatus);
            }
        }
        public string SearchedReceiptNo
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchedReceiptNo);
                return searchedReceiptNo;
            }
            set
            {
                log.LogMethodEntry(searchedReceiptNo, value);
                SetProperty(ref searchedReceiptNo, value);
                log.LogMethodExit(screenNumber);
            }
        }

        public string SearchedCardNo
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchedCardNo);
                return searchedCardNo;
            }
            set
            {
                log.LogMethodEntry(searchedCardNo, value);
                SetProperty(ref searchedCardNo, value);
                log.LogMethodExit(searchedCardNo);
            }
        }

        public string SearchedBalanceTicketFromOrOrderNo
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchedBalanceTicketFromOrOrderNo);
                return searchedBalanceTicketFromOrOrderNo;
            }
            set
            {
                log.LogMethodEntry(searchedBalanceTicketFromOrOrderNo, value);
                SetProperty(ref searchedBalanceTicketFromOrOrderNo, value);
                log.LogMethodExit(searchedBalanceTicketFromOrOrderNo);
            }
        }

        public string SearchedBalanceTicketToOrPrdCodeDescBarcode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchedBalanceTicketToOrPrdCodeDesBarCode);
                return searchedBalanceTicketToOrPrdCodeDesBarCode;
            }
            set
            {
                log.LogMethodEntry(searchedBalanceTicketToOrPrdCodeDesBarCode, value);
                SetProperty(ref searchedBalanceTicketToOrPrdCodeDesBarCode, value);
                log.LogMethodExit(searchedBalanceTicketToOrPrdCodeDesBarCode);
            }
        }

        public string SearchedIssuedDateFrom
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchedIssuedDateFrom);
                return searchedIssuedDateFrom;
            }
            set
            {
                log.LogMethodEntry(searchedIssuedDateFrom, value);
                SetProperty(ref searchedIssuedDateFrom, value);
                log.LogMethodExit(searchedIssuedDateFrom);
            }
        }

        public string SearchedIssuedDateTo
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchedIssuedDateTo);
                return searchedIssuedDateTo;
            }
            set
            {
                log.LogMethodEntry(searchedIssuedDateTo, value);
                SetProperty(ref searchedIssuedDateTo, value);
                log.LogMethodExit(searchedIssuedDateTo);
            }
        }

        public string TransactionID
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(transactionID);
                if (string.IsNullOrEmpty(transactionID))
                {
                    TransactionID = defaultTransactionID;
                }
                return transactionID;
            }
            set
            {
                log.LogMethodEntry(transactionID, value);
                SetProperty(ref transactionID, value);
                if (GenericTransactionListVM != null)
                {
                    GenericTransactionListVM.TransactionID = transactionID;
                }
                log.LogMethodExit(transactionID);
            }
        }
        public bool IsLowerResoultion
        {
            get
            {
                log.LogMethodEntry();
                if (SystemParameters.PrimaryScreenWidth < 1024 || SystemParameters.PrimaryScreenHeight < 700)
                {
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }

            }
        }
        public bool IsChangeStatusEnable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isChangeStatusEnable);
                return isChangeStatusEnable;
            }
            set
            {
                log.LogMethodEntry(isChangeStatusEnable, value);
                SetProperty(ref isChangeStatusEnable, value);
                log.LogMethodExit(isChangeStatusEnable);
            }
        }

        public bool IsContentAreaVisible
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(iscontentAreaVisible);
                return iscontentAreaVisible;
            }
            set
            {
                log.LogMethodEntry(IsContentAreaVisible, value);
                SetProperty(ref iscontentAreaVisible, value);
                log.LogMethodExit(iscontentAreaVisible);
            }
        }

        public bool MultiScreenMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(multiScreenMode);
                return multiScreenMode;
            }
            set
            {
                log.LogMethodEntry(multiScreenMode, value);
                SetProperty(ref multiScreenMode, value);
                SetMutliScreenMode();
                log.LogMethodExit(multiScreenMode);
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
        public ICommand DatePickerLoadedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(datePickerLoadedCommand);
                return datePickerLoadedCommand;
            }
            set
            {
                log.LogMethodEntry(datePickerLoadedCommand, value);
                SetProperty(ref datePickerLoadedCommand, value);
                log.LogMethodExit(datePickerLoadedCommand);
            }
        }
        public ICommand SuspendCompleteActionsCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(suspendCompleteActionsCommand);
                return suspendCompleteActionsCommand;
            }
            set
            {
                log.LogMethodEntry(suspendCompleteActionsCommand, value);
                SetProperty(ref suspendCompleteActionsCommand, value);
                log.LogMethodExit(suspendCompleteActionsCommand);
            }
        }

        public ICommand ScanEnterCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(scanEnterCommand);
                return scanEnterCommand;
            }
            set
            {
                log.LogMethodEntry(scanEnterCommand, value);
                SetProperty(ref scanEnterCommand, value);
                log.LogMethodExit(scanEnterCommand);
            }
        }

        public ICommand SearchActionsCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchActionsCommand);
                return searchActionsCommand;
            }
            private set
            {
                log.LogMethodEntry(searchActionsCommand, value);
                SetProperty(ref searchActionsCommand, value);
                log.LogMethodExit(searchActionsCommand);
            }
        }

        public ICommand TotalTicketClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(totalTicketClickedCommand);
                return totalTicketClickedCommand;
            }
            private set
            {
                log.LogMethodEntry(totalTicketClickedCommand, value);
                SetProperty(ref totalTicketClickedCommand, value);
                log.LogMethodExit(totalTicketClickedCommand);
            }
        }

        public ICommand TransactionActionsCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(transactionActionsCommand);
                return transactionActionsCommand;
            }
            set
            {
                log.LogMethodEntry(transactionActionsCommand, value);
                SetProperty(ref transactionActionsCommand, value);
                log.LogMethodExit(transactionActionsCommand);
            }
        }

        public ICommand IsSelectedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isSelectedCommand);
                return isSelectedCommand;
            }
            set
            {
                log.LogMethodEntry(isSelectedCommand, value);
                SetProperty(ref isSelectedCommand, value);
                log.LogMethodExit(isSelectedCommand);
            }
        }


        public ICommand ShowContentAreaClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showContentAreaClickedCommand);
                return showContentAreaClickedCommand;
            }
            set
            {
                log.LogMethodEntry(showContentAreaClickedCommand, value);
                SetProperty(ref showContentAreaClickedCommand, value);
                log.LogMethodExit(showContentAreaClickedCommand);
            }
        }

        public ICommand ShowTransactionAreaClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showTransactionAreaClickedCommand);
                return showTransactionAreaClickedCommand;
            }
            set
            {
                log.LogMethodEntry(showTransactionAreaClickedCommand, value);
                SetProperty(ref showTransactionAreaClickedCommand, value);
                log.LogMethodExit(showTransactionAreaClickedCommand);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchCommand);
                return searchCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                searchCommand = value;
            }
        }

        public ICommand ShowAllClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showAllClickedCommand);
                return showAllClickedCommand;
            }
            set
            {
                log.LogMethodEntry(showAllClickedCommand, value);
                SetProperty(ref showAllClickedCommand, value);
                log.LogMethodExit(showAllClickedCommand);
            }
        }

        public ICommand ToggleCheckedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(toggleCheckedCommand);
                return toggleCheckedCommand;
            }
            set
            {
                log.LogMethodEntry(toggleCheckedCommand, value);
                SetProperty(ref toggleCheckedCommand, value);
                log.LogMethodExit(toggleCheckedCommand);
            }
        }

        public ICommand ToggleUncheckedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(toggleUncheckedCommand);
                return toggleUncheckedCommand;
            }
            set
            {
                log.LogMethodEntry(toggleUncheckedCommand, value);
                SetProperty(ref toggleUncheckedCommand, value);
                log.LogMethodExit(toggleUncheckedCommand);
            }
        }

        public ICommand ContentRenderedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(contentRenderedCommand);
                return contentRenderedCommand;
            }
            private set
            {
                log.LogMethodEntry(contentRenderedCommand, value);
                SetProperty(ref contentRenderedCommand, value);
                log.LogMethodExit(contentRenderedCommand);
            }
        }
        public ICommand ScrollChangedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(scrollChangedCommand);
                return scrollChangedCommand;
            }
            private set
            {
                log.LogMethodEntry(scrollChangedCommand, value);
                SetProperty(ref scrollChangedCommand, value);
                log.LogMethodExit(scrollChangedCommand);
            }
        }
        public ICommand ItemClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(itemClickedCommand);
                return itemClickedCommand;
            }
            private set
            {
                log.LogMethodEntry(itemClickedCommand, value);
                SetProperty(ref itemClickedCommand, value);
                log.LogMethodExit(itemClickedCommand);
            }
        }

        public ICommand ItemOfferOrInfoClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(itemOfferOrInfoClickedCommand);
                return itemOfferOrInfoClickedCommand;
            }
            private set
            {
                log.LogMethodEntry(itemOfferOrInfoClickedCommand, value);
                SetProperty(ref itemOfferOrInfoClickedCommand, value);
                log.LogMethodExit(itemOfferOrInfoClickedCommand);
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
            private set
            {
                log.LogMethodEntry(deleteCommand, value);
                SetProperty(ref deleteCommand, value);
                log.LogMethodExit(deleteCommand);
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
            private set
            {
                log.LogMethodEntry(resetCommand, value);
                SetProperty(ref resetCommand, value);
                log.LogMethodExit(resetCommand);
            }
        }

        public ICommand TransactionItemClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(transactionItemClickedCommand);
                return transactionItemClickedCommand;
            }
            set
            {
                log.LogMethodEntry(transactionItemClickedCommand, value);
                SetProperty(ref transactionItemClickedCommand, value);
                log.LogMethodExit(transactionItemClickedCommand);
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

        public ICommand PreviousNavigationCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(previousNavigationCommand);
                return previousNavigationCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                previousNavigationCommand = value;
            }
        }

        public ICommand NextNavigationCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(nextNavigationCommand);
                return nextNavigationCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                nextNavigationCommand = value;
            }
        }

        public RedemptionsType RedemptionsType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionsType);
                return redemptionsType;
            }
            private set
            {
                log.LogMethodEntry(redemptionsType, value);
                SetProperty(ref redemptionsType, value);
                log.LogMethodExit(redemptionsType);
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

        public DisplayTagsVM RightSectionDisplayTagsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rightSectionDisplayTagsVM);
                return rightSectionDisplayTagsVM;
            }
            set
            {
                log.LogMethodEntry(rightSectionDisplayTagsVM, value);
                SetProperty(ref rightSectionDisplayTagsVM, value);
                log.LogMethodExit(rightSectionDisplayTagsVM);
            }
        }

        public GenericRightSectionContentVM GenericRightSectionContentVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericRightSectionContentVM);
                return genericRightSectionContentVM;
            }
            set
            {
                log.LogMethodEntry(genericRightSectionContentVM, value);
                SetProperty(ref genericRightSectionContentVM, value);
                log.LogMethodExit(genericRightSectionContentVM);
            }
        }

        public CustomDataGridVM CustomDataGridVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(customDataGridVM);
                if (customDataGridVM == null)
                {
                    customDataGridVM = new CustomDataGridVM(this.ExecutionContext) { IsComboAndSearchVisible = false };
                }
                SetCustomDataGridBackground();
                return customDataGridVM;
            }
            set
            {
                log.LogMethodEntry(customDataGridVM, value);
                SetProperty(ref customDataGridVM, value);
                log.LogMethodExit(customDataGridVM);
            }
        }
        public GenericDisplayItemsVM GenericDisplayItemsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericDisplayItemsVM);
                return genericDisplayItemsVM;
            }
            set
            {
                log.LogMethodEntry(genericDisplayItemsVM, value);
                SetProperty(ref genericDisplayItemsVM, value);
                log.LogMethodExit(genericDisplayItemsVM);
            }
        }
        #endregion

        #region Methods
        private void SetLoadingVisible(bool isVisible)
        {
            log.LogMethodEntry(isVisible);
            if (redemptionMainUserControlVM != null && !Equals(redemptionMainUserControlVM.IsLoadingVisible, isVisible))
            {
                redemptionMainUserControlVM.IsLoadingVisible = isVisible;
                RaiseCanExecuteChanged();
            }
            log.LogMethodExit();
        }
        internal void ExecuteActionWithMainUserControlFooter(Action method)
        {
            log.LogMethodEntry();
            try
            {
                method();
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                log.LogMethodExit();
            }
        }
        internal void TranslatePage()
        {
            log.LogMethodEntry();
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (redemptionuserControl != null)
                {
                    redemptionuserControl.UpdateLayout();
                    TranslateHelper.Translate(this.ExecutionContext, redemptionuserControl);
                }
            });
            log.LogMethodExit();
        }

        internal void OnSearchClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                SetFooterContentEmpty();
                if (ShowSearchCloseIcon)
                {
                    PerformSearch();
                    return;
                }
                if (redemptionsType == RedemptionsType.New && !isLoadTicket)
                {
                    ShowSearchArea();
                }
                else if (SearchFilterVisbility == Visibility.Collapsed)
                {
                    SearchFilterVisbility = Visibility.Visible;
                }
                else
                {
                    SearchFilterVisbility = redemptionsType == RedemptionsType.Completed || isVoucher ? Visibility.Visible : Visibility.Collapsed;
                }
                if (SearchFilterVisbility == Visibility.Visible && redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetKeyboardWindow();
                }
                if (redemptionsType == RedemptionsType.Completed)
                {
                    SetSearchDates();
                }
            });
            log.LogMethodExit();
        }
        internal void ClearSearchFields()
        {
            log.LogMethodEntry();
            SearchedReceiptNo = string.Empty;
            SearchedCardNo = string.Empty;
            SetSearchDates();
            SearchedBalanceTicketFromOrOrderNo = string.Empty;
            SearchedBalanceTicketToOrPrdCodeDescBarcode = string.Empty;
            log.LogMethodExit();
        }
        internal void OnSearchActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                SetFooterContentEmpty();
                if (parameter != null)
                {
                    Button button = parameter as Button;
                    if (button != null && !string.IsNullOrEmpty(button.Name))
                    {
                        switch (button.Name.ToLower())
                        {
                            case "btnclear":
                                {
                                    ClearSearchFields();
                                }
                                break;
                            case "btnactionsearch":
                                {                                   
                                    PerformSearch();
                                }
                                break;
                        }
                    }
                }
            });
            log.LogMethodExit();
        }

        private void OnShowAllClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetLoadingVisible(true);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                SetFooterContentEmpty();
                if (redemptionsType == RedemptionsType.Suspended)
                {
                    if (customDataGridVM != null && customDataGridVM.SelectedItem != null)
                    {
                        customDataGridVM.SelectedItem = null;
                    }
                    OnToggleChecked(null);
                }
            });
            SetLoadingVisible(false);
            log.LogMethodExit();
        }

        internal async Task UpdateVoucherUI(string remarks)
        {
            log.LogMethodEntry(remarks);
            receiptDTO = null;
            int index = -1;
            receiptDTO = GetSelectedVoucher(ref index);
            if (receiptDTO != null)
            {
                string footerContent = string.Empty;
                switch (redemptionsType)
                {
                    case RedemptionsType.Voucher:
                        {
                            receiptDTO.IsSuspected = true;
                            footerContent = MessageViewContainerList.GetMessage(ExecutionContext, 1526, receiptDTO.ManualTicketReceiptNo);
                        }
                        break;
                    case RedemptionsType.Flagged:
                        {
                            receiptDTO.IsSuspected = false;
                            footerContent = MessageViewContainerList.GetMessage(ExecutionContext, 2917, receiptDTO.ManualTicketReceiptNo);
                        }
                        break;
                }
                bool result = await UpdateVoucher(receiptDTO, remarks);
                if (result)
                {
                    AddToVoucherList(index);
                    redemptionMainUserControlVM.SetFooterContent(footerContent, MessageType.Info);
                    SetOtherRedemptionList();
                }
            }
            receiptDTO = null;
            log.LogMethodExit();
        }
        internal void AddToVoucherList(int index)
        {
            log.LogMethodEntry();
            ExecuteActionWithMainUserControlFooter(() =>
            {
                List<TicketReceiptDTO> ticketReceipts = new List<TicketReceiptDTO>();
                if (currentDayTicketReceiptList != null && currentDayTicketReceiptList.Any(x => x.Id == receiptDTO.Id))
                {
                    currentDayTicketReceiptList[index] = receiptDTO;
                    ticketReceipts = currentDayTicketReceiptList;
                }
                else if (ticketReceiptDTOList != null && ticketReceiptDTOList.Any(x => x.Id == receiptDTO.Id))
                {
                    ticketReceiptDTOList[index] = receiptDTO;
                    ticketReceipts = ticketReceiptDTOList;
                }
                else if (flaggedticketReceiptDTO != null)
                {
                    ticketReceipts.Add(flaggedticketReceiptDTO);
                }
                if (redemptionsType == RedemptionsType.Flagged)
                {
                    if (ticketReceipts != null)
                    {
                        ticketReceipts = ticketReceipts.Where(t => t.IsSuspected == true).ToList();
                    }
                    if (ticketReceipts.Count > 0)
                    {
                        SetCustomDataGridVM(ticketReceipts: ticketReceipts);
                    }
                    else if (CustomDataGridVM != null)
                    {
                        CustomDataGridVM.Clear();
                    }
                }
                else
                {
                    OnIsSelected(null);
                }
                if (ticketReceipts != null)
                {
                    SetCompletedSuspenedCount(RedemptionsType.Voucher, ticketReceipts.Where(t => t.IsSuspected == true).ToList().Count);
                }
            });
        }
        private string GetDateString(DateTime dateText)
        {
            log.LogMethodEntry(dateText);
            string date = "-";
            if (dateText.ToString() != DateTime.MinValue.ToString())
            {
                date = dateText.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT"));
            }
            log.LogMethodExit();
            return date;
        }
        internal string GetPropertyValueWithFormat(PropertyInfo prop, object propValue)
        {
            log.LogMethodEntry();
            string value = "-";
            if (propValue != null)
            {
                if (propValue is IEnumerable<object>)
                {
                    value = GetNumberFormattedString((propValue as IEnumerable<object>).Count());
                }
                else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    value = GetDateString((DateTime)propValue);
                }
                else if (prop.PropertyType == typeof(Int32))
                {
                    if (prop.Name == "CurrencyTickets" || prop.Name == "ETickets" ||
                        prop.Name == "GraceTickets" || prop.Name == "ManualTickets" ||
                        prop.Name == "ReceiptTickets" || prop.Name == "ReprintCount" ||
                        prop.Name == "BalanceTickets" || prop.Name == "Tickets")
                    {
                        value = GetNumberFormattedString((Int32)propValue);
                    }
                    else
                    {
                        if ((Int32)propValue >= 0)
                        {
                            value = ((Int32)propValue).ToString();
                        }
                        else
                        {
                            value = "-";
                        }

                    }

                }
                else if (prop.PropertyType == typeof(Double))
                {
                    value = GetNumberFormattedString(Convert.ToInt32(propValue));
                }
                else if (propValue.ToString() != "-1")
                {
                    value = propValue.ToString();
                }
            }
            log.LogMethodExit();
            return value;
        }
        private void SetVocuherContentArea(List<TicketReceiptDTO> voucherList)
        {
            log.LogMethodEntry(voucherList);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (voucherList != null && voucherList.Count > 0)
                {
                    SetCustomDataGridVM(ticketReceipts: voucherList);
                    return;
                }
                else
                {
                    CustomDataGridVM.Clear();
                }
            });
            log.LogMethodExit();
        }
        internal TicketReceiptDTO GetSelectedVoucher(ref int index)
        {
            log.LogMethodEntry();
            TicketReceiptDTO selectedTicketReceiptDTO = null;
            try
            {
                if (isVoucher && CustomDataGridVM != null && CustomDataGridVM.SelectedItem != null)
                {
                    selectedTicketReceiptDTO = CustomDataGridVM.SelectedItem as TicketReceiptDTO;
                    if (currentDayTicketReceiptList != null && currentDayTicketReceiptList.Count > 0)
                    {
                        index = currentDayTicketReceiptList.IndexOf(selectedTicketReceiptDTO);
                    }
                    if (index < 0 && ticketReceiptDTOList != null && ticketReceiptDTOList.Count > 0)
                    {
                        index = ticketReceiptDTOList.IndexOf(selectedTicketReceiptDTO);
                    }
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            log.LogMethodExit();
            return selectedTicketReceiptDTO;
        }

        internal async Task PerformSearch()
        {
            log.LogMethodEntry();
            from = null;
            to = null;
            try
            {
                if (!string.IsNullOrEmpty(SearchedIssuedDateFrom))
                {
                    from = DateTime.ParseExact(SearchedIssuedDateFrom, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATE_FORMAT"), null);
                }
                if (!string.IsNullOrEmpty(SearchedIssuedDateTo))
                {
                    to = DateTime.ParseExact(SearchedIssuedDateTo, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATE_FORMAT"), null);
                }
                if (isLoadTicket && GenericDisplayItemsVM != null
                    && this.redemptionsType == RedemptionsType.New)
                {
                    List<RedemptionCurrencyContainerDTO> currencyContainerDTOs = mainVM.GetRedemptionCurrencyContainerDTOList(this.ExecutionContext);
                    if (!ShowSearchCloseIcon && currencyContainerDTOs != null)
                    {
                        if (!string.IsNullOrEmpty(SearchedCardNo) &&
                            !string.IsNullOrWhiteSpace(SearchedCardNo))
                        {
                            currencyContainerDTOs = currencyContainerDTOs.
                                Where(c => c.CurrencyName != null && c.CurrencyName.ToLower().Contains(SearchedCardNo.ToLower())).ToList();
                        }
                        int fromTicket = 0;
                        int toTicket = 0;
                        if (!string.IsNullOrEmpty(SearchedBalanceTicketFromOrOrderNo) &&
                            !string.IsNullOrWhiteSpace(SearchedBalanceTicketFromOrOrderNo))
                        {
                            fromTicket = Int32.Parse(SearchedBalanceTicketFromOrOrderNo);
                        }
                        if (!string.IsNullOrEmpty(SearchedBalanceTicketToOrPrdCodeDescBarcode) &&
                            !string.IsNullOrWhiteSpace(SearchedBalanceTicketToOrPrdCodeDescBarcode))
                        {
                            toTicket = Int32.Parse(SearchedBalanceTicketToOrPrdCodeDescBarcode);
                        }
                        if (!string.IsNullOrEmpty(SearchedBalanceTicketFromOrOrderNo) &&
                            !string.IsNullOrWhiteSpace(SearchedBalanceTicketFromOrOrderNo)&&
                            !string.IsNullOrEmpty(SearchedBalanceTicketToOrPrdCodeDescBarcode) &&
                            !string.IsNullOrWhiteSpace(SearchedBalanceTicketToOrPrdCodeDescBarcode)
                            && fromTicket > toTicket)
                        {
                            redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 2639), MessageType.Info);
                            return;
                        }
                        if (fromTicket >= 0)
                        {
                            currencyContainerDTOs = currencyContainerDTOs.Where(c => c.ValueInTickets >= fromTicket).ToList();
                        }
                        if (toTicket > 0)
                        {
                            currencyContainerDTOs = currencyContainerDTOs.Where(c => c.ValueInTickets <= toTicket).ToList();
                        }
                        ShowSearchCloseIcon = true;
                    }
                    else
                    {
                        ShowSearchCloseIcon = false;
                    }
                    GenericDisplayItemsVM.DisplayItemModels = new ObservableCollection<object>(currencyContainerDTOs.Cast<object>().ToList());
                    //GenericDisplayItemsVM.UIDisplayItemModels.Clear();
                    GenericDisplayItemsVM.SetUIDisplayItemModels();
                }
                else if (redemptionsType == RedemptionsType.New && mainVM != null)
                {
                    int fromPrice = 0;
                    int toPrice = 0;
                    if(!string.IsNullOrWhiteSpace(SearchedBalanceTicketFromOrOrderNo))
                    {
                        int.TryParse(SearchedBalanceTicketFromOrOrderNo, out fromPrice);
                    }
                    if(!string.IsNullOrWhiteSpace(SearchedBalanceTicketToOrPrdCodeDescBarcode))
                    {
                        int.TryParse(SearchedBalanceTicketToOrPrdCodeDescBarcode, out toPrice);
                    }
                    if (fromPrice > toPrice)
                    {
                        redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2639), MessageType.Warning);
                        log.LogMethodExit("from price is less than to price.");
                        return;
                    }
                    ShowSearchCloseIcon = !showSearchCloseIcon;
                    ShowSearchArea();
                    SetPaginationDefaults();
                    ResetGenericDisplayItemsVM();
                    if (showSearchCloseIcon)
                    {
                        await PerformProductSearch(SearchedReceiptNo, SearchedCardNo, fromPrice, toPrice, pageNumber, pageSize);
                    }
                    return;
                }
                else if (RedemptionsType == RedemptionsType.Completed)
                {
                    if (!ShowSearchCloseIcon)
                    {
                        if (!string.IsNullOrEmpty(SearchedReceiptNo) || !string.IsNullOrEmpty(SearchedBalanceTicketFromOrOrderNo)
                            || !string.IsNullOrEmpty(SearchedBalanceTicketToOrPrdCodeDescBarcode))
                        {
                            from = null;
                            to = null;
                        }
                        if (from != null && to != null)
                        {
                            DateTime fromDate = (DateTime)from;
                            DateTime toDate = (DateTime)to;
                            if (fromDate > toDate)
                            {
                                redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 724), MessageType.Info);
                                return;
                            }
                            if (toDate.Subtract(fromDate).TotalDays > 365 && string.IsNullOrEmpty(SearchedCardNo) &&
                                string.IsNullOrEmpty(SearchedBalanceTicketFromOrOrderNo) && string.IsNullOrEmpty(SearchedBalanceTicketToOrPrdCodeDescBarcode))
                            {
                                redemptionMainUserControlVM.OpenGenericMessagePopupView(
                                    MessageViewContainerList.GetMessage(this.ExecutionContext, 2911),
                                    string.Empty,
                                    MessageViewContainerList.GetMessage(this.ExecutionContext, 2667),
                                    MessageViewContainerList.GetMessage(this.ExecutionContext, "YES", null),
                                    MessageViewContainerList.GetMessage(this.ExecutionContext, "NO", null),
                                    MessageButtonsType.OkCancel);
                                if (redemptionMainUserControlVM.MessagePopupView != null)
                                {
                                    redemptionMainUserControlVM.MessagePopupView.Closed += OnDateRangeMessagePopupClosed;
                                    redemptionMainUserControlVM.MessagePopupView.Show();
                                }
                                return;
                            }
                            if (!string.IsNullOrEmpty(SearchedCardNo) && SearchedCardNo.Length < 4)
                            {
                                redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 2256), MessageType.Info);
                                return;
                            }
                        }
                        PerformCompletedSearch();
                    }
                    else
                    {
                        ShowSearchCloseIcon = false;
                        SetContentRightSectionVM();
                        if (todayCompletedRedemptions != null && todayCompletedRedemptions.Any())
                        {
                            todayCompletedRedemptions.Clear();
                            SetCompletedSuspenedCount(RedemptionsType.Completed, todayCompletedRedemptions.Count);
                        }
                        if (!isTurnIn)
                        {
                            RenderCompleteValues(todayCompletedRedemptions);
                        }
                        else
                        {
                            RenderCompleteValues(todayCompletedRedemptions.Where(r => r.Remarks != null && r.Remarks.ToLower() == "TURNINREDEMPTION".ToLower()).ToList());
                        }
                    }
                }
                else if (isVoucher)
                {
                    if (!ShowSearchCloseIcon)
                    {
                        if (!string.IsNullOrEmpty(SearchedReceiptNo) || !string.IsNullOrEmpty(SearchedCardNo))
                        {
                            from = null;
                            to = null;
                        }
                        if (from != null && to != null)
                        {
                            DateTime fromDate = (DateTime)from;
                            DateTime toDate = (DateTime)to;
                            if (fromDate > toDate)
                            {
                                redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 724), MessageType.Info);
                                return;
                            }
                            if (toDate.Subtract(fromDate).TotalDays > 365 && string.IsNullOrEmpty(SearchedCardNo) &&
                                string.IsNullOrEmpty(SearchedBalanceTicketFromOrOrderNo) && string.IsNullOrEmpty(SearchedBalanceTicketToOrPrdCodeDescBarcode))
                            {
                                redemptionMainUserControlVM.OpenGenericMessagePopupView(
                                    MessageViewContainerList.GetMessage(this.ExecutionContext, 2911),
                                    string.Empty,
                                    MessageViewContainerList.GetMessage(this.ExecutionContext, 2667),
                                    MessageViewContainerList.GetMessage(this.ExecutionContext, "YES", null),
                                    MessageViewContainerList.GetMessage(this.ExecutionContext, "NO", null),
                                    MessageButtonsType.OkCancel);
                                if (redemptionMainUserControlVM.MessagePopupView != null)
                                {
                                    redemptionMainUserControlVM.MessagePopupView.Closed += OnVoucherDateRangeMessagePopupClosed;
                                    redemptionMainUserControlVM.MessagePopupView.Show();
                                }
                                return;
                            }
                            if (!string.IsNullOrEmpty(SearchedCardNo) && SearchedCardNo.Length < 4)
                            {
                                redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 2256), MessageType.Info);
                                return;
                            }
                        }
                        int fromPrice = 0;
                        int toPrice = 0;
                        if (!string.IsNullOrEmpty(SearchedBalanceTicketFromOrOrderNo) &&
                            !string.IsNullOrWhiteSpace(SearchedBalanceTicketFromOrOrderNo))
                        {
                            fromPrice = Int32.Parse(SearchedBalanceTicketFromOrOrderNo);
                        }
                        if (!string.IsNullOrEmpty(SearchedBalanceTicketToOrPrdCodeDescBarcode) &&
                            !string.IsNullOrWhiteSpace(SearchedBalanceTicketToOrPrdCodeDescBarcode))
                        {
                            toPrice = Int32.Parse(SearchedBalanceTicketToOrPrdCodeDescBarcode);
                        }
                        if (!string.IsNullOrEmpty(SearchedBalanceTicketFromOrOrderNo) &&
                            !string.IsNullOrWhiteSpace(SearchedBalanceTicketFromOrOrderNo) &&
                            !string.IsNullOrEmpty(SearchedBalanceTicketToOrPrdCodeDescBarcode) &&
                            !string.IsNullOrWhiteSpace(SearchedBalanceTicketToOrPrdCodeDescBarcode)
                            && fromPrice > toPrice)
                        {
                            redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 2639), MessageType.Info);
                            return;
                        }
                        PerformVoucherSearch();
                    }
                    else
                    {
                        ShowSearchCloseIcon = false;
                        SetContentRightSectionVM();
                        if (currentDayTicketReceiptList != null && currentDayTicketReceiptList.Any())
                        {
                            currentDayTicketReceiptList.Clear();
                            SetCompletedSuspenedCount(RedemptionsType.Voucher, currentDayTicketReceiptList.Count);
                        }
                        if (redemptionsType == RedemptionsType.Flagged)
                        {
                            List<TicketReceiptDTO> flaggedList = currentDayTicketReceiptList.Where(c => c.IsSuspected == true).ToList();
                            SetVocuherContentArea(flaggedList);
                            SetCompletedSuspenedCount(RedemptionsType.Voucher, flaggedList.Count);
                        }
                        else
                        {
                            SetVocuherContentArea(currentDayTicketReceiptList);
                        }
                    }
                }
                SearchFilterVisbility = !showSearchCloseIcon && (redemptionsType == RedemptionsType.Completed || isVoucher) ?
                    Visibility.Visible : Visibility.Collapsed;
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            log.LogMethodExit();

        }

        private async void PerformVoucherSearch()
        {
            log.LogMethodEntry();
            try
            {
                bool flagged = false;
                if (redemptionsType == RedemptionsType.Flagged)
                {
                    flagged = true;
                }
                ShowSearchCloseIcon = true;
                ticketReceiptDTOList = await GetVouchers(SearchedReceiptNo, SearchedCardNo, SearchedBalanceTicketFromOrOrderNo, SearchedBalanceTicketToOrPrdCodeDescBarcode,
                from, to, flagged);
                SetContentRightSectionVM();
                SetVocuherContentArea(ticketReceiptDTOList);
                if (flagged)
                {
                    SetCompletedSuspenedCount(RedemptionsType.Voucher, ticketReceiptDTOList.Where(t => t.IsSuspected == true).ToList().Count);
                }
                else
                {
                    SetCompletedSuspenedCount(RedemptionsType.Voucher, currentDayTicketReceiptList.Where(c => c.IsSuspected == true).ToList().Count);
                }
                SetOtherRedemptionList(RedemptionUserControlVM.ActionType.VoucherSearch);
                if (currentDayTicketReceiptList != null && ticketReceiptDTOList != null)
                {
                    foreach (TicketReceiptDTO searchedTicket in ticketReceiptDTOList)
                    {
                        TicketReceiptDTO ticket = currentDayTicketReceiptList.FirstOrDefault(r => r.Id == searchedTicket.Id);
                        if (ticket != null)
                        {
                            int index = currentDayTicketReceiptList.IndexOf(ticket);
                            currentDayTicketReceiptList[index] = searchedTicket;
                        }
                    }
                }
                SearchFilterVisbility = Visibility.Collapsed;
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            log.LogMethodExit();
        }

        private async void PerformCompletedSearch()
        {
            log.LogMethodEntry();
            try
            {
                ShowSearchCloseIcon = true;
                redemptionDTOList = await GetRedemptions(SearchedReceiptNo, SearchedBalanceTicketFromOrOrderNo, SearchedCardNo, selectedStatus,
                from, to, SearchedBalanceTicketToOrPrdCodeDescBarcode);
                if (IsTurnIn)
                {
                    redemptionDTOList = redemptionDTOList.Where(r => r.Remarks != null && r.Remarks.ToLower() == "TURNINREDEMPTION".ToLower()).ToList();
                }
                else if (isLoadTicket)
                {
                    redemptionDTOList = redemptionDTOList.Where(r => r.RedemptionGiftsListDTO == null ||
                    (r.RedemptionGiftsListDTO != null && !r.RedemptionGiftsListDTO.Any())).ToList();
                }
                else
                {
                    redemptionDTOList = redemptionDTOList.Where(r => r.Remarks == null || (r.Remarks != null && r.Remarks.ToLower() != "TURNINREDEMPTION".ToLower())).ToList();
                    redemptionDTOList = redemptionDTOList.Where(r => r.RedemptionGiftsListDTO != null && r.RedemptionGiftsListDTO.Any()).ToList();
                }
                SetContentRightSectionVM();
                SetCustomDataGridVM(completedOrSuspendedRedemptions: redemptionDTOList);
                SetOtherRedemptionList(RedemptionUserControlVM.ActionType.CompleteSearch);
                if (todayCompletedRedemptions != null && redemptionDTOList != null)
                {
                    foreach (RedemptionDTO searchedRedemption in redemptionDTOList)
                    {
                        RedemptionDTO redemption = todayCompletedRedemptions.FirstOrDefault(r => r.RedemptionId == searchedRedemption.RedemptionId);
                        if (redemption != null)
                        {
                            int index = todayCompletedRedemptions.IndexOf(redemption);
                            if (index > -1)
                            {
                                todayCompletedRedemptions[index] = searchedRedemption;
                            }
                        }
                    }
                }
                SearchFilterVisbility = Visibility.Collapsed;
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            log.LogMethodExit();
        }

        private void OnVoucherDateRangeMessagePopupClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok)
                {
                    PerformVoucherSearch();
                }
                SetMainViewFocus();
            });
            log.LogMethodExit();
        }

        private void OnDateRangeMessagePopupClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok)
                {
                    PerformCompletedSearch();
                }
                SetMainViewFocus();
            });
            log.LogMethodExit();
        }
        internal string GetNumberFormattedString(int count)
        {
            log.LogMethodEntry(count);
            string numberFormattedString = "-";
            numberFormattedString = count.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT"));
            log.LogMethodExit();
            return numberFormattedString;
        }
        internal void OnToggleChecked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                ClearSearchFields();
                if (GenericToggleButtonsVM != null && GenericToggleButtonsVM.SelectedToggleButtonItem != null)
                {
                    if (showSearchCloseIcon && parameter != null)
                    {
                        if (GenericToggleButtonsVM.SelectedToggleButtonItem.Key.ToLower() == this.redemptionsType.ToString().ToLower())
                        {
                            return;
                        }
                        else if (GenericToggleButtonsVM.SelectedToggleButtonItem.Key.ToLower() != this.redemptionsType.ToString().ToLower())
                        {
                            ShowSearchCloseIcon = false;
                        }
                    }
                    if (IsTurnIn || isLoadTicket)
                    {
                        int count = 0;
                        if (IsTurnIn)
                        {
                            count = todayCompletedRedemptions != null ? todayCompletedRedemptions.Where(c => c.Remarks != null && c.Remarks.ToLower() == "TURNINREDEMPTION".ToLower()).ToList().Count : 0;
                        }
                        if (isLoadTicket)
                        {
                            count = todayCompletedRedemptions != null ? todayCompletedRedemptions.Where(c => !c.RedemptionGiftsListDTO.Any()).ToList().Count : 0;
                        }
                        SetCompletedSuspenedCount(RedemptionsType.Completed, count);
                    }
                    else if (GenericToggleButtonsVM.ToggleButtonItems.Count > 2)
                    {
                        SetCompletedSuspenedCount(RedemptionsType.Completed, todayCompletedRedemptions != null ? todayCompletedRedemptions.Where(c => (c.Remarks == null || c.Remarks.ToLower() != "TURNINREDEMPTION".ToLower())
                         && c.RedemptionGiftsListDTO.Count > 0).ToList().Count : 0);
                        SetCompletedSuspenedCount(RedemptionsType.Suspended, suspendedRedemptions != null ? suspendedRedemptions.Where(s => (s.CreatedBy != null && s.CreatedBy.ToLower() == ExecutionContext.UserId.ToLower())
                        || (s.LastUpdatedBy.ToLower() == this.ExecutionContext.UserId.ToLower())).Count() : 0);
                    }
                    switch (GenericToggleButtonsVM.SelectedToggleButtonItem.Key.ToLower())
                    {
                        case "new":
                            {
                                if (SearchFilterVisbility == Visibility.Visible)
                                {
                                    SearchFilterVisbility = Visibility.Collapsed;
                                }
                                RedemptionsType = RedemptionsType.New;
                                ShowNoCurrencyTextBlock = false;
                                if (isLoadTicket)
                                {
                                    if (GenericDisplayItemsVM != null
                          && GenericDisplayItemsVM.DisplayItemModels != null && GenericDisplayItemsVM.DisplayItemModels.Count <= 0)
                                    {
                                        ShowNoCurrencyTextBlock = true;
                                    }
                                    if (GenericDisplayItemsVM != null && (GenericDisplayItemsVM.CurrentDisplayItemModels == null ||
                            !GenericDisplayItemsVM.CurrentDisplayItemModels.Any()))
                                    {
                                        GenericDisplayItemsVM.SetUIDisplayItemModels();
                                    }
                                }
                                else
                                {
                                    if (AutoShowSearchSection)
                                    {
                                        SearchFilterVisbility = Visibility.Visible;
                                    }
                                    else
                                    {
                                        SetPaginationDefaults();
                                        ResetGenericDisplayItemsVM();
                                        PerformProductSearch(string.Empty, string.Empty, 0, 0, pageNumber, pageSize);
                                    }
                                }
                                if (redemptionMainUserControlVM != null && redemptionMainUserControlVM.RedemptionDTO != null
                                && !string.IsNullOrEmpty(redemptionMainUserControlVM.RedemptionDTO.RedemptionOrderNo))
                                {
                                    TransactionID = redemptionMainUserControlVM.RedemptionDTO.RedemptionOrderNo;
                                }
                                else
                                {
                                    TransactionID = defaultTransactionID;
                                }
                            }
                            break;
                        case "suspended":
                            {
                                if (SearchFilterVisbility == Visibility.Visible)
                                {
                                    SearchFilterVisbility = Visibility.Collapsed;
                                }
                                RedemptionsType = RedemptionsType.Suspended;
                                SetContentRightSectionVM();
                                PerformSuspended();
                            }
                            break;
                        case "completed":
                            {
                                if (todayCompletedRedemptions != null && todayCompletedRedemptions.Any())
                                {
                                    todayCompletedRedemptions.Clear();
                                }
                                if (isLoadTicket && showNoCurrencyTextBlock)
                                {
                                    ShowNoCurrencyTextBlock = false;
                                }
                                if (RedemptionsType != RedemptionsType.Completed && SearchFilterVisbility == Visibility.Visible)
                                {
                                    SearchFilterVisbility = Visibility.Collapsed;
                                }
                                RedemptionsType = RedemptionsType.Completed;
                                SearchFilterVisbility = Visibility.Visible;
                                SetContentRightSectionVM();
                                if (todayCompletedRedemptions != null && todayCompletedRedemptions.Count > 0)
                                {
                                    RenderCompleteValues(todayCompletedRedemptions);
                                }
                                else if (CustomDataGridVM != null)
                                {
                                    CustomDataGridVM.Clear();
                                }
                                SetCompletedSuspenedCount(RedemptionsType.Completed, 0);
                            }
                            break;
                        case "vouchers":
                            {
                                FlaggedTicketReceiptDTO = null;
                                if (currentDayTicketReceiptList != null && currentDayTicketReceiptList.Any())
                                {
                                    currentDayTicketReceiptList.Clear();
                                }
                                if (RedemptionsType != RedemptionsType.Voucher && SearchFilterVisbility == Visibility.Visible)
                                {
                                    SearchFilterVisbility = Visibility.Collapsed;
                                }
                                this.RedemptionsType = RedemptionsType.Voucher;
                                SearchFilterVisbility = Visibility.Visible;
                                SetContentRightSectionVM();
                                if (currentDayTicketReceiptList != null && currentDayTicketReceiptList.Count > 0)
                                {
                                    SetVocuherContentArea(currentDayTicketReceiptList);
                                }
                                else if (CustomDataGridVM != null)
                                {
                                    CustomDataGridVM.Clear();
                                }
                                if (GenericToggleButtonsVM != null && GenericToggleButtonsVM.ToggleButtonItems != null
                                        && GenericToggleButtonsVM.ToggleButtonItems.Count > 1)
                                {
                                    SetCompletedSuspenedCount(RedemptionsType.Voucher, currentDayTicketReceiptList != null ? currentDayTicketReceiptList.Where(t => t.IsSuspected == true).ToList().Count
                                        : 0);
                                }
                            }
                            break;
                        case "flagged":
                            {
                                if (RedemptionsType != RedemptionsType.Flagged && SearchFilterVisbility == Visibility.Visible)
                                {
                                    SearchFilterVisbility = Visibility.Collapsed;
                                }
                                this.RedemptionsType = RedemptionsType.Flagged;
                                SearchFilterVisbility = Visibility.Visible;
                                SetContentRightSectionVM();
                                if (currentDayTicketReceiptList != null && currentDayTicketReceiptList.Any())
                                {
                                    currentDayTicketReceiptList.Clear();
                                }
                                List<TicketReceiptDTO> flaggedList = new List<TicketReceiptDTO>();
                                if (this.FlaggedTicketReceiptDTO != null)
                                {
                                    flaggedList.Add(this.FlaggedTicketReceiptDTO);
                                }
                                else if (currentDayTicketReceiptList != null)
                                {
                                    flaggedList = currentDayTicketReceiptList.Where(t => t.IsSuspected == true).ToList();
                                }
                                if (flaggedList != null && flaggedList.Count > 0)
                                {
                                    SetVocuherContentArea(flaggedList);
                                }
                                else if (CustomDataGridVM != null)
                                {
                                    CustomDataGridVM.Clear();
                                }
                                SetCompletedSuspenedCount(RedemptionsType.Voucher, flaggedList != null ? flaggedList.Count : 0);
                            }
                            break;
                    }
                    if (ShowAll && redemptionsType != RedemptionsType.Suspended)
                    {
                        ShowAll = false;
                    }
                }
                SetMutliScreenMode();
                if (redemptionMainUserControlVM != null)
                {
                    if (string.IsNullOrWhiteSpace(redemptionMainUserControlVM.DeviceErrorMessage))
                    {
                        SetFooterContentEmpty();
                    }
                    redemptionMainUserControlVM.OnFooterSideBarButtonClicked(null);
                    if (parameter != null && redemptionsType != RedemptionsType.New)
                    {
                        uiClicked = true;
                        SetFooterContent();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(redemptionMainUserControlVM.DeviceErrorMessage))
                        {
                            SetFooterContentEmpty();
                        }
                    }
                }
                TranslatePage();
            });
            log.LogMethodExit();
        }
        private async void PerformSuspended()
        {
            log.LogMethodEntry();
            if (showAll)
            {
                List<RedemptionDTO> redemptionDTOList = await GetRedemptions(null, null, null, "SUSPENDED", null, null, null);
                if (redemptionDTOList == null)
                {
                    redemptionDTOList = new List<RedemptionDTO>();
                }
            }
            else
            {
                redemptionDTOList = suspendedRedemptions.Where(s => (s.CreatedBy != null && s.CreatedBy.ToLower() == ExecutionContext.UserId.ToLower())
                || (s.LastUpdatedBy != null && s.LastUpdatedBy.ToLower() == ExecutionContext.UserId.ToLower())).ToList();
            }
            if (redemptionDTOList.Count > 0)
            {
                SetCustomDataGridVM(completedOrSuspendedRedemptions: redemptionDTOList);
                SetCompletedSuspenedCount(RedemptionsType, redemptionDTOList.Count);
            }
            else if (CustomDataGridVM != null)
            {
                CustomDataGridVM.Clear();
            }
            log.LogMethodExit();
        }
        private void SetFooterContent()
        {
            log.LogMethodEntry();
            if (redemptionMainUserControlVM != null)
            {
                switch (redemptionsType)
                {
                    case RedemptionsType.Suspended:
                        {
                            redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 3028), MessageType.Info);
                        }
                        break;
                    case RedemptionsType.Completed:
                        {
                            redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 3027), MessageType.Info);
                        }
                        break;
                    case RedemptionsType.Voucher:
                    case RedemptionsType.Flagged:
                        {
                            redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 3029), MessageType.Info);
                        }
                        break;
                }
            }
            log.LogMethodExit();
        }
        private void SetSuspendDataGrid(List<RedemptionDTO> suspendedRedemptions)
        {
            log.LogMethodEntry();
            CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(suspendedRedemptions.OrderByDescending(x => x.RedemptionId));
            CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                            {
                                { "RedeemedDate", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Time")
                                , DataGridColumnStringFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT") } },
                                { "RedemptionId", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Id")} },
                                { "ReceiptTickets", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Ticket"),
                                    Properties = new Dictionary<string, ArthemeticOperationType>()
                                    {
                                        { "ReceiptTickets", ArthemeticOperationType.Add },
                                        { "CurrencyTickets", ArthemeticOperationType.Add },
                                        { "ManualTickets", ArthemeticOperationType.Add },
                                        { "ETickets", ArthemeticOperationType.Add }
                                    },
                                    DataGridColumnStringFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT") } },
                                { "RedemptionGiftsListDTO.Count", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Gift"),
                                    DataGridColumnStringFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT") } },
                                { "RedemptionGiftsListDTO", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Redeemed")
                                 ,  DataGridColumnStringFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT") ,
                                   ChildOrSecondarySourcePropertyName = "Tickets", ChildPropertyListOperationType = ListOperationType.Sum } }
                            };
            log.LogMethodExit();
        }
        private void SetCompletedDataGrid(List<RedemptionDTO> completedRedemptions)
        {
            log.LogMethodEntry();
            if (completedRedemptions != null)
            {
                CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(completedRedemptions.OrderByDescending(x => x.RedeemedDate));
                CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                {
                    { "RedemptionId", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Id") } },
                    { "RedemptionOrderNo", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Order No") } },
                    { "RedemptionStatus", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Status") } },
                    { "PrimaryCardNumber", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Card Number") } },
                    { "CustomerName", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Customer") } },
                    { "RedemptionGiftsListDTO.Count", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Gifts")
                    ,  DataGridColumnStringFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT") } }
                };
            }
            log.LogMethodExit();
        }
        private void SetVoucherDataGrid(List<TicketReceiptDTO> ticketReceipts)
        {
            log.LogMethodEntry(ticketReceipts);
            CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(ticketReceipts.OrderByDescending(t => t.IssueDate));
            CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
            {
                { "ManualTicketReceiptNo", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Receipt No") } },
                { "Tickets", new CustomDataGridColumnElement() {  Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Tkts")
                ,  DataGridColumnStringFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT")} },
                { "BalanceTickets", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Balance")
                ,  DataGridColumnStringFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT")} },
                { "IssueDate", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Issued Date")
                , DataGridColumnStringFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT")} },
            };
            log.LogMethodExit();
        }
        private void SetCustomDataGridBackground()
        {
            log.LogMethodEntry();
            customDataGridVM.MultiScreenItemBackground = MultiScreenItemBackground.Grey;
            if (redemptionMainUserControlVM != null && redemptionMainUserControlVM.ApplyColorCode)
            {
                customDataGridVM.MultiScreenItemBackground = MultiScreenItemBackground.White;
            }
            log.LogMethodExit();
        }
        internal void SetCustomDataGridVM(List<RedemptionDTO> completedOrSuspendedRedemptions = null, List<TicketReceiptDTO> ticketReceipts = null)
        {
            log.LogMethodEntry(completedOrSuspendedRedemptions, ticketReceipts);
            if (CustomDataGridVM.SelectedItem != null)
            {
                CustomDataGridVM.SelectedItem = null;
            }
            CustomDataGridVM.IsComboAndSearchVisible = false;
            SetCustomDataGridBackground();
            int count = 0;
            switch (redemptionsType)
            {
                case RedemptionsType.Suspended:
                    {
                        SetSuspendDataGrid(completedOrSuspendedRedemptions);
                        count = completedOrSuspendedRedemptions.Count;
                    }
                    break;
                case RedemptionsType.Completed:
                    {
                        SetCompletedDataGrid(completedOrSuspendedRedemptions);
                        count = completedOrSuspendedRedemptions.Count;
                    }
                    break;
                case RedemptionsType.Voucher:
                case RedemptionsType.Flagged:
                    {
                        SetVoucherDataGrid(ticketReceipts);
                        count = ticketReceipts.Count;
                    }
                    break;
            }
            if (CustomDataGridVM.SelectedItem == null && customDataGridVM.UICollectionToBeRendered.Count > 0)
            {
                CustomDataGridVM.SelectedItem = customDataGridVM.UICollectionToBeRendered[0];
            }
            SetCompletedSuspenedCount(redemptionsType, count);
            log.LogMethodExit();
        }
        internal async Task<List<TicketReceiptDTO>> GetVouchers(string searchedReceiptNo, string searchedCardNo, string searchedBalanceTicketFromOrOrderNo, string searchedBalanceTicketToOrPrdCodeDesBarCode,
        DateTime? searchedIssuedDateFrom, DateTime? searchedIssuedDateTo, bool isflagged)
        {
            log.LogMethodEntry(searchedReceiptNo, searchedCardNo, searchedBalanceTicketFromOrOrderNo,
                searchedBalanceTicketToOrPrdCodeDesBarCode, searchedIssuedDateFrom, searchedIssuedDateTo, isflagged);
            List<TicketReceiptDTO> searchedTicketReceiptDTO = new List<TicketReceiptDTO>();
            try
            {
                SetLoadingVisible(true);
                ITicketReceiptUseCases TicketUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(ExecutionContext);
                List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchparams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, Convert.ToString(ExecutionContext.GetSiteId())));
                if (!String.IsNullOrEmpty(searchedReceiptNo))
                {
                    searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_TICKET_RECEIPT_NO_LIKE, searchedReceiptNo));
                }
                if (!String.IsNullOrEmpty(searchedCardNo))
                {
                    KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string> cardSearch;
                    cardSearch = await getSearchParameterforCard(searchedCardNo);
                    if (cardSearch.ToString() != null)
                    {
                        searchparams.Add(cardSearch);
                    }
                }
                if (!String.IsNullOrEmpty(searchedBalanceTicketFromOrOrderNo))
                {
                    searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.BALANCE_TICKETS_FROM, searchedBalanceTicketFromOrOrderNo));
                }
                if (!String.IsNullOrEmpty(searchedBalanceTicketToOrPrdCodeDesBarCode))
                {
                    searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.BALANCE_TICKETS_TO, searchedBalanceTicketToOrPrdCodeDesBarCode));
                }
                if (string.IsNullOrEmpty(searchedReceiptNo) && string.IsNullOrEmpty(searchedCardNo))
                {
                    if (searchedIssuedDateFrom != null)
                    {
                        searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_FROM_DATE, ((DateTime)searchedIssuedDateFrom).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                    if (searchedIssuedDateTo != null)
                    {
                        searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_TO_DATE, ((DateTime)searchedIssuedDateTo).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                }
                if (isflagged)
                {
                    searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.IS_SUSPECTED, "1"));
                }
                Task<List<TicketReceiptDTO>> taskGetreceipts = TicketUseCases.GetTicketReceipts(searchparams, 0, 0, null);
                searchedTicketReceiptDTO = await taskGetreceipts;

                if (searchedTicketReceiptDTO == null)
                {
                    searchedTicketReceiptDTO = new List<TicketReceiptDTO>();
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodExit(ticketReceiptDTOList);
            return searchedTicketReceiptDTO;
        }

        internal async Task<bool> UpdateVoucher(TicketReceiptDTO ticketReceiptDTO, string remarks)
        {
            log.LogMethodEntry(ticketReceiptDTO, remarks);
            bool result = false;
            string response = string.Empty;
            try
            {
                SetLoadingVisible(true);
                ITicketReceiptUseCases TicketUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(ExecutionContext);
                response = await TicketUseCases.UpdateTicketReceipts(new List<TicketReceiptDTO>() { ticketReceiptDTO });
                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                ApplicationRemarksDTO applicationRemarksDTO = new ApplicationRemarksDTO(-1, "", "ManualTicketReceipts", ticketReceiptDTO.Guid, remarks, true);
                applicationRemarksDTO = await redemptionUseCases.SaveApplicationRemarks(new List<ApplicationRemarksDTO>() { applicationRemarksDTO });
                if (response == null || response != "Failed")
                {
                    result = true;
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodExit(result);
            return result;
        }

        private async Task<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> getSearchParameterforCard(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            string redemptionIdList = string.Empty;
            try
            {
                SetLoadingVisible(true);
                List<RedemptionDTO> sourceredemptionListDTO = new List<RedemptionDTO>();
                IRedemptionUseCases RedemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchparams = new List<KeyValuePair<RedemptionDTO.SearchByParameters, string>>();
                searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.SITE_ID, Convert.ToString(ExecutionContext.GetSiteId())));
                if (!String.IsNullOrEmpty(cardNumber))
                {
                    searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.PRIMARY_CARD, cardNumber));
                }
                Task<List<RedemptionDTO>> taskGetSourceRedemptions = RedemptionUseCases.GetRedemptionOrders(searchparams);
                sourceredemptionListDTO = await taskGetSourceRedemptions;
                if (sourceredemptionListDTO != null && sourceredemptionListDTO.Count > 0)
                {
                    foreach (RedemptionDTO item in sourceredemptionListDTO)
                    {
                        redemptionIdList = redemptionIdList + item.RedemptionId + ",";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                SetLoadingVisible(false);
            }
            if (string.IsNullOrEmpty(redemptionIdList) == false)
            {
                return new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID_LIST, redemptionIdList);
            }
            else
            {
                return new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>();
            }
        }

        internal async Task<List<RedemptionDTO>> GetRedemptions(string redemptionId, string redemptionOrderNo, string cardNumber, string redemptionStatus,
            DateTime? fromDate, DateTime? toDate, string productCode)
        {
            log.LogMethodEntry(redemptionId, redemptionOrderNo, cardNumber, redemptionStatus, fromDate, toDate, productCode);

            List<RedemptionDTO> searchedRedemptionList = new List<RedemptionDTO>();
            try
            {
                SetLoadingVisible(true);
                IRedemptionUseCases RedemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchparams = new List<KeyValuePair<RedemptionDTO.SearchByParameters, string>>();

                searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.SITE_ID, Convert.ToString(ExecutionContext.GetSiteId())));
                if (!String.IsNullOrEmpty(redemptionId))
                {
                    searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEPTION_ID, redemptionId));
                }
                if (!String.IsNullOrEmpty(redemptionOrderNo))
                {
                    searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEMPTION_ORDER_NO_LIKE, redemptionOrderNo));
                }
                if (!String.IsNullOrEmpty(cardNumber))
                {
                    searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.CARD_NUMBER, cardNumber));
                }
                if (redemptionStatus != null && redemptionStatus != MessageViewContainerList.GetMessage(ExecutionContext, "All"))
                {
                    searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEMPTION_STATUS, redemptionStatus));
                }
                else
                {
                    searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEMPTION_STATUS_NOT_IN, RedemptionDTO.RedemptionStatusEnum.NEW.ToString() + "," + RedemptionDTO.RedemptionStatusEnum.ABANDONED.ToString() + "," + RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString()));
                }
                if (fromDate != null)
                {
                    searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.FROM_REDEMPTION_DATE, ((DateTime)fromDate).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (toDate != null)
                {
                    searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.TO_REDEMPTION_DATE, ((DateTime)toDate).ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (!String.IsNullOrEmpty(productCode))
                {
                    searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.GIFT_CODE_DESC_BARCODE, productCode));
                }
                searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.LOAD_GIFT_CARD_TICKET_ALLOCATION_DETAILS, "Y"));
                Task<List<RedemptionDTO>> taskGetRedemption = RedemptionUseCases.GetRedemptionOrders(searchparams);
                searchedRedemptionList = await taskGetRedemption;
                if (searchedRedemptionList == null)
                {
                    searchedRedemptionList = new List<RedemptionDTO>();
                }
                log.LogMethodExit(searchedRedemptionList);
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            return searchedRedemptionList;
        }

        internal async void SetDefaultCollections(bool isRedemption)
        {
            log.LogMethodEntry(isRedemption);
            //ExecuteActionWithMainUserControlFooter(() =>
            //{
            if (isRedemption)
            {
                todayCompletedRedemptions = new List<RedemptionDTO>();
                if (suspendedRedemptions == null)
                {
                    suspendedRedemptions = await GetRedemptions(null, null, null, "SUSPENDED", null, null, null);
                }
                if (suspendedRedemptions == null)
                {
                    suspendedRedemptions = new List<RedemptionDTO>();
                }
                SetCompletedSuspenedCount(RedemptionsType.Suspended, suspendedRedemptions.Where(s => (s.CreatedBy != null && s.CreatedBy.ToLower() == this.ExecutionContext.UserId.ToLower())
                || (s.LastUpdatedBy.ToLower() == this.ExecutionContext.UserId.ToLower())).Count());
            }
            else
            {
                currentDayTicketReceiptList = new List<TicketReceiptDTO>();
                ticketReceiptDTOList = new List<TicketReceiptDTO>();
            }
            //if (isRedemption)
            //{
            //    using (NoSynchronizationContextScope.Enter())
            //    {
            //        int businessstart = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "BUSINESS_DAY_START_TIME");
            //        Task<List<RedemptionDTO>> task = this.GetRedemptions(null, null, null, MessageViewContainerList.GetMessage(ExecutionContext, "All"), DateTime.Today.AddHours(businessstart), DateTime.Today.AddDays(1).AddHours(businessstart), null);
            //        task.Wait();
            //        if (todayCompletedRedemptions == null)
            //        {
            //            if (task.Result != null)
            //            {
            //                todayCompletedRedemptions = task.Result.Where(c => c.RedemptionStatus.ToLower() != RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString().ToLower()
            //                && c.RedemptionStatus.ToLower() != RedemptionDTO.RedemptionStatusEnum.ABANDONED.ToString().ToLower()
            //                && c.RedemptionStatus.ToLower() != RedemptionDTO.RedemptionStatusEnum.NEW.ToString().ToLower()).ToList();
            //                if (todayCompletedRedemptions != null)
            //                {
            //                    todayCompletedRedemptions = todayCompletedRedemptions.Where(r => (r.CreatedBy != null && r.CreatedBy.ToLower() == ExecutionContext.UserId.ToLower())
            //                    || (r.LastUpdatedBy != null && r.CreatedBy.ToLower() == ExecutionContext.UserId.ToLower())).ToList();
            //                }
            //            }
            //            else
            //            {
            //                todayCompletedRedemptions = new List<RedemptionDTO>();
            //            }
            //            SetCompletedSuspenedCount(RedemptionsType.Completed, todayCompletedRedemptions.Count);
            //        }
            //    }
            //    using (NoSynchronizationContextScope.Enter())
            //    {
            //        Task<List<RedemptionDTO>> task = this.GetRedemptions(null, null, null, "SUSPENDED", null, null, null);
            //        task.Wait();
            //        if (suspendedRedemptions == null)
            //        {
            //            suspendedRedemptions = task.Result;
            //        }
            //        if (suspendedRedemptions == null)
            //        {
            //            suspendedRedemptions = new List<RedemptionDTO>();
            //        }
            //        SetCompletedSuspenedCount(RedemptionsType.Suspended, suspendedRedemptions.Where(s => (s.CreatedBy != null && s.CreatedBy.ToLower() == this.ExecutionContext.UserId.ToLower())
            //        || (s.LastUpdatedBy.ToLower() == this.ExecutionContext.UserId.ToLower())).Count());

            //    }
            //}
            //else
            //{
            //    using (NoSynchronizationContextScope.Enter())
            //    {
            //        int businessstart = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "BUSINESS_DAY_START_TIME");
            //        Task<List<TicketReceiptDTO>> task = this.GetVouchers(null, null, null, null, DateTime.Today.AddHours(businessstart), DateTime.Today.AddDays(1).AddHours(businessstart), false);
            //        task.Wait();
            //        ticketReceiptDTOList = task.Result;
            //        //if (currentDayTicketReceiptList == null)
            //        {
            //            currentDayTicketReceiptList = ticketReceiptDTOList;
            //        }
            //    }
            //}
            //});
            log.LogMethodExit();
        }

        private void OnToggleUnchecked(object parameter)
        {
            log.LogMethodEntry(parameter);
            log.LogMethodExit();
        }

        internal async void OnItemClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContentEmpty();
            await SetTransactionSectionItem();
            CheckIsLoadTicket();
            if (redemptionuserControl == null)
            {
                redemptionuserControl = parameter as RedemptionUserControl;
            }
            TranslatePage();
            log.LogMethodExit();
        }

        private void CheckIsLoadTicket()
        {
            log.LogMethodEntry();
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (redemptionMainUserControlVM != null)
                {
                    switch (redemptionMainUserControlVM.LeftPaneSelectedItem)
                    {
                        case LeftPaneSelectedItem.Redemption:
                            UpdateTicketValues();
                            redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(null, redemptionMainUserControlVM.GetBalanceTickets());
                            break;
                        case LeftPaneSelectedItem.LoadTicket:
                            LoadTotatlTicketCount = redemptionMainUserControlVM.GetLoadTicketTotalCount();
                            redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(null, redemptionMainUserControlVM.GetBalanceTickets());
                            break;
                        case LeftPaneSelectedItem.TurnIn:
                            redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(null, redemptionMainUserControlVM.SetTurnInTotalTicketCount());
                            break;
                    }

                }
            });
            log.LogMethodExit();
        }
        internal void SetTransactionSection(ProductsContainerDTO productDTO, int quantity = 1, bool fromTransaction = false)
        {
            log.LogMethodEntry(productDTO, quantity, fromTransaction);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                GenericTransactionListItem redemptionRightSectionItem = GenericTransactionListVM.ItemCollection.FirstOrDefault(s => s.Key == productDTO.ProductId);
                if (redemptionRightSectionItem != null)
                {
                    if (!fromTransaction)
                    {
                        redemptionRightSectionItem.Count += quantity;
                    }
                    else
                    {
                        redemptionRightSectionItem.Count = quantity;
                    }

                    if (GenericTransactionListVM.TransactionListUserControl != null &&
                    GenericTransactionListVM.TransactionListUserControl.TransactionItemsControl != null
                    && GenericTransactionListVM.TransactionListUserControl.TransactionItemsControl.ItemContainerGenerator != null)
                    {
                        FrameworkElement item = GenericTransactionListVM.TransactionListUserControl.TransactionItemsControl.ItemContainerGenerator.
                        ContainerFromItem(redemptionRightSectionItem) as FrameworkElement;
                        if (item != null)
                        {
                            item.BringIntoView();
                        }
                    }
                }
                else
                {
                    GenericTransactionListVM.ItemCollection.Add(new GenericTransactionListItem(ExecutionContext)
                    {
                        ProductName = mainVM.GetProductContainerDTOList(ExecutionContext).FirstOrDefault(x => x.ProductId == productDTO.ProductId).ProductName,
                        Ticket = isTurnIn ? productDTO.InventoryItemContainerDTO.TurnInPriceInTickets : Convert.ToInt32(Math.Round(RedemptionPriceViewContainerList.GetLeastPriceInTickets(ExecutionContext.SiteId, productDTO.ProductId, redemptionMainUserControlVM.MembershipIDList))),
                        TicketDisplayText = MessageViewContainerList.GetMessage(ExecutionContext, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets")),
                        Count = quantity,
                        RedemptionRightSectionItemType = GenericTransactionListItemType.Item,
                        Key = productDTO.ProductId
                    });
                    if (GenericTransactionListVM.TransactionListUserControl != null &&
                    GenericTransactionListVM.TransactionListUserControl.scvItems != null)
                    {
                        GenericTransactionListVM.TransactionListUserControl.scvItems.ScrollToBottom();
                    }
                }
                SetSpecificRedemptionDTOGiftItem(productDTO);
            });
            log.LogMethodExit();
        }
        private async Task SetTransactionSectionItem(int quantity = 1)
        {
            log.LogMethodEntry(quantity);
            if (GenericDisplayItemsVM != null)
            {
                if (string.IsNullOrEmpty(GenericTransactionListVM.TransactionID))
                {
                    this.TransactionID = defaultTransactionID;
                }
                if (GenericDisplayItemsVM.SelectedItem != null)
                {
                    if (GenericDisplayItemsVM.SelectedItem is GenericDisplayItemModel)
                    {
                        GenericDisplayItemModel displayItemModel = GenericDisplayItemsVM.SelectedItem as GenericDisplayItemModel;
                        await redemptionMainUserControlVM.AddProductToUI(mainVM.GetProductContainerDTOList(ExecutionContext).FirstOrDefault(p => p.ProductId ==
                        Convert.ToInt32(displayItemModel.Key)), quantity);
                    }
                    if (GenericDisplayItemsVM.SelectedItem is ProductsContainerDTO)
                    {
                        await redemptionMainUserControlVM.AddProductToUI(GenericDisplayItemsVM.SelectedItem as ProductsContainerDTO, quantity);
                    }
                    else if (GenericDisplayItemsVM.SelectedItem is RedemptionCurrencyContainerDTO)
                    {
                        redemptionMainUserControlVM.AddCurrency((GenericDisplayItemsVM.SelectedItem as RedemptionCurrencyContainerDTO).CurrencyName, null, null, null);
                    }
                    HideContentArea();
                }
            }
            log.LogMethodExit();
        }
        internal void HideContentArea()
        {
            log.LogMethodEntry();
            if (multiScreenMode && iscontentAreaVisible)
            {
                IsContentAreaVisible = false;
            }
            SetTransactionListVM(transactionID);
            log.LogMethodExit();
        }
        internal void SetTransactionListVM(string transactionID)
        {
            log.LogMethodEntry(transactionID);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (GenericTransactionListVM != null)
                {
                    if (!string.IsNullOrEmpty(transactionID))
                    {
                        GenericTransactionListVM.TransactionID = transactionID;
                    }
                    else
                    {
                        GenericTransactionListVM.TransactionID = defaultTransactionID;
                    }
                    GenericTransactionListVM.UserName = userName;
                    GenericTransactionListVM.ScreenNumber = screenNumber;
                    if (multiScreenMode && redemptionMainUserControlVM != null && redemptionMainUserControlVM.FooterVM != null &&
                        redemptionMainUserControlVM.FooterVM.SideBarContent == MessageViewContainerList.GetMessage(ExecutionContext, "Show Sidebar"))
                    {
                        GenericTransactionListVM.ScreenUserAreaVisible = true;
                    }
                    else
                    {
                        GenericTransactionListVM.ScreenUserAreaVisible = false;
                    }
                }
            });
            log.LogMethodExit();
        }
        internal void SetDefaultValues(RedemptionMainUserControlVM mainUserControlVM)
        {
            log.LogMethodEntry(mainUserControlVM);
            if (mainUserControlVM != null)
            {
                this.redemptionMainUserControlVM = mainUserControlVM;
                this.redemptionMainUserControl = this.redemptionMainUserControlVM.RedemptionMainUserControl;
                this.mainVM = this.redemptionMainUserControlVM.MainVM;
                this.redemptionMainView = this.redemptionMainUserControlVM.RedemptionMainView;
            }
            log.LogMethodExit();
        }
        private void SetFooterContentEmpty()
        {
            log.LogMethodEntry();
            if (redemptionMainUserControlVM != null)
            {
                redemptionMainUserControlVM.SetFooterContent(string.Empty, MessageType.None);
            }
            log.LogMethodExit();
        }

        private async void OnItemOfferOrInfoClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContentEmpty();
            if (GenericDisplayItemsVM != null && GenericDisplayItemsVM.SelectedItem != null
                && redemptionMainUserControlVM != null)
            {
                GenericDisplayItemModel displayItemModel = GenericDisplayItemsVM.SelectedItem as GenericDisplayItemModel;
                if (displayItemModel != null)
                {
                    if (isTurnIn)
                    {
                        selectedProductContainerDTO = GenericDisplayItemsVM.DisplayItemModels.FirstOrDefault(d =>
                        (d as ProductsContainerDTO).ProductName.ToLower() == displayItemModel.Heading.ToLower())
                        as ProductsContainerDTO;
                    }
                    else
                    {
                        selectedProductContainerDTO = mainVM.GetProductContainerDTOList(ExecutionContext).FirstOrDefault(p => p.ProductId == Convert.ToInt32(displayItemModel.Key));
                    }
                    if (selectedProductContainerDTO != null && selectedProductContainerDTO.InventoryItemContainerDTO != null)
                    {
                        List<InventoryDTO> inventories = await GetInventories(new List<int>()
                            { selectedProductContainerDTO.InventoryItemContainerDTO.ProductId });
                        ObservableCollection<string> displayItemDetails = new ObservableCollection<string>();
                        int totalStock = 0;
                        if (inventories != null)
                        {
                            totalStock = (int)inventories.Sum(i => i.Quantity);
                            foreach (InventoryDTO inventory in inventories)
                            {
                                string lotno = string.Empty;
                                string locationName = LocationViewContainerList.GetLocationContainerDTOList(ExecutionContext).Where(x =>
                                x.LocationId == inventory.LocationId).Select(x => x.Name).FirstOrDefault();
                                if (selectedProductContainerDTO.InventoryItemContainerDTO.LotControlled)
                                {
                                    lotno = MessageViewContainerList.GetMessage(ExecutionContext, "Lot No") + " - " +
                                    (inventory.LotNumber == "-1" ? string.Empty : inventory.LotNumber);
                                }
                                displayItemDetails.Add(MessageViewContainerList.GetMessage(ExecutionContext, "Location") + " - " + locationName + "  " +
                                                         MessageViewContainerList.GetMessage(ExecutionContext, "Qty") + " - " +
                                                         GetNumberFormattedString((int)inventory.Quantity) + "  " + lotno);
                            }
                        }
                        redemptionMainUserControlVM.ItemInfoPopUpView = new GenericItemInfoPopUp();
                        GenericItemInfoPopUpVM itemInfoPopUpVM = new GenericItemInfoPopUpVM(ExecutionContext)
                        {
                            MultiScreenMode = this.multiScreenMode,
                            ImageVisibility = Visibility.Collapsed
                        };
                        if (mainVM != null && mainVM.RowCount > 1)
                        {
                            itemInfoPopUpVM.IsMultiScreenRowTwo = true;
                        }
                        itemInfoPopUpVM.ItemDetailsHeadersContent = new ObservableCollection<ItemInfoPopupTextBlockModel>()
                            {
                                new ItemInfoPopupTextBlockModel(selectedProductContainerDTO.ProductName,FontWeights.Bold),
                                new ItemInfoPopupTextBlockModel(MessageViewContainerList.GetMessage(ExecutionContext,ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT","Tickets")) + " - " +
                               GetNumberFormattedString(Convert.ToInt32(Math.Round(RedemptionPriceViewContainerList.GetLeastPriceInTickets(ExecutionContext.SiteId, selectedProductContainerDTO.ProductId, redemptionMainUserControlVM.MembershipIDList)))) ,FontWeights.Normal),
                                new ItemInfoPopupTextBlockModel(MessageViewContainerList.GetMessage(ExecutionContext,"Available") + " - " + GetNumberFormattedString(totalStock), FontWeights.Normal),
                                new ItemInfoPopupTextBlockModel(MessageViewContainerList.GetMessage(ExecutionContext,"Description"), FontWeights.Bold),
                                new ItemInfoPopupTextBlockModel(selectedProductContainerDTO.Description, FontWeights.Normal),
                                new ItemInfoPopupTextBlockModel(MessageViewContainerList.GetMessage(ExecutionContext,"Code"), FontWeights.Bold),
                                new ItemInfoPopupTextBlockModel(selectedProductContainerDTO.InventoryItemContainerDTO.Code, FontWeights.Normal)
                            };
                        itemInfoPopUpVM.DisplayInventoryDetails = displayItemDetails;
                        redemptionMainUserControlVM.ItemInfoPopUpView.DataContext = itemInfoPopUpVM;
                        redemptionMainUserControlVM.ItemInfoPopUpView.PreviewMouseDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                        redemptionMainUserControlVM.ItemInfoPopUpView.PreviewKeyDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                        if (redemptionMainUserControlVM.ItemInfoPopUpView.KeyBoardHelper != null)
                        {
                            redemptionMainUserControlVM.SetKeyBoardHelperColorCode();
                            redemptionMainUserControlVM.ItemInfoPopUpView.KeyBoardHelper.KeypadMouseDownEvent -= redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                            redemptionMainUserControlVM.ItemInfoPopUpView.KeyBoardHelper.KeypadMouseDownEvent += redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                        }
                        redemptionMainUserControlVM.ItemInfoPopUpView.Loaded += redemptionMainUserControlVM.OnWindowLoaded;
                        redemptionMainUserControlVM.ItemInfoPopUpView.Closed += OnItemInfoPopUpViewClosed;
                        redemptionMainUserControlVM.ItemInfoPopUpView.Show();
                    }
                }
            }
            log.LogMethodExit();
        }

        private async void OnItemInfoPopUpViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            GenericItemInfoPopUpVM itemInfoPopUpVM = (sender as GenericItemInfoPopUp).DataContext as GenericItemInfoPopUpVM;
            if (itemInfoPopUpVM != null && selectedProductContainerDTO != null && itemInfoPopUpVM.SelectedValue > 0
                && itemInfoPopUpVM.AddClicked)
            {
                GenericDisplayItemsVM.SelectedItem = (object)selectedProductContainerDTO;
                await SetTransactionSectionItem(itemInfoPopUpVM.SelectedValue);
                SetSpecificRedemptionDTOGiftItem(selectedProductContainerDTO);
                CheckIsLoadTicket();
                selectedProductContainerDTO = null;
            }
            SetMainViewFocus();
            log.LogMethodExit();
        }

        private void OnShowContentAreaClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                SetFooterContentEmpty();
                if (GenericDisplayItemsVM != null)
                {
                    //if (GenericDisplayItemsVM.UIDisplayItemModels != null)
                    //{
                    //    GenericDisplayItemsVM.UIDisplayItemModels.Clear();
                    //}
                    if (multiScreenMode)
                    {
                        IsContentAreaVisible = true;
                        GenericDisplayItemsVM.SetUIDisplayItemModels();
                    }
                    else
                    {
                        AutoShowProductMenu(redemptionMainUserControlVM, !iscontentAreaVisible);
                    }
                    SetMutliScreenMode();
                }
            });
            log.LogMethodExit();
        }
        internal void AutoShowProductMenu(RedemptionMainUserControlVM redemptionMainUserControlVM, bool showContentArea)
        {
            log.LogMethodEntry();
            if (redemptionMainUserControlVM != null && !multiScreenMode)
            {
                LeftPaneSelectedItem leftPaneSelectedItem = redemptionMainUserControlVM.LeftPaneSelectedItem;
                bool autoShowRedemptionProductMenu = redemptionMainUserControlVM.AutoShowRedemptionProductMenu;
                bool autoShowLoadTicketProductMenu = redemptionMainUserControlVM.AutoShowLoadTicketProductMenu;
                bool isRedemptionOrTurnIn = leftPaneSelectedItem == LeftPaneSelectedItem.Redemption ||
                    leftPaneSelectedItem == LeftPaneSelectedItem.TurnIn;
                if ((isRedemptionOrTurnIn && !autoShowRedemptionProductMenu) || (leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket
                    && !autoShowLoadTicketProductMenu))
                {
                    GenericTransactionListVM.ShowNavigationButton = true;
                    GenericRightSectionContentVM.ShowNavigationButton = true;
                    IsContentAreaVisible = showContentArea;
                    GenericDisplayItemsVM.SetUIDisplayItemModels();
                }
            }
            log.LogMethodExit();
        }
        private void SetMutliScreenMode()
        {
            log.LogMethodEntry();
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (CustomDataGridVM != null)
                {
                    SetCustomDataGridBackground();
                    CustomDataGridVM.MultiScreenMode = multiScreenMode;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        CustomDataGridVM.IsMultiScreenRowTwo = true;
                    }
                    else
                    {
                        CustomDataGridVM.IsMultiScreenRowTwo = false;
                    }
                }
                if (GenericTransactionListVM != null)
                {
                    GenericTransactionListVM.MultiScreenMode = multiScreenMode;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        GenericTransactionListVM.IsMultiScreenRowTwo = true;
                    }
                    else if (GenericRightSectionContentVM.IsMultiScreenRowTwo)
                    {
                        GenericTransactionListVM.IsMultiScreenRowTwo = false;
                    }
                    SetTransactionListVM(transactionID);
                }
                if (GenericRightSectionContentVM != null)
                {
                    GenericRightSectionContentVM.MultiScreenMode = multiScreenMode;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        GenericRightSectionContentVM.IsMultiScreenRowTwo = true;
                    }
                    else if (GenericRightSectionContentVM.IsMultiScreenRowTwo)
                    {
                        GenericRightSectionContentVM.IsMultiScreenRowTwo = false;
                    }
                }
                if (GenericDisplayItemsVM != null)
                {
                    GenericDisplayItemsVM.MultiScreenMode = multiScreenMode;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        GenericDisplayItemsVM.IsMultiScreenRowTwo = true;
                    }
                    else if (GenericDisplayItemsVM.IsMultiScreenRowTwo)
                    {
                        GenericDisplayItemsVM.IsMultiScreenRowTwo = false;
                    }
                }
            });
            log.LogMethodExit();
        }

        private void OnShowTransactionAreaClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                SetFooterContentEmpty();
                if (multiScreenMode && IsContentAreaVisible)
                {
                    IsContentAreaVisible = false;
                    if (this.redemptionsType != RedemptionsType.New && redemptionMainUserControlVM != null && redemptionMainUserControlVM.FooterVM != null)
                    {
                        if (redemptionMainUserControlVM.FooterVM.SideBarContent == MessageViewContainerList.GetMessage(ExecutionContext, "Show Sidebar"))
                        {
                            this.GenericRightSectionContentVM.IsScreenUserAreaVisble = true;
                        }
                        else if (this.GenericRightSectionContentVM.IsScreenUserAreaVisble)
                        {
                            this.GenericRightSectionContentVM.IsScreenUserAreaVisble = false;
                        }
                    }
                }
                SetMutliScreenMode();
            });
            log.LogMethodExit();
        }

        internal void OnResetClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                SetFooterContentEmpty();
                bool isDelivered = false;
                redemptionMainUserControlVM.NewOrCloseScreen = 'N';
                if (redemptionMainUserControlVM.RedemptionDTO != null
                    && !string.IsNullOrEmpty(redemptionMainUserControlVM.RedemptionDTO.RedemptionStatus))
                {
                    if (redemptionMainUserControlVM.RedemptionDTO.RedemptionStatus.ToLower() == "DELIVERED".ToLower()
                    || (isTurnIn && redemptionMainUserControlVM.RedemptionDTO.RedemptionStatus.ToLower() == "NEW".ToLower()))
                    {
                        isDelivered = true;
                    }
                }
                //if (isDelivered || (redemptionMainUserControlVM != null && redemptionMainUserControlVM.ShowDiscardConfirmation(true))
                //)
                //{
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.Discard();
                }
                if (GenericTransactionListVM.ItemCollection != null &&
                    GenericTransactionListVM.ItemCollection.Count > 0)
                {
                    GenericTransactionListVM.ItemCollection.Clear();
                }
                redemptionMainUserControlVM.SetNewRedemption();
                this.TransactionID = defaultTransactionID;
                StayInTransactionMode = false;
                UpdateTicketValues();
                if(redemptionMainUserControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption)
                {
                    UpdateStock();
                }
                redemptionMainUserControlVM.CallRecalculatePriceandStock(true,true,true);
                if (redemptionMainUserControlVM.UserView == null)
                {
                    if (ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "REQUIRE_LOGIN_FOR_EACH_TRX", false))
                    {
                        redemptionMainUserControlVM.ShowRelogin(this.UserName);
                    }
                }
                redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(string.Empty, 0);
                //}
            });
            log.LogMethodExit();
        }

        internal void AddRetreivedSuspendBackup()
        {
            log.LogMethodEntry();
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (redemptionMainUserControlVM.RetreivedBackupDTO != null)
                {
                    if (suspendedRedemptions != null)
                    {
                        if (redemptionMainUserControlVM.RetreivedBackupDTO.RedemptionId >= 0)
                        {
                            this.suspendedRedemptions.Add(redemptionMainUserControlVM.RetreivedBackupDTO);
                        }
                    }
                    if (redemptionDTOList != null)
                    {
                        if (redemptionMainUserControlVM.RetreivedBackupDTO.RedemptionId >= 0)
                        {
                            this.redemptionDTOList.Add(redemptionMainUserControlVM.RetreivedBackupDTO);
                        }
                    }
                    SetOtherRedemptionList(RedemptionUserControlVM.ActionType.SuspendRetrieve);
                    redemptionMainUserControlVM.SetNewRedemption();
                    SetCompletedSuspenedCount(RedemptionsType.Suspended, suspendedRedemptions.Where(r => (r.CreatedBy.ToLower() == ExecutionContext.UserId.ToLower())
                    || (r.LastUpdatedBy.ToLower() == this.ExecutionContext.UserId.ToLower())).Count());
                    redemptionMainUserControlVM.RetreivedBackupDTO = null;
                }
            });
            log.LogMethodExit();
        }
        private void OnDeleteClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                SetFooterContentEmpty();
                if (GenericTransactionListVM.ItemCollection != null &&
                    GenericTransactionListVM.ItemCollection.Count > 0)
                {
                    StayInTransactionMode = true;
                    GenericTransactionListVM.ItemCollection.Clear();
                    if (redemptionMainUserControlVM != null)
                    {
                        int headerBalance = 0;
                        if (isLoadTicket)
                        {
                            redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Clear();
                            if (redemptionMainUserControlVM.RedemptionDTO.TicketReceiptListDTO != null)
                            {
                                redemptionMainUserControlVM.RedemptionDTO.TicketReceiptListDTO.Clear();
                            }
                            redemptionMainUserControlVM.SetTotalCurrencyTickets();
                            LoadTotatlTicketCount = 0;
                            headerBalance = redemptionMainUserControlVM.GetBalanceTickets();
                        }
                        else
                        {
                            redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Clear();
                            if (isTurnIn)
                            {
                                headerBalance = redemptionMainUserControlVM.SetTurnInTotalTicketCount();
                            }
                            else
                            {
                                headerBalance = redemptionMainUserControlVM.GetBalanceTickets();
                            }
                        }
                        redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(null, headerBalance);
                    }
                }
            });
            log.LogMethodExit();
        }

        private void OnTransactionItemClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                SetFooterContentEmpty();
                if (GenericTransactionListVM != null && GenericTransactionListVM.ItemCollection.Count > 0
                    && GenericTransactionListVM.SelectedItem != null && GenericTransactionListVM.SelectedItem.IsEnabled)
                {
                    if (GenericTransactionListVM.SelectedItem.RedemptionRightSectionItemType == GenericTransactionListItemType.Item)
                    {
                        redemptionMainUserControlVM.DataEntryView = new GenericDataEntryView();
                        redemptionMainUserControlVM.DataEntryView.PreviewMouseDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                        redemptionMainUserControlVM.DataEntryView.PreviewKeyDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                        if (redemptionMainUserControlVM.DataEntryView.KeyBoardHelper != null)
                        {
                            redemptionMainUserControlVM.SetKeyBoardHelperColorCode();
                            redemptionMainUserControlVM.DataEntryView.KeyBoardHelper.KeypadMouseDownEvent -= redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                            redemptionMainUserControlVM.DataEntryView.KeyBoardHelper.KeypadMouseDownEvent += redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                        }
                        redemptionMainUserControlVM.DataEntryView.Loaded += redemptionMainUserControlVM.OnWindowLoaded;
                        redemptionMainUserControlVM.DataEntryView.Closed += OnDataEntryViewClosed;
                        redemptionMainUserControlVM.DataEntryView.NumericDeleteButtonClicked += OnDataEntryViewNumericDeleteButtonClicked;

                        GenericDataEntryVM genericDataEntryVM = new GenericDataEntryVM(ExecutionContext)
                        {
                            Heading = GenericTransactionListVM.SelectedItem.ProductName + "( " + GenericTransactionListVM.SelectedItem.Ticket + " " + ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets") + " )",
                            DataEntryCollections = new ObservableCollection<DataEntryElement>()
                            {
                                new DataEntryElement()
                                {
                                    Type = DataEntryType.NumericUpDown,
                                    NumericButtonType = NumericButtonType.Delete,
                                    NumericUpDownValue = GenericTransactionListVM.SelectedItem.Count
                                }
                            },
                            OkButtonContent = MessageViewContainerList.GetMessage(ExecutionContext, "CONFIRM"),
                            IsKeyboardVisible = false
                        };

                        redemptionMainUserControlVM.DataEntryView.DataContext = genericDataEntryVM;
                        redemptionMainUserControlVM.DataEntryView.Show();
                    }
                    else if (GenericTransactionListVM.SelectedItem.RedemptionRightSectionItemType == GenericTransactionListItemType.Ticket)
                    {
                        redemptionMainUserControlVM.OpenTicketAllocation(false, false, true);
                    }
                }
            });
            log.LogMethodExit();
        }
        private void OnDataEntryViewNumericDeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                GenericDataEntryView genericDataEntryView = sender as GenericDataEntryView;
                if (genericDataEntryView != null && GenericTransactionListVM.ItemCollection.Contains(GenericTransactionListVM.SelectedItem))
                {
                    GenericDataEntryVM genericDataEntryVM = genericDataEntryView.DataContext as GenericDataEntryVM;
                    if (genericDataEntryVM != null)
                    {
                        genericDataEntryVM.DataEntryCollections[0].NumericUpDownValue = 0;
                        genericDataEntryVM.ButtonClickType = ButtonClickType.Ok;
                    }
                    genericDataEntryView.Close();
                }
            });
            log.LogMethodExit();
        }

        private async void OnDataEntryViewClosed(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int? viewGroupingNumber = null;
            GenericDataEntryVM genericDataEntryVM = (sender as GenericDataEntryView).DataContext as GenericDataEntryVM;
            if (genericDataEntryVM.ButtonClickType == ButtonClickType.Ok && GenericTransactionListVM.ItemCollection.Contains(GenericTransactionListVM.SelectedItem))
            {
                if (isLoadTicket)
                {
                    RedemptionCurrencyContainerDTO currencyContainerDTO = mainVM.GetRedemptionCurrencyContainerDTOList(this.ExecutionContext).FirstOrDefault(
                            r => r.CurrencyId == GenericTransactionListVM.SelectedItem.Key);
                    if (genericDataEntryVM.DataEntryCollections[0].NumericUpDownValue <= 0)
                    {
                        if (currencyContainerDTO != null && redemptionMainUserControlVM.RedemptionDTO != null
                            && redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO != null
                            && redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Count > 0)
                        {
                            List<RedemptionCardsDTO> cardstobeRemoved = new List<RedemptionCardsDTO>();
                            if (redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Any(c => c.CurrencyId == currencyContainerDTO.CurrencyId))
                            {
                                if (GenericTransactionListVM.SelectedItem.ViewGroupingNumber == null)
                                {
                                    List<RedemptionCardsDTO> redemptionCardsDTOs = redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Where(c => c.CurrencyId == currencyContainerDTO.CurrencyId && c.ViewGroupingNumber == null).ToList();
                                    if (redemptionCardsDTOs != null)
                                    {
                                        cardstobeRemoved.AddRange(redemptionCardsDTOs);
                                    }
                                }
                                else
                                {
                                    List<RedemptionCardsDTO> redemptionCardsDTOs = redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Where(c => c.CurrencyId == currencyContainerDTO.CurrencyId && c.ViewGroupingNumber == GenericTransactionListVM.SelectedItem.ViewGroupingNumber).ToList();
                                    if (redemptionCardsDTOs != null)
                                    {
                                        cardstobeRemoved.AddRange(redemptionCardsDTOs);
                                    }
                                }
                                foreach (RedemptionCardsDTO cardDTOtobeRemoved in cardstobeRemoved)
                                {
                                    redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Remove(cardDTOtobeRemoved);
                                }
                            }
                        }
                        GenericTransactionListVM.ItemCollection.Remove(GenericTransactionListVM.SelectedItem);
                    }
                    else if (currencyContainerDTO != null)
                    {
                        if (GenericTransactionListVM.SelectedItem.ViewGroupingNumber != null)
                        {
                            viewGroupingNumber = GenericTransactionListVM.SelectedItem.ViewGroupingNumber;
                        }
                        redemptionMainUserControlVM.AddCurrencyToUI(currencyContainerDTO, genericDataEntryVM.DataEntryCollections[0].NumericUpDownValue, true, viewGroupingNumber);
                    }
                    if (redemptionMainUserControlVM.ApplyCurrencyRule())
                    {
                        redemptionMainUserControlVM.RefreshTransactionItem();
                    }
                    redemptionMainUserControlVM.SetTotalCurrencyTickets();
                    LoadTotatlTicketCount = redemptionMainUserControlVM.GetLoadTicketTotalCount();
                }
                else
                {
                    ProductsContainerDTO productsContainerDTO = mainVM.GetProductContainerDTOList(ExecutionContext).FirstOrDefault(x => x.ProductId == GenericTransactionListVM.SelectedItem.Key);
                    if (productsContainerDTO != null)
                    {
                        if (genericDataEntryVM.DataEntryCollections[0].NumericUpDownValue <= 0)
                        {
                            GenericTransactionListVM.ItemCollection.Remove(GenericTransactionListVM.SelectedItem);
                            List<RedemptionGiftsDTO> backupList = new List<RedemptionGiftsDTO>();
                            foreach (RedemptionGiftsDTO giftsDTO in redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Where(
r => r.ProductId == productsContainerDTO.InventoryItemContainerDTO.ProductId))
                            {
                                backupList.Add(giftsDTO);
                            }
                            if (backupList != null)
                            {
                                foreach (RedemptionGiftsDTO giftsDTO in backupList)
                                {
                                    if (giftsDTO != null)
                                    {
                                        redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Remove(giftsDTO);
                                    }
                                }
                            }
                        }
                        else
                        {
                            await redemptionMainUserControlVM.AddProductToUI(productsContainerDTO, genericDataEntryVM.DataEntryCollections[0].NumericUpDownValue, true);
                        }
                        if (isTurnIn)
                        {
                            redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(null, redemptionMainUserControlVM.SetTurnInTotalTicketCount());
                        }
                    }
                }
            }
            if (GenericTransactionListVM.ItemCollection.Count == 0)
            {
                StayInTransactionMode = true;
            }
            CheckIsLoadTicket();
            SetMainViewFocus();
            log.LogMethodExit();
        }
        private void SetMainViewFocus()
        {
            log.LogMethodEntry();
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetUserControlFocus();
                }
            });
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (parameter != null)
                {
                    redemptionuserControl = parameter as RedemptionUserControl;
                    if (redemptionuserControl != null)
                    {
                        FindAncestor(redemptionuserControl);
                    }
                }
            });
            log.LogMethodExit();
        }

        private void FindAncestor(Visual myVisual)
        {
            log.LogMethodEntry(myVisual);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (myVisual == null)
                {
                    return;
                }
                object visual = VisualTreeHelper.GetParent(myVisual);
                if (visual is Window)
                {
                    redemptionMainView = visual as RedemptionView;
                    if (redemptionMainView != null && redemptionMainView.DataContext != null)
                    {
                        RedemptionMainVM redemptionMainVM = redemptionMainView.DataContext as RedemptionMainVM;
                        if (redemptionMainVM != null)
                        {
                            mainVM = redemptionMainVM;
                            if (redemptionMainUserControlVM != null)
                            {
                                redemptionMainUserControlVM.MainVM = redemptionMainVM;
                                redemptionMainUserControlVM.RedemptionMainView = redemptionMainView;
                                redemptionMainUserControlVM.RedemptionMainUserControl = redemptionMainUserControl;
                                redemptionMainUserControlVM.RedempMainUserControlVM = redemptionMainUserControlVM;
                            }
                        }
                    }
                }
                else if (visual is RedemptionMainUserControl)
                {
                    redemptionMainUserControl = visual as RedemptionMainUserControl;
                    if (redemptionMainUserControl != null)
                    {
                        if (redemptionMainUserControl.DataContext != null)
                        {
                            redemptionMainUserControlVM = redemptionMainUserControl.DataContext as RedemptionMainUserControlVM;
                            FindAncestor(redemptionMainUserControl);
                        }
                    }
                }
                else
                {
                    FindAncestor(visual as Visual);
                }
            });
            log.LogMethodExit();
        }

        internal void AddCurrencyToTransaction(RedemptionCurrencyContainerDTO currencyDTO, int quantity = 0,int? viewGroupingNumber=null)
        {
            log.LogMethodEntry(currencyDTO, quantity);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                GenericTransactionListVM.TransactionID = this.TransactionID;
                GenericTransactionListItem redemptionRightSectionItem = GenericTransactionListVM.ItemCollection.FirstOrDefault(s => s.ProductName.ToLower() == currencyDTO.CurrencyName.ToLower() && ((viewGroupingNumber==null && s.ViewGroupingNumber==null) || (viewGroupingNumber!=null&&s.ViewGroupingNumber==viewGroupingNumber)));
                if (redemptionRightSectionItem != null)
                {
                    if (quantity == 0)
                    {
                        quantity = 1;
                        redemptionRightSectionItem.Count += quantity;
                    }
                    else
                    {
                        redemptionRightSectionItem.Count = quantity;
                    }
                }
                else
                {
                    if (quantity == 0)
                    {
                        quantity = 1;
                    }
                    GenericTransactionListVM.ItemCollection.Add(new GenericTransactionListItem(ExecutionContext)
                    {
                        ProductName = currencyDTO.CurrencyName,
                        Ticket = (int)currencyDTO.ValueInTickets,
                        TicketDisplayText = MessageViewContainerList.GetMessage(ExecutionContext, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets")),
                        Count = quantity,
                        RedemptionRightSectionItemType = GenericTransactionListItemType.Item,
                        Key = currencyDTO.CurrencyId,
                        ViewGroupingNumber=viewGroupingNumber
                    });
                }
            });
            log.LogMethodExit();
        }
        protected override bool ButtonEnable(object state)
        {
            if (redemptionMainUserControlVM != null)
            {
                return !redemptionMainUserControlVM.IsLoadingVisible;
            }
            else
            {
                return true;
            }
        }
        public void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            if (GenericToggleButtonsVM != null)
            {
                GenericToggleButtonsVM.IsLoadingVisible = redemptionMainUserControlVM != null ? redemptionMainUserControlVM.IsLoadingVisible : false;
                GenericToggleButtonsVM.RaiseCanExecuteChanged();
            }
            (ScanEnterCommand as DelegateCommand).RaiseCanExecuteChanged();
            (TransactionActionsCommand as DelegateCommand).RaiseCanExecuteChanged();
            (SuspendCompleteActionsCommand as DelegateCommand).RaiseCanExecuteChanged();
            (SearchCommand as DelegateCommand).RaiseCanExecuteChanged();
            (SearchActionsCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ShowAllClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ToggleCheckedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ToggleUncheckedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ItemClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ItemOfferOrInfoClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ShowContentAreaClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ShowTransactionAreaClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ResetCommand as DelegateCommand).RaiseCanExecuteChanged();
            (DeleteCommand as DelegateCommand).RaiseCanExecuteChanged();
            (TransactionItemClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (TransactionActionsCommand as DelegateCommand).RaiseCanExecuteChanged();
            (SuspendCompleteActionsCommand as DelegateCommand).RaiseCanExecuteChanged();
            (TotalTicketClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ActionsCommand as DelegateCommand).RaiseCanExecuteChanged();
            log.LogMethodExit();
        }
        private void OnTransactionActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (redemptionMainUserControl == null)
                {
                    FindAncestor(parameter as Button);
                }
                SetFooterContentEmpty();
                if (parameter != null && redemptionMainUserControl != null)
                {
                    Button button = parameter as Button;
                    if (button != null && !string.IsNullOrEmpty(button.Name))
                    {
                        switch (button.Name.ToLower())
                        {
                            case "btnturninscan":
                            case "btnscan":
                                {
                                    bool showPopup = false;
                                    GenericScanPopupVM.PopupType popupType = GenericScanPopupVM.PopupType.SCANTICKET;
                                    if (isTurnIn)
                                    {
                                        showPopup = true;
                                        popupType = GenericScanPopupVM.PopupType.SCANGIFT;
                                    }
                                    if (isLoadTicket && !mainVM.EnableEnterTicketNumberManually)
                                    {
                                        showPopup = true;
                                        popupType = GenericScanPopupVM.PopupType.SCANTICKET;
                                    }
                                    ScanClicked(showPopup, popupType);
                                }
                                break;
                            case "btnaddticket":
                                {
                                    OnAddTicketClicked();
                                }
                                break;
                            case "btnloadticket":
                                {
                                    LoadToCardClicked();
                                }
                                break;
                            case "btnsuspend":
                                {
                                    SuspendClicked();
                                }
                                break;
                            case "btnturninprint":
                            case "btnprint":
                                if (isLoadTicket)
                                {
                                    PrintConsolidateClicked();
                                }
                                else
                                {
                                    PrintRedemptionClicked();
                                }
                                break;
                            case "btnturnincomplete":
                                {
                                    CompleteTurninClicked();
                                }
                                break;
                            case "btncomplete":
                                {
                                    CompleteClicked();
                                }
                                break;
                        }
                    }
                }
            });
            log.LogMethodExit();
        }
        internal void OnAddTicketClicked()
        {
            log.LogMethodEntry();
            // redemptionMainUserControlVM.RedemptionActivityManualTicketDTO.ManagerToken = this.ExecutionContext.WebApiToken;
            redemptionMainUserControlVM.OpenTicketAllocation(false, true);
            log.LogMethodExit();
        }

        internal void ScanClicked(bool showPopup, GenericScanPopupVM.PopupType popupType)
        {
            log.LogMethodEntry(showPopup, popupType);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (!showPopup)
                {
                    redemptionMainUserControlVM.ScanView = new RedemptionScanView();
                    RedemptionScanVM scanVM = new RedemptionScanVM(this.ExecutionContext, redemptionMainUserControlVM);
                    scanVM.MultiScreenMode = multiScreenMode;
                    if (isLoadTicket)
                    {
                        scanVM.ScanGiftButtonVisible = false;
                    }
                    if (!mainVM.EnableEnterTicketNumberManually)
                    {
                        scanVM.EnterTicketButtonVisible = false;
                    }
                    redemptionMainUserControlVM.ScanView.DataContext = scanVM;
                    redemptionMainUserControlVM.ScanView.PreviewMouseDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                    redemptionMainUserControlVM.ScanView.PreviewKeyDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                    redemptionMainUserControlVM.ScanView.Loaded += redemptionMainUserControlVM.OnWindowLoaded;
                    redemptionMainUserControlVM.ScanView.Closed += OnScanViewClosed;
                    redemptionMainUserControlVM.ScanView.Show();
                }
                if (showPopup)
                {
                    GenericScanPopupVM scanpopupVM = new GenericScanPopupVM(this.ExecutionContext);
                    scanpopupVM.ScanPopupType = popupType;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        scanpopupVM.IsMultiScreenRowTwo = true;
                    }
                    redemptionMainUserControlVM.ScanPopUpView = new GenericScanPopupView();
                    redemptionMainUserControlVM.ScanPopUpView.DataContext = scanpopupVM;
                    redemptionMainUserControlVM.ScanPopUpView.PreviewMouseDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                    redemptionMainUserControlVM.ScanPopUpView.PreviewKeyDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                    redemptionMainUserControlVM.ScanPopUpView.Loaded += redemptionMainUserControlVM.OnWindowLoaded;
                    if (scanpopupVM.ScanPopupType == GenericScanPopupVM.PopupType.SCANTICKET)
                    {
                        redemptionMainUserControlVM.ScanTicketGiftMode = 'T';
                        scanpopupVM.Title = MessageViewContainerList.GetMessage(this.ExecutionContext, "SCAN TICKET NOW");
                    }
                    if (scanpopupVM.ScanPopupType == GenericScanPopupVM.PopupType.SCANGIFT)
                    {
                        redemptionMainUserControlVM.ScanTicketGiftMode = 'G';
                        scanpopupVM.Title = MessageViewContainerList.GetMessage(this.ExecutionContext, "SCAN GIFT NOW");
                    }
                    redemptionMainUserControlVM.ScanPopUpView.Show();
                }
            });
            log.LogMethodExit();
        }

        internal void OnScanViewClosed(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RedemptionScanVM redemptionScanVM = (sender as RedemptionScanView).DataContext as RedemptionScanVM;
            if (redemptionMainUserControlVM != null && redemptionScanVM != null)
            {
                redemptionMainUserControlVM.ScanTicketGiftMode = redemptionScanVM.ScanTicketGiftMode;
            }
            SetMainViewFocus();
            log.LogMethodExit();
        }
        internal async Task SuspendClicked()
        {
            log.LogMethodEntry();
            try
            {
                if (redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Any()
                || redemptionMainUserControlVM.GetTotalTickets() > 0)
                {
                    SetRedemptionDTOGiftItems();
                    SetLoadingVisible(true);
                    bool result = await PostRedemption();
                    if (result)
                    {
                        result = await SuspendRedemption();
                        if (result)
                        {
                            string message = MessageViewContainerList.GetMessage(ExecutionContext, 2930, redemptionMainUserControlVM.NewRedemptionDTO.RedemptionOrderNo);
                            suspendedRedemptions.Add(redemptionMainUserControlVM.NewRedemptionDTO);
                            SetOtherRedemptionList(RedemptionUserControlVM.ActionType.Suspend);
                            redemptionMainUserControlVM.RedemptionDTO = new RedemptionDTO();
                            redemptionMainUserControlVM.RedemptionActivityDTO = new RedemptionActivityDTO();
                            redemptionMainUserControlVM.MembershipIDList.Clear();
                            redemptionMainUserControlVM.MembershipIDCardIDList.Clear();
                            redemptionMainUserControlVM.CardIDcustomerIDList.Clear();
                            redemptionMainUserControlVM.CustomerIDcustomerInfoList.Clear();
                            GenericTransactionListVM.ItemCollection.Clear();
                            UpdateTicketValues();
                            redemptionMainUserControlVM.CallRecalculatePriceandStock();
                            int count = 0;
                            if (suspendedRedemptions != null && suspendedRedemptions.Count > 0)
                            {
                                count = suspendedRedemptions.Where(r => (r.CreatedBy.ToLower() == this.ExecutionContext.UserId.ToLower())
                                || (r.LastUpdatedBy.ToLower() == this.ExecutionContext.UserId.ToLower())).Count();
                            }
                            SetCompletedSuspenedCount(RedemptionsType.Suspended, count);
                            redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(string.Empty, 0);
                            this.TransactionID = defaultTransactionID;
                            if (redemptionMainUserControlVM.RetreivedBackupDTO != null)
                            {
                                redemptionMainUserControlVM.RetreivedBackupDTO = null;
                            }
                            redemptionMainUserControlVM.ClearCompletedRedemption();
                            redemptionMainUserControlVM.SetFooterContent(message, MessageType.Info);
                        }
                    }
                    SetLoadingVisible(false);
                }
                else
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 371), MessageType.Info);
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
                SetMainViewFocus();
            }
            log.LogMethodExit();
        }

        internal void SetContentRightSectionVM()
        {
            log.LogMethodEntry();
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if(GenericRightSectionContentVM == null)
                {
                    GenericRightSectionContentVM = new GenericRightSectionContentVM();
                }
                GenericRightSectionContentVM.IsScreenUserAreaVisble =
                    GenericRightSectionContentVM.MultiScreenMode = multiScreenMode;
                GenericRightSectionContentVM.UserName = userName;
                GenericRightSectionContentVM.ScreenNumber = screenNumber;
            });
            log.LogMethodExit();
        }

        internal void RenderCompleteValues(List<RedemptionDTO> completedList)
        {
            log.LogMethodEntry(completedList);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (completedList == null)
                {
                    completedList = new List<RedemptionDTO>();
                }
                if (!isTurnIn && !isLoadTicket)
                {
                    completedList = completedList.Where(c => (c.Remarks == null || (c.Remarks != null && c.Remarks.ToLower() != "TURNINREDEMPTION".ToLower()))
                                        && c.RedemptionGiftsListDTO.Any()).ToList();
                }
                if (isLoadTicket && this.redemptionsType == RedemptionsType.Completed)
                {
                    completedList = completedList.Where(c => !c.RedemptionGiftsListDTO.Any()).ToList();
                }
                if (completedList.Count > 0)
                {
                    SetCustomDataGridVM(completedOrSuspendedRedemptions: completedList);
                }
                else
                {
                    CustomDataGridVM.Clear();
                }
                SetCompletedSuspenedCount(RedemptionsType.Completed, completedList.Where(r => r.RedemptionStatus.ToLower() != "suspended").ToList().Count);
            });
            log.LogMethodExit();
        }
        internal void SetCompletedSuspenedCount(RedemptionsType redemptionsType, int count)
        {
            log.LogMethodEntry(redemptionsType, count);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                string countText = GetNumberFormattedString(count);
                if (GenericToggleButtonsVM != null && GenericToggleButtonsVM.ToggleButtonItems != null)
                {
                    if (redemptionsType == RedemptionsType.Completed)
                    {
                        CustomToggleButtonItem toggleButtonItem = GenericToggleButtonsVM.ToggleButtonItems.FirstOrDefault(d => d.DisplayTags[0].Text == MessageViewContainerList.GetMessage(ExecutionContext, "Completed"));
                        if (toggleButtonItem != null)
                        {
                            toggleButtonItem.DisplayTags[1].Text = showSearchCloseIcon ? countText : "0";
                        }
                    }
                    else if (redemptionsType == RedemptionsType.Suspended && GenericToggleButtonsVM.ToggleButtonItems.Count > 1 &&
                    GenericToggleButtonsVM.ToggleButtonItems[1].DisplayTags[0].Text == MessageViewContainerList.GetMessage(ExecutionContext, "Suspended"))
                    {
                        GenericToggleButtonsVM.ToggleButtonItems[1].DisplayTags[1].Text = countText;
                    }
                    else if (redemptionsType == RedemptionsType.Voucher && GenericToggleButtonsVM.ToggleButtonItems.Count > 1)
                    {
                        GenericToggleButtonsVM.ToggleButtonItems[1].DisplayTags[1].Text = showSearchCloseIcon && this.redemptionsType == RedemptionsType.Flagged ? countText : "0";
                    }
                }
            });
            log.LogMethodExit();
        }
        internal async void CompleteTurninClicked()
        {
            log.LogMethodEntry();
            try
            {
                ticketsToLoad = 0;
                if (redemptionMainUserControlVM.RedemptionDTO != null)
                {
                    redemptionMainUserControlVM.RedemptionDTO.Remarks = "TURNINREDEMPTION";
                    if (!ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ALLOW_REDEMPTION_WITHOUT_CARD", false))
                    {
                        if (redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Count() == 0)
                        {
                            this.redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 475, null), MessageType.Error);
                            log.LogMethodExit();
                            return;
                        }
                    }
                    if (!redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Any())
                    {
                        this.redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 119, null), MessageType.Error);
                        log.LogMethodExit();
                        return;
                    }
                    if (SelectedTargetLocation == null)
                    {
                        this.redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 806, null), MessageType.Error);
                        log.Info("Ends-btnTurnInSave_Click() as not selected the target location");//Added for logger function on 08-Mar-2016
                        log.LogMethodExit();
                        return;
                    }
                    else
                    {
                        redemptionMainUserControlVM.RedemptionActivityDTO.TurninLocationId = -1;
                        redemptionMainUserControlVM.RedemptionActivityDTO.TargetLocationId = SelectedTargetLocation.LocationId;
                    }
                    foreach (RedemptionGiftsDTO redemptionGifts in redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO)
                    {
                        redemptionGifts.LocationId = SelectedTargetLocation.LocationId;
                        ticketsToLoad += Convert.ToInt32(redemptionGifts.Tickets);
                    }
                    ticketsToLoad = -1 * ticketsToLoad;
                    int mgrApprovalLimit = 0;
                    mgrApprovalLimit = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "REDEMPTION_REVERSAL_LIMIT_FOR_MANAGER_APPROVAL", 0);
                    if ((ticketsToLoad > mgrApprovalLimit && mgrApprovalLimit != 0) || mgrApprovalLimit == 0)
                    {
                        if (!UserViewContainerList.IsSelfApprovalAllowed(this.ExecutionContext.SiteId, this.ExecutionContext.UserPKId))
                        {
                            redemptionMainUserControlVM.OpenManagerView(ManageViewType.TurninLimit);
                        }
                        else
                        {
                            redemptionMainUserControlVM.RedemptionActivityDTO.ManagerToken = ExecutionContext.WebApiToken;
                            if (ticketsToLoad > 0 && !redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
                            {
                                ShowTurninPrintConfirmation();
                            }
                            else
                            {
                                bool result = await PostTurnInRedemption();
                                if (result)
                                {
                                    result = await CompleteTurnInRedemption();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ticketsToLoad > 0 && !redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
                        {
                            ShowTurninPrintConfirmation();
                        }
                        else
                        {
                            bool result = await PostTurnInRedemption();
                            if (result)
                            {
                                result = await CompleteTurnInRedemption();
                            }
                        }
                    }
                    redemptionMainUserControlVM.TurnInCardInfoText = string.Empty;
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            log.LogMethodExit();
        }

        private async Task<bool> PostTurnInRedemption(RedemptionDTO redemptionDTO = null)
        {
            log.LogMethodEntry(redemptionDTO);
            bool result = false;
            try
            {
                SetLoadingVisible(true);
                if (redemptionDTO == null)
                {
                    redemptionDTO = redemptionMainUserControlVM.RedemptionDTO;
                }
                RedemptionDTO backupRedemptionDTO = new RedemptionDTO();
                if (redemptionMainUserControlVM.NewRedemptionDTO != null && redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId >= 0)
                {
                    // fetch current server redemption
                    List<RedemptionDTO> redemptions = await GetRedemptions(redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId.ToString(), null, null, RedemptionDTO.RedemptionStatusEnum.NEW.ToString(), null, null, null);
                    if(redemptions != null && redemptions.Any())
                    {
                        backupRedemptionDTO = redemptions.FirstOrDefault();
                    }
                }
                if (backupRedemptionDTO != null && backupRedemptionDTO.RedemptionId >= 0)
                {
                    if (backupRedemptionDTO.RedemptionCardsListDTO != null && backupRedemptionDTO.RedemptionCardsListDTO.Any(x => x.RedemptionCardsId >= 0))
                    {
                        List<RedemptionCardsDTO> deleteRedemptionCardsList = new List<RedemptionCardsDTO>();
                        if (backupRedemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0 && x.RedemptionCardsId >= 0))
                        {
                            foreach (RedemptionCardsDTO redemptionCardsDTO in backupRedemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0 && x.RedemptionCardsId >= 0))
                            {
                                if (!redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId == redemptionCardsDTO.CardId))
                                {
                                    deleteRedemptionCardsList.Add(redemptionCardsDTO);
                                }
                                else
                                {
                                    if (((redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardId == redemptionCardsDTO.CardId).TotalCardTickets == null) ? 0 : redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardId == redemptionCardsDTO.CardId).TotalCardTickets)
                                        != ((redemptionCardsDTO.TotalCardTickets == null) ? 0 : redemptionCardsDTO.TotalCardTickets))
                                    {
                                        deleteRedemptionCardsList.Add(redemptionCardsDTO);
                                    }
                                }
                            }
                            if (deleteRedemptionCardsList != null && deleteRedemptionCardsList.Count > 0)
                            {
                                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                                backupRedemptionDTO = await redemptionUseCases.RemoveCard(backupRedemptionDTO.RedemptionId, deleteRedemptionCardsList);
                            }
                        }
                    }
                    if (backupRedemptionDTO.RedemptionGiftsListDTO != null)
                    {
                        List<RedemptionGiftsDTO> deleteRedemptionGiftsList = new List<RedemptionGiftsDTO>();
                        if (backupRedemptionDTO.RedemptionGiftsListDTO.Any(x => x.RedemptionGiftsId >= 0))
                        {
                            foreach (RedemptionGiftsDTO redemptionGiftsDTO in backupRedemptionDTO.RedemptionGiftsListDTO)
                            {
                                if (!redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Any(x => x.ProductId == redemptionGiftsDTO.ProductId))
                                {
                                    deleteRedemptionGiftsList.Add(redemptionGiftsDTO);
                                }
                                else
                                {
                                    if (redemptionGiftsDTO.RedemptionGiftsId >= 0)
                                    {
                                        if (!redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Any(x => x.RedemptionGiftsId == redemptionGiftsDTO.RedemptionGiftsId))
                                        {
                                            deleteRedemptionGiftsList.Add(redemptionGiftsDTO);
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                        if (deleteRedemptionGiftsList != null && deleteRedemptionGiftsList.Count > 0)
                        {
                            IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                            backupRedemptionDTO = await redemptionUseCases.RemoveGift(backupRedemptionDTO.RedemptionId, deleteRedemptionGiftsList);
                        }
                    }
                }
                if (redemptionDTO == null)
                {
                    redemptionDTO = redemptionMainUserControlVM.RedemptionDTO;
                }
                int newRedemptionId = -1;
                if (redemptionDTO != null)
                {
                    if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Count > 0)
                    {
                        if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0 && x.RedemptionCardsId < 0))
                        {
                            IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                            redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.AddTurnInCards(redemptionDTO.RedemptionId, redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0 && x.RedemptionCardsId < 0).ToList());
                        }
                        else if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0 && x.RedemptionCardsId >= 0 && x.IsChanged))
                        {
                            IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                            //NewRedemptionDTO = await redemptionUseCases.UpdateTurnInCards(redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardId >= 0 && x.RedemptionCardsId >= 0).RedemptionId, redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0 && x.RedemptionCardsId >= 0).ToList());
                        }
                        redemptionMainUserControlVM.NewRedemptionDTO.AcceptChanges();
                        if (redemptionMainUserControlVM.NewRedemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
                        {
                            foreach (RedemptionCardsDTO rcards in redemptionMainUserControlVM.NewRedemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0))
                            {
                                RedemptionCardsDTO rcard = redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardId == rcards.CardId);
                                int index = redemptionDTO.RedemptionCardsListDTO.IndexOf(rcard);
                                redemptionDTO.RedemptionCardsListDTO.Remove(rcard);
                                redemptionDTO.RedemptionCardsListDTO.Insert(index, rcards);
                            }
                        }
                    }
                    if (redemptionDTO.RedemptionGiftsListDTO != null && redemptionDTO.RedemptionGiftsListDTO.Count > 0)
                    {
                        IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                        if (redemptionMainUserControlVM.NewRedemptionDTO != null && redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId >= 0)
                        {
                            newRedemptionId = redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId;
                        }
                        else
                        {
                            newRedemptionId = redemptionDTO.RedemptionId;
                        }
                        if (redemptionDTO.RedemptionGiftsListDTO.Any(x => x.RedemptionGiftsId < 0))
                        {
                            redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.AddTurnInGifts(newRedemptionId, redemptionDTO.RedemptionGiftsListDTO.Where(x => x.RedemptionGiftsId < 0).ToList());
                        }
                        else if (redemptionDTO.RedemptionGiftsListDTO.Any(x => x.RedemptionGiftsId >= 0 && x.IsChanged))
                        {
                            //NewRedemptionDTO = await redemptionUseCases.UpdateTurnInGifts(redemptionDTO.RedemptionGiftsListDTO.FirstOrDefault(x => x.RedemptionGiftsId >= 0).RedemptionId, redemptionDTO.RedemptionGiftsListDTO.Where(x => x.RedemptionGiftsId >= 0).ToList());
                        }
                        redemptionMainUserControlVM.NewRedemptionDTO.AcceptChanges();
                        redemptionDTO.RedemptionGiftsListDTO = redemptionMainUserControlVM.NewRedemptionDTO.RedemptionGiftsListDTO;
                    }
                }
                result = true;
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal async Task CompleteClicked()
        {
            log.LogMethodEntry();
            bool result = false;
            printErrorMessage = string.Empty;
            try
            {
                ticketsToLoad = 0;
                if (redemptionMainUserControlVM.RedemptionDTO != null)
                {
                    if (!ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ALLOW_REDEMPTION_WITHOUT_CARD", false))
                    {
                        if (redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Count() == 0)
                        {
                            this.redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 475, null), MessageType.Error);
                            return;
                        }
                    }
                    if (!redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Any())
                    {
                        this.redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 119, null), MessageType.Error);
                        return;
                    }
                    else
                    {
                        if (redemptionMainUserControlVM != null &&
                            redemptionMainUserControlVM.RedemptionDTO != null &&
                            redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO != null
                            )
                        {
                            if (redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Any(x => x.ProductQuantity == 0))
                            {
                                this.redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 146, null), MessageType.Error);
                                return;
                            }
                            if (!ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "ALLOW_TRANSACTION_ON_ZERO_STOCK", false))
                            {
                                SetLoadingVisible(true);
                                foreach (int productId in redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Select(x => x.ProductId).Distinct())
                                {
                                    int quantity = redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Where(x => x.ProductId == productId).
                                      Sum(y => y.ProductQuantity);
                                    double availableqty = await GetTotalStock(mainVM.GetProductContainerDTOList(ExecutionContext).FirstOrDefault(x =>
                                    x.InventoryItemContainerDTO.ProductId == productId));
                                    if (quantity > availableqty)
                                    {
                                        this.redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 120, null), MessageType.Error);
                                        SetLoadingVisible(false);
                                        return;
                                    }
                                }
                                SetLoadingVisible(false);
                            }
                        }
                    }
                    if (redemptionMainUserControlVM.GetTotalRedeemed() > redemptionMainUserControlVM.GetTotalTickets() + redemptionMainUserControlVM.GetGraceTickets())
                    {
                        this.redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 121, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"), ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets")), MessageType.Error);
                        return;
                    }
                    int redemptionTicketLimit = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "REDEMPTION_TRANSACTION_TICKET_LIMIT", 0);
                    if (redemptionTicketLimit > 0 && redemptionMainUserControlVM.GetTotalRedeemed() > redemptionTicketLimit)
                    {
                        this.redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1438, redemptionMainUserControlVM.GetTotalRedeemed(), redemptionTicketLimit), MessageType.Error);
                        return;
                    }
                    int managerApprovalTicketLimit = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "REDEMPTION_LIMIT_FOR_MANAGER_APPROVAL", 0);
                    if (managerApprovalTicketLimit != 0 && redemptionMainUserControlVM.GetTotalRedeemed() > managerApprovalTicketLimit)
                    {
                        if (!UserViewContainerList.IsSelfApprovalAllowed(ExecutionContext.SiteId, ExecutionContext.UserPKId))
                        {
                            redemptionMainUserControlVM.OpenManagerView(ManageViewType.RedemptionLimit);
                            return;
                        }
                        else
                        {
                            redemptionMainUserControlVM.RedemptionActivityDTO.ManagerToken = ExecutionContext.WebApiToken;
                        }
                    }
                    ticketsToLoad = Convert.ToInt32(redemptionMainUserControlVM.GetTotalPhysicalTickets() + ((redemptionMainUserControlVM.RedemptionDTO.ManualTickets == null) ? 0 : redemptionMainUserControlVM.RedemptionDTO.ManualTickets) - redemptionMainUserControlVM.GetTotalRedeemed());
                    if (ticketsToLoad > 0)
                    {
                        if (redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO != null && redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
                        {
                            if ((ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "AUTO_LOAD_BALANCE_TICKETS_TO_CARD", true)
                                || redemptionMainUserControlVM.RedemptionDTO.TicketReceiptListDTO.Any()))
                            {
                                if (ticketsToLoad > ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LOAD_TICKETS_LIMIT"))
                                {
                                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2830, ticketsToLoad, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"), ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "LOAD_TICKETS_LIMIT")), MessageType.Error);
                                    return;
                                }
                                if (!LoadTicketLimitCheck(ManageViewType.LoadBalanceTicketLimit))
                                {
                                    return;
                                }
                                redemptionMainUserControlVM.RedemptionActivityDTO.LoadToCard = true;
                            }
                            else
                            {
                                redemptionMainUserControlVM.OpenGenericMessagePopupView(
                                    MessageViewContainerList.GetMessage(ExecutionContext, "Load Balance") + MessageViewContainerList.GetMessage(ExecutionContext, 2693, ScreenNumber),
                                    string.Empty,
                                    MessageViewContainerList.GetMessage(ExecutionContext, 2924, ticketsToLoad, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets")),
                                    MessageViewContainerList.GetMessage(this.ExecutionContext, "CONFIRM", null),
                                    MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                                    MessageButtonsType.OkCancel);
                                if (redemptionMainUserControlVM.MessagePopupView != null)
                                {
                                    redemptionMainUserControlVM.MessagePopupView.Closed += OnLoadBalancePopupViewClosed;
                                    redemptionMainUserControlVM.MessagePopupView.Show();
                                }
                                return;
                            }
                        }
                        else
                        {
                            if ((ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "AUTO_PRINT_BALANCE_TICKETS", true)
                                || redemptionMainUserControlVM.RedemptionDTO.TicketReceiptListDTO.Any()))
                            {
                                if (ticketsToLoad > ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LOAD_TICKETS_LIMIT"))
                                {
                                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2830, ticketsToLoad, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"), ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "LOAD_TICKETS_LIMIT")), MessageType.Error);
                                    return;
                                }
                                if (!LoadTicketLimitCheck(ManageViewType.PrintBalanceTicketLimit))
                                {
                                    return;
                                }
                                redemptionMainUserControlVM.RedemptionActivityDTO.PrintBalanceTicket = true;
                            }
                            else
                            {
                                redemptionMainUserControlVM.OpenGenericMessagePopupView(
                                     MessageViewContainerList.GetMessage(ExecutionContext, "Print Balance") + MessageViewContainerList.GetMessage(ExecutionContext, 2693, ScreenNumber),
                                    string.Empty,
                                    MessageViewContainerList.GetMessage(ExecutionContext, 125, ticketsToLoad, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets")),//change message no
                                    MessageViewContainerList.GetMessage(this.ExecutionContext, "CONFIRM", null),
                                    MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                                    MessageButtonsType.OkCancel);
                                if (redemptionMainUserControlVM.MessagePopupView != null)
                                {
                                    redemptionMainUserControlVM.MessagePopupView.Closed += OnPrintBalancePopupViewClosed;
                                    redemptionMainUserControlVM.MessagePopupView.Show();
                                }
                                return;
                            }
                        }
                    }
                    result = await PostRedemption();
                    if (result)
                    {
                        result = await CompleteRedemption();
                        if (result)
                        {
                            if (ticketsToLoad > 0)
                            {
                                if (redemptionMainUserControlVM.RedemptionActivityDTO.PrintBalanceTicket)
                                {
                                    ITicketReceiptUseCases ticketUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(ExecutionContext);
                                    List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchparams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                                    searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, Convert.ToString(ExecutionContext.GetSiteId())));
                                    if (redemptionMainUserControlVM.RedemptionDTO != null && redemptionMainUserControlVM.RedemptionDTO.RedemptionId >= 0)
                                    {
                                        searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID, redemptionMainUserControlVM.RedemptionDTO.RedemptionId.ToString()));
                                    }
                                    SetLoadingVisible(true);
                                    Task<List<TicketReceiptDTO>> taskGetreceipts = ticketUseCases.GetTicketReceipts(searchparams, 0, 0, null);
                                    List<TicketReceiptDTO> ticketReceiptDTOs = await taskGetreceipts;
                                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                                    if (ticketReceiptDTOs != null && ticketReceiptDTOs.Any())
                                    {
                                        try
                                        {
                                            clsTicket clsticket = await redemptionUseCases.PrintManualTicketReceipt(ticketReceiptDTOs.FirstOrDefault().Id);
                                            result = POSPrintHelper.PrintTicketsToPrinter(ExecutionContext, new List<clsTicket>() { clsticket }, redemptionMainUserControlVM.ScreenNumber.ToString());
                                            if (!result)
                                            {
                                                printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error") + "-" + ex.Message;
                                        }
                                        finally
                                        {
                                            SetLoadingVisible(false);
                                        }
                                    }
                                    else
                                    {
                                        log.Debug("No tickets receipt found for the redemption " + redemptionMainUserControlVM.RedemptionDTO.RedemptionId);
                                        printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                                    }
                                }
                            }
                            UpdateRedemptionUI();
                        }
                    }
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodExit();
        }

        private void UpdateRedemptionUI()
        {
            log.LogMethodEntry();
            string redemptionSuccessMessage = string.Empty;
            try
            {
                redemptionDTOList.Add(redemptionMainUserControlVM.RedemptionDTO);
            todayCompletedRedemptions.Add(redemptionMainUserControlVM.RedemptionDTO);
            if (!string.IsNullOrWhiteSpace(redemptionMainUserControlVM.RedemptionDTO.RedemptionOrderNo))
            {
                this.TransactionID = redemptionMainUserControlVM.RedemptionDTO.RedemptionOrderNo;
            }
            UpdateTicketValues();
            SetOtherRedemptionList(RedemptionUserControlVM.ActionType.Complete);
            redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(null, redemptionMainUserControlVM.GetBalanceTickets());
            SetCompletedSuspenedCount(RedemptionsType.Completed, todayCompletedRedemptions != null ? todayCompletedRedemptions.Where(c => c.RedemptionGiftsListDTO.Count > 0 && (c.Remarks == null || (c.Remarks != null && c.Remarks.ToLower() != "TURNINREDEMPTION".ToLower()))).ToList().Count : 0);
            if(!string.IsNullOrWhiteSpace(redemptionMainUserControlVM.RedemptionDTO.RedemptionOrderNo))
            {
                    redemptionSuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 2918,
                    redemptionMainUserControlVM.RedemptionDTO.RedemptionOrderNo) + " " + printErrorMessage;
            }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                CallClearRedemption();
                if (!string.IsNullOrWhiteSpace(redemptionSuccessMessage))
                {
                    redemptionMainUserControlVM.SetFooterContent(redemptionSuccessMessage, MessageType.Info);
                }
            }
            log.LogMethodExit();
        }
        private bool LoadTicketLimitCheck(ManageViewType viewType)
        {
            log.LogMethodEntry(viewType);
            bool managerApprovalNeeded = false;
            bool loadTicketLimit = false;
            ExecuteActionWithMainUserControlFooter(() =>
            {
                managerApprovalNeeded = (ProductViewContainerList.GetSystemProductContainerDTO(ExecutionContext.GetSiteId(), ManualProductType.REDEEMABLE.ToString(), "LOADTICKETS", "LOADTICKETS").ManagerApprovalRequired == "Y" || ProductViewContainerList.GetSystemProductContainerDTO(ExecutionContext.GetSiteId(), ManualProductType.REDEEMABLE.ToString(), "LOADTICKETS", "LOADTICKETS_NOLOYALTY").ManagerApprovalRequired == "Y") ? true : false;
                int loadTicketManagerApprovalLimit = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL");
                if ((loadTicketManagerApprovalLimit != 0 && ticketsToLoad > loadTicketManagerApprovalLimit) ||
                    (loadTicketManagerApprovalLimit == 0 && managerApprovalNeeded))
                {
                    if (!UserViewContainerList.IsSelfApprovalAllowed(ExecutionContext.SiteId, ExecutionContext.UserPKId))
                    {
                        redemptionMainUserControlVM.OpenManagerView(viewType);
                    }
                    else
                    {
                        switch (viewType)
                        {
                            case ManageViewType.LoadBalanceTicketLimit:
                                {
                                    redemptionMainUserControlVM.RedemptionActivityDTO.ManagerToken = ExecutionContext.WebApiToken;
                                    redemptionMainUserControlVM.RedemptionActivityDTO.LoadToCard = true;
                                }
                                break;
                            case ManageViewType.PrintBalanceTicketLimit:
                                {
                                    redemptionMainUserControlVM.RedemptionActivityDTO.ManagerToken = ExecutionContext.WebApiToken;
                                    redemptionMainUserControlVM.RedemptionActivityDTO.PrintBalanceTicket = true;
                                }
                                break;
                            case ManageViewType.LoadtoCardTicketLimit:
                                {
                                    redemptionMainUserControlVM.RedemptionLoadToCardRequestDTO.ManagerToken = ExecutionContext.WebApiToken;
                                }
                                break;
                            case ManageViewType.PrintConsolidateTicketLimit:
                                {
                                    redemptionMainUserControlVM.RedemptionActivityDTO.ManagerToken = ExecutionContext.WebApiToken;
                                }
                                break;
                        }
                        loadTicketLimit = true;
                    }
                }
                else
                {
                    loadTicketLimit = false;
                    switch (viewType)
                    {
                        case ManageViewType.LoadBalanceTicketLimit:
                            {
                                redemptionMainUserControlVM.RedemptionActivityDTO.LoadToCard = true;
                                loadTicketLimit = true;
                            }
                            break;
                        case ManageViewType.PrintBalanceTicketLimit:
                            {
                                redemptionMainUserControlVM.RedemptionActivityDTO.PrintBalanceTicket = true;
                                loadTicketLimit = true;
                            }
                            break;
                        case ManageViewType.LoadtoCardTicketLimit:
                        case ManageViewType.PrintConsolidateTicketLimit:
                            {
                                loadTicketLimit = true;
                            }
                            break;
                    }
                }
            });
            log.LogMethodExit();
            return loadTicketLimit;
        }

        private async void OnLoadBalancePopupViewClosed(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok)
                {
                    if (ticketsToLoad > ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LOAD_TICKETS_LIMIT"))
                    {
                        redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2830, ticketsToLoad, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"), ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "LOAD_TICKETS_LIMIT")), MessageType.Error);
                        return;
                    }
                    if (!LoadTicketLimitCheck(ManageViewType.LoadBalanceTicketLimit))
                    {
                        return;
                    }
                    redemptionMainUserControlVM.RedemptionActivityDTO.LoadToCard = true;
                }
                else
                {
                    if ((ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "AUTO_PRINT_BALANCE_TICKETS", false)
                        || redemptionMainUserControlVM.RedemptionDTO.TicketReceiptListDTO.Any()))
                    {
                        if (ticketsToLoad > ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LOAD_TICKETS_LIMIT"))
                        {
                            redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2830, ticketsToLoad, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"), ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "LOAD_TICKETS_LIMIT")), MessageType.Error);
                            return;
                        }
                        if (!LoadTicketLimitCheck(ManageViewType.PrintBalanceTicketLimit))
                        {
                            return;
                        }
                        redemptionMainUserControlVM.RedemptionActivityDTO.PrintBalanceTicket = true;
                    }
                    else
                    {
                        redemptionMainUserControlVM.OpenGenericMessagePopupView(
                             MessageViewContainerList.GetMessage(ExecutionContext, "Print Balance") + MessageViewContainerList.GetMessage(ExecutionContext, 2693, ScreenNumber),
                            string.Empty,
                            MessageViewContainerList.GetMessage(ExecutionContext, 125, ticketsToLoad, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets")),//change message no
                            MessageViewContainerList.GetMessage(this.ExecutionContext, "CONFIRM", null),
                            MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                            MessageButtonsType.OkCancel);
                        if (redemptionMainUserControlVM.MessagePopupView != null)
                        {
                            redemptionMainUserControlVM.MessagePopupView.Closed += OnPrintBalancePopupViewClosed;
                            redemptionMainUserControlVM.MessagePopupView.Show();
                        }
                        return;
                    }
                }
                bool result = await PostRedemption();
                if (result)
                {
                    result = await CompleteRedemption();
                    UpdateRedemptionUI();
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            SetMainViewFocus();
            log.LogMethodExit();
        }

        private async void OnPrintBalancePopupViewClosed(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
            if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok)
            {
                if (ticketsToLoad > ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LOAD_TICKETS_LIMIT"))
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2830, ticketsToLoad, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"), ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "LOAD_TICKETS_LIMIT")), MessageType.Error);
                    return;
                }
                if (!LoadTicketLimitCheck(ManageViewType.PrintBalanceTicketLimit))
                {
                    return;
                }
                redemptionMainUserControlVM.RedemptionActivityDTO.PrintBalanceTicket = true;
            }
            try
            {
                bool result = await PostRedemption();
                if (result)
                {
                    result = await CompleteRedemption();
                    if (result)
                    {
                        if (redemptionMainUserControlVM.RedemptionActivityDTO.PrintBalanceTicket)
                        {
                            try
                            {
                                //get receipt and print
                                ITicketReceiptUseCases ticketUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(ExecutionContext);
                                List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchparams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                                searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, Convert.ToString(ExecutionContext.GetSiteId())));
                                if (redemptionMainUserControlVM.RedemptionDTO != null && redemptionMainUserControlVM.RedemptionDTO.RedemptionId >= 0)
                                {
                                    searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID, redemptionMainUserControlVM.RedemptionDTO.RedemptionId.ToString()));
                                }
                                Task<List<TicketReceiptDTO>> taskGetreceipts = ticketUseCases.GetTicketReceipts(searchparams, 0, 0, null);
                                List<TicketReceiptDTO> ticketReceiptDTOs = await taskGetreceipts;
                                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                                if (ticketReceiptDTOs != null && ticketReceiptDTOs.Any())
                                {
                                    clsTicket clsticket = await redemptionUseCases.PrintManualTicketReceipt(ticketReceiptDTOs.FirstOrDefault().Id);
                                    result = POSPrintHelper.PrintTicketsToPrinter(ExecutionContext, new List<clsTicket>() { clsticket }, redemptionMainUserControlVM.ScreenNumber.ToString());
                                    if (!result)
                                    {
                                        printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                                    }
                                }
                                else
                                {
                                    log.Debug("No tickets receipt found for the redemption " + redemptionMainUserControlVM.RedemptionDTO.RedemptionId);
                                    printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                                }
                            }
                            catch (Exception ex)
                            {
                                printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error") +"-"+ ex.Message;
                            }
                        }
                        UpdateRedemptionUI();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                redemptionMainUserControlVM.SetFooterContent(ex.InnerException != null ? ex.InnerException.Message : ex.Message, MessageType.Error);
            }
            finally
            {
                SetLoadingVisible(false);
                SetMainViewFocus();
            }
            log.LogMethodExit();
        }

        internal async void OnTurninLimitManagerViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                AuthenticateManagerVM managerVM = (sender as AuthenticateManagerView).DataContext as AuthenticateManagerVM;
                if (managerVM != null && managerVM.IsValid)
                {
                    redemptionMainUserControlVM.RedemptionActivityDTO.ManagerToken = managerVM.ExecutionContext.WebApiToken;
                    if (ticketsToLoad > 0 && !redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
                    {
                        ShowTurninPrintConfirmation();
                    }
                    else
                    {
                        bool result = await PostTurnInRedemption();
                        if (result)
                        {
                            result = await CompleteTurnInRedemption();
                        }
                    }
                }
                else
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268, null), MessageType.Error);
                    log.Debug("End turn in ticket limit check -manager didnt approve");
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            SetMainViewFocus();
            log.LogMethodExit();
        }

        internal async void OnManagerViewTicketLimitClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                AuthenticateManagerVM managerVM = (sender as AuthenticateManagerView).DataContext as AuthenticateManagerVM;
                if (managerVM != null && managerVM.IsValid)
                {
                    redemptionMainUserControlVM.RedemptionActivityDTO.ManagerToken = managerVM.ExecutionContext.WebApiToken;
                    bool result = await PostRedemption();
                    if (result)
                    {
                        result = await CompleteRedemption();
                        if (result)
                        {
                            UpdateRedemptionUI();
                        }
                    }
                }
                else
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268, null), MessageType.Error);
                    log.Debug("End ticket limit check -manager didnt approve");
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            redemptionMainUserControlVM.ManagerView = null;
            SetMainViewFocus();
            log.LogMethodExit();
        }

        internal async void OnManagerViewLoadBalanceLimitClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                AuthenticateManagerVM managerVM = (sender as AuthenticateManagerView).DataContext as AuthenticateManagerVM;
                if (managerVM != null && managerVM.IsValid)
                {
                    redemptionMainUserControlVM.RedemptionActivityDTO.ManagerToken = managerVM.ExecutionContext.WebApiToken;
                    redemptionMainUserControlVM.RedemptionActivityDTO.LoadToCard = true;
                }
                else
                {
                    redemptionMainUserControlVM.RedemptionActivityDTO.LoadToCard = false;
                }
                bool result = await PostRedemption();
                if (result)
                {
                    result = await CompleteRedemption();
                    if (result)
                    { UpdateRedemptionUI(); }
                }

            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            redemptionMainUserControlVM.ManagerView = null;
            SetMainViewFocus();
            log.LogMethodExit();
        }
        internal async void OnManagerViewPrintBalanceLimitClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                AuthenticateManagerVM managerVM = (sender as AuthenticateManagerView).DataContext as AuthenticateManagerVM;
                if (managerVM != null && managerVM.IsValid)
                {
                    redemptionMainUserControlVM.RedemptionActivityDTO.ManagerToken = managerVM.ExecutionContext.WebApiToken;
                    redemptionMainUserControlVM.RedemptionActivityDTO.PrintBalanceTicket = true;
                }
                else
                {
                    redemptionMainUserControlVM.RedemptionActivityDTO.PrintBalanceTicket = false;
                }
                bool result = await PostRedemption();
                if (result)
                {
                    result = await CompleteRedemption();
                    if (result)
                    {
                        if (redemptionMainUserControlVM.RedemptionActivityDTO.PrintBalanceTicket)
                        {
                            try
                            {
                                //get receipt and print
                                ITicketReceiptUseCases ticketUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(ExecutionContext);
                                List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchparams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                                searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, Convert.ToString(ExecutionContext.GetSiteId())));
                                if (redemptionMainUserControlVM.RedemptionDTO != null && redemptionMainUserControlVM.RedemptionDTO.RedemptionId >= 0)
                                {
                                    searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID, redemptionMainUserControlVM.RedemptionDTO.RedemptionId.ToString()));
                                }
                                Task<List<TicketReceiptDTO>> taskGetreceipts = ticketUseCases.GetTicketReceipts(searchparams, 0, 0, null);
                                List<TicketReceiptDTO> ticketReceiptDTOs = await taskGetreceipts;
                                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                                if (ticketReceiptDTOs != null && ticketReceiptDTOs.Any())
                                {
                                    clsTicket clsticket = await redemptionUseCases.PrintManualTicketReceipt(ticketReceiptDTOs.FirstOrDefault().Id);
                                    result = POSPrintHelper.PrintTicketsToPrinter(ExecutionContext, new List<clsTicket>() { clsticket }, redemptionMainUserControlVM.ScreenNumber.ToString());
                                    if (!result)
                                    {
                                        printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                                    }
                                }
                                else
                                {
                                    log.Debug("No tickets receipt found for the redemption " + redemptionMainUserControlVM.RedemptionDTO.RedemptionId);
                                    printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                                }
                            }
                            catch (Exception ex)
                            {
                                printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error") +"-"+ ex.Message;
                            }
                        }
                        UpdateRedemptionUI();
                    }
                }

            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            redemptionMainUserControlVM.ManagerView = null;
            SetMainViewFocus();
            log.LogMethodExit();
        }

        internal async void OnManagerViewPrintConsolidateLimitClosed(object sender, EventArgs e)
        {
            string redemptionSuccessMessage = string.Empty;
            log.LogMethodEntry(sender, e);
            try
            {
                AuthenticateManagerVM managerVM = (sender as AuthenticateManagerView).DataContext as AuthenticateManagerVM;
                if (managerVM != null && managerVM.IsValid)
                {
                    redemptionMainUserControlVM.RedemptionActivityDTO.ManagerToken = managerVM.ExecutionContext.WebApiToken;
                    bool result = await PostRedemption();
                    if (result)
                    {
                        result = await PrintConsolidatedReceipt();
                        if (result)
                        {
                            redemptionSuccessMessage = UpdateTransactionUI();
                            CallClearRedemption();
                            if (!string.IsNullOrWhiteSpace(redemptionSuccessMessage))
                            {
                                redemptionMainUserControlVM.SetFooterContent(redemptionSuccessMessage, MessageType.Info);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(printErrorMessage))
                            {
                                redemptionMainUserControlVM.SetFooterContent(printErrorMessage, MessageType.Error);
                            }
                            else
                            {
                                redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 144), MessageType.Info);
                            }
                        }
                    }
                }
                else
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268), MessageType.Error);
                    log.Debug("End Print Consolidate-manager didnt approve");
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
                SetMainViewFocus();
            }
            log.LogMethodExit();
        }

        internal async void OnManagerViewLoadtoCardLimitClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            string redemptionSuccessMessage = string.Empty;
            try
            {
                AuthenticateManagerVM managerVM = (sender as AuthenticateManagerView).DataContext as AuthenticateManagerVM;
                if (managerVM != null && managerVM.IsValid)
                {
                    redemptionMainUserControlVM.RedemptionLoadToCardRequestDTO.ManagerToken = managerVM.ExecutionContext.WebApiToken;
                    bool result = await PostRedemption();
                    if (result)
                    {
                        result = await LoadtoCard();
                        if (result)
                        {
                            redemptionSuccessMessage = UpdateTransactionUI();
                            if (!string.IsNullOrWhiteSpace(redemptionSuccessMessage))
                            {
                                redemptionMainUserControlVM.SetFooterContent(redemptionSuccessMessage, MessageType.Info);
                            }
                        }
                    }
                }
                else
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268), MessageType.Error);
                    log.Debug("End Load to card-manager didnt approve");
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
                SetMainViewFocus();
            }
            log.LogMethodExit();
        }

        private async Task<bool> PostRedemption(RedemptionDTO redemptionDTO = null)
        {
            log.Info("PostRedemption"+redemptionMainUserControlVM.RedemptionDTO);
            log.LogMethodEntry();
            bool result = false;
            try
            {
                SetLoadingVisible(true);
                RedemptionDTO backupRedemptionDTO = new RedemptionDTO();
                if (redemptionMainUserControlVM.RetreivedBackupDTO != null)
                {
                    List<RedemptionDTO> suspendedDTO = await GetRedemptions(redemptionMainUserControlVM.RetreivedBackupDTO.RedemptionId.ToString(), null, null, "SUSPENDED", null, null, null);
                    backupRedemptionDTO = suspendedDTO.FirstOrDefault();
                }
                else
                {
                    if (redemptionDTO == null)
                    {
                        redemptionDTO = redemptionMainUserControlVM.RedemptionDTO;
                    }
                    if (redemptionMainUserControlVM.NewRedemptionDTO != null && redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId >= 0)
                    {
                        // fetch current server redemption
                        List<RedemptionDTO> redemptions = await GetRedemptions(redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId.ToString(), null, null, redemptionMainUserControlVM.NewRedemptionDTO.RedemptionStatus, null, null, null);
                        if (redemptions != null && redemptions.Any())
                        {
                            backupRedemptionDTO = redemptions.FirstOrDefault();
                        }
                    }
                }
                if (backupRedemptionDTO.RedemptionCardsListDTO != null && backupRedemptionDTO.RedemptionCardsListDTO.Any(x => x.RedemptionCardsId >= 0))
                {
                    List<RedemptionCardsDTO> deleteRedemptionCardsList = new List<RedemptionCardsDTO>();
                    if (backupRedemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0 && x.RedemptionCardsId >= 0))
                    {
                        deleteRedemptionCardsList.AddRange(backupRedemptionDTO.RedemptionCardsListDTO.Where(x => !redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Any(y => y.CardId == x.CardId && ((y.TotalCardTickets == null ? 0 : y.TotalCardTickets) == (x.TotalCardTickets == null ? 0 : x.TotalCardTickets)))));
                        if (deleteRedemptionCardsList != null && deleteRedemptionCardsList.Count > 0)
                        {
                            IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                            backupRedemptionDTO = await redemptionUseCases.RemoveCard(backupRedemptionDTO.RedemptionId, deleteRedemptionCardsList);
                        }
                    }
                    deleteRedemptionCardsList = new List<RedemptionCardsDTO>();
                    if (backupRedemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyId >= 0 && x.RedemptionCardsId >= 0))
                    {
                        deleteRedemptionCardsList.AddRange(backupRedemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0 && !redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Any(y => y.RedemptionCardsId == x.RedemptionCardsId && ((y.CurrencyQuantity == null ? 0 : y.CurrencyQuantity) == (x.CurrencyQuantity == null ? 0 : x.CurrencyQuantity)))));
                        if (deleteRedemptionCardsList != null && deleteRedemptionCardsList.Count > 0)
                        {
                            IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                            backupRedemptionDTO = await redemptionUseCases.RemoveCurrency(backupRedemptionDTO.RedemptionId, deleteRedemptionCardsList);
                        }
                    }
                }
                if (backupRedemptionDTO.TicketReceiptListDTO != null && backupRedemptionDTO.TicketReceiptListDTO.Any(x => x.Id >= 0))
                {
                    List<TicketReceiptDTO> deleteTicketReceiptList = new List<TicketReceiptDTO>();
                    foreach (TicketReceiptDTO ticketReceiptDTO in backupRedemptionDTO.TicketReceiptListDTO)
                    {
                        if (!redemptionMainUserControlVM.RedemptionDTO.TicketReceiptListDTO.Any(x => x.Id == ticketReceiptDTO.Id))
                        {
                            deleteTicketReceiptList.Add(ticketReceiptDTO);
                        }
                    }
                    if (deleteTicketReceiptList != null && deleteTicketReceiptList.Count > 0)
                    {
                        IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                        backupRedemptionDTO = await redemptionUseCases.RemoveTicket(backupRedemptionDTO.RedemptionId, deleteTicketReceiptList);
                    }
                }
                if (backupRedemptionDTO.ManualTickets != null && backupRedemptionDTO.ManualTickets > 0)
                {
                    if (redemptionMainUserControlVM.RedemptionDTO.ManualTickets != backupRedemptionDTO.ManualTickets)
                    {
                        IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                        RedemptionActivityDTO redemptionActivityManualTicketDTO = new RedemptionActivityDTO();
                        redemptionActivityManualTicketDTO.Source = "POS Redemption";
                        redemptionActivityManualTicketDTO.Tickets = Convert.ToInt32(backupRedemptionDTO.ManualTickets);
                        backupRedemptionDTO = await redemptionUseCases.RemoveManualTickets(backupRedemptionDTO.RedemptionId, redemptionActivityManualTicketDTO); // change to return redemtion dto
                    }
                }
                if (backupRedemptionDTO.RedemptionGiftsListDTO != null)
                {

                    List<RedemptionGiftsDTO> deleteRedemptionGiftsList = new List<RedemptionGiftsDTO>();
                    if (backupRedemptionDTO.RedemptionGiftsListDTO.Any(x => x.RedemptionGiftsId >= 0))
                    {
                        deleteRedemptionGiftsList.AddRange(backupRedemptionDTO.RedemptionGiftsListDTO.Where(x => !redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Any(y => y.RedemptionGiftsId == x.RedemptionGiftsId)));
                    }

                    if (deleteRedemptionGiftsList != null && deleteRedemptionGiftsList.Any())
                    {
                        IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                        backupRedemptionDTO = await redemptionUseCases.RemoveGift(backupRedemptionDTO.RedemptionId, deleteRedemptionGiftsList);
                    }
                }
                if (redemptionDTO == null)
                {
                    redemptionDTO = redemptionMainUserControlVM.RedemptionDTO;
                }
                int newRedemptionId = -1;
                if (redemptionDTO != null)
                {
                    if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Count > 0)
                    {
                        if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0 && x.RedemptionCardsId < 0))
                        {
                            IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                            redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.AddCard(redemptionDTO.RedemptionId, redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0 && x.RedemptionCardsId < 0).ToList());
                        }
                        else if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0 && x.RedemptionCardsId >= 0 && x.IsChanged))
                        {
                            IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                            redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.UpdateCard(redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardId >= 0 && x.RedemptionCardsId >= 0).RedemptionId, redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0 && x.RedemptionCardsId >= 0).ToList());
                        }
                        redemptionMainUserControlVM.NewRedemptionDTO.AcceptChanges();
                        if (redemptionMainUserControlVM.NewRedemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
                        {
                            foreach (RedemptionCardsDTO rcards in redemptionMainUserControlVM.NewRedemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0))
                            {
                                RedemptionCardsDTO rcard = redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardId == rcards.CardId);
                                int index = redemptionDTO.RedemptionCardsListDTO.IndexOf(rcard);
                                redemptionDTO.RedemptionCardsListDTO.Remove(rcard);
                                redemptionDTO.RedemptionCardsListDTO.Insert(index, rcards);
                            }
                        }
                        if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyId >= 0))
                        {
                            IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                            if (redemptionMainUserControlVM.NewRedemptionDTO != null && redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId >= 0)
                            {
                                newRedemptionId = redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId;
                            }
                            else
                            {
                                newRedemptionId = redemptionDTO.RedemptionId;
                            }
                            if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.RedemptionCardsId < 0 && x.CurrencyId >= 0))
                            {
                                redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.AddCurrency(newRedemptionId, redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0 && x.RedemptionCardsId < 0).ToList());
                            }
                            else if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.RedemptionCardsId >= 0 && !backupRedemptionDTO.RedemptionCardsListDTO.Any(y=> y.RedemptionCardsId==x.RedemptionCardsId) && x.IsChanged))
                            {
                                redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.AddCurrency(newRedemptionId, redemptionDTO.RedemptionCardsListDTO.Where(x => x.RedemptionCardsId >= 0 && !backupRedemptionDTO.RedemptionCardsListDTO.Any(y => y.RedemptionCardsId == x.RedemptionCardsId) && x.IsChanged).ToList());
                            }
                            redemptionMainUserControlVM.NewRedemptionDTO.AcceptChanges();
                            if (redemptionMainUserControlVM.NewRedemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyId >= 0))
                            {
                                foreach (RedemptionCardsDTO rcards in redemptionMainUserControlVM.NewRedemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0))
                                {
                                    RedemptionCardsDTO rcard = redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CurrencyId == rcards.CurrencyId);
                                    int index = redemptionDTO.RedemptionCardsListDTO.IndexOf(rcard);
                                    redemptionDTO.RedemptionCardsListDTO.Remove(rcard);
                                    redemptionDTO.RedemptionCardsListDTO.Insert(index, rcards);
                                }
                            }
                        }
                    }
                    if (redemptionDTO.TicketReceiptListDTO != null && redemptionDTO.TicketReceiptListDTO.Count > 0)
                    {
                        IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                        if (redemptionMainUserControlVM.NewRedemptionDTO != null && redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId >= 0)
                        {
                            newRedemptionId = redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId;
                        }
                        else
                        {
                            newRedemptionId = redemptionDTO.RedemptionId;
                        }
                        if (redemptionDTO.TicketReceiptListDTO.Any(x => x.SOurceRedemptionId < 0))
                        {
                            redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.AddTicket(newRedemptionId, redemptionDTO.TicketReceiptListDTO.Where(x => x.SOurceRedemptionId < 0).ToList());
                        }
                        else if (redemptionDTO.TicketReceiptListDTO.Any(x => x.SOurceRedemptionId >= 0 && x.IsChanged))
                        {
                            // NewRedemptionDTO = await redemptionUseCases.UpdateTicket(redemptionDTO.TicketReceiptListDTO.FirstorDefault(x => x.SOurceRedemptionId >= 0).SOurceRedemptionId, redemptionDTO.TicketReceiptListDTO.Where(x => x.SOurceRedemptionId >= 0).ToList());
                        }
                        redemptionMainUserControlVM.NewRedemptionDTO.AcceptChanges();
                        redemptionDTO.TicketReceiptListDTO = redemptionMainUserControlVM.NewRedemptionDTO.TicketReceiptListDTO;
                    }
                    if (redemptionDTO.ManualTickets != null && redemptionDTO.ManualTickets > 0 && redemptionDTO.IsChanged)
                    {
                        bool doManualTicketAction = true;
                        if (redemptionMainUserControlVM.NewRedemptionDTO != null && redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId >= 0)
                        {
                            newRedemptionId = redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId;
                            if (redemptionMainUserControlVM.NewRedemptionDTO.ManualTickets != null && redemptionMainUserControlVM.NewRedemptionDTO.ManualTickets > 0 && redemptionMainUserControlVM.NewRedemptionDTO.ManualTickets == redemptionDTO.ManualTickets)
                            {
                                doManualTicketAction = false;
                            }
                        }
                        else
                        {
                            newRedemptionId = redemptionDTO.RedemptionId;
                        }
                        if (doManualTicketAction)
                        {
                            IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                            redemptionMainUserControlVM.RedemptionActivityManualTicketDTO.Tickets = Convert.ToInt32(redemptionDTO.ManualTickets);
                            redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.AddManualTickets(newRedemptionId, redemptionMainUserControlVM.RedemptionActivityManualTicketDTO); // use case modify to handle list
                            redemptionMainUserControlVM.NewRedemptionDTO.AcceptChanges();
                        }
                    }
                    if (redemptionDTO.RedemptionGiftsListDTO != null && redemptionDTO.RedemptionGiftsListDTO.Any())
                    {
                        IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                        if (redemptionMainUserControlVM.NewRedemptionDTO != null && redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId >= 0)
                        {
                            newRedemptionId = redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId;
                        }
                        else
                        {
                            newRedemptionId = redemptionDTO.RedemptionId;
                        }
                        if (redemptionDTO.RedemptionGiftsListDTO.Any(x => x.RedemptionGiftsId < 0))
                        {
                            redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.AddGift(newRedemptionId, redemptionDTO.RedemptionGiftsListDTO.Where(x => x.RedemptionGiftsId < 0).ToList());
                        }
                        else if (redemptionDTO.RedemptionGiftsListDTO.Any(x => x.RedemptionGiftsId >= 0 && x.IsChanged))
                        {
                            redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.UpdateGift(redemptionDTO.RedemptionGiftsListDTO.FirstOrDefault(x => x.RedemptionGiftsId >= 0).RedemptionId, redemptionDTO.RedemptionGiftsListDTO.Where(x => x.RedemptionGiftsId >= 0).ToList());
                        }
                        redemptionMainUserControlVM.NewRedemptionDTO.AcceptChanges();
                        redemptionDTO.RedemptionGiftsListDTO = redemptionMainUserControlVM.NewRedemptionDTO.RedemptionGiftsListDTO;
                    }
                }
                result = true;
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                redemptionMainUserControlVM.RetreivedBackupDTO = null;
            }
            log.LogMethodEntry(result);
            return result;
        }
        private async Task<bool> CompleteRedemption()
        {
            log.Info("Complete Redemption" + redemptionMainUserControlVM.RedemptionDTO);
            log.LogMethodEntry();
            bool result = false;
            printErrorMessage = string.Empty;
            try
            {
                SetLoadingVisible(true);
                int newRedemptionId = -1;
                if (redemptionMainUserControlVM.NewRedemptionDTO != null && redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId >= 0)
                {
                    newRedemptionId = redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId;
                }
                else
                {
                    newRedemptionId = redemptionMainUserControlVM.RedemptionDTO.RedemptionId;
                }
                redemptionMainUserControlVM.RedemptionActivityDTO.Source = "POS Redemption";
                redemptionMainUserControlVM.RedemptionActivityDTO.Status = RedemptionActivityDTO.RedemptionActivityStatusEnum.DELIVERED;
                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.UpdateRedemptionStatus(newRedemptionId, redemptionMainUserControlVM.RedemptionActivityDTO); // change to return redemptionDTO
                try
                {
                    Task.Factory.StartNew(UpdateStock, redemptionMainUserControlVM.CancellationTokenSource.Token);
                }
                catch (OperationCanceledException ex)
                {
                    redemptionMainUserControlVM.ResetRecalculateFlags();
                }
                redemptionMainUserControlVM.RedemptionDTO = redemptionMainUserControlVM.NewRedemptionDTO;
                redemptionMainUserControlVM.NewRedemptionDTO = new RedemptionDTO();
                redemptionMainUserControlVM.RetreivedBackupDTO = null;
                if (ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "AUTO_PRINT_REDEMPTION_RECEIPT"))
                {
                    result = await RedemptionPrint(redemptionMainUserControlVM.RedemptionDTO.RedemptionId);
                    if (!result && string.IsNullOrWhiteSpace(printErrorMessage))
                    {
                        printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                    }
                }
                result = true;
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                //SetLoadingVisible(false);
            }
            log.LogMethodEntry(result);
            return result;
        }
        private void ShowTurninPrintConfirmation()
        {
            redemptionMainUserControlVM.OpenGenericMessagePopupView(
                MessageViewContainerList.GetMessage(this.ExecutionContext, "Print Receipt"),
                string.Empty,
                MessageViewContainerList.GetMessage(this.ExecutionContext, 138, ticketsToLoad, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets")),
                MessageViewContainerList.GetMessage(this.ExecutionContext, "YES", null),
                MessageViewContainerList.GetMessage(this.ExecutionContext, "NO", null),
                MessageButtonsType.OkCancel);
            if (redemptionMainUserControlVM.MessagePopupView != null)
            {
                redemptionMainUserControlVM.MessagePopupView.Closed += OnTurninPrintMessagePopupClosed;
                redemptionMainUserControlVM.MessagePopupView.Show();
            }
        }
        private async Task<bool> CompleteTurnInRedemption()
        {
            bool result = false;
            string redemptionSuccessMessage = string.Empty;
            log.LogMethodEntry();
            printErrorMessage = string.Empty;
            try
            {
                SetLoadingVisible(true);
                int newRedemptionId = -1;
                if (redemptionMainUserControlVM.NewRedemptionDTO != null && redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId >= 0)
                {
                    newRedemptionId = redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId;
                }
                else
                {
                    newRedemptionId = redemptionMainUserControlVM.RedemptionDTO.RedemptionId;
                }
                redemptionMainUserControlVM.RedemptionActivityDTO.Source = "POS Redemption";
                redemptionMainUserControlVM.RedemptionActivityDTO.Status = RedemptionActivityDTO.RedemptionActivityStatusEnum.DELIVERED;
                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.UpdateRedemptionStatus(newRedemptionId, redemptionMainUserControlVM.RedemptionActivityDTO); // change to return redemptionDTO
                try
                {
                    Task.Factory.StartNew(UpdateStock, redemptionMainUserControlVM.CancellationTokenSource.Token);
                }
                catch (OperationCanceledException ex)
                {
                    redemptionMainUserControlVM.ResetRecalculateFlags();
                }
                redemptionMainUserControlVM.RedemptionDTO = redemptionMainUserControlVM.NewRedemptionDTO;
                if (redemptionMainUserControlVM.RedemptionActivityDTO.PrintBalanceTicket)
                {
                    ITicketReceiptUseCases ticketUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(ExecutionContext);
                    List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchparams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                    searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, Convert.ToString(ExecutionContext.GetSiteId())));
                    if (redemptionMainUserControlVM.RedemptionDTO != null && redemptionMainUserControlVM.RedemptionDTO.RedemptionId >= 0)
                    {
                        searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID, redemptionMainUserControlVM.RedemptionDTO.RedemptionId.ToString()));
                    }
                    Task<List<TicketReceiptDTO>> taskGetreceipts = ticketUseCases.GetTicketReceipts(searchparams, 0, 0, null);
                    List<TicketReceiptDTO> ticketReceiptDTOs = await taskGetreceipts;
                    redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                    if (ticketReceiptDTOs != null && ticketReceiptDTOs.Any())
                    {
                        try
                        {
                            clsTicket clsticket = await redemptionUseCases.PrintManualTicketReceipt(ticketReceiptDTOs.FirstOrDefault().Id);
                            result = POSPrintHelper.PrintTicketsToPrinter(ExecutionContext, new List<clsTicket>() { clsticket }, redemptionMainUserControlVM.ScreenNumber.ToString());
                            if (!result)
                            {
                                printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                            }
                        }
                        catch (Exception ex)
                        {
                            printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error") +"-"+ ex.Message;
                        }
                    }
                    else
                    {
                        log.Debug("No tickets receipt found for the redemption " + redemptionMainUserControlVM.RedemptionDTO.RedemptionId);
                        printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                    }
                }
                if (ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "AUTO_PRINT_REDEMPTION_RECEIPT"))
                {
                    result = await RedemptionPrint(redemptionMainUserControlVM.RedemptionDTO.RedemptionId);
                    if (!result && string.IsNullOrWhiteSpace(printErrorMessage))
                    {
                        printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                    }
                }
                result = true;
                this.todayCompletedRedemptions.Add(redemptionMainUserControlVM.NewRedemptionDTO);
                SetCompletedSuspenedCount(RedemptionsType.Completed, this.todayCompletedRedemptions.Where(r => r.Remarks != null
                && r.Remarks.ToLower() == "TURNINREDEMPTION".ToLower()).Count());
                this.TransactionID = redemptionMainUserControlVM.NewRedemptionDTO.RedemptionOrderNo;
                if (redemptionMainUserControlVM != null)
                {
                    redemptionSuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 2918, redemptionMainUserControlVM.NewRedemptionDTO.RedemptionOrderNo) + " " + printErrorMessage;
                    CallClearRedemption();
                    redemptionMainUserControlVM.NewRedemptionDTO = new RedemptionDTO();
                    redemptionMainUserControlVM.RetreivedBackupDTO = null;
                    if (!string.IsNullOrWhiteSpace(redemptionSuccessMessage))
                    {
                        redemptionMainUserControlVM.SetFooterContent(redemptionSuccessMessage, MessageType.Info);
                    }

                }
                SetOtherRedemptionList();
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodEntry(result);
            return result;
        }

        private async Task<bool> SuspendRedemption()
        {
            bool result = false;
            log.LogMethodEntry();
            try
            {
                SetLoadingVisible(true);
                if (redemptionMainUserControlVM.RedemptionActivityDTO == null)
                {
                    redemptionMainUserControlVM.RedemptionActivityDTO = new RedemptionActivityDTO();
                }
                int newRedemptionId = -1;
                if (redemptionMainUserControlVM.NewRedemptionDTO != null && redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId >= 0)
                {
                    newRedemptionId = redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId;
                }
                else
                {
                    newRedemptionId = redemptionMainUserControlVM.RedemptionDTO.RedemptionId;
                }
                redemptionMainUserControlVM.RedemptionActivityDTO.Status = RedemptionActivityDTO.RedemptionActivityStatusEnum.SUSPENDED;
                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.UpdateRedemptionStatus(newRedemptionId, redemptionMainUserControlVM.RedemptionActivityDTO);// change to return redemptionDTO
                try
                {
                    Task.Factory.StartNew(UpdateStock, redemptionMainUserControlVM.CancellationTokenSource.Token);
                }
                catch (OperationCanceledException ex)
                {
                    redemptionMainUserControlVM.ResetRecalculateFlags();
                }
                result = await SuspendedRedemptionPrint(redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId);
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodExit(result);
            return result;
        }
        internal void UpdateStock()
        {
            log.LogMethodEntry();
            redemptionMainUserControlVM.CancellationTokenSource.Token.ThrowIfCancellationRequested();
            redemptionMainUserControlVM.CallRecalculatePriceandStock(true,true,true);
            log.LogMethodExit();
        }
        private async Task<bool> AbandonRedemption(RedemptionDTO redemptionDTO = null)
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                SetLoadingVisible(true);
                if (redemptionDTO == null)
                {
                    redemptionDTO = redemptionMainUserControlVM.RedemptionDTO;
                }
                redemptionMainUserControlVM.RedemptionActivityDTO = new RedemptionActivityDTO();
                redemptionMainUserControlVM.RedemptionActivityDTO.Status = RedemptionActivityDTO.RedemptionActivityStatusEnum.ABANDONED;
                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                redemptionDTO = await redemptionUseCases.UpdateRedemptionStatus(redemptionDTO.RedemptionId, redemptionMainUserControlVM.RedemptionActivityDTO);// change to return redemptionDTO
                result = true;
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodEntry(result);
            return result;
        }
        private async Task<bool> PrintConsolidatedReceipt()
        {
            bool result = false;
            printErrorMessage = string.Empty;
            log.LogMethodEntry();
            try
            {
                SetLoadingVisible(true);
                redemptionMainUserControlVM.RedemptionActivityDTO.Status = RedemptionActivityDTO.RedemptionActivityStatusEnum.DELIVERED;
                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.ConsolidateTicketReceipts(redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId, redemptionMainUserControlVM.RedemptionActivityDTO);// change to return redemptionDTO
                redemptionMainUserControlVM.RedemptionDTO = redemptionMainUserControlVM.NewRedemptionDTO;
                redemptionMainUserControlVM.NewRedemptionDTO = new RedemptionDTO();
                redemptionMainUserControlVM.RetreivedBackupDTO = null;
                ITicketReceiptUseCases ticketUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(ExecutionContext);
                List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchparams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, Convert.ToString(ExecutionContext.GetSiteId())));
                if (redemptionMainUserControlVM.RedemptionDTO != null && redemptionMainUserControlVM.RedemptionDTO.RedemptionId >= 0)
                {
                    searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID, redemptionMainUserControlVM.RedemptionDTO.RedemptionId.ToString()));
                }
                Task<List<TicketReceiptDTO>> taskGetreceipts = ticketUseCases.GetTicketReceipts(searchparams, 0, 0, null);
                List<TicketReceiptDTO> ticketReceiptDTOs = await taskGetreceipts;
                redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                if (ticketReceiptDTOs != null && ticketReceiptDTOs.Any())
                {
                    try
                    {
                        clsTicket clsticket = await redemptionUseCases.PrintManualTicketReceipt(ticketReceiptDTOs.FirstOrDefault().Id);
                        result = POSPrintHelper.PrintTicketsToPrinter(ExecutionContext, new List<clsTicket>() { clsticket }, redemptionMainUserControlVM.ScreenNumber.ToString());
                        if (!result)
                        {
                            printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error") +"-"+ ex.Message;
                    }
                }
                else
                {
                    log.Debug("No tickets receipt found for the redemption " + redemptionMainUserControlVM.RedemptionDTO.RedemptionId);
                    printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                }
                if (ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AUTO_PRINT_REDEMPTION_RECEIPT") == "Y")
                {
                    result = await RedemptionPrint(redemptionMainUserControlVM.RedemptionDTO.RedemptionId);
                }
                if (!result && string.IsNullOrWhiteSpace(printErrorMessage))
                {
                    printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                }
                result = true;
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    printErrorMessage = vex.Message;
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    printErrorMessage = pax.Message;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodExit(result);
            return result;
        }

        private async Task<bool> LoadtoCard()
        {
            bool result = false;
            printErrorMessage = string.Empty;
            log.LogMethodEntry();
            try
            {
                SetLoadingVisible(true);
                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                redemptionMainUserControlVM.NewRedemptionDTO = await redemptionUseCases.LoadTicketsToCard(redemptionMainUserControlVM.NewRedemptionDTO.RedemptionId, redemptionMainUserControlVM.RedemptionLoadToCardRequestDTO);// change to return redemptionDTO
                redemptionMainUserControlVM.RedemptionDTO = redemptionMainUserControlVM.NewRedemptionDTO;
                redemptionMainUserControlVM.NewRedemptionDTO = new RedemptionDTO();
                redemptionMainUserControlVM.RetreivedBackupDTO = null;
                result = true;
                if (ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AUTO_PRINT_LOAD_TICKETS") == "Y")
                {
                    result = await RedemptionPrint(redemptionMainUserControlVM.RedemptionDTO.RedemptionId);
                    if (!result && string.IsNullOrWhiteSpace(printErrorMessage))
                    {
                        printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                    }
                    if (result)
                    {
                        string redemptionSuccessMessage = ticketsToLoad + " " + MessageViewContainerList.GetMessage(ExecutionContext, 1381) + " " + printErrorMessage;
                        CallClearRedemption();
                        if (!string.IsNullOrWhiteSpace(redemptionSuccessMessage))
                        {
                            redemptionMainUserControlVM.SetFooterContent(redemptionSuccessMessage,MessageType.Info);
                        }
                    }
                }
                else if (ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AUTO_PRINT_LOAD_TICKETS") == "A")
                {
                    redemptionMainUserControlVM.OpenGenericMessagePopupView(
                    MessageViewContainerList.GetMessage(this.ExecutionContext, "Print Receipt"),
                    string.Empty,
                    MessageViewContainerList.GetMessage(this.ExecutionContext, 484),
                    MessageViewContainerList.GetMessage(this.ExecutionContext, "YES", null),
                    MessageViewContainerList.GetMessage(this.ExecutionContext, "NO", null),
                    MessageButtonsType.OkCancel);
                    if (redemptionMainUserControlVM.MessagePopupView != null)
                    {
                        redemptionMainUserControlVM.MessagePopupView.Closed += OnPrintPromptMessagePopupClosed;
                        redemptionMainUserControlVM.MessagePopupView.Show();
                        result = true;
                        log.LogMethodEntry(result);
                        SetLoadingVisible(false);
                        return result;
                    }
                }
                result = true;
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodExit(result);
            return result;
        }

        private async void OnTurninPrintMessagePopupClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
            if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok)
            {
                redemptionMainUserControlVM.RedemptionActivityDTO.PrintBalanceTicket = true;
            }
            else
            {
                redemptionMainUserControlVM.RedemptionActivityDTO.PrintBalanceTicket = false;
            }
            bool result = await PostTurnInRedemption();
            if (result)
            {
                result = await CompleteTurnInRedemption();
            }
            SetMainViewFocus();
            log.LogMethodExit();
        }

        private async void OnPrintPromptMessagePopupClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
            if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok)
            {
                bool result = await RedemptionPrint(redemptionMainUserControlVM.RedemptionDTO.RedemptionId);
                if (!result && string.IsNullOrWhiteSpace(printErrorMessage))
                {
                    printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                }
                if (!string.IsNullOrWhiteSpace(printErrorMessage))
                {
                    redemptionMainUserControlVM.SetFooterContent(printErrorMessage, MessageType.Error);
                }
            }
            string redemptionSuccessMessage = ticketsToLoad + " " + MessageViewContainerList.GetMessage(ExecutionContext, 1381) + " " + printErrorMessage;
            CallClearRedemption();
            if (!string.IsNullOrWhiteSpace(redemptionSuccessMessage))
            {
                redemptionMainUserControlVM.SetFooterContent(redemptionSuccessMessage, MessageType.Info);
            }
            SetMainViewFocus();
            log.LogMethodExit();
        }

        private async Task<bool> SuspendedRedemptionPrint(int id)
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                SetLoadingVisible(true);
                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                ReceiptClass receiptClass = await redemptionUseCases.GetSuspendedReceiptPrint(id);//change to suspended print
                result = POSPrintHelper.PrintReceiptToPrinter(ExecutionContext, receiptClass, MessageViewContainerList.GetMessage(ExecutionContext, "Redemption Receipt"), redemptionMainUserControlVM.ScreenNumber.ToString());
                if (!result)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Print Error"), MessageType.Error);
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodExit(result);
            return result;
        }

        private async Task<bool> RedemptionPrint(int id)
        {
            bool result = false;
            log.LogMethodEntry();
            printErrorMessage = string.Empty;
            try
            {
                SetLoadingVisible(true);
                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                ReceiptClass receiptClass = await redemptionUseCases.GetRedemptionOrderPrint(id);
                result = POSPrintHelper.PrintReceiptToPrinter(ExecutionContext, receiptClass, MessageViewContainerList.GetMessage(ExecutionContext, "Redemption Receipt"), redemptionMainUserControlVM.ScreenNumber.ToString());
                if (!result)
                {
                    printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error"+"-"+ vex.Message.ToString());
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error" + "-"+pax.Message.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error" + "-"+ex.Message.ToString());
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodExit(result);
            return result;
        }
        private void CallClearRedemption()
        {
            log.LogMethodEntry();
            try
            {
                if (redemptionMainUserControlVM != null && redemptionMainUserControlVM.RedemptionDTO != null
                       && !string.IsNullOrWhiteSpace(redemptionMainUserControlVM.RedemptionDTO.RedemptionStatus)
                       && (redemptionMainUserControlVM.RedemptionDTO.RedemptionStatus.ToLower() != RedemptionDTO.RedemptionStatusEnum.NEW.ToString().ToLower()
                       ))
                {
                    redemptionMainUserControlVM.ClearCompletedRedemption();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private async void PrintConsolidateClicked()
        {
            log.LogMethodEntry();
            string redemptionSuccessMessage = string.Empty;
            try
            {
                ticketsToLoad = 0;
                if (redemptionMainUserControlVM.RedemptionDTO.RedemptionStatus != null && redemptionMainUserControlVM.RedemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString())
                {
                    bool result = await RedemptionPrint(redemptionMainUserControlVM.RedemptionDTO.RedemptionId);
                    if (!result && string.IsNullOrWhiteSpace(printErrorMessage))
                    {
                        printErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Print Error");
                    }
                    if (!string.IsNullOrWhiteSpace(printErrorMessage))
                    {
                        redemptionMainUserControlVM.SetFooterContent(printErrorMessage, MessageType.Error);
                    }
                    return;
                }
                if (redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Any())
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1377), MessageType.Error);
                    return;
                }
                //should we check again if ticket receipt is not used?
                ticketsToLoad = Convert.ToInt32(redemptionMainUserControlVM.GetTotalPhysicalTickets() + (redemptionMainUserControlVM.RedemptionDTO.ManualTickets == null ? 0 : redemptionMainUserControlVM.RedemptionDTO.ManualTickets)); // manual ticket not applicable
                if (ticketsToLoad == 0)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 3043), MessageType.Warning);//Please add tickets to proceed with Load Ticket
                    return;
                }
                if (ticketsToLoad > ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LOAD_TICKETS_LIMIT"))
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2830, ticketsToLoad, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"), ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "LOAD_TICKETS_LIMIT")), MessageType.Error);
                    return;
                }
                if (LoadTicketLimitCheck(ManageViewType.PrintConsolidateTicketLimit))
                {
                    bool result = await PostRedemption();
                    if (result)
                    {
                        result = await PrintConsolidatedReceipt();
                        if (result)
                        {
                            redemptionSuccessMessage = UpdateTransactionUI();
                            CallClearRedemption();
                            if (!string.IsNullOrWhiteSpace(redemptionSuccessMessage))
                            {
                                redemptionMainUserControlVM.SetFooterContent(redemptionSuccessMessage, MessageType.Info);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(printErrorMessage))
                            {
                                redemptionMainUserControlVM.SetFooterContent(printErrorMessage, MessageType.Error);
                            }
                            else
                            {
                                redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 144), MessageType.Info);
                            }
                        }
                    }
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodExit();
        }

        internal async Task PrintRedemptionClicked()
        {
            log.LogMethodEntry();
            SetFooterContentEmpty();
            RedemptionDTO redemptionDTO = redemptionMainUserControlVM.RedemptionDTO;
            if (redemptionsType == RedemptionsType.Completed && selectedRedemptionDTO != null)
            {
                redemptionDTO = selectedRedemptionDTO;
            }
            if (redemptionDTO.RedemptionStatus != null && (redemptionDTO.RedemptionStatus != RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString() &&
               redemptionDTO.RedemptionStatus != RedemptionDTO.RedemptionStatusEnum.NEW.ToString()))
            {
                bool result = await RedemptionPrint(redemptionDTO.RedemptionId);
                if (result)
                {
                    CallClearRedemption();
                }
                return;
            }
            if (redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString())
            {
                bool result = await SuspendedRedemptionPrint(redemptionDTO.RedemptionId);
                if (result)
                {
                    CallClearRedemption();
                }
                return;
            }
            if (redemptionDTO.RedemptionGiftsListDTO.Any())
            {
                redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1410), MessageType.Warning);
                log.Warn("Please save before printing");
                return;
            }
            if (redemptionMainUserControlVM.EnableConsolidatePrint)
            {
                PrintConsolidateClicked();
            }
            else
            {
                redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4140), MessageType.Error);//9999
                return;
            }
            log.LogMethodExit();
        }

        internal void LoadToCardClicked()
        {
            log.LogMethodEntry();
            ExecuteActionWithMainUserControlFooter(() =>
            {
                ticketsToLoad = 0;
                SetFooterContentEmpty();
                if (!redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 459), MessageType.Error);
                    return;
                }
                else if (redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO != null && redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0).Count() > 1)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1376), MessageType.Error);
                    return;
                }
                if (redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Any())
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1377), MessageType.Error);
                    return;
                }
                if (redemptionMainUserControlVM.RedemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString())
                {
                    //redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1377), MessageType.Error);//new message Load to card completed.
                    return;
                }
                ticketsToLoad = Convert.ToInt32(redemptionMainUserControlVM.GetTotalPhysicalTickets() + (redemptionMainUserControlVM.RedemptionDTO.ManualTickets == null ? 0 : redemptionMainUserControlVM.RedemptionDTO.ManualTickets)); // manual ticket not applicable
                if (ticketsToLoad == 0)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2691), MessageType.Warning);//Please add tickets to proceed with Load Ticket
                    return;
                }
                if (ticketsToLoad > ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LOAD_TICKETS_LIMIT"))
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2830, ticketsToLoad, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"), ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "LOAD_TICKETS_LIMIT")), MessageType.Error);
                    return;
                }
                redemptionMainUserControlVM.RedemptionLoadToCardRequestDTO.AccountId = redemptionMainUserControlVM.RedemptionDTO.CardId;
                redemptionMainUserControlVM.RedemptionLoadToCardRequestDTO.TotalTickets = ticketsToLoad;
                if (ticketsToLoad > 0)
                {
                    redemptionMainUserControlVM.OpenGenericMessagePopupView(
                     MessageViewContainerList.GetMessage(ExecutionContext, "Load Tickets")
                                                        + MessageViewContainerList.GetMessage(ExecutionContext, 2693, ScreenNumber),
                    string.Empty,
                    MessageViewContainerList.GetMessage(ExecutionContext, 2683, ticketsToLoad.ToString()),
                    MessageViewContainerList.GetMessage(this.ExecutionContext, "YES", null),
                    MessageViewContainerList.GetMessage(this.ExecutionContext, "NO", null),
                    MessageButtonsType.OkCancel);
                    if (redemptionMainUserControlVM.MessagePopupView != null)
                    {
                        redemptionMainUserControlVM.MessagePopupView.Closed += OnLoadtoCardMessagePopupClosed;
                        redemptionMainUserControlVM.MessagePopupView.Show();
                    }
                }
                else
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2691), MessageType.Warning);//Please add tickets to proceed with Load Ticket
                    return;
                }
            });
            log.LogMethodExit();
        }
        private async void OnLoadtoCardMessagePopupClosed(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            string redemptionSuccessMessage = string.Empty;
            try
            {
                SetLoadingVisible(true);
                GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok)
                {
                    if (LoadTicketLimitCheck(ManageViewType.LoadtoCardTicketLimit))
                    {
                        bool result = await PostRedemption();
                        result = await LoadtoCard();
                        if (result)
                        {
                            redemptionSuccessMessage=UpdateTransactionUI();
                            if (!string.IsNullOrWhiteSpace(redemptionSuccessMessage))
                            {
                                redemptionMainUserControlVM.SetFooterContent(redemptionSuccessMessage, MessageType.Info);
                            }
                            SetLoadingVisible(false);
                        }
                    }
                }
                else
                {
                    SetLoadingVisible(false);
                    log.Debug("Ends LoadTickets_Click");
                    return;
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
                SetMainViewFocus();
            }
            log.LogMethodExit();
        }
        private string UpdateTransactionUI()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            ExecuteActionWithMainUserControlFooter(() =>
            {
                this.todayCompletedRedemptions.Add(redemptionMainUserControlVM.RedemptionDTO);
                this.TransactionID = redemptionMainUserControlVM.RedemptionDTO.RedemptionOrderNo;
                SetCompletedSuspenedCount(RedemptionsType.Completed, todayCompletedRedemptions.Where(r => r.RedemptionGiftsListDTO == null || (r.RedemptionGiftsListDTO != null && !r.RedemptionGiftsListDTO.Any())).Count());
                SetOtherRedemptionList();
                result = ticketsToLoad + " " + MessageViewContainerList.GetMessage(ExecutionContext, 1381) + " " + printErrorMessage;                
            });
            log.LogMethodExit(result);
            return result;
        }
        private void SetSelectedItem(int id)
        {
            log.LogMethodEntry(id);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (redemptionDTOList != null && redemptionDTOList.Count > 0)
                {
                    List<RedemptionGiftsDTO> GiftDTO = null;
                    if (redemptionDTOList != null && redemptionDTOList.Count > 0
                        && redemptionDTOList.Any(s => s.RedemptionId == id))
                    {
                        GiftDTO = redemptionDTOList.FirstOrDefault(s => s.RedemptionId == id).RedemptionGiftsListDTO;
                    }
                    else if (GiftDTO == null && todayCompletedRedemptions != null && todayCompletedRedemptions.Count > 0)
                    {
                        GiftDTO = todayCompletedRedemptions.FirstOrDefault(s => s.RedemptionId == id).RedemptionGiftsListDTO;
                    }
                    if (GiftDTO != null)
                    {
                        GenericTransactionListVM.ItemCollection.Clear();
                        foreach (RedemptionGiftsDTO redemptionGifts in GiftDTO)
                        {
                            GenericTransactionListItem redemptionRightSectionItem = GenericTransactionListVM.ItemCollection.FirstOrDefault(s => s.Key == mainVM.GetProductContainerDTOList(ExecutionContext).FirstOrDefault(x => x.InventoryItemContainerDTO.ProductId == redemptionGifts.ProductId).ProductId);
                            if (redemptionRightSectionItem != null)
                            {
                                redemptionRightSectionItem.Count += 1;
                            }
                            else
                            {
                                GenericTransactionListVM.ItemCollection.Add(new GenericTransactionListItem(ExecutionContext)
                                {
                                    ProductName = mainVM.GetProductContainerDTOList(ExecutionContext).FirstOrDefault(x => x.InventoryItemContainerDTO.ProductId == redemptionGifts.ProductId).ProductName,
                                    Ticket = Convert.ToInt32(redemptionGifts.Tickets),
                                    RedemptionRightSectionItemType = GenericTransactionListItemType.Item,
                                    TicketDisplayText = MessageViewContainerList.GetMessage(ExecutionContext, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets")),
                                    Count = 1,
                                    Key = mainVM.GetProductContainerDTOList(ExecutionContext).FirstOrDefault(x => x.InventoryItemContainerDTO.ProductId == redemptionGifts.ProductId).ProductId
                                });
                            }
                        }
                    }
                }
            });
            log.LogMethodExit();
        }

        internal void SetRedemptionDTOGiftItems(ProductsContainerDTO productDTO = null, int quantity = 1)
        {
            log.LogMethodEntry(quantity);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                LoadTotatlTicketCount = 0;
                if (redemptionMainUserControlVM == null)
                {
                    return;
                }
                if (redemptionMainUserControlVM.RedemptionDTO != null && !isLoadTicket)
                {
                    int poslocationId = POSMachineViewContainerList.GetPOSMachineContainerDTO(ExecutionContext).InventoryLocationId;
                    foreach (int productId in GenericTransactionListVM.ItemCollection.Select(x => x.Key).Distinct())
                    {
                        ProductsContainerDTO product = mainVM.GetProductContainerDTOList(ExecutionContext).FirstOrDefault(x => x.ProductId == productId);
                        SetSpecificRedemptionDTOGiftItem(product);
                    }
                }
            });
            log.LogMethodExit();
        }
        internal void SetSpecificRedemptionDTOGiftItem(ProductsContainerDTO product, int quantity = 1)
        {
            log.LogMethodEntry(quantity);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                int poslocationId = POSMachineViewContainerList.GetPOSMachineContainerDTO(ExecutionContext).InventoryLocationId;
                InventoryItemContainerDTO inventoryItem = product!=null?product.InventoryItemContainerDTO:null;
                if (inventoryItem != null)
                {
                    if (redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Any(x => x.ProductId == inventoryItem.ProductId))
                    {
                        if (redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Where(x => x.ProductId == inventoryItem.ProductId).Sum(x => x.ProductQuantity) != GenericTransactionListVM.ItemCollection.Where(x => x.Key == product.ProductId).Sum(y => y.Count))
                        {
                            int dtocount = redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Where(x => x.ProductId == inventoryItem.ProductId).Sum(y => y.ProductQuantity);
                            int rightsectioncount = GenericTransactionListVM.ItemCollection.Where(x => x.Key == product.ProductId).Sum(y => y.Count);
                            if (dtocount < rightsectioncount)
                            {
                                for (int i = dtocount; i < rightsectioncount; i++)
                                {
                                    redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Add(new RedemptionGiftsDTO()
                                    {
                                        ProductName = product.Description,
                                        GiftCode = inventoryItem.Code,
                                        ProductId = inventoryItem.ProductId,
                                        ProductQuantity = 1,
                                        Tickets = isTurnIn ? (-1) * inventoryItem.TurnInPriceInTickets : Convert.ToInt32(Math.Round(RedemptionPriceViewContainerList.GetLeastPriceInTickets(ExecutionContext.SiteId, product.ProductId, redemptionMainUserControlVM.MembershipIDList))),
                                        ProductDescription = inventoryItem.Description,
                                        OriginalPriceInTickets = Convert.ToInt32(inventoryItem.PriceInTickets),
                                        LocationId = (poslocationId == -1 ? inventoryItem.OutBoundLocationId : poslocationId)
                                    });
                                }
                                if (redemptionMainUserControlVM != null)
                                {
                                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2931, product.ProductName, dtocount, rightsectioncount), MessageType.Info);
                                }
                            }
                            else if (dtocount > rightsectioncount)
                            {
                                for (int i = rightsectioncount; i < dtocount; i++)
                                {
                                    RedemptionGiftsDTO deleteredemptionGiftsDTO = redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Any(x => x.RedemptionGiftsId < 0 && x.ProductId == inventoryItem.ProductId) ? redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.FirstOrDefault(x => x.RedemptionGiftsId < 0 && x.ProductId == inventoryItem.ProductId) :
                                    redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.FirstOrDefault(x => x.ProductId == inventoryItem.ProductId);
                                    redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Remove(deleteredemptionGiftsDTO);
                                }
                                if (redemptionMainUserControlVM != null)
                                {
                                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2931, product.ProductName, dtocount, rightsectioncount), MessageType.Info);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < quantity; i++)
                        {
                            redemptionMainUserControlVM.RedemptionDTO.RedemptionGiftsListDTO.Add(new RedemptionGiftsDTO()
                            {
                                ProductName = product.Description,
                                GiftCode = inventoryItem.Code,
                                ProductId = inventoryItem.ProductId,
                                ProductQuantity = 1,
                                Tickets = isTurnIn ? (-1) * inventoryItem.TurnInPriceInTickets : Convert.ToInt32(Math.Round(RedemptionPriceViewContainerList.GetLeastPriceInTickets(ExecutionContext.SiteId, product.ProductId, redemptionMainUserControlVM.MembershipIDList))),
                                ProductDescription = inventoryItem.Description,
                                OriginalPriceInTickets = Convert.ToInt32(inventoryItem.PriceInTickets),
                                LocationId = (poslocationId == -1 ? inventoryItem.OutBoundLocationId : poslocationId)
                            });
                        }
                        if (redemptionMainUserControlVM != null)
                        {
                            redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2676, product.ProductName), MessageType.Info);
                        }
                    }
                }
            });
            log.LogMethodExit();
        }

        private void SetRightSectionPropertyCollection(object selectedItem)
        {
            log.LogMethodEntry(selectedItem);
            IList<PropertyInfo> props = new List<PropertyInfo>(selectedItem.GetType().GetProperties());
            foreach (PropertyInfo prop in props.Where(x => x.Name != "SiteId" && x.Name != "SynchStatus" && x.Name != "Guid" && x.Name != "MasterEntityId" && x.Name != "IsChanged" && x.Name != "IsChangedRecursive"))
            {
                GenericRightSectionContentVM.PropertyCollections.Add(new RightSectionPropertyValues()
                {
                    Property = prop.Name,
                    Value = GetPropertyValueWithFormat(prop, prop.GetValue(selectedItem, null))
                });
                if (selectedItem is TicketReceiptDTO && prop.Name == "IsSuspected")
                {
                    using (NoSynchronizationContextScope.Enter())
                    {
                        Task<string> t = GetApplicationRemarks((selectedItem as TicketReceiptDTO).Guid);
                        t.Wait();
                        string remarks = t.Result;
                        GenericRightSectionContentVM.PropertyCollections.Add(new RightSectionPropertyValues()
                        {
                            Property = MessageViewContainerList.GetMessage(ExecutionContext, "Remarks"),
                            Value = remarks
                        });
                    }
                }
            }
            log.LogMethodExit();
        }

        private void OnIsSelected(object parameter)
        {
            log.LogMethodEntry(parameter);
            try
            {
                SetFooterContentEmpty();
                if (CustomDataGridVM != null && CustomDataGridVM.SelectedItem != null)
                {
                    TransactionID = defaultTransactionID;
                    GenericRightSectionContentVM.PropertyCollections.Clear();
                    int index = customDataGridVM.CollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem);
                    string heading = string.Empty;
                    switch (redemptionsType)
                    {
                        case RedemptionsType.Suspended:
                        case RedemptionsType.Completed:
                            {
                                SelectedRedemptionDTO = CustomDataGridVM.SelectedItem as RedemptionDTO;
                                if (SelectedRedemptionDTO != null)
                                {
                                    if (!string.IsNullOrEmpty(selectedRedemptionDTO.RedemptionOrderNo))
                                    {
                                        TransactionID = selectedRedemptionDTO.RedemptionOrderNo;
                                    }
                                    if (redemptionsType == RedemptionsType.Completed)
                                    {
                                        IsChangeStatusEnable = true;
                                        if ((!string.IsNullOrEmpty(this.selectedRedemptionDTO.RedemptionStatus) && this.selectedRedemptionDTO.RedemptionStatus.ToLower() == "delivered")
                           || selectedRedemptionDTO.OrigRedemptionId != -1)
                                        {
                                            IsChangeStatusEnable = false;
                                        }
                                    }
                                    SetRightSectionPropertyCollection(selectedRedemptionDTO);
                                    heading = selectedRedemptionDTO.RedemptionOrderNo + " " + selectedRedemptionDTO.RedemptionStatus;
                                }
                            }
                            break;
                        case RedemptionsType.Voucher:
                        case RedemptionsType.Flagged:
                            {
                                TicketReceiptDTO receiptDTO = CustomDataGridVM.SelectedItem as TicketReceiptDTO;
                                if (receiptDTO != null)
                                {
                                    SetRightSectionPropertyCollection(receiptDTO);
                                    heading = receiptDTO.ManualTicketReceiptNo;
                                }
                            }
                            break;
                    }
                    SetRightSideHeader(index, heading);
                }
                if (uiClicked)
                {
                    if (CustomDataGridVM != null && CustomDataGridVM.SelectedItem != null &&
                        CustomDataGridVM.SelectedItem == customDataGridVM.CollectionToBeRendered[0])
                    {
                        SetFooterContent();
                    }
                    uiClicked = false;
                }
                TranslatePage();
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            log.LogMethodExit();
        }
        private async Task<string> GetApplicationRemarks(string voucherGuid)
        {
            string result = string.Empty;
            log.LogMethodExit();
            try
            {
                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>> applicationRemarksSearchParams = new List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>>();
                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.ACTIVE_FLAG, "1"));
                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SITE_ID, ExecutionContext.SiteId.ToString()));
                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_NAME, "ManualTicketReceipts"));
                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_GUID, voucherGuid));
                List<ApplicationRemarksDTO> applicationRemarksDTOList = await redemptionUseCases.GetApplicationRemarks(applicationRemarksSearchParams);
                result = (applicationRemarksDTOList != null && applicationRemarksDTOList.Count > 0) ? applicationRemarksDTOList[0].Remarks : " ";
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            log.LogMethodExit(result);
            return result;
        }
        private void OnNextNavigation(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null &&
                   CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) < CustomDataGridVM.UICollectionToBeRendered.Count - 1)
                {
                    CustomDataGridVM.SelectedItem = CustomDataGridVM.UICollectionToBeRendered[CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) + 1];
                    this.OnIsSelected(null);
                }
            });
            log.LogMethodExit();
        }

        private void OnPreviousNavigation(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null &&
                 CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) > 0)
                {
                    CustomDataGridVM.SelectedItem = CustomDataGridVM.UICollectionToBeRendered[CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) - 1];
                    this.OnIsSelected(null);
                }
            });
            log.LogMethodExit();
        }
        private bool CanPreviousNavigationExecute()
        {
            log.LogMethodEntry();
            if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null &&
                 CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) == 0)
            {
                return false;
            }
            log.LogMethodExit();
            return true;
        }

        private bool CanNextNavigationExecute()
        {
            log.LogMethodEntry();
            if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null &&
                 CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) == CustomDataGridVM.UICollectionToBeRendered.Count - 1)
            {
                return false;
            }
            log.LogMethodExit();
            return true;
        }

        private async void OnSuspendCompleteActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            try
            {
                if (redemptionMainUserControl == null)
                {
                    FindAncestor(parameter as Button);
                }
                SetFooterContentEmpty();
                if (parameter != null)
                {
                    CustomActionButton button = parameter as CustomActionButton;
                    if (button != null && !string.IsNullOrEmpty(button.Name)
                        && CustomDataGridVM != null && CustomDataGridVM.SelectedItem != null
                        && redemptionDTOList != null)
                    {
                        switch (button.Name.ToLower())
                        {
                            case "btnretreive":
                                {
                                    redemptionMainUserControlVM.SetFooterContent(string.Empty, MessageType.None);
                                    RedemptionDTO redemptionDTO = customDataGridVM.SelectedItem as RedemptionDTO;
                                    SetSelectedItem(redemptionDTO.RedemptionId);
                                    if (redemptionDTO != null)
                                    {
                                        if (redemptionDTO.RedemptionId >= 0)
                                        {
                                            SetLoadingVisible(true);
                                            List<RedemptionDTO> suspendedDTO = await GetRedemptions(redemptionDTO.RedemptionId.ToString(), null, null, "SUSPENDED", null, null, null);
                                            if (suspendedDTO == null)
                                            {
                                                redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Unable to retrieve suspended redemption"), MessageType.Error);
                                                log.Info("Ends-retrieveSuspended - for redemption id" + redemptionDTO.RedemptionId + " as Unable to retrieve suspended redemption");
                                                SetLoadingVisible(false);
                                                return;
                                            }
                                            else
                                            {
                                                if (redemptionMainUserControlVM.RetreivedBackupDTO == null)
                                                {
                                                    redemptionMainUserControlVM.RetreivedBackupDTO = new RedemptionDTO(redemptionDTO.RedemptionId, redemptionDTO.PrimaryCardNumber, redemptionDTO.ManualTickets,
                                                        redemptionDTO.ETickets, redemptionDTO.RedeemedDate, redemptionDTO.CardId, redemptionDTO.OrigRedemptionId, redemptionDTO.Remarks,
                                                        redemptionDTO.GraceTickets, redemptionDTO.ReceiptTickets, redemptionDTO.CurrencyTickets, redemptionDTO.LastUpdatedBy,
                                                        redemptionDTO.SiteId, redemptionDTO.Guid, redemptionDTO.SynchStatus, redemptionDTO.MasterEntityId, redemptionDTO.Source,
                                                        redemptionDTO.RedemptionOrderNo, redemptionDTO.LastUpdateDate, redemptionDTO.OrderCompletedDate, redemptionDTO.OrderDeliveredDate,
                                                        redemptionDTO.RedemptionStatus, redemptionDTO.CreationDate, redemptionDTO.CreatedBy, redemptionDTO.RedemptionGiftsListDTO.ToList(),
                                                        redemptionDTO.RedemptionCardsListDTO.ToList(), redemptionDTO.TicketReceiptListDTO.ToList(), redemptionDTO.RedemptionTicketAllocationListDTO.ToList(),
                                                        redemptionDTO.CustomerName, redemptionDTO.POSMachineId, redemptionDTO.CustomerId, redemptionDTO.PosMachineName, redemptionDTO.OriginalRedemptionOrderNo);
                                                }
                                                redemptionDTOList.Remove(redemptionDTO);
                                                suspendedRedemptions.Remove(suspendedRedemptions.FirstOrDefault(x => x.RedemptionId == redemptionDTO.RedemptionId));
                                                GenericToggleButtonsVM.ToggleButtonItems[0].IsChecked = true;
                                                StayInTransactionMode = true;
                                                SetTransactionListVM(redemptionDTO.RedemptionOrderNo);
                                                redemptionMainUserControlVM.RedemptionDTO = redemptionDTO;
                                                bool result = await redemptionMainUserControlVM.RecomputeSuspendedRedemption();
                                                SetOtherRedemptionList(RedemptionUserControlVM.ActionType.Retrieve);
                                                SetCompletedSuspenedCount(RedemptionsType.Suspended, suspendedRedemptions.Where(r => (r.CreatedBy.ToLower() == this.ExecutionContext.UserId.ToLower())
                                                || (r.LastUpdatedBy.ToLower() == this.ExecutionContext.UserId.ToLower())).Count());
                                                UpdateTicketValues();
                                                redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo((!string.IsNullOrWhiteSpace(redemptionDTO.CustomerName) ? redemptionDTO.CustomerName : redemptionDTO.PrimaryCardNumber), redemptionMainUserControlVM.GetBalanceTickets());
                                                if (selectedRedemptionDTO != null)
                                                {
                                                    selectedRedemptionDTO = null;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case "btnreverse":
                                {
                                    redemptionMainUserControlVM.SetFooterContent(string.Empty, MessageType.None);
                                    if (selectedRedemptionDTO != null)
                                    {
                                        redemptionMainUserControlVM.ReverseView = new RedemptionReverseView();
                                        RedemptionReverseVM reverseVM = new RedemptionReverseVM(this.ExecutionContext, selectedRedemptionDTO, screenNumber, redemptionMainUserControlVM.CardReader, redemptionMainUserControlVM);
                                        if (mainVM != null && mainVM.RowCount > 1)
                                        {
                                            reverseVM.IsMultiScreenRowTwo = true;
                                        }
                                        reverseVM.MultiScreenMode = this.MultiScreenMode;
                                        redemptionMainUserControlVM.ReverseView.DataContext = reverseVM;
                                        redemptionMainUserControlVM.ReverseView.PreviewMouseDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                                        redemptionMainUserControlVM.ReverseView.PreviewKeyDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                                        redemptionMainUserControlVM.ReverseView.Loaded += redemptionMainUserControlVM.OnWindowLoaded;
                                        redemptionMainUserControlVM.ReverseView.Show();
                                    }
                                }
                                break;
                            case "btnchangestatus":
                                {
                                    if (RedemptionsType == RedemptionsType.Completed)
                                    {
                                        redemptionMainUserControlVM.UpdateView = new RedemptionUpdateView();
                                        RedemptionUpdateVM updateVM = new RedemptionUpdateVM(this.ExecutionContext, this.redemptionMainUserControlVM);
                                        if (mainVM != null && mainVM.RowCount > 1)
                                        {
                                            updateVM.IsMultiScreenRowTwo = true;
                                        }
                                        updateVM.MultiScreenMode = this.multiScreenMode;
                                        redemptionMainUserControlVM.UpdateView.DataContext = updateVM;
                                        redemptionMainUserControlVM.UpdateView.PreviewMouseDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                                        redemptionMainUserControlVM.UpdateView.PreviewKeyDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                                        if (redemptionMainUserControlVM.UpdateView.KeyboardHelper != null)
                                        {
                                            redemptionMainUserControlVM.SetKeyBoardHelperColorCode();
                                            redemptionMainUserControlVM.UpdateView.KeyboardHelper.KeypadMouseDownEvent -= redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                                            redemptionMainUserControlVM.UpdateView.KeyboardHelper.KeypadMouseDownEvent += redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                                        }
                                        redemptionMainUserControlVM.UpdateView.Loaded += redemptionMainUserControlVM.OnWindowLoaded;
                                        redemptionMainUserControlVM.UpdateView.Closed += OnUpdateViewClosed;
                                        redemptionMainUserControlVM.UpdateView.Show();
                                    }
                                }
                                break;
                            case "btndelete":
                                {
                                    redemptionMainUserControlVM.OpenGenericMessagePopupView(
                                        MessageViewContainerList.GetMessage(this.ExecutionContext, "Confirm Delete" + MessageViewContainerList.GetMessage(ExecutionContext, 2693, ScreenNumber)),
                                        string.Empty,
                                        MessageViewContainerList.GetMessage(this.ExecutionContext, 1766),
                                        MessageViewContainerList.GetMessage(this.ExecutionContext, "YES", null),
                                        MessageViewContainerList.GetMessage(this.ExecutionContext, "NO", null),
                                        MessageButtonsType.OkCancel);
                                    if (redemptionMainUserControlVM.MessagePopupView != null)
                                    {
                                        redemptionMainUserControlVM.MessagePopupView.Closed += OnSuspendDeletePopupClosed;
                                        redemptionMainUserControlVM.MessagePopupView.Show();
                                    }
                                }
                                break;
                            case "btncomplete":
                                {
                                    if (isVoucher)
                                    {
                                        if (!UserViewContainerList.IsSelfApprovalAllowed(this.ExecutionContext.SiteId, this.ExecutionContext.UserPKId))
                                        {
                                            if (redemptionMainUserControlVM != null)
                                            {
                                                redemptionMainUserControlVM.OpenManagerView(ManageViewType.Flag);
                                            }
                                        }
                                        else
                                        {
                                            redemptionMainUserControlVM.ShowFlagOrEnterTicketView(true);
                                        }
                                    }

                                }
                                break;
                            case "btnunflag":
                                {
                                    if (isVoucher)
                                    {
                                        if (!UserViewContainerList.IsSelfApprovalAllowed(this.ExecutionContext.SiteId, this.ExecutionContext.UserPKId))
                                        {
                                            if (redemptionMainUserControlVM != null)
                                            {
                                                redemptionMainUserControlVM.OpenManagerView(ManageViewType.Flag);
                                            }
                                        }
                                        else
                                        {
                                            redemptionMainUserControlVM.ShowFlagOrEnterTicketView(true);
                                        }
                                    }
                                }
                                break;
                            case "btnreprint":
                                {
                                    if (isVoucher && redemptionsType == RedemptionsType.Voucher)
                                    {
                                        if (redemptionMainUserControlVM != null && redemptionMainUserControlVM.VoucherUserControlVM != null &&
                                            redemptionMainUserControlVM.VoucherUserControlVM.CustomDataGridVM != null &&
                                            redemptionMainUserControlVM.VoucherUserControlVM.CustomDataGridVM.SelectedItem != null &&
                                            (redemptionMainUserControlVM.VoucherUserControlVM.CustomDataGridVM.SelectedItem as TicketReceiptDTO).BalanceTickets <= 0)
                                        {
                                            redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 112), MessageType.Warning);
                                            return;
                                        }
                                        redemptionMainUserControlVM.RedemptionActivityDTO = new RedemptionActivityDTO();
                                        bool managerApprovalRequired = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(this.ExecutionContext, "MANAGER_APPROVAL_TO_REPRINT_TICKET_RECEIPT");
                                        if (managerApprovalRequired && !UserViewContainerList.IsSelfApprovalAllowed(this.ExecutionContext.SiteId, this.ExecutionContext.UserPKId))
                                        {
                                            redemptionMainUserControlVM.OpenManagerView(ManageViewType.RePrint);
                                        }
                                        else
                                        {
                                            redemptionMainUserControlVM.RedemptionActivityDTO.ManagerToken = ExecutionContext.WebApiToken;
                                            redemptionMainUserControlVM.RePrint();
                                        }
                                    }
                                }
                                break;
                            case "btnprint":
                                {
                                    PrintRedemptionClicked();
                                }
                                break;
                        }
                    }
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodExit();
        }
        private void OnSuspendDeletePopupClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok)
                {
                    SuspendDeleteClicked();
                }
                SetMainViewFocus();
            });
            log.LogMethodExit();
        }
        private void SetRightSideHeader(int index, string heading)
        {
            log.LogMethodEntry();
            ExecuteActionWithMainUserControlFooter(() =>
            {
                GenericRightSectionContentVM.Heading = heading;
                GenericRightSectionContentVM.SubHeading = (index + 1).ToString() + " " + MessageViewContainerList.GetMessage(ExecutionContext, "of") + " " + CustomDataGridVM.UICollectionToBeRendered.Count.ToString();
                GenericRightSectionContentVM.IsPreviousNavigationEnabled = CanPreviousNavigationExecute();
                GenericRightSectionContentVM.IsNextNavigationEnabled = CanNextNavigationExecute();
            });
            log.LogMethodExit();
        }
        internal void SetOtherRedemptionList(RedemptionUserControlVM.ActionType actionType = RedemptionUserControlVM.ActionType.None, RedemptionDTO completedRedemptionDTO = null)
        {
            log.LogMethodEntry();
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (mainVM != null && mainVM.RedemptionUserControlVMs != null && mainVM.RedemptionUserControlVMs.Count > 1
                && redemptionMainUserControlVM != null)
                {
                    foreach (RedemptionMainUserControlVM mainUserControlVM in mainVM.RedemptionUserControlVMs)
                    {
                        if (mainUserControlVM != null && mainUserControlVM != redemptionMainUserControlVM)
                        {
                            if (mainUserControlVM.RedemptionUserControlVM != null)
                            {
                                if (actionType != RedemptionUserControlVM.ActionType.Complete && actionType != RedemptionUserControlVM.ActionType.None)
                                {
                                    RedemptionDTO redemptionDTO = null;
                                    if (actionType == RedemptionUserControlVM.ActionType.SuspendRetrieve && redemptionMainUserControlVM.RetreivedBackupDTO != null)
                                    {
                                        redemptionDTO = redemptionMainUserControlVM.RetreivedBackupDTO;
                                    }
                                    else if (actionType == RedemptionUserControlVM.ActionType.Retrieve && redemptionMainUserControlVM.RetreivedBackupDTO != null)
                                    {
                                        if (mainUserControlVM.RedemptionUserControlVM.redemptionDTOList != null)
                                        {
                                            redemptionDTO = mainUserControlVM.RedemptionUserControlVM.redemptionDTOList.FirstOrDefault(r => r.RedemptionId == redemptionMainUserControlVM.RetreivedBackupDTO.RedemptionId);
                                        }
                                        else if (mainUserControlVM.RedemptionUserControlVM.suspendedRedemptions != null)
                                        {
                                            redemptionDTO = mainUserControlVM.RedemptionUserControlVM.suspendedRedemptions.FirstOrDefault(r => r.RedemptionId == redemptionMainUserControlVM.RetreivedBackupDTO.RedemptionId);
                                        }
                                    }
                                    else if (actionType == RedemptionUserControlVM.ActionType.Suspend)
                                    {
                                        redemptionDTO = redemptionMainUserControlVM.NewRedemptionDTO;
                                    }
                                    else
                                    {
                                        redemptionDTO = selectedRedemptionDTO;
                                    }
                                    if (redemptionDTO != null)
                                    {
                                        switch (actionType)
                                        {
                                            case RedemptionUserControlVM.ActionType.Delete:
                                            case RedemptionUserControlVM.ActionType.Retrieve:
                                                {
                                                    if (mainUserControlVM.RedemptionUserControlVM.redemptionDTOList != null)
                                                    {
                                                        RedemptionDTO removableRedemption = mainUserControlVM.RedemptionUserControlVM.redemptionDTOList.FirstOrDefault(r => r.RedemptionId == redemptionDTO.RedemptionId);
                                                        if (removableRedemption != null)
                                                        {
                                                            mainUserControlVM.RedemptionUserControlVM.redemptionDTOList.Remove(removableRedemption);
                                                        }
                                                    }
                                                    if (mainUserControlVM.RedemptionUserControlVM.suspendedRedemptions != null)
                                                    {
                                                        RedemptionDTO removableRedemption = mainUserControlVM.RedemptionUserControlVM.suspendedRedemptions.FirstOrDefault(r => r.RedemptionId == redemptionDTO.RedemptionId);
                                                        if (removableRedemption != null)
                                                        {
                                                            mainUserControlVM.RedemptionUserControlVM.suspendedRedemptions.Remove(removableRedemption);
                                                        }
                                                    }
                                                }
                                                break;
                                            case RedemptionUserControlVM.ActionType.Suspend:
                                            case RedemptionUserControlVM.ActionType.SuspendRetrieve:
                                                {
                                                    if (mainUserControlVM.RedemptionUserControlVM.redemptionDTOList != null)
                                                    {
                                                        RedemptionDTO existingRedemption = mainUserControlVM.RedemptionUserControlVM.redemptionDTOList.FirstOrDefault(r => r.RedemptionId == redemptionDTO.RedemptionId);
                                                        if (existingRedemption != null)
                                                        {
                                                            int index = mainUserControlVM.RedemptionUserControlVM.redemptionDTOList.IndexOf(existingRedemption);
                                                            if (index > -1)
                                                            {
                                                                mainUserControlVM.RedemptionUserControlVM.redemptionDTOList[index] = redemptionDTO;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            mainUserControlVM.RedemptionUserControlVM.redemptionDTOList.Add(redemptionDTO);
                                                        }
                                                    }
                                                    if (mainUserControlVM.RedemptionUserControlVM.suspendedRedemptions != null)
                                                    {
                                                        RedemptionDTO existingRedemption = mainUserControlVM.RedemptionUserControlVM.suspendedRedemptions.FirstOrDefault(r => r.RedemptionId == redemptionDTO.RedemptionId);
                                                        if (existingRedemption != null)
                                                        {
                                                            int index = mainUserControlVM.RedemptionUserControlVM.suspendedRedemptions.IndexOf(existingRedemption);
                                                            if (index > -1)
                                                            {
                                                                mainUserControlVM.RedemptionUserControlVM.suspendedRedemptions[index] = redemptionDTO;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            mainUserControlVM.RedemptionUserControlVM.suspendedRedemptions.Add(redemptionDTO);
                                                        }

                                                    }
                                                }
                                                break;
                                        }
                                    }
                                    if (mainUserControlVM.RedemptionUserControlVM.redemptionsType == RedemptionsType.Suspended)
                                    {
                                        object selectedRedemption = null;
                                        if (mainUserControlVM.RedemptionUserControlVM.CustomDataGridVM.SelectedItem != null)
                                        {
                                            selectedRedemption = mainUserControlVM.RedemptionUserControlVM.CustomDataGridVM.SelectedItem;
                                        }
                                        mainUserControlVM.RedemptionUserControlVM.OnToggleChecked(null);
                                        if (selectedRedemption != null && mainUserControlVM.RedemptionUserControlVM.CustomDataGridVM.SelectedItem != null &&
                                        selectedRedemption != mainUserControlVM.RedemptionUserControlVM.CustomDataGridVM.SelectedItem)
                                        {
                                            object redemptiontoBeSelected = mainUserControlVM.RedemptionUserControlVM.CustomDataGridVM.UICollectionToBeRendered.FirstOrDefault(s
                                                => (s as RedemptionDTO).RedemptionId == (selectedRedemption as RedemptionDTO).RedemptionId);
                                            if (redemptiontoBeSelected != null)
                                            {
                                                mainUserControlVM.RedemptionUserControlVM.CustomDataGridVM.SelectedItem = redemptiontoBeSelected;
                                            }
                                        }
                                    }
                                    if (mainUserControlVM.RedemptionUserControlVM.RedemptionsType == RedemptionsType.Suspended &&
                                    mainUserControlVM.RedemptionUserControlVM.ShowAll)
                                    {
                                        mainUserControlVM.RedemptionUserControlVM.SetCompletedSuspenedCount(RedemptionsType.Suspended,
                                         mainUserControlVM.RedemptionUserControlVM.redemptionDTOList.Count);
                                    }
                                    else
                                    {
                                        mainUserControlVM.RedemptionUserControlVM.SetCompletedSuspenedCount(RedemptionsType.Suspended,
                                             mainUserControlVM.RedemptionUserControlVM.suspendedRedemptions.Where(s => (s.CreatedBy != null && s.CreatedBy.ToLower() == ExecutionContext.UserId.ToLower())
                                         || (s.LastUpdatedBy != null && s.LastUpdatedBy.ToLower() == ExecutionContext.UserId.ToLower())).Count());
                                    }
                                    if (mainUserControlVM.RedemptionUserControlVM.CustomDataGridVM.UICollectionToBeRendered.Count == 0)
                                    {
                                        mainUserControlVM.RedemptionUserControlVM.CustomDataGridVM.Clear();
                                    }
                                }
                                if (!string.IsNullOrEmpty(mainUserControlVM.UserName) && !string.IsNullOrEmpty(userName) &&
                                mainUserControlVM.UserName.ToLower() == this.userName.ToLower() && (actionType == RedemptionUserControlVM.ActionType.Complete || actionType == RedemptionUserControlVM.ActionType.CompleteSearch))
                                {
                                    if (mainUserControlVM.RedemptionUserControlVM.todayCompletedRedemptions == null)
                                    {
                                        mainUserControlVM.RedemptionUserControlVM.todayCompletedRedemptions = this.todayCompletedRedemptions;
                                    }
                                    switch (actionType)
                                    {
                                        case RedemptionUserControlVM.ActionType.Complete:
                                            {
                                                RedemptionDTO existingRedemption = mainUserControlVM.RedemptionUserControlVM.todayCompletedRedemptions.FirstOrDefault(s => s.RedemptionId == (completedRedemptionDTO != null ? completedRedemptionDTO.RedemptionId : redemptionMainUserControlVM.RedemptionDTO.RedemptionId));
                                                if (existingRedemption != null)
                                                {
                                                    int index = mainUserControlVM.RedemptionUserControlVM.todayCompletedRedemptions.IndexOf(existingRedemption);
                                                    if (index >= 0)
                                                    {
                                                        mainUserControlVM.RedemptionUserControlVM.todayCompletedRedemptions[index] = completedRedemptionDTO != null ? completedRedemptionDTO : redemptionMainUserControlVM.RedemptionDTO;
                                                    }
                                                }
                                                else
                                                {
                                                    mainUserControlVM.RedemptionUserControlVM.todayCompletedRedemptions.Add(completedRedemptionDTO != null ? completedRedemptionDTO : redemptionMainUserControlVM.RedemptionDTO);
                                                }
                                            }
                                            break;
                                        case RedemptionUserControlVM.ActionType.CompleteSearch:
                                            {
                                                if (mainUserControlVM.RedemptionUserControlVM.todayCompletedRedemptions != null && redemptionDTOList != null)
                                                {
                                                    foreach (RedemptionDTO searchedRedemption in redemptionDTOList)
                                                    {
                                                        RedemptionDTO redemption = mainUserControlVM.RedemptionUserControlVM.todayCompletedRedemptions.FirstOrDefault(r => r.RedemptionId == searchedRedemption.RedemptionId);
                                                        if (redemption != null)
                                                        {
                                                            int index = mainUserControlVM.RedemptionUserControlVM.todayCompletedRedemptions.IndexOf(redemption);
                                                            mainUserControlVM.RedemptionUserControlVM.todayCompletedRedemptions[index] = searchedRedemption;
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                    if (!mainUserControlVM.RedemptionUserControlVM.ShowSearchCloseIcon)
                                    {
                                        if (mainUserControlVM.RedemptionUserControlVM.RedemptionsType == RedemptionsType.Completed)
                                        {
                                            object selectedRedemption = null;
                                            if (mainUserControlVM.RedemptionUserControlVM.CustomDataGridVM.SelectedItem != null)
                                            {
                                                selectedRedemption = mainUserControlVM.RedemptionUserControlVM.CustomDataGridVM.SelectedItem;
                                            }
                                            mainUserControlVM.RedemptionUserControlVM.OnToggleChecked(null);
                                            if (selectedRedemption != null && mainUserControlVM.RedemptionUserControlVM.CustomDataGridVM.UICollectionToBeRendered.Contains(selectedRedemption))
                                            {
                                                mainUserControlVM.RedemptionUserControlVM.CustomDataGridVM.SelectedItem = selectedRedemption;
                                            }
                                        }
                                        mainUserControlVM.RedemptionUserControlVM.SetCompletedSuspenedCount(RedemptionsType.Completed,
                                                mainUserControlVM.RedemptionUserControlVM.todayCompletedRedemptions.Where(c => (c.Remarks == null || (c.Remarks != null && c.Remarks.ToLower() != "TURNINREDEMPTION".ToLower()))
                                                && c.RedemptionGiftsListDTO.Any()).Count());
                                    }
                                }
                            }
                            if (isLoadTicket)
                            {
                                if (mainUserControlVM.LoadTicketRedemptionUserControlVM != null
                                && !string.IsNullOrEmpty(mainUserControlVM.UserName) && !string.IsNullOrEmpty(userName) && mainUserControlVM.UserName.ToLower() == this.userName.ToLower())
                                {
                                    if (mainUserControlVM.LoadTicketRedemptionUserControlVM.todayCompletedRedemptions == null)
                                    {
                                        mainUserControlVM.LoadTicketRedemptionUserControlVM.todayCompletedRedemptions = this.todayCompletedRedemptions;
                                    }
                                    switch (actionType)
                                    {
                                        case RedemptionUserControlVM.ActionType.CompleteSearch:
                                            {
                                                if (mainUserControlVM.LoadTicketRedemptionUserControlVM.todayCompletedRedemptions != null && redemptionDTOList != null)
                                                {
                                                    foreach (RedemptionDTO searchedRedemption in redemptionDTOList)
                                                    {
                                                        RedemptionDTO redemption = mainUserControlVM.LoadTicketRedemptionUserControlVM.todayCompletedRedemptions.FirstOrDefault(r => r.RedemptionId == searchedRedemption.RedemptionId);
                                                        if (redemption != null)
                                                        {
                                                            int index = mainUserControlVM.LoadTicketRedemptionUserControlVM.todayCompletedRedemptions.IndexOf(redemption);
                                                            mainUserControlVM.LoadTicketRedemptionUserControlVM.todayCompletedRedemptions[index] = searchedRedemption;
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        default:
                                            {
                                                RedemptionDTO existingRedemption = mainUserControlVM.LoadTicketRedemptionUserControlVM.todayCompletedRedemptions.FirstOrDefault(s => s.RedemptionId == redemptionMainUserControlVM.RedemptionDTO.RedemptionId);
                                                if (existingRedemption != null)
                                                {
                                                    int index = mainUserControlVM.LoadTicketRedemptionUserControlVM.todayCompletedRedemptions.IndexOf(existingRedemption);
                                                    if (index >= 0)
                                                    {
                                                        mainUserControlVM.LoadTicketRedemptionUserControlVM.todayCompletedRedemptions[index] = redemptionMainUserControlVM.RedemptionDTO;
                                                    }
                                                }
                                                else
                                                {
                                                    mainUserControlVM.LoadTicketRedemptionUserControlVM.todayCompletedRedemptions.Add(redemptionMainUserControlVM.RedemptionDTO);
                                                }
                                            }
                                            break;
                                    }
                                    if (!mainUserControlVM.LoadTicketRedemptionUserControlVM.ShowSearchCloseIcon)
                                    {
                                        if (mainUserControlVM.LoadTicketRedemptionUserControlVM.RedemptionsType == RedemptionsType.Completed)
                                        {
                                            object selectedRedemption = null;
                                            if (mainUserControlVM.LoadTicketRedemptionUserControlVM.CustomDataGridVM.SelectedItem != null)
                                            {
                                                selectedRedemption = mainUserControlVM.LoadTicketRedemptionUserControlVM.CustomDataGridVM.SelectedItem;
                                            }
                                            mainUserControlVM.LoadTicketRedemptionUserControlVM.OnToggleChecked(null);
                                            if (selectedRedemption != null && mainUserControlVM.LoadTicketRedemptionUserControlVM.CustomDataGridVM.UICollectionToBeRendered.Contains(selectedRedemption))
                                            {
                                                mainUserControlVM.LoadTicketRedemptionUserControlVM.CustomDataGridVM.SelectedItem = selectedRedemption;
                                            }
                                        }
                                        mainUserControlVM.LoadTicketRedemptionUserControlVM.SetCompletedSuspenedCount(RedemptionsType.Completed,
                                                  mainUserControlVM.LoadTicketRedemptionUserControlVM.todayCompletedRedemptions.Where(c => !c.RedemptionGiftsListDTO.Any()).Count());
                                    }
                                }
                                if (!redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO.Any(r => r.CardId > 0)
                                && mainUserControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Voucher && mainUserControlVM.VoucherUserControlVM != null
                                && !string.IsNullOrEmpty(mainUserControlVM.UserName) && !string.IsNullOrEmpty(userName) && mainUserControlVM.UserName.ToLower() == this.userName.ToLower())
                                {
                                    mainUserControlVM.IsLoadingVisible = true;
                                    mainUserControlVM.VoucherUserControlVM.currentDayTicketReceiptList = null;
                                    mainUserControlVM.VoucherUserControlVM.SetDefaultCollections(false);
                                    if (mainUserControlVM.VoucherUserControlVM.RedemptionsType == RedemptionsType.Voucher)
                                    {
                                        int backupId = -1;
                                        if (mainUserControlVM.VoucherUserControlVM.CustomDataGridVM != null &&
                                        mainUserControlVM.VoucherUserControlVM.CustomDataGridVM.SelectedItem != null)
                                        {
                                            backupId = (mainUserControlVM.VoucherUserControlVM.CustomDataGridVM.SelectedItem as TicketReceiptDTO).Id;
                                        }
                                        mainUserControlVM.VoucherUserControlVM.OnToggleChecked(null);
                                        if (backupId != -1 && mainUserControlVM.VoucherUserControlVM.CustomDataGridVM != null
                                        && mainUserControlVM.VoucherUserControlVM.CustomDataGridVM.UICollectionToBeRendered != null
                                        && mainUserControlVM.VoucherUserControlVM.CustomDataGridVM.UICollectionToBeRendered.Any(d => (d as TicketReceiptDTO).Id == backupId))
                                        {
                                            mainUserControlVM.VoucherUserControlVM.CustomDataGridVM.SelectedItem =
                                            mainUserControlVM.VoucherUserControlVM.CustomDataGridVM.UICollectionToBeRendered.FirstOrDefault(d => (d as TicketReceiptDTO).Id == backupId);
                                        }
                                    }
                                    mainUserControlVM.IsLoadingVisible = false;
                                }
                            }
                            if (isTurnIn && mainUserControlVM.TurnInUserControlVM != null && !string.IsNullOrEmpty(mainUserControlVM.UserName)
                            && !string.IsNullOrEmpty(userName) && mainUserControlVM.UserName.ToLower() == this.userName.ToLower())
                            {
                                if (mainUserControlVM.TurnInUserControlVM.todayCompletedRedemptions == null)
                                {
                                    mainUserControlVM.TurnInUserControlVM.todayCompletedRedemptions = this.todayCompletedRedemptions;
                                }
                                switch (actionType)
                                {
                                    case RedemptionUserControlVM.ActionType.CompleteSearch:
                                        {
                                            if (mainUserControlVM.TurnInUserControlVM.todayCompletedRedemptions != null && redemptionDTOList != null)
                                            {
                                                foreach (RedemptionDTO searchedRedemption in redemptionDTOList)
                                                {
                                                    RedemptionDTO redemption = mainUserControlVM.TurnInUserControlVM.todayCompletedRedemptions.FirstOrDefault(r => r.RedemptionId == searchedRedemption.RedemptionId);
                                                    if (redemption != null)
                                                    {
                                                        int index = mainUserControlVM.TurnInUserControlVM.todayCompletedRedemptions.IndexOf(redemption);
                                                        mainUserControlVM.TurnInUserControlVM.todayCompletedRedemptions[index] = searchedRedemption;
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    default:
                                        {
                                            RedemptionDTO existingRedemption = mainUserControlVM.TurnInUserControlVM.todayCompletedRedemptions.FirstOrDefault(s => s.RedemptionId == redemptionMainUserControlVM.RedemptionDTO.RedemptionId);
                                            if (existingRedemption != null)
                                            {
                                                int index = mainUserControlVM.TurnInUserControlVM.todayCompletedRedemptions.IndexOf(existingRedemption);
                                                if (index >= 0)
                                                {
                                                    mainUserControlVM.TurnInUserControlVM.todayCompletedRedemptions[index] = redemptionMainUserControlVM.RedemptionDTO;
                                                }
                                            }
                                            else
                                            {
                                                mainUserControlVM.TurnInUserControlVM.todayCompletedRedemptions.Add(redemptionMainUserControlVM.RedemptionDTO);
                                            }
                                        }
                                        break;
                                }
                                if (!mainUserControlVM.TurnInUserControlVM.ShowSearchCloseIcon)
                                {
                                    if (mainUserControlVM.TurnInUserControlVM.RedemptionsType == RedemptionsType.Completed
                                    && mainUserControlVM.TurnInUserControlVM.CustomDataGridVM != null
                                    && mainUserControlVM.TurnInUserControlVM.CustomDataGridVM.UICollectionToBeRendered != null)
                                    {
                                        object selectedRedemption = null;
                                        if (mainUserControlVM.TurnInUserControlVM.CustomDataGridVM.SelectedItem != null)
                                        {
                                            selectedRedemption = mainUserControlVM.TurnInUserControlVM.CustomDataGridVM.SelectedItem;
                                        }
                                        mainUserControlVM.TurnInUserControlVM.OnToggleChecked(null);
                                        if (selectedRedemption != null && mainUserControlVM.TurnInUserControlVM.CustomDataGridVM.UICollectionToBeRendered.Contains(selectedRedemption))
                                        {
                                            mainUserControlVM.TurnInUserControlVM.CustomDataGridVM.SelectedItem = selectedRedemption;
                                        }
                                    }
                                    mainUserControlVM.TurnInUserControlVM.SetCompletedSuspenedCount(RedemptionsType.Completed,
                                        mainUserControlVM.TurnInUserControlVM.todayCompletedRedemptions.Where(r => r.Remarks != null && r.Remarks.ToLower() == "TURNINREDEMPTION".ToLower()).Count());
                                }
                            }
                            if (isVoucher && mainUserControlVM.VoucherUserControlVM != null)
                            {
                                if (mainUserControlVM.VoucherUserControlVM.currentDayTicketReceiptList == null)
                                {
                                    mainUserControlVM.VoucherUserControlVM.currentDayTicketReceiptList = currentDayTicketReceiptList;
                                }
                                if (actionType == RedemptionUserControlVM.ActionType.VoucherSearch)
                                {
                                    if (mainUserControlVM.VoucherUserControlVM.currentDayTicketReceiptList != null && ticketReceiptDTOList != null)
                                    {
                                        foreach (TicketReceiptDTO searchedRedemption in ticketReceiptDTOList)
                                        {
                                            TicketReceiptDTO redemption = mainUserControlVM.VoucherUserControlVM.currentDayTicketReceiptList.FirstOrDefault(r => r.Id == searchedRedemption.Id);
                                            if (redemption != null)
                                            {
                                                int index = mainUserControlVM.VoucherUserControlVM.currentDayTicketReceiptList.IndexOf(redemption);
                                                mainUserControlVM.VoucherUserControlVM.currentDayTicketReceiptList[index] = searchedRedemption;
                                            }
                                        }
                                    }
                                }
                                else if (receiptDTO != null)
                                {
                                    mainUserControlVM.VoucherUserControlVM.receiptDTO = mainUserControlVM.VoucherUserControlVM.ticketReceiptDTOList.FirstOrDefault(d => d.Id == receiptDTO.Id);
                                    if (mainUserControlVM.VoucherUserControlVM.receiptDTO != null)
                                    {
                                        mainUserControlVM.VoucherUserControlVM.receiptDTO.IsSuspected = receiptDTO.IsSuspected;
                                        int index = mainUserControlVM.VoucherUserControlVM.ticketReceiptDTOList.IndexOf(mainUserControlVM.VoucherUserControlVM.receiptDTO);
                                        mainUserControlVM.VoucherUserControlVM.AddToVoucherList(index);
                                    }
                                }
                                if (!mainUserControlVM.VoucherUserControlVM.ShowSearchCloseIcon)
                                {
                                    switch (mainUserControlVM.VoucherUserControlVM.RedemptionsType)
                                    {
                                        case RedemptionsType.Flagged:
                                        case RedemptionsType.Voucher:
                                            {
                                                object selectedRedemption = null;
                                                if (mainUserControlVM.VoucherUserControlVM.CustomDataGridVM.SelectedItem != null)
                                                {
                                                    selectedRedemption = mainUserControlVM.VoucherUserControlVM.CustomDataGridVM.SelectedItem;
                                                }
                                                mainUserControlVM.VoucherUserControlVM.OnToggleChecked(null);
                                                if (selectedRedemption != null && mainUserControlVM.VoucherUserControlVM.CustomDataGridVM.UICollectionToBeRendered.Contains(selectedRedemption))
                                                {
                                                    mainUserControlVM.VoucherUserControlVM.CustomDataGridVM.SelectedItem = selectedRedemption;
                                                }
                                            }
                                            break;
                                    }
                                    mainUserControlVM.VoucherUserControlVM.SetCompletedSuspenedCount(RedemptionsType.Voucher, mainUserControlVM.VoucherUserControlVM.ticketReceiptDTOList.Where(t => t.IsSuspected == true).ToList().Count);
                                }
                                mainUserControlVM.VoucherUserControlVM.receiptDTO = null;
                            }
                            if (mainUserControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption && mainUserControlVM.RedemptionUserControlVM != null
                                && mainUserControlVM.RedemptionUserControlVM.redemptionsType == RedemptionsType.New && !mainUserControlVM.ShowTextBlock)
                            {
                                mainUserControlVM.CallRecalculatePriceandStock(false,true,true);
                            }
                        }
                    }
                }
            });
        }

        private void OnUpdateViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                RedemptionUpdateVM updateVM = (sender as RedemptionUpdateView).DataContext as RedemptionUpdateVM;
                if (updateVM != null && updateVM.UpdateClicked)
                {
                    if (CustomDataGridVM != null && CustomDataGridVM.SelectedItem != null)
                    {
                        int index = CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem);
                        if (index >= 0)
                        {
                            CustomDataGridVM.SelectedItem = selectedRedemptionDTO;
                        }
                        if (redemptionMainUserControlVM != null && updateVM.IsSuccess)
                        {
                            redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 2919, selectedRedemptionDTO.RedemptionOrderNo), MessageType.Info);
                        }
                    }
                }
            });
            SetMainViewFocus();
            log.LogMethodExit();
        }
        private async void SuspendDeleteClicked()
        {
            log.LogMethodEntry();
            try
            {
                SetLoadingVisible(true);
                if (this.redemptionsType == RedemptionsType.Suspended && selectedRedemptionDTO != null)
                {
                    bool result = await AbandonRedemption(selectedRedemptionDTO);
                    if (suspendedRedemptions != null && suspendedRedemptions.Count > 0 && suspendedRedemptions.Contains(selectedRedemptionDTO))
                    {
                        suspendedRedemptions.Remove(this.selectedRedemptionDTO);
                    }
                    if (redemptionDTOList != null && redemptionDTOList.Count > 0 && redemptionDTOList.Contains(selectedRedemptionDTO))
                    {
                        redemptionDTOList.Remove(this.selectedRedemptionDTO);
                    }
                    SetOtherRedemptionList(RedemptionUserControlVM.ActionType.Delete);
                    this.selectedRedemptionDTO = null;
                    OnToggleChecked(null);
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodExit();
        }

        private void OnTotalTicketClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                SetFooterContentEmpty();

                if (redemptionMainUserControl == null)
                {
                    FindAncestor(parameter as Visual);
                }
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.OpenTicketAllocation();
                }
            });
            log.LogMethodExit();
        }
        internal void SetRightSectionVMOnFooterClick()
        {
            log.LogMethodEntry();
            if (redemptionMainUserControlVM != null && redemptionMainUserControlVM.FooterVM != null)
            {
                bool visible = false;
                if (redemptionMainUserControlVM.FooterVM.SideBarContent == MessageViewContainerList.GetMessage(ExecutionContext, "Show Sidebar"))
                {
                    visible = true;
                }
                if (redemptionsType == RedemptionsType.New && GenericTransactionListVM != null)
                {
                    GenericTransactionListVM.ScreenUserAreaVisible = visible;
                }
                if (redemptionsType != RedemptionsType.New && GenericRightSectionContentVM != null)
                {
                    GenericRightSectionContentVM.IsScreenUserAreaVisble = visible;
                }
            }
            log.LogMethodExit();
        }
        internal void OnAllocationViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                UpdateTicketValues();
                if (redemptionMainUserControlVM.AllocationView != null && redemptionMainUserControlVM.AllocationView.DataContext
                != null)
                {
                    RedemptionTicketAllocationVM allocationVM = redemptionMainUserControlVM.AllocationView.DataContext as RedemptionTicketAllocationVM;
                    if (allocationVM != null && allocationVM.TicketType == TicketType.TICKETS
                    && !string.IsNullOrEmpty(allocationVM.ManualTicketAppliedContent))
                    {
                        HideContentArea();
                        redemptionMainUserControlVM.SetFooterContent(allocationVM.ManualTicketAppliedContent, MessageType.Info);
                    }
                }
                redemptionMainUserControlVM.AllocationView = null;
                // redemptionMainUserControlVM.RegisterDevices();
            });
            SetMainViewFocus();
            log.LogMethodExit();
        }

        internal void UpdateTicketValues()
        {
            log.LogMethodEntry();

            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (RightSectionDisplayTagsVM != null && RightSectionDisplayTagsVM.DisplayTags != null
                && RightSectionDisplayTagsVM.DisplayTags.Count > 0 && redemptionMainUserControlVM != null
                && redemptionMainUserControlVM.RedemptionDTO != null)
                {
                    foreach (ObservableCollection<DisplayTag> tags in RightSectionDisplayTagsVM.DisplayTags)
                    {
                        int index = RightSectionDisplayTagsVM.DisplayTags.IndexOf(tags);
                        if (tags != null && tags.Count > 1 && !string.IsNullOrEmpty(tags[0].Text))
                        {
                            if (IsLowerResoultion)
                            {
                                if ((GenericTransactionListVM != null && GenericTransactionListVM.ItemCollection != null)
                                || stayInTransactionMode)
                                {
                                    RightSectionDisplayTagsVM.DisplayTags[index][0].TextSize = TextSize.Medium;
                                    RightSectionDisplayTagsVM.DisplayTags[index][1].TextSize = TextSize.Small;
                                    RightSectionDisplayTagsVM.DisplayTags[index][0].FontWeight = FontWeights.Bold;
                                }
                                else if (RightSectionDisplayTagsVM.DisplayTags[index][0].FontWeight == FontWeights.Bold)
                                {
                                    RightSectionDisplayTagsVM.DisplayTags[index][0].TextSize = TextSize.Small;
                                    RightSectionDisplayTagsVM.DisplayTags[index][1].TextSize = TextSize.XSmall;
                                    RightSectionDisplayTagsVM.DisplayTags[index][0].FontWeight = FontWeights.Normal;
                                }
                            }
                            if (MessageViewContainerList.GetMessage(ExecutionContext, "Total Tkt").ToLower() == tags[0].Text.ToLower())
                            {
                                RightSectionDisplayTagsVM.DisplayTags[index][1].Text = GetNumberFormattedString(redemptionMainUserControlVM.GetTotalTickets());
                            }
                            else if (MessageViewContainerList.GetMessage(ExecutionContext, "Redeemed").ToLower() == tags[0].Text.ToLower())
                            {
                                RightSectionDisplayTagsVM.DisplayTags[index][1].Text = GetNumberFormattedString(redemptionMainUserControlVM.GetTotalRedeemed());
                            }
                            else if (MessageViewContainerList.GetMessage(ExecutionContext, "Balance").ToLower() == tags[0].Text.ToLower())
                            {
                                RightSectionDisplayTagsVM.DisplayTags[index][1].Text = GetNumberFormattedString(redemptionMainUserControlVM.GetTotalTickets() - redemptionMainUserControlVM.GetTotalRedeemed());
                            }
                            else if (MessageViewContainerList.GetMessage(ExecutionContext, "Grace").ToLower() == tags[0].Text.ToLower())
                            {
                                RightSectionDisplayTagsVM.DisplayTags[index][1].Text = GetNumberFormattedString(redemptionMainUserControlVM.GetGraceTickets());
                            }
                        }
                    }
                }
            });
            log.LogMethodExit();
        }

        private void OnScanEnterClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (redemptionMainUserControl == null)
                {
                    FindAncestor(parameter as Button);
                }

                if (parameter != null)
                {
                    Button button = parameter as Button;
                    if (button != null && !string.IsNullOrEmpty(button.Name))
                    {
                        switch (button.Name.ToLower())
                        {
                            case "btnscanticket":
                                {
                                    if (isTurnIn)
                                    {
                                        ScanClicked(isTurnIn, GenericScanPopupVM.PopupType.SCANGIFT);
                                    }
                                    else
                                    {
                                        if (mainVM.EnableEnterTicketNumberManually)
                                        {
                                            ScanClicked(false, GenericScanPopupVM.PopupType.SCANTICKET);
                                        }
                                        else
                                        {
                                            if (isLoadTicket)
                                            {
                                                ScanClicked(true, GenericScanPopupVM.PopupType.SCANTICKET);
                                            }
                                            else
                                            {
                                                ScanClicked(false, GenericScanPopupVM.PopupType.SCANTICKET);
                                            }
                                        }
                                    }
                                }
                                break;
                            case "btnaddtkt":
                                {
                                    OnAddTicketClicked();
                                }
                                break;
                        }
                    }
                }
            });
            log.LogMethodExit();
        }

        private void OnDatePickerLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (parameter != null)
                {
                    selectedDatePicker = parameter as CustomTextBoxDatePicker;
                    if (redemptionMainUserControlVM != null && selectedDatePicker != null
                    && selectedDatePicker.DatePickerView != null)
                    {
                        redemptionMainUserControlVM.DatePickerView = selectedDatePicker.DatePickerView;
                        selectedDatePicker.DatePickerView.Closed += OnDatePickerViewClosed;
                        redemptionMainUserControlVM.OnWindowLoaded(selectedDatePicker.DatePickerView, null);
                        redemptionMainUserControlVM.DatePickerView.PreviewMouseDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                        if (redemptionMainUserControlVM.RedemptionMainUserControl != null && redemptionMainUserControlVM.RedemptionMainUserControl.KeyboardHelper != null
                        && redemptionMainUserControlVM.RedemptionMainUserControl.KeyboardHelper.KeyboardView != null)
                        {
                            redemptionMainUserControlVM.RedemptionMainUserControl.KeyboardHelper.KeyboardView.Close();
                        }
                    }
                }
            });
            log.LogMethodExit();
        }

        private void OnDatePickerViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DatePickerView datePickerView = sender as DatePickerView;
            if (selectedDatePicker != null && !string.IsNullOrEmpty(selectedDatePicker.Name) && datePickerView != null
                && !string.IsNullOrEmpty(datePickerView.SelectedDate))
            {
                string dateText = Convert.ToDateTime(datePickerView.SelectedDate).ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATE_FORMAT"));
                switch (selectedDatePicker.Name)
                {
                    case "tbFromDate":
                    case "tbIssuedDateFrom":
                        SearchedIssuedDateFrom = dateText;
                        break;
                    case "tbToDate":
                    case "tbIssuedDateTo":
                        SearchedIssuedDateTo = dateText;
                        break;

                }
            }
            log.LogMethodExit();
        }
        private void PerformReCalculate(bool fromScroll = false)
        {
            log.LogMethodEntry();
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (RedemptionsType == RedemptionsType.New && redemptionMainUserControlVM != null
                && redemptionMainUserControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption)
                {
                    redemptionMainUserControlVM.CallRecalculatePriceandStock(fromScroll,true);
                }
            });
            log.LogMethodExit();
        }

        private void OnContentRendered(object parameter)
        {
            log.LogMethodEntry(parameter);
            log.LogMethodExit();
        }
        private void OnScrollChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithMainUserControlFooter(() =>
            {
                if (RedemptionsType == RedemptionsType.New && redemptionMainUserControlVM != null
                && redemptionMainUserControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption)
                {
                    redemptionMainUserControlVM.CallRecalculatePriceandStock(true,true);
                }
            });
            log.LogMethodExit();
        }
        private void SetSearchDates()
        {
            log.LogMethodEntry();
            SearchedIssuedDateFrom = DateTime.Now.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATE_FORMAT"));
            SearchedIssuedDateTo = DateTime.Today.AddDays(1).ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATE_FORMAT"));
            log.LogMethodExit();
        }
        private void InitializeCommands()
        {
            log.LogMethodEntry();
            PropertyChanged += OnPropertyChanged;
            log.LogMethodExit();
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry();
            if (!string.IsNullOrWhiteSpace(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case "SearchFilterVisbility":
                        if (searchFilterVisbility == Visibility.Visible && redemptionMainUserControlVM != null)
                        {
                            redemptionMainUserControlVM.SetKeyboardWindow();
                        }
                        break;
                }
            }
            log.LogMethodExit();
        }
        private void ResetGenericDisplayItemsVM()
        {
            log.LogMethodEntry();
            if(GenericDisplayItemsVM.DisplayItemModels != null)
            {
                GenericDisplayItemsVM.DisplayItemModels.Clear();
            }
            if (GenericDisplayItemsVM.BackupDisplayItemModels != null)
            {
                GenericDisplayItemsVM.BackupDisplayItemModels.Clear();
            }
            if (GenericDisplayItemsVM.CurrentDisplayItemModels != null)
            {
                GenericDisplayItemsVM.CurrentDisplayItemModels.Clear();
            }
            log.LogMethodExit();
        }
        private void SetPaginationDefaults()
        {
            log.LogMethodEntry();
            AutoShowSearchSection = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AUTO_SHOW_PRODUCT_MENU_SEARCH_SECTION", false);
            pageSize = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_PAGE_SIZE", 20);
            SetEnablePreviousNextNavigation(false, false);
            pageNumber = 0;
            totalPageCount = 0;
            log.LogMethodExit();
        }
        private void OnActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                KeyValuePair<int, string> action = parameter is KeyValuePair<int, string> ? (KeyValuePair<int, string>)parameter
                    : new KeyValuePair<int, string>();
                int fromPrice = 0;
                int toPrice = 0;
                if (showSearchCloseIcon)
                {
                    if (!string.IsNullOrWhiteSpace(SearchedBalanceTicketFromOrOrderNo))
                    {
                        int.TryParse(SearchedBalanceTicketFromOrOrderNo, out fromPrice);
                    }
                    if (!string.IsNullOrWhiteSpace(SearchedBalanceTicketToOrPrdCodeDescBarcode))
                    {
                        int.TryParse(SearchedBalanceTicketToOrPrdCodeDescBarcode, out toPrice);
                    }
                    if (fromPrice > toPrice)
                    {
                        redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2639), MessageType.Warning);
                        log.LogMethodExit("from price is less than to price.");
                        return;
                    }
                }
                string receiptText = SearchedReceiptNo;
                string cardText = SearchedCardNo;
                if (!AutoShowSearchSection && !showSearchCloseIcon)
                {
                    receiptText = string.Empty;
                    cardText = string.Empty;
                    fromPrice = 0;
                    toPrice = 0;
                }
                if (action.Key > 0)
                {
                    switch (action.Key)
                    {
                        case 1:
                            --pageNumber;
                            break;
                        case 2:
                            ++pageNumber;
                            break;
                    }
                    PerformProductSearch(receiptText, cardText, fromPrice, toPrice, pageNumber, pageSize);
                }
            }
            log.LogMethodExit();
        }
        private void ShowSearchArea()
        {
            log.LogMethodEntry();
            if (redemptionsType == RedemptionsType.New)
            {
                if (!AutoShowSearchSection)
                {
                    SearchFilterVisbility = SearchFilterVisbility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    if (SearchFilterVisbility == Visibility.Collapsed)
                    {
                        PerformProductSearch(string.Empty, string.Empty, 0, 0, 0, pageSize);
                    }
                }
                else
                {
                    SearchFilterVisbility = !showSearchCloseIcon && redemptionMainUserControlVM != null &&
                           (redemptionMainUserControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption
                           || redemptionMainUserControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.TurnIn) ? Visibility.Visible : Visibility.Collapsed;
                }
                if (searchFilterVisbility == Visibility.Visible)
                {
                    ResetGenericDisplayItemsVM();
                }
            }
            log.LogMethodExit();
        }
        internal int GetPosLocationId(ProductsContainerDTO product)
        {
            log.LogMethodEntry(product);
            int poslocationId = POSMachineViewContainerList.GetPOSMachineContainerDTO(ExecutionContext).InventoryLocationId;
            if (product != null && product.InventoryItemContainerDTO != null && poslocationId <= -1)
            {
                poslocationId = product.InventoryItemContainerDTO.OutBoundLocationId;
            }
            log.LogMethodExit(poslocationId);
            return poslocationId;
        }
        internal async Task<double> GetTotalStock(ProductsContainerDTO product)
        {
            log.LogMethodEntry(product);
            double totalStock = 0;
            try
            {
                if (product != null && product.InventoryItemContainerDTO != null)
                {
                    List<InventoryDTO> inventories = await GetInventories(new List<int>() { product.InventoryItemContainerDTO.ProductId },
                        GetPosLocationId(product));
                    totalStock = inventories != null ? inventories.Sum(i => i.Quantity) : 0;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(totalStock);
            return totalStock;
        }
        internal async Task<List<InventoryDTO>> GetInventories(List<int> productIds, int locationId = -1)
        {
            log.LogMethodEntry(productIds, locationId);
            List<InventoryDTO> inventories = null;
            try
            {
                List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchparameters = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                if (productIds.Count == 1)
                {
                    searchparameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, productIds.First().ToString()));
                }
                if (productIds.Count > 1)
                {
                    string productIdsText = string.Join(",", productIds.Select(s => s));
                    searchparameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID_LIST, productIdsText));
                }
                if (locationId > -1)
                {
                    searchparameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID,
                    locationId.ToString()));
                }
                SetLoadingVisible(true);
                inventories = await InventoryUseCaseFactory.GetInventoryStockUseCases(ExecutionContext).GetInventoryDTOList(searchparameters);
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                SetLoadingVisible(false);
                redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(ex.InnerException != null ? ex.InnerException.Message : ex.Message, MessageType.Error);
                }
            }
            finally
            {
                SetLoadingVisible(false);
            }
            log.LogMethodExit(inventories);
            return inventories;
        }
        private string GetNumberFormattedText(int intValue)
        {
            log.LogMethodEntry(intValue);
            string text = "0";
            if (intValue > 0)
            {
                text = intValue.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT"));
            }
            log.LogMethodExit(intValue);
            return text;
        }
        private void SetEnablePreviousNextNavigation(bool enablePreviousNavigation, bool enableNextNavigtaion)
        {
            log.LogMethodEntry();
            EnableNextNavigation = enableNextNavigtaion;
            EnablePreviousNavigation = enablePreviousNavigation;
            log.LogMethodExit();
        }
        private bool IsSearchedCode(ProductsContainerDTO product, string searchText)
        {
            log.LogMethodEntry(product);
            bool isSearchedCode = false;
            if (product != null && searchText != null)
            {
                searchText = searchText.ToLower();
                isSearchedCode = (product.InventoryItemContainerDTO != null && !string.IsNullOrEmpty(product.InventoryItemContainerDTO.Code)
                    && product.InventoryItemContainerDTO.Code.ToLower().Contains(searchText))
                    || (!string.IsNullOrEmpty(product.ProductName) && product.ProductName.ToLower().Contains(searchText));
            }
            log.LogMethodExit(isSearchedCode);
            return isSearchedCode;
        }
        private async Task PerformProductSearch(string searchedReceiptNo, string searchedCardNo, int fromPrice, int toPrice, int pageNumber, int pageSize)
        {
            log.LogMethodEntry(pageNumber, pageSize, fromPrice, toPrice, pageNumber, pageSize);
            bool isRedemption = redemptionMainUserControlVM != null && redemptionMainUserControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption;
            List<ProductsContainerDTO> searchedProducts = mainVM.GetProductContainerDTOList(ExecutionContext);
            if (!string.IsNullOrWhiteSpace(searchedReceiptNo))
            {
                searchedProducts = searchedProducts.Where(c => IsSearchedCode(c, SearchedReceiptNo)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(searchedCardNo))
            {
                searchedProducts = searchedProducts.Where(c => !string.IsNullOrEmpty(c.Description) &&
                c.Description.ToLower().Contains(SearchedCardNo.ToLower())).ToList();
            }
            if (isRedemption)
            {
                if (fromPrice >= 0)
                {
                    searchedProducts = searchedProducts.Where(c => c.InventoryItemContainerDTO != null &&
                    c.InventoryItemContainerDTO.PriceInTickets >= fromPrice).ToList();
                }
                if (toPrice > 0)
                {
                    searchedProducts = searchedProducts.Where(c => c.InventoryItemContainerDTO != null &&
                    c.InventoryItemContainerDTO.PriceInTickets <= toPrice).ToList();
                }
            }
            string message = string.Empty;
            MessageType messageType = MessageType.None;
            bool enablePrevNavigation = false;
            bool enabelNextNavigation = false;
            if (searchedProducts.Any())
            {
                int itemsCount = searchedProducts.Count;
                int startIndex = pageNumber * pageSize;
                totalPageCount = (int)Math.Ceiling((double)itemsCount / pageSize);
                bool isMoreItems = totalPageCount > 1;
                enablePrevNavigation = isMoreItems && pageNumber > 0 ? true : false;
                enabelNextNavigation = isMoreItems && pageNumber + 1 < totalPageCount ? true : false;
                if (itemsCount <= startIndex + pageSize)
                {
                    pageSize = itemsCount - startIndex;
                }
                searchedProducts = searchedProducts.GetRange(startIndex, pageSize).ToList();
                message = MessageViewContainerList.GetMessage(ExecutionContext, 5056,
                    GetNumberFormattedText(startIndex + 1), GetNumberFormattedText(startIndex + pageSize),
                    GetNumberFormattedText(itemsCount), GetNumberFormattedText(pageNumber + 1),
                    GetNumberFormattedText(totalPageCount));
                messageType = MessageType.Info;

            }
            else
            {
                message = MessageViewContainerList.GetMessage(ExecutionContext, 1710);
                messageType = MessageType.Warning;
            }
            SetEnablePreviousNextNavigation(enablePrevNavigation, enabelNextNavigation);
            if (searchedProducts == null)
            {
                searchedProducts = new List<ProductsContainerDTO>();
            }
            GenericDisplayItemsVM.DisplayItemModels = new ObservableCollection<object>(searchedProducts);
            if (isRedemption)
            {
                await UpdateBackupDisplayItemsModels(searchedProducts);
            }
            GenericDisplayItemsVM.SetUIDisplayItemModels();
            redemptionMainUserControlVM.SetFooterContent(message, messageType);
            if (searchedProducts.Count <= 0 && GenericDisplayItemsVM.CurrentDisplayItemModels != null)
            {
                GenericDisplayItemsVM.CurrentDisplayItemModels.Clear();
            }
            if (isRedemption)
            {
                redemptionMainUserControlVM.CancellationTokenSource.Cancel();
                redemptionMainUserControlVM.ResetRecalculateFlags();
                redemptionMainUserControlVM.CancellationTokenSource = new System.Threading.CancellationTokenSource();
                redemptionMainUserControlVM.CallRecalculatePriceandStock(false, true, false);
            }
            log.LogMethodExit();
        }
        private async Task UpdateBackupDisplayItemsModels(List<ProductsContainerDTO> searchedProducts)
        {
            log.LogMethodEntry(searchedProducts);
            if (mainVM != null && GenericDisplayItemsVM != null)
            {
                List<GenericDisplayItemModel> displayItemModels = new List<GenericDisplayItemModel>();
                List<InventoryDTO> inventories = null;
                if (!mainVM.AllowTransactionOnZeroStock)
                {
                    inventories = await GetInventories(searchedProducts.Where(m => m.InventoryItemContainerDTO != null).Select(s =>
                    s.InventoryItemContainerDTO.ProductId).ToList());
                }
                for (int i = 0; i < searchedProducts.Count; i++)
                {
                    ProductsContainerDTO product = searchedProducts[i];
                    int poslocationId = GetPosLocationId(product);

                    GenericDisplayItemModel displayItemModel = new GenericDisplayItemModel()
                    {
                        Heading = searchedProducts[i].ProductName,
                        ItemsSource = new ObservableCollection<DiscountItem>()
                            {
                                new DiscountItem()
                                {
                                    OriginalValue = GetNumberFormattedText((int)product.InventoryItemContainerDTO.PriceInTickets) + " " +
                                    ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets")
                                }
                            },
                        Key = product.ProductId.ToString(),
                        ButtonType = ButtonType.Info,
                        IsEnabled = true
                    };
                    if (!mainVM.AllowTransactionOnZeroStock)
                    {
                        double stock = inventories == null ? 0 : inventories.Where(inv => inv.LocationId == poslocationId &&
                        product.InventoryItemContainerDTO.ProductId == inv.ProductId).Sum(inv => inv.Quantity);
                        displayItemModel.IsEnabled = stock > 0;
                    }
                    displayItemModels.Add(displayItemModel);
                }
                GenericDisplayItemsVM.BackupDisplayItemModels = new ObservableCollection<GenericDisplayItemModel>(displayItemModels);
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public RedemptionUserControlVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            ExecuteAction(() =>
            {
                this.ExecutionContext = executionContext;
                InitializeCommands();

                isChangeStatusEnable = false;
                multiScreenMode = false;
                iscontentAreaVisible = false;
                isLoadTicket = false;
                showNoCurrencyTextBlock = false;
                stayInTransactionMode = false;
                isVoucher = false;
                isTurnIn = false;
                SetPaginationDefaults();
                transactionID = string.Empty;
                userName = string.Empty;
                screenNumber = string.Empty;
                searchedReceiptNo = string.Empty;
                searchedCardNo = string.Empty;
                searchedBalanceTicketFromOrOrderNo = string.Empty;
                searchedBalanceTicketToOrPrdCodeDesBarCode = string.Empty;
                SetSearchDates();
                redemptionDTOList = new List<RedemptionDTO>();
                statusCollection = new ObservableCollection<string>();
                RedemptionDTO.RedemptionStatusEnum[] statusEnumCollection = Enum.GetValues(RedemptionDTO.RedemptionStatusEnum.DELIVERED.GetType()) as
                                                          RedemptionDTO.RedemptionStatusEnum[];
                if (statusEnumCollection != null && statusEnumCollection.Count() > 0)
                {
                    foreach (RedemptionDTO.RedemptionStatusEnum statusValue in statusEnumCollection.Where(x => x != RedemptionDTO.RedemptionStatusEnum.NEW && x != RedemptionDTO.RedemptionStatusEnum.ABANDONED
                    && x != RedemptionDTO.RedemptionStatusEnum.SUSPENDED))
                    {
                        statusCollection.Add(statusValue.ToString());
                    }
                }
                statusCollection.Add(MessageViewContainerList.GetMessage(ExecutionContext, "All", null));
                selectedStatus = statusCollection.FirstOrDefault(x => x.Equals(MessageViewContainerList.GetMessage(ExecutionContext, "All", null)));
                datePickerLoadedCommand = new DelegateCommand(OnDatePickerLoaded);
                SearchCommand = new DelegateCommand(OnSearchClicked, ButtonEnable);
                searchActionsCommand = new DelegateCommand(OnSearchActionsClicked, ButtonEnable);
                scrollChangedCommand = new DelegateCommand(OnScrollChanged);
                contentRenderedCommand = new DelegateCommand(OnContentRendered);
                showAllClickedCommand = new DelegateCommand(OnShowAllClicked, ButtonEnable);
                toggleCheckedCommand = new DelegateCommand(OnToggleChecked, ButtonEnable);
                toggleUncheckedCommand = new DelegateCommand(OnToggleUnchecked, ButtonEnable);
                itemClickedCommand = new DelegateCommand(OnItemClicked, ButtonEnable);
                itemOfferOrInfoClickedCommand = new DelegateCommand(OnItemOfferOrInfoClicked, ButtonEnable);
                showContentAreaClickedCommand = new DelegateCommand(OnShowContentAreaClicked, ButtonEnable);
                showTransactionAreaClickedCommand = new DelegateCommand(OnShowTransactionAreaClicked, ButtonEnable);
                resetCommand = new DelegateCommand(OnResetClicked, ButtonEnable);
                deleteCommand = new DelegateCommand(OnDeleteClicked, ButtonEnable);
                transactionItemClickedCommand = new DelegateCommand(OnTransactionItemClicked, ButtonEnable);
                loadedCommand = new DelegateCommand(OnLoaded);
                transactionActionsCommand = new DelegateCommand(OnTransactionActionsClicked, ButtonEnable);
                isSelectedCommand = new DelegateCommand(OnIsSelected);
                nextNavigationCommand = new DelegateCommand(OnNextNavigation);
                previousNavigationCommand = new DelegateCommand(OnPreviousNavigation);
                suspendCompleteActionsCommand = new DelegateCommand(OnSuspendCompleteActionsClicked, ButtonEnable);
                totalTicketClickedCommand = new DelegateCommand(OnTotalTicketClicked, ButtonEnable);
                scanEnterCommand = new DelegateCommand(OnScanEnterClicked, ButtonEnable);
                ActionsCommand = new DelegateCommand(OnActionsClicked, ButtonEnable);
                searchFilterVisbility = Visibility.Collapsed;
                GenericDisplayItemsVM = new GenericDisplayItemsVM(executionContext) { ShowDisabledItems = true };
                GenericToggleButtonsVM = new GenericToggleButtonsVM();
                RightSectionDisplayTagsVM = new DisplayTagsVM();
                GenericTransactionListVM = new GenericTransactionListVM();

                GenericRightSectionContentVM = new GenericRightSectionContentVM();
            });
            log.LogMethodExit();
        }
        #endregion
    }
}
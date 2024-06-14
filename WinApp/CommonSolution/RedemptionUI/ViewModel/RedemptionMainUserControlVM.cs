/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - view model for redemption main user control
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Controls;

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Inventory.Location;
using Semnox.Parafait.Product;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Authentication;
using System.Threading;
using Semnox.Parafait.User;

namespace Semnox.Parafait.RedemptionUI
{
    public enum LeftPaneSelectedItem
    {
        Redemption,
        LoadTicket,
        TurnIn,
        Voucher
    }

    internal enum ManageViewType
    {
        ShortCutKey = 0,
        AddTicket = 1,
        Flag = 2,
        RePrint = 3,
        LoadBalanceTicketLimit = 4,
        LoadtoCardTicketLimit = 5,
        PrintConsolidateTicketLimit = 6,
        RedemptionLimit = 7,
        TurninLimit = 8,
        PrintBalanceTicketLimit = 9
    }

    public class RedemptionMainUserControlVM : BaseWindowViewModel
    {
        #region Members        
        private string newSelectedLeftPaneItem;
        private string tappedCardNumber;
        private string newloginid;
        private string cardInfoText;
        private string turnInCardInfoText;
        private char lastScannedType;
        private char scanTicketGiftmode;
        private char newOrCloseScreen;
        private string userName;
        private string oldLeftPaneSelectedItem;
        private string scannedBarCode;
        private string shortCutKeys;
        private string deviceErrorMessage;
        private int screenNumber;
        private bool reloginInitiated;
        private int carddeviceaddress = -1;
        private int barcodedeviceaddress = -1;
        private int posInActivityTime;
        private LeftPaneSelectedItem leftPaneSelectedItem;
        private ColorCode colorCode;
        private NumericUpDownAssigning numericUpDownAssigning = NumericUpDownAssigning.Add;
        private bool enableSuspend;
        private bool isTurnInSecondCardTapped;
        private bool reLoginRequired;
        private bool enableManualTicket;
        private bool enableLoadToCard;
        private bool enableConsolidatePrint;
        private bool enableTurnIn;
        private bool enableRePrint;
        private bool enableUnflagVoucher;
        private bool recalculateproductprice;
        private bool singleUserMultiscreen;
        private bool multiUserMultiscreen;
        private bool isTicket;
        private bool applyColorCode;
        private bool multiScreenMode;
        private bool isActive;
        private bool showTextBlock;
        private bool isChildWindowOpen;
        private bool autoShowRedemptionProductMenu;
        private bool autoShowLoadTicketProductMenu;
        private bool reCalculatePriceInProgress = false;
        private bool reCalculateStockInProgress = false;
        private CancellationTokenSource cancellationTokenSource;
        private List<RedemptionCardsDTO> updatedcardsDTO;
        private Dictionary<int, int> membershipIDcardIDList;
        private Dictionary<int, int> cardIDcustomerIDList;
        private Dictionary<int, string> customerIDcustomerInfoList;
        private List<int> membershipIDList;
        private TicketReceiptDTO flaggedticketReceiptDTO;
        private RedemptionCurrencyContainerDTO waitingCurrencyDTO;
        private RedemptionCurrencyContainerDTO lastScannedCurrency;


        private int? lastScannedViewGrouping;
        private ProductsContainerDTO lastScannedProduct;
        private GenericItemInfoPopUp itemInfoPopUpView;
        private GenericDataEntryView dataEntryView;
        private RedemptionScanView scanView;
        private GenericScanPopupView scanPopupView;
        private GenericMessagePopupView messagePopupView;
        private RedemptionReverseView reverseView;
        private RedemptionUpdateView updateView;
        private RedemptionTicketAllocationView allocationView;
        private AuthenticateUserView userView;
        private GenericDataEntryView enterTicketView;
        private AuthenticateManagerView managerView;
        private ChangePasswordView changePasswordView;
        private DatePickerView datePickerView;
        private NumberKeyboardView numberKeyboardView;
        //private FingerPrintLoginView fingerPrintView;
        private Semnox.Core.Utilities.ExecutionContext olduserContext;
        private AccountDTO accountDTO;
        private RedemptionActivityDTO redemptionActivityDTO;
        private RedemptionActivityDTO redemptionActivityManualTicketDTO;
        private RedemptionLoadToCardRequestDTO redemptionLoadToCardRequestDTO;
        private RedemptionDTO redemptionDTO;
        private RedemptionDTO newredemptionDTO;
        private RedemptionDTO retreivedBackupDTO;
        private RedemptionUserControlVM redemptionUserControlVM;
        private RedemptionUserControlVM loadTicketRedemptionUserControlVM;
        private RedemptionUserControlVM turnInUserControlVM;
        private RedemptionUserControlVM voucherUserControlVM;
        private FooterUserControl footerUserControl;
        private RedemptionMainUserControl redemptionMainUserControl;
        private RedemptionMainUserControlVM redemptionMainUserControlVM;
        private RedemptionView redemptionMainView;
        private RedemptionMainVM mainVM;
        private DateTime lastActivityTime;
        private DispatcherTimer timer;
        private DeviceClass cardReader;
        private DeviceClass barcodeReader;
        private readonly TagNumberViewParser tagNumberViewParser;

        private ICommand addButtonClickedCommand;
        private ICommand removeButtonClickedCommand;
        private ICommand sidebarClickedCommand;
        private ICommand footerMessageClickedCommand;
        private ICommand loadedCommand;
        private ICommand footerLoadedCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public bool ApplyColorCode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(applyColorCode);
                return applyColorCode;
            }
            set
            {
                log.LogMethodEntry(applyColorCode, value);
                SetProperty(ref applyColorCode, value);
                if (LeftPaneVM != null)
                {
                    LeftPaneVM.ApplyColorCode = applyColorCode;
                }
                SetItemBackground();
                log.LogMethodExit(applyColorCode);
            }
        }

        public ColorCode ColorCode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(colorCode);
                return colorCode;
            }
            set
            {
                log.LogMethodEntry(colorCode, value);
                SetProperty(ref colorCode, value);
                if (LeftPaneVM != null)
                {
                    LeftPaneVM.ColorCode = colorCode;
                }
                SetKeyBoardHelperColorCode();
                log.LogMethodExit(colorCode);
            }
        }
        public bool ReloginInitiated
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(reloginInitiated);
                return reloginInitiated;
            }
            set
            {
                log.LogMethodEntry(reloginInitiated, value);
                SetProperty(ref reloginInitiated, value);
                if (value)
                {
                    CloseKeyboarWindow();
                }
                log.LogMethodExit(reloginInitiated);
            }
        }
        public bool EnableManualTicket
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enableManualTicket);
                return enableManualTicket;
            }
            set
            {
                log.LogMethodEntry(enableManualTicket, value);
                SetProperty(ref enableManualTicket, value);
                log.LogMethodExit(enableManualTicket);
            }
        }

        public bool EnableLoadToCard
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enableLoadToCard);
                return enableLoadToCard;
            }
            set
            {
                log.LogMethodEntry(enableLoadToCard, value);
                SetProperty(ref enableLoadToCard, value);
                log.LogMethodExit(enableLoadToCard);
            }
        }
        public CancellationTokenSource CancellationTokenSource
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cancellationTokenSource);
                return cancellationTokenSource;
            }
            set
            {
                log.LogMethodEntry(cancellationTokenSource, value);
                SetProperty(ref cancellationTokenSource, value);
                log.LogMethodExit(cancellationTokenSource);
            }
        }
        public bool EnableConsolidatePrint
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enableConsolidatePrint);
                return enableConsolidatePrint;
            }
            set
            {
                log.LogMethodEntry(enableConsolidatePrint, value);
                SetProperty(ref enableConsolidatePrint, value);
                log.LogMethodExit(enableConsolidatePrint);
            }
        }

        public bool EnableTurnIn
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enableTurnIn);
                return enableTurnIn;
            }
            set
            {
                log.LogMethodEntry(enableTurnIn, value);
                SetProperty(ref enableTurnIn, value);
                log.LogMethodExit(enableTurnIn);
            }
        }

        public bool EnableRePrint
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enableRePrint);
                return enableRePrint;
            }
            set
            {
                log.LogMethodEntry(enableRePrint, value);
                SetProperty(ref enableRePrint, value);
                log.LogMethodExit(enableRePrint);
            }
        }

        public bool EnableUnFlagVoucher
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enableUnflagVoucher);
                return enableUnflagVoucher;
            }
            set
            {
                log.LogMethodEntry(enableUnflagVoucher, value);
                SetProperty(ref enableUnflagVoucher, value);
                log.LogMethodExit(enableUnflagVoucher);
            }
        }

        public List<int> MembershipIDList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(membershipIDList);
                return membershipIDList;
            }
            set
            {
                log.LogMethodEntry(membershipIDList, value);
                SetProperty(ref membershipIDList, value);
                log.LogMethodExit(membershipIDList);
            }
        }
        public Dictionary<int, int> MembershipIDCardIDList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(membershipIDcardIDList);
                return membershipIDcardIDList;
            }
            set
            {
                log.LogMethodEntry(membershipIDcardIDList, value);
                SetProperty(ref membershipIDcardIDList, value);
                log.LogMethodExit(membershipIDcardIDList);
            }
        }
        public Dictionary<int, int> CardIDcustomerIDList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardIDcustomerIDList);
                return cardIDcustomerIDList;
            }
            set
            {
                log.LogMethodEntry(cardIDcustomerIDList, value);
                SetProperty(ref cardIDcustomerIDList, value);
                log.LogMethodExit(cardIDcustomerIDList);
            }
        }
        public Dictionary<int, string> CustomerIDcustomerInfoList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(customerIDcustomerInfoList);
                return customerIDcustomerInfoList;
            }
            set
            {
                log.LogMethodEntry(customerIDcustomerInfoList, value);
                SetProperty(ref customerIDcustomerInfoList, value);
                log.LogMethodExit(customerIDcustomerInfoList);
            }
        }
        public bool EnableSuspend
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enableSuspend);
                return enableSuspend;
            }
            set
            {
                log.LogMethodEntry(enableSuspend, value);
                SetProperty(ref enableSuspend, value);
                log.LogMethodExit(enableSuspend);
            }
        }
        public DispatcherTimer Timer
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(timer);
                return timer;
            }
            set
            {
                log.LogMethodEntry(timer, value);
                SetProperty(ref timer, value);
                log.LogMethodExit(timer);
            }
        }
        public DatePickerView DatePickerView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(datePickerView);
                return datePickerView;
            }
            set
            {
                log.LogMethodEntry(datePickerView, value);
                SetProperty(ref datePickerView, value);
                log.LogMethodExit(datePickerView);
            }
        }
        public NumberKeyboardView NumberKeyboardView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(numberKeyboardView);
                return numberKeyboardView;
            }
            set
            {
                log.LogMethodEntry(numberKeyboardView, value);
                SetProperty(ref numberKeyboardView, value);
                log.LogMethodExit(numberKeyboardView);
            }
        }
        public GenericDataEntryView FlagOrEnterTicketView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enterTicketView);
                return enterTicketView;
            }
            set
            {
                log.LogMethodEntry(enterTicketView, value);
                SetProperty(ref enterTicketView, value);
                log.LogMethodExit(enterTicketView);
            }
        }
        public AuthenticateManagerView ManagerView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(managerView);
                return managerView;
            }
            set
            {
                log.LogMethodEntry(managerView, value);
                SetProperty(ref managerView, value);
                log.LogMethodExit(managerView);
            }
        }

        public ChangePasswordView PasswordView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(changePasswordView);
                return changePasswordView;
            }
            set
            {
                log.LogMethodEntry(changePasswordView, value);
                SetProperty(ref changePasswordView, value);
                log.LogMethodExit(changePasswordView);
            }
        }

        //public FingerPrintLoginView FingerPrintView
        //{
        //    get
        //    {
        //        log.LogMethodEntry();
        //        log.LogMethodExit(fingerPrintView);
        //        return fingerPrintView;
        //    }
        //    set
        //    {
        //        log.LogMethodEntry(fingerPrintView, value);
        //        SetProperty(ref fingerPrintView, value);
        //        log.LogMethodExit(fingerPrintView);
        //    }
        //}
        public char NewOrCloseScreen
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(newOrCloseScreen);
                return newOrCloseScreen;
            }
            set
            {
                log.LogMethodEntry(newOrCloseScreen, value);
                SetProperty(ref newOrCloseScreen, value);
                log.LogMethodExit(newOrCloseScreen);
            }
        }
        public char ScanTicketGiftMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(scanTicketGiftmode);
                return scanTicketGiftmode;
            }
            set
            {
                log.LogMethodEntry(scanTicketGiftmode, value);
                SetProperty(ref scanTicketGiftmode, value);
                log.LogMethodExit(scanTicketGiftmode);
            }
        }
        internal RedemptionMainUserControl RedemptionMainUserControl
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionMainUserControl);
                return redemptionMainUserControl;
            }
            set
            {
                log.LogMethodEntry(redemptionMainUserControl, value);
                SetProperty(ref redemptionMainUserControl, value);
                log.LogMethodExit(redemptionMainUserControl);
            }
        }

        internal RedemptionMainUserControlVM RedempMainUserControlVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionMainUserControlVM);
                return redemptionMainUserControlVM;
            }
            set
            {
                log.LogMethodEntry(redemptionMainUserControlVM, value);
                SetProperty(ref redemptionMainUserControlVM, value);
                log.LogMethodExit(redemptionMainUserControlVM);
            }
        }

        internal RedemptionView RedemptionMainView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionMainView);
                return redemptionMainView;
            }
            set
            {
                log.LogMethodEntry(redemptionMainView, value);
                SetProperty(ref redemptionMainView, value);
                log.LogMethodExit(redemptionMainView);
            }
        }

        internal RedemptionMainVM MainVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(mainVM);
                return mainVM;
            }
            set
            {
                log.LogMethodEntry(mainVM, value);
                SetProperty(ref mainVM, value);
                log.LogMethodExit(mainVM);
            }
        }

        public GenericDataEntryView DataEntryView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(dataEntryView);
                return dataEntryView;
            }
            set
            {
                log.LogMethodEntry(dataEntryView, value);
                SetProperty(ref dataEntryView, value);
                log.LogMethodExit(dataEntryView);
            }
        }

        public RedemptionScanView ScanView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(scanView);
                return scanView;
            }
            set
            {
                log.LogMethodEntry(scanView, value);
                SetProperty(ref scanView, value);
                log.LogMethodExit(scanView);
            }
        }

        public GenericScanPopupView ScanPopUpView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(scanPopupView);
                return scanPopupView;
            }
            set
            {
                log.LogMethodEntry(scanPopupView, value);
                SetProperty(ref scanPopupView, value);
                log.LogMethodExit(scanPopupView);
            }
        }
        public GenericItemInfoPopUp ItemInfoPopUpView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(itemInfoPopUpView);
                return itemInfoPopUpView;
            }
            set
            {
                log.LogMethodEntry(itemInfoPopUpView, value);
                SetProperty(ref itemInfoPopUpView, value);
                log.LogMethodExit(itemInfoPopUpView);
            }
        }
        public GenericMessagePopupView MessagePopupView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(messagePopupView);
                return messagePopupView;
            }
            set
            {
                log.LogMethodEntry(messagePopupView, value);
                SetProperty(ref messagePopupView, value);
                log.LogMethodExit(messagePopupView);
            }
        }

        public RedemptionReverseView ReverseView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(reverseView);
                return reverseView;
            }
            set
            {
                log.LogMethodEntry(reverseView, value);
                SetProperty(ref reverseView, value);
                log.LogMethodExit(reverseView);
            }
        }

        public RedemptionUpdateView UpdateView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(updateView);
                return updateView;
            }
            set
            {
                log.LogMethodEntry(updateView, value);
                SetProperty(ref updateView, value);
                log.LogMethodExit(updateView);
            }
        }

        public RedemptionTicketAllocationView AllocationView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(allocationView);
                return allocationView;
            }
            set
            {
                log.LogMethodEntry(allocationView, value);
                SetProperty(ref allocationView, value);
                log.LogMethodExit(allocationView);
            }
        }

        public AuthenticateUserView UserView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(userView);
                return userView;
            }
            set
            {
                log.LogMethodEntry(userView, value);
                SetProperty(ref userView, value);
                log.LogMethodExit(userView);
            }
        }

        public LeftPaneSelectedItem LeftPaneSelectedItem
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(leftPaneSelectedItem);
                return leftPaneSelectedItem;
            }
            set
            {
                log.LogMethodEntry(leftPaneSelectedItem, value);
                SetProperty(ref leftPaneSelectedItem, value);
                log.LogMethodExit(leftPaneSelectedItem);
            }
        }
        public string DeviceErrorMessage
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(deviceErrorMessage);
                return deviceErrorMessage;
            }
            set
            {
                log.LogMethodEntry(deviceErrorMessage, value);
                SetProperty(ref deviceErrorMessage, value);
                log.LogMethodExit(deviceErrorMessage);
            }
        }
        public string ScannedBarCode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(scannedBarCode);
                return scannedBarCode;
            }
            set
            {
                log.LogMethodEntry(scannedBarCode, value);
                SetProperty(ref scannedBarCode, value);
                log.LogMethodExit(scannedBarCode);
            }
        }
        public string TurnInCardInfoText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(turnInCardInfoText);
                return turnInCardInfoText;
            }
            set
            {
                log.LogMethodEntry(turnInCardInfoText, value);
                SetProperty(ref turnInCardInfoText, value);
                log.LogMethodExit(turnInCardInfoText);
            }
        }
        public RedemptionDTO RetreivedBackupDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(retreivedBackupDTO);
                return retreivedBackupDTO;
            }
            set
            {
                log.LogMethodEntry(retreivedBackupDTO, value);
                SetProperty(ref retreivedBackupDTO, value);
                log.LogMethodExit(retreivedBackupDTO);
            }
        }
        public RedemptionDTO RedemptionDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            }
            set
            {
                log.LogMethodEntry(redemptionDTO, value);
                SetProperty(ref redemptionDTO, value);
                log.LogMethodExit(redemptionDTO);
            }
        }
        public RedemptionDTO NewRedemptionDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(newredemptionDTO);
                return newredemptionDTO;
            }
            set
            {
                log.LogMethodEntry(newredemptionDTO, value);
                SetProperty(ref newredemptionDTO, value);
                log.LogMethodExit(newredemptionDTO);
            }
        }
        public RedemptionActivityDTO RedemptionActivityManualTicketDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionActivityManualTicketDTO);
                return redemptionActivityManualTicketDTO;
            }
            set
            {
                log.LogMethodEntry(redemptionActivityManualTicketDTO, value);
                SetProperty(ref redemptionActivityManualTicketDTO, value);
                log.LogMethodExit(redemptionActivityManualTicketDTO);
            }
        }
        public RedemptionActivityDTO RedemptionActivityDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionActivityDTO);
                return redemptionActivityDTO;
            }
            set
            {
                log.LogMethodEntry(redemptionActivityDTO, value);
                SetProperty(ref redemptionActivityDTO, value);
                log.LogMethodExit(redemptionActivityDTO);
            }
        }
        public RedemptionLoadToCardRequestDTO RedemptionLoadToCardRequestDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionLoadToCardRequestDTO);
                return redemptionLoadToCardRequestDTO;
            }
            set
            {
                log.LogMethodEntry(redemptionLoadToCardRequestDTO, value);
                SetProperty(ref redemptionLoadToCardRequestDTO, value);
                log.LogMethodExit(redemptionLoadToCardRequestDTO);
            }
        }
        public bool ShowTextBlock
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showTextBlock);
                return showTextBlock;
            }
            set
            {
                log.LogMethodEntry(showTextBlock, value);
                SetProperty(ref showTextBlock, value);
                log.LogMethodExit(showTextBlock);
            }
        }

        public RedemptionUserControlVM RedemptionUserControlVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionUserControlVM);
                return redemptionUserControlVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref redemptionUserControlVM, value);
                log.LogMethodExit(redemptionUserControlVM);
            }
        }

        public RedemptionUserControlVM VoucherUserControlVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(voucherUserControlVM);
                return voucherUserControlVM;
            }
            set
            {
                log.LogMethodEntry(voucherUserControlVM, value);
                SetProperty(ref voucherUserControlVM, value);
                if (voucherUserControlVM != null && !voucherUserControlVM.IsVoucher)
                {
                    VoucherUserControlVM.IsVoucher = true;
                }
                log.LogMethodExit(voucherUserControlVM);
            }
        }

        public RedemptionUserControlVM TurnInUserControlVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(turnInUserControlVM);
                return turnInUserControlVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref turnInUserControlVM, value);
                if (turnInUserControlVM != null && !turnInUserControlVM.IsTurnIn)
                {
                    TurnInUserControlVM.IsTurnIn = true;
                }
                log.LogMethodExit(turnInUserControlVM);
            }
        }

        public RedemptionUserControlVM LoadTicketRedemptionUserControlVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loadTicketRedemptionUserControlVM);
                return loadTicketRedemptionUserControlVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref loadTicketRedemptionUserControlVM, value);
                if (loadTicketRedemptionUserControlVM != null && !loadTicketRedemptionUserControlVM.IsLoadTicket)
                {
                    LoadTicketRedemptionUserControlVM.IsLoadTicket = true;
                }
                log.LogMethodExit(loadTicketRedemptionUserControlVM);
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
                if (LeftPaneVM != null)
                {
                    LeftPaneVM.ModuleName = userName;
                }
                log.LogMethodExit(userName);
            }
        }
        public int ScreenNumber
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
                if (LeftPaneVM != null)
                {
                    LeftPaneVM.ScreenName = screenNumber.ToString();
                }
                log.LogMethodExit(screenNumber);
            }
        }

        public DeviceClass BarCodeReader
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(userName);
                return barcodeReader;
            }
            set
            {
                log.LogMethodEntry(barcodeReader, value);
                SetProperty(ref barcodeReader, value);
                log.LogMethodExit(barcodeReader);
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

        public int CardDeviceAddress
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(carddeviceaddress);
                return carddeviceaddress;
            }
            set
            {
                log.LogMethodEntry(carddeviceaddress, value);
                SetProperty(ref carddeviceaddress, value);
                log.LogMethodExit(carddeviceaddress);
            }
        }
        public int BarCodeDeviceAddress
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(barcodedeviceaddress);
                return barcodedeviceaddress;
            }
            set
            {
                log.LogMethodEntry(barcodedeviceaddress, value);
                SetProperty(ref barcodedeviceaddress, value);
                log.LogMethodExit(barcodedeviceaddress);
            }
        }
        public DateTime LastActivityTime
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(lastActivityTime);
                return lastActivityTime;
            }
            set
            {
                log.LogMethodEntry(lastActivityTime, value);
                SetProperty(ref lastActivityTime, value);
                log.LogMethodExit(lastActivityTime);
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
                SetChildControlsMultiScreenMode();
                SetPreviousViewVisible();
                log.LogMethodExit(multiScreenMode);
            }
        }
        public bool AutoShowLoadTicketProductMenu
        {
            get { return autoShowLoadTicketProductMenu; }
            set { SetProperty(ref autoShowLoadTicketProductMenu, value); }
        }
        public bool AutoShowRedemptionProductMenu
        {
            get { return autoShowRedemptionProductMenu; }
            set { SetProperty(ref autoShowRedemptionProductMenu, value); }
        }
        public bool IsActive
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isActive);
                return isActive;
            }
            set
            {
                log.LogMethodEntry(isActive, value);
                if (LeftPaneVM != null)
                {
                    if (mainVM != null && !multiUserMultiscreen && !singleUserMultiscreen
                      && LeftPaneVM != null)
                    {
                        LeftPaneVM.AddButtonVisiblity = false;
                    }
                    else
                    {
                        if (mainVM != null && mainVM.RedemptionUserControlVMs.Count == 8)
                        {
                            LeftPaneVM.AddButtonVisiblity = false;
                        }
                        else
                        {
                            LeftPaneVM.AddButtonVisiblity = value;
                        }
                    }
                }
                if (timer != null)
                {
                    if (value)
                    {
                        if (value != isActive)
                        {
                            RegisterDevices();
                            timer.Start();
                        }
                    }
                    else
                    {
                        if (value != isActive)
                        {
                            UnRegisterDevices();
                            timer.Stop();
                        }
                    }
                }
                else
                {
                    if (value)
                    {
                        if (value != isActive)
                        {
                            RegisterDevices();
                        }
                    }
                    else
                    {
                        if (value != isActive)
                        {
                            UnRegisterDevices();
                        }
                    }
                }
                if (value)
                {
                    if (reloginInitiated)
                    {
                        ShowRelogin(this.UserName);
                    }
                }
                SetProperty(ref isActive, value);
                SetChildWindowsEnable();
                SetPreviousViewVisible();
                log.LogMethodExit(isActive);
            }
        }

        public ICommand AddButtonClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addButtonClickedCommand);
                return addButtonClickedCommand;
            }
            private set
            {
                log.LogMethodEntry(value);
                SetProperty(ref addButtonClickedCommand, value);
                log.LogMethodExit(addButtonClickedCommand);
            }
        }
        public ICommand FooterLoadedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(footerLoadedCommand);
                return footerLoadedCommand;
            }
            set
            {
                log.LogMethodEntry(footerLoadedCommand, value);
                SetProperty(ref footerLoadedCommand, value);
                log.LogMethodExit(footerLoadedCommand);
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
        public ICommand FooterMessageClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(footerMessageClickedCommand);
                return footerMessageClickedCommand;
            }
            set
            {
                log.LogMethodEntry(footerMessageClickedCommand, value);
                SetProperty(ref footerMessageClickedCommand, value);
                log.LogMethodExit(footerMessageClickedCommand);
            }
        }
        public ICommand SidebarClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(sidebarClickedCommand);
                return sidebarClickedCommand;
            }
            private set
            {
                log.LogMethodEntry(sidebarClickedCommand, value);
                SetProperty(ref sidebarClickedCommand, value);
                log.LogMethodExit(sidebarClickedCommand);
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
        protected override bool ButtonEnable(object state)
        {
            return !IsLoadingVisible;
        }
        public void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            if (LeftPaneVM != null)
            {
                LeftPaneVM.IsLoadingVisible = IsLoadingVisible;
                LeftPaneVM.RaiseCanExecuteChanged();
            }
            if (redemptionUserControlVM != null)
            {
                redemptionUserControlVM.RaiseCanExecuteChanged();
            }
            if (loadTicketRedemptionUserControlVM != null)
            {
                loadTicketRedemptionUserControlVM.RaiseCanExecuteChanged();
            }
            if (turnInUserControlVM != null)
            {
                turnInUserControlVM.RaiseCanExecuteChanged();
            }
            if (voucherUserControlVM != null)
            {
                voucherUserControlVM.RaiseCanExecuteChanged();
            }
            log.LogMethodExit();
        }
        internal void UpdateDefaultValues()
        {
            log.LogMethodEntry();
            if (RedemptionUserControlVM != null)
            {
                RedemptionUserControlVM.SetDefaultValues(RedempMainUserControlVM);
            }
            if (LoadTicketRedemptionUserControlVM != null)
            {
                LoadTicketRedemptionUserControlVM.SetDefaultValues(RedempMainUserControlVM);
            }
            if (TurnInUserControlVM != null)
            {
                TurnInUserControlVM.SetDefaultValues(RedempMainUserControlVM);
            }
            if (VoucherUserControlVM != null)
            {
                VoucherUserControlVM.SetDefaultValues(RedempMainUserControlVM);
            }
            WireupEvents();
            log.LogMethodExit();
        }
        private void SetItemBackground()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                MultiScreenItemBackground multiScreenItemBackground = MultiScreenItemBackground.Grey;
                if (applyColorCode)
                {
                    multiScreenItemBackground = MultiScreenItemBackground.White;
                }
                if (RedemptionUserControlVM != null)
                {
                    if (RedemptionUserControlVM.GenericToggleButtonsVM != null)
                    {
                        RedemptionUserControlVM.GenericToggleButtonsVM.MultiScreenItemBackground = multiScreenItemBackground;
                    }
                    if (RedemptionUserControlVM.GenericDisplayItemsVM != null)
                    {
                        RedemptionUserControlVM.GenericDisplayItemsVM.MultiScreenItemBackground = multiScreenItemBackground;
                    }
                }
                if (LoadTicketRedemptionUserControlVM != null)
                {
                    if (LoadTicketRedemptionUserControlVM.GenericToggleButtonsVM != null)
                    {
                        LoadTicketRedemptionUserControlVM.GenericToggleButtonsVM.MultiScreenItemBackground = multiScreenItemBackground;
                    }
                    if (LoadTicketRedemptionUserControlVM.GenericDisplayItemsVM != null)
                    {
                        LoadTicketRedemptionUserControlVM.GenericDisplayItemsVM.MultiScreenItemBackground = multiScreenItemBackground;
                    }
                }
                if (TurnInUserControlVM != null)
                {
                    if (TurnInUserControlVM.GenericToggleButtonsVM != null)
                    {
                        TurnInUserControlVM.GenericToggleButtonsVM.MultiScreenItemBackground = multiScreenItemBackground;
                    }
                    if (TurnInUserControlVM.GenericDisplayItemsVM != null)
                    {
                        TurnInUserControlVM.GenericDisplayItemsVM.MultiScreenItemBackground = multiScreenItemBackground;
                    }
                }
                if (VoucherUserControlVM != null)
                {
                    if (VoucherUserControlVM.GenericToggleButtonsVM != null)
                    {
                        VoucherUserControlVM.GenericToggleButtonsVM.MultiScreenItemBackground = multiScreenItemBackground;
                    }
                    if (VoucherUserControlVM.GenericDisplayItemsVM != null)
                    {
                        VoucherUserControlVM.GenericDisplayItemsVM.MultiScreenItemBackground = multiScreenItemBackground;
                    }
                }
            });
            log.LogMethodExit();
        }
        private void CloseKeyboarWindow()
        {
            log.LogMethodEntry();
            if (!isActive || reloginInitiated)
            {
                if (itemInfoPopUpView != null && itemInfoPopUpView.KeyBoardHelper != null)
                {
                    itemInfoPopUpView.KeyBoardHelper.CloseWindows();
                }
                if (dataEntryView != null && dataEntryView.KeyBoardHelper != null)
                {
                    dataEntryView.KeyBoardHelper.CloseWindows();
                }
                if (updateView != null && updateView.KeyboardHelper != null)
                {
                    updateView.KeyboardHelper.CloseWindows();
                }
                if (allocationView != null && allocationView.KeyBoardHelper != null)
                {
                    allocationView.KeyBoardHelper.CloseWindows();
                }
                if (enterTicketView != null && enterTicketView.KeyBoardHelper != null)
                {
                    enterTicketView.KeyBoardHelper.CloseWindows();
                }
                if (changePasswordView != null && changePasswordView.KeyboardHelper != null)
                {
                    changePasswordView.KeyboardHelper.CloseWindows();
                }
                if (managerView != null && managerView.KeyboardHelper != null)
                {
                    managerView.KeyboardHelper.CloseWindows();
                }
                CloseNumberPad();
                if (redemptionMainUserControl != null && redemptionMainUserControl.KeyboardHelper != null)
                {
                    redemptionMainUserControl.KeyboardHelper.CloseWindows();
                }
            }
            log.LogMethodExit();
        }
        private void SetChildWindowsEnable()
        {
            log.LogMethodEntry();
            CloseKeyboarWindow();
            if (itemInfoPopUpView != null)
            {
                SetWindowEnable(itemInfoPopUpView);
            }
            if (dataEntryView != null)
            {
                SetWindowEnable(dataEntryView);
            }
            if (scanView != null)
            {
                SetWindowEnable(scanView);
            }
            if (scanPopupView != null)
            {
                SetWindowEnable(scanPopupView);
            }
            if (messagePopupView != null)
            {
                SetWindowEnable(messagePopupView);
            }
            if (reverseView != null)
            {
                SetWindowEnable(reverseView);
            }
            if (updateView != null)
            {
                SetWindowEnable(updateView);
            }
            if (allocationView != null)
            {
                SetWindowEnable(allocationView);
            }
            if (userView != null)
            {
                SetWindowEnable(userView);
            }
            if (enterTicketView != null)
            {
                SetWindowEnable(enterTicketView);
            }
            if (managerView != null)
            {
                SetWindowEnable(managerView);
            }
            if (changePasswordView != null)
            {
                SetWindowEnable(changePasswordView);
            }
            if (datePickerView != null)
            {
                SetWindowEnable(datePickerView);
            }
            CloseNumberPad();
            if (FooterVM != null && FooterVM.MessagePopupView != null)
            {
                SetWindowEnable(FooterVM.MessagePopupView);
            }
            SetOpacityforScreens();
            log.LogMethodExit();
        }
        internal void SetOpacityforScreens()
        {
            log.LogMethodEntry();
            if (redemptionMainUserControl != null)
            {
                if (isActive || (mainVM != null && !mainVM.ShowAllRedemptions))
                {
                    redemptionMainUserControl.Opacity = 1;
                }
                else
                {
                    redemptionMainUserControl.Opacity = 0.5;
                    if (isChildWindowOpen)
                    {
                        redemptionMainUserControl.Opacity = 0;
                        isChildWindowOpen = false;
                    }
                }
            }
            log.LogMethodExit();
        }
        private void SetWindowEnable(Window window)
        {
            log.LogMethodEntry();
            if (window != null)
            {
                if (!isChildWindowOpen && !isActive)
                {
                    isChildWindowOpen = Application.Current.Windows.Cast<Window>().Any(x => x == window);
                }
                if (isActive && window.Opacity != 1)
                {
                    window.Opacity = 1;
                }
                else if (!isActive && window.Opacity != 0.5)
                {
                    window.Opacity = 0.5;
                }
                if (reloginInitiated)
                {
                    window.IsEnabled = false;
                }
                else
                {
                    window.IsEnabled = true;
                }
                if (window.OwnedWindows != null)
                {
                    foreach (Window childWindow in window.OwnedWindows)
                    {
                        if (!isActive && window.Opacity != 0)
                        {
                            window.Opacity = 0;
                        }
                        childWindow.PreviewMouseDown -= UpdateActivityTimeOnMouseOrKeyBoardAction;
                        childWindow.PreviewMouseDown += UpdateActivityTimeOnMouseOrKeyBoardAction;
                        childWindow.PreviewKeyDown -= UpdateActivityTimeOnMouseOrKeyBoardAction;
                        childWindow.PreviewKeyDown += UpdateActivityTimeOnMouseOrKeyBoardAction;
                        SetWindowEnable(childWindow);
                    }
                }
            }
            log.LogMethodExit();
        }
        private void SetPreviousViewVisible()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (LeftPaneVM != null && LeftPaneVM.MenuItems != null)
                {
                    if (LeftPaneVM.MenuItems.Contains(MessageViewContainerList.GetMessage(this.ExecutionContext, "Previous View", null)))
                    {
                        if (multiScreenMode)
                        {
                            LeftPaneVM.MenuItems.Remove(MessageViewContainerList.GetMessage(this.ExecutionContext, "Previous View", null));
                        }
                    }
                    else if ((!multiScreenMode && isActive)
                        || (mainVM != null && mainVM.RedemptionHeaderTagsVM != null && mainVM.RedemptionHeaderTagsVM.HeaderGroups != null
                        && mainVM.RedemptionHeaderTagsVM.HeaderGroups.Count == 1 && isActive))
                    {
                        if (SystemOptionViewContainerList.GetSystemOption<bool>(ExecutionContext, "Show Previous View", "Redemption"))
                        {
                            LeftPaneVM.MenuItems.Add(MessageViewContainerList.GetMessage(this.ExecutionContext, "Previous View", null));
                        }
                    }
                }
            });
            log.LogMethodExit();
        }
        private void SetChildControlsMultiScreenMode()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (FooterVM != null)
                {
                    FooterVM.MultiScreenMode = multiScreenMode;
                }
                if (LeftPaneVM != null)
                {
                    LeftPaneVM.MultiScreenMode = multiScreenMode;
                    LeftPaneVM.ModuleName = userName;
                }
                if (RedemptionUserControlVM != null)
                {
                    RedemptionUserControlVM.MultiScreenMode = multiScreenMode;
                    RedemptionUserControlVM.UserName = userName;
                    RedemptionUserControlVM.ScreenNumber = screenNumber.ToString();
                }
                if (LoadTicketRedemptionUserControlVM != null)
                {
                    LoadTicketRedemptionUserControlVM.MultiScreenMode = multiScreenMode;
                    LoadTicketRedemptionUserControlVM.UserName = userName;
                    LoadTicketRedemptionUserControlVM.ScreenNumber = screenNumber.ToString();
                }
                if (TurnInUserControlVM != null)
                {
                    TurnInUserControlVM.MultiScreenMode = multiScreenMode;
                    TurnInUserControlVM.UserName = userName;
                    TurnInUserControlVM.ScreenNumber = screenNumber.ToString();
                }
                if (VoucherUserControlVM != null)
                {
                    VoucherUserControlVM.MultiScreenMode = multiScreenMode;
                    VoucherUserControlVM.UserName = userName;
                    VoucherUserControlVM.ScreenNumber = screenNumber.ToString();
                }
                if (!multiScreenMode)
                {
                    ShowTextBlock = false;
                }
                SetKeyBoardHelperColorCode();
                switch (leftPaneSelectedItem)
                {
                    case LeftPaneSelectedItem.Redemption:
                        {
                            if (RedemptionUserControlVM != null)
                            {
                                RedemptionUserControlVM.TranslatePage();
                            }
                        }
                        break;
                    case LeftPaneSelectedItem.LoadTicket:
                        {
                            if (LoadTicketRedemptionUserControlVM != null)
                            {
                                LoadTicketRedemptionUserControlVM.TranslatePage();
                            }
                        }
                        break;
                    case LeftPaneSelectedItem.TurnIn:
                        {
                            if (TurnInUserControlVM != null)
                            {
                                TurnInUserControlVM.TranslatePage();
                            }
                        }
                        break;
                    case LeftPaneSelectedItem.Voucher:
                        {
                            if (VoucherUserControlVM != null)
                            {
                                VoucherUserControlVM.TranslatePage();
                            }
                        }
                        break;
                }
            });
            log.LogMethodExit();
        }
        internal void SetProductMenuDisplay(RedemptionMainVM mainVM)
        {
            log.LogMethodEntry(mainVM);
            if (mainVM != null && mainVM.RedemptionUserControlVMs != null && mainVM.RedemptionUserControlVMs.Count > 1)
            {
                log.LogMethodExit("multiscreen mode.");
                return;
            }
            switch (leftPaneSelectedItem)
            {
                case LeftPaneSelectedItem.Redemption:
                    if (!autoShowRedemptionProductMenu && RedemptionUserControlVM != null
                        && RedemptionUserControlVM.RedemptionsType == RedemptionsType.New)
                    {
                        FooterVM.OnHideSideBarClicked(null);
                        RedemptionUserControlVM.GenericTransactionListVM.ShowNavigationButton = true;
                        RedemptionUserControlVM.GenericRightSectionContentVM.ShowNavigationButton = true;
                    }
                    break;
                case LeftPaneSelectedItem.LoadTicket:
                    if (!autoShowLoadTicketProductMenu && LoadTicketRedemptionUserControlVM != null
                        && LoadTicketRedemptionUserControlVM.RedemptionsType == RedemptionsType.New)
                    {
                        FooterVM.OnHideSideBarClicked(null);
                        LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ShowNavigationButton = true;
                        LoadTicketRedemptionUserControlVM.GenericRightSectionContentVM.ShowNavigationButton = true;
                    }
                    break;
                case LeftPaneSelectedItem.TurnIn:
                    if (!autoShowRedemptionProductMenu && TurnInUserControlVM != null
                        && TurnInUserControlVM.RedemptionsType == RedemptionsType.New)
                    {
                        FooterVM.OnHideSideBarClicked(null);
                        TurnInUserControlVM.GenericTransactionListVM.ShowNavigationButton = true;
                        TurnInUserControlVM.GenericRightSectionContentVM.ShowNavigationButton = true;
                    }
                    break;
            }
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithFooter(() =>
            {
                if (parameter != null)
                {
                    redemptionMainUserControl = parameter as RedemptionMainUserControl;
                    if (redemptionMainUserControl != null)
                    {
                        redemptionMainUserControl.SizeChanged -= OnSizeChanged;
                        redemptionMainUserControl.SizeChanged += OnSizeChanged;
                        HookFooterMouseDownEvent();
                        OnSizeChanged(redemptionMainUserControl, null);
                        if (redemptionMainUserControl.DataContext != null)
                        {
                            redemptionMainUserControlVM = redemptionMainUserControl.DataContext as RedemptionMainUserControlVM;
                        }
                        FindAncestor(redemptionMainUserControl);
                    }
                }
            });
            log.LogMethodExit();
        }
        internal void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                if (datePickerView != null && datePickerView.DataContext != null && datePickerView.DataContext is DatePickerVM)
                {
                    DatePickerVM datePickerVM = datePickerView.DataContext as DatePickerVM;
                    OnWindowLoaded(datePickerView, null);
                }
                if (FooterVM != null && FooterVM.MessagePopupView != null)
                {
                    OnWindowLoaded(FooterVM.MessagePopupView, null);
                }
                if (FlagOrEnterTicketView != null)
                {
                    OnWindowLoaded(FlagOrEnterTicketView, null);
                }
                if (DataEntryView != null)
                {
                    OnWindowLoaded(DataEntryView, null);
                }
                if (numberKeyboardView != null && numberKeyboardView.DataContext != null && numberKeyboardView.DataContext is NumberKeyboardVM)
                {
                    NumberKeyboardVM numberKeyboardVM = numberKeyboardView.DataContext as NumberKeyboardVM;
                    OnWindowLoaded(numberKeyboardView, null);
                }
                if (updateView != null && updateView.DataContext != null && updateView.DataContext is RedemptionUpdateVM)
                {
                    RedemptionUpdateVM updateVM = updateView.DataContext as RedemptionUpdateVM;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        updateVM.IsMultiScreenRowTwo = true;
                    }
                    else if (updateVM.IsMultiScreenRowTwo)
                    {
                        updateVM.IsMultiScreenRowTwo = false;
                    }
                    updateVM.MultiScreenMode = this.MultiScreenMode;
                    OnWindowLoaded(updateView, null);
                }
                if (allocationView != null && allocationView.DataContext != null && allocationView.DataContext is RedemptionTicketAllocationVM)
                {
                    RedemptionTicketAllocationVM ticketAllocationVM = allocationView.DataContext as RedemptionTicketAllocationVM;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        ticketAllocationVM.IsMultiScreenRowTwo = true;
                    }
                    else if (ticketAllocationVM.IsMultiScreenRowTwo)
                    {
                        ticketAllocationVM.IsMultiScreenRowTwo = false;
                    }
                    ticketAllocationVM.MultiScreenMode = this.MultiScreenMode;
                    OnWindowLoaded(allocationView, null);
                }
                if (scanView != null && scanView.DataContext != null && scanView.DataContext is RedemptionScanVM)
                {
                    RedemptionScanVM scanVM = scanView.DataContext as RedemptionScanVM;
                    scanVM.MultiScreenMode = this.MultiScreenMode;
                    OnWindowLoaded(scanView, null);
                }
                if (reverseView != null && reverseView.DataContext != null && reverseView.DataContext is RedemptionReverseVM)
                {
                    RedemptionReverseVM reverseVM = reverseView.DataContext as RedemptionReverseVM;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        reverseVM.IsMultiScreenRowTwo = true;
                    }
                    else if (reverseVM.IsMultiScreenRowTwo)
                    {
                        reverseVM.IsMultiScreenRowTwo = false;
                    }
                    reverseVM.MultiScreenMode = this.MultiScreenMode;
                    OnWindowLoaded(reverseView, null);
                }
                if (userView != null && userView.DataContext != null && userView.DataContext is AuthenticateUserVM)
                {
                    AuthenticateUserVM userVM = userView.DataContext as AuthenticateUserVM;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        userVM.IsMultiScreenRowTwo = true;
                    }
                    else if (userVM.IsMultiScreenRowTwo)
                    {
                        userVM.IsMultiScreenRowTwo = false;
                    }
                    userVM.MultiScreenMode = this.MultiScreenMode;
                    OnWindowLoaded(userView, null);
                }
                if (messagePopupView != null && messagePopupView.DataContext != null && messagePopupView.DataContext is AuthenticateUserVM)
                {
                    GenericMessagePopupVM messagePopupVM = messagePopupView.DataContext as GenericMessagePopupVM;
                    OnWindowLoaded(messagePopupView, null);
                }
                if (managerView != null && managerView.DataContext != null && managerView.DataContext is AuthenticateManagerVM)
                {
                    AuthenticateManagerVM managerVM = managerView.DataContext as AuthenticateManagerVM;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        managerVM.IsMultiScreenRowTwo = true;
                    }
                    else if (managerVM.IsMultiScreenRowTwo)
                    {
                        managerVM.IsMultiScreenRowTwo = false;
                    }
                    managerVM.MultiScreenMode = this.MultiScreenMode;
                    OnWindowLoaded(managerView, null);
                }
                if (changePasswordView != null && changePasswordView.DataContext != null && changePasswordView.DataContext is ChangePasswordVM)
                {
                    ChangePasswordVM passwordVM = changePasswordView.DataContext as ChangePasswordVM;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        passwordVM.IsMultiScreenRowTwo = true;
                    }
                    else if (passwordVM.IsMultiScreenRowTwo)
                    {
                        passwordVM.IsMultiScreenRowTwo = false;
                    }
                    passwordVM.MultiScreenMode = this.MultiScreenMode;
                    OnWindowLoaded(changePasswordView, null);
                }
                //if (fingerPrintView != null && fingerPrintView.DataContext != null && fingerPrintView.DataContext is FingerPrintLoginVM)
                //{
                //    FingerPrintLoginVM fingerPrintVM = fingerPrintView.DataContext as FingerPrintLoginVM;
                //    if (mainVM != null && mainVM.RowCount > 1)
                //    {
                //        fingerPrintVM.IsMultiScreenRowTwo = true;
                //    }
                //    else if (fingerPrintVM.IsMultiScreenRowTwo)
                //    {
                //        fingerPrintVM.IsMultiScreenRowTwo = false;
                //    }
                //    fingerPrintVM.MultiScreenMode = this.MultiScreenMode;
                //    OnWindowLoaded(fingerPrintView, null);
                //}
                if (itemInfoPopUpView != null && itemInfoPopUpView.DataContext != null && itemInfoPopUpView.DataContext is GenericItemInfoPopUpVM)
                {
                    GenericItemInfoPopUpVM itemInfoPopUpVM = itemInfoPopUpView.DataContext as GenericItemInfoPopUpVM;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        itemInfoPopUpVM.IsMultiScreenRowTwo = true;
                    }
                    else if (itemInfoPopUpVM.IsMultiScreenRowTwo)
                    {
                        itemInfoPopUpVM.IsMultiScreenRowTwo = false;
                    }
                    itemInfoPopUpVM.MultiScreenMode = this.MultiScreenMode;
                    OnWindowLoaded(itemInfoPopUpView, null);
                }
            });
            log.LogMethodExit();
        }
        internal void UnWireupEvents()
        {
            log.LogMethodEntry();
            if (redemptionMainUserControl != null)
            {
                redemptionMainUserControl.PreviewMouseDown -= OnPreviewMouseDown;
                redemptionMainUserControl.KeyDown -= OnKeyDown;
                redemptionMainUserControl.KeyUp -= OnKeyUp;
            }
            log.LogMethodExit();
        }
        private void WireupEvents()
        {
            log.LogMethodEntry();
            if (redemptionMainUserControl != null)
            {
                if (!redemptionMainUserControl.Focusable)
                {
                    redemptionMainUserControl.Focusable = true;
                }
                bool shortCutKeyEnabled = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(this.ExecutionContext, "ENABLE_REDEMPTION_CURRENCY_SHORTCUT_KEYS");
                if (shortCutKeyEnabled)
                {
                    redemptionMainUserControl.KeyDown -= OnKeyDown;
                    redemptionMainUserControl.KeyDown += OnKeyDown;
                    redemptionMainUserControl.KeyUp -= OnKeyUp;
                    redemptionMainUserControl.KeyUp += OnKeyUp;
                }
                redemptionMainUserControl.PreviewMouseDown -= OnPreviewMouseDown;
                redemptionMainUserControl.PreviewMouseDown += OnPreviewMouseDown;
                if (isActive && !redemptionMainUserControl.IsFocused)
                {
                    redemptionMainUserControl.Focus();
                }
            }
            log.LogMethodExit();
        }
        private void FindAncestor(Visual myVisual)
        {
            log.LogMethodEntry(myVisual);
            ExecuteActionWithFooter(() =>
            {
                if (myVisual == null)
                {
                    return;
                }
                object visual = VisualTreeHelper.GetParent(myVisual);
                if (visual is Window)
                {
                    redemptionMainView = visual as RedemptionView;
                    if (redemptionMainView != null && redemptionMainView.DataContext != null && mainVM == null)
                    {
                        RedemptionMainVM redemptionMainVM = redemptionMainView.DataContext as RedemptionMainVM;
                        if (redemptionMainVM != null)
                        {
                            mainVM = redemptionMainVM;
                        }
                        //bool shortCutKeyEnabled = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(this.ExecutionContext, "ENABLE_REDEMPTION_CURRENCY_SHORTCUT_KEYS");
                        //if (shortCutKeyEnabled)
                        //{
                        WireupEvents();
                        //}
                    }
                    if (mainVM != null)
                    {
                        SetPreviousViewVisible();
                    }
                }
                else
                {
                    FindAncestor(visual as Visual);
                }
            });
            log.LogMethodExit();
        }
        private bool IsRedemptionDelivered()
        {
            log.LogMethodEntry();
            bool isDelivered = false;
            ExecuteActionWithFooter(() =>
            {
                switch (leftPaneSelectedItem)
                {
                    case LeftPaneSelectedItem.Redemption:
                        {
                            if (RedemptionUserControlVM != null && this.redemptionDTO != null && this.redemptionDTO.RedemptionStatus != null
                    && this.redemptionDTO.RedemptionStatus.ToLower() == "DELIVERED".ToLower())
                            {
                                isDelivered = true;
                            }
                        }
                        break;
                    case LeftPaneSelectedItem.LoadTicket:
                        {
                            if (LoadTicketRedemptionUserControlVM != null && this.redemptionDTO != null && this.redemptionDTO.RedemptionId != -1
                            && this.redemptionDTO.RedemptionStatus != null
                            && this.redemptionDTO.RedemptionStatus.ToLower() == "DELIVERED".ToLower())
                            {
                                isDelivered = true;
                            }
                        }
                        break;
                    case LeftPaneSelectedItem.TurnIn:
                        {
                            if (TurnInUserControlVM != null && this.redemptionDTO != null && this.redemptionDTO.RedemptionId != -1 && this.redemptionDTO.Remarks != null
                    && this.redemptionDTO.Remarks.ToLower() == "TURNINREDEMPTION".ToLower())
                            {
                                isDelivered = true;
                            }
                        }
                        break;

                }
            });
            log.LogMethodExit();
            return isDelivered;
        }
        internal void ClearCompletedRedemption()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                SetNewRedemption();
                switch (leftPaneSelectedItem)
                {
                    case LeftPaneSelectedItem.Redemption:
                        {
                            if (RedemptionUserControlVM != null)
                            {
                                if (RedemptionUserControlVM.GenericTransactionListVM != null)
                                {
                                    RedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Clear();
                                    RedemptionUserControlVM.GenericTransactionListVM.SelectedItem = null;
                                }
                                RedemptionUserControlVM.TransactionID = "RO-";
                                RedemptionUserControlVM.StayInTransactionMode = false;
                                RedemptionUserControlVM.UpdateTicketValues();
                            }
                        }
                        break;
                    case LeftPaneSelectedItem.TurnIn:
                        {
                            if (TurnInUserControlVM != null)
                            {
                                if (TurnInUserControlVM.GenericTransactionListVM != null)
                                {
                                    TurnInUserControlVM.GenericTransactionListVM.ItemCollection.Clear();
                                    TurnInUserControlVM.GenericTransactionListVM.SelectedItem = null;
                                }
                                TurnInUserControlVM.TransactionID = "RO-";
                                TurnInUserControlVM.StayInTransactionMode = false;
                                TurnInUserControlVM.LoadTotatlTicketCount = 0;
                                TurnInUserControlVM.SelectedTargetLocation = null;
                                this.TurnInCardInfoText = string.Empty;
                            }
                        }
                        break;
                    case LeftPaneSelectedItem.LoadTicket:
                        {
                            if (LoadTicketRedemptionUserControlVM != null)
                            {
                                if (LoadTicketRedemptionUserControlVM.GenericTransactionListVM != null)
                                {
                                    LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Clear();
                                    LoadTicketRedemptionUserControlVM.GenericTransactionListVM.SelectedItem = null;
                                }
                                LoadTicketRedemptionUserControlVM.TransactionID = "RO-";
                                LoadTicketRedemptionUserControlVM.StayInTransactionMode = false;
                            }
                        }
                        break;
                }
                CallRecalculatePriceandStock(true,true);
                SetHeaderCustomerBalanceInfo(string.Empty, 0);
                SetFooterContent(string.Empty, MessageType.None);
                if (UserView == null)
                {
                    if (ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "REQUIRE_LOGIN_FOR_EACH_TRX", false))
                    {
                        ShowRelogin(this.UserName);
                    }
                }
            });
            log.LogMethodExit();
        }
        internal void CallRecalculatePriceandStock(bool fromScroll = false,bool recalculateStock=false,bool skipPriceupdate=false)
        {
            log.LogMethodEntry();
            try
            {
                if (redemptionMainUserControlVM != null)
                {
                    List<Task> priceTasks = new List<Task>();
                    priceTasks.Add(Task.Factory.StartNew(() => { RecalculateProductPriceandStock(fromScroll, recalculateStock, skipPriceupdate); }, cancellationTokenSource.Token));
                }
            }
            catch(OperationCanceledException ex)
            {
                ResetRecalculateFlags();
            }
            log.LogMethodExit();
        }
        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                if (isActive)
                {
                    lastActivityTime = DateTime.Now;
                    if (IsRedemptionDelivered())
                    {
                        bool isPrintButtonClicked = false;
                        if (e != null && e.OriginalSource != null)
                        {
                            CustomActionButton customActionButton = null;
                            if (e.OriginalSource is Border)
                            {
                                Border border = e.OriginalSource as Border;
                                if (border != null && border.TemplatedParent != null)
                                {
                                    customActionButton = border.TemplatedParent as CustomActionButton;
                                }
                            }
                            if (customActionButton == null && e.OriginalSource is TextBlock)
                            {
                                TextBlock textBlock = e.OriginalSource as TextBlock;
                                if (textBlock != null && textBlock.TemplatedParent != null)
                                {
                                    customActionButton = textBlock.TemplatedParent as CustomActionButton;
                                }
                            }
                            if (customActionButton != null && (customActionButton.Name.ToLower() == "btnPrint".ToLower()
                            || customActionButton.Name.ToLower() == "btnturninprint".ToLower()))
                            {
                                isPrintButtonClicked = true;
                                if (leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket && redemptionDTO != null
                                && !redemptionDTO.RedemptionCardsListDTO.Any(r => r.CardId > 0))
                                {
                                    isPrintButtonClicked = false;
                                }
                            }
                        }
                        if (!isPrintButtonClicked)
                        {
                            ClearCompletedRedemption();
                        }
                    }
                }
                RedemptionMainUserControl mainUserControl = sender as RedemptionMainUserControl;
                if (mainUserControl != null && !mainUserControl.IsFocused && !this.IsActive && !this.reloginInitiated)
                {
                    lastActivityTime = DateTime.Now;
                    mainUserControl.Focus();
                }
                else if (!mainUserControl.IsFocused && mainVM != null && mainVM.RedemptionUserControlVMs.Count > 1 && !reloginInitiated)
                {
                    CustomComboBox customComboBox = FocusManager.GetFocusedElement(redemptionMainView) as CustomComboBox;
                    ComboBoxItem comboBoxItem = FocusManager.GetFocusedElement(redemptionMainView) as ComboBoxItem;
                    System.Windows.Controls.Primitives.ToggleButton toggleButton = FocusManager.GetFocusedElement(redemptionMainView) as System.Windows.Controls.Primitives.ToggleButton;
                    if(toggleButton != null)
                    {
                        customComboBox = toggleButton.TemplatedParent as CustomComboBox;
                    }
                    else
                    {
                        CustomScrollViewer customScrollViewer = FocusManager.GetFocusedElement(redemptionMainView) as CustomScrollViewer;
                        if(customScrollViewer != null)
                        {
                            customComboBox = customScrollViewer.TemplatedParent as CustomComboBox;
                        }
                    }
                    lastActivityTime = DateTime.Now;
                    if (customComboBox == null && comboBoxItem == null)
                    {
                        int distinctCount = mainVM.RedemptionUserControlVMs.Select(r => r.UserName).Distinct().Count();
                        if (isActive && distinctCount > 1)
                        {
                            mainUserControl.Focus();
                        }
                    }
                }
                else if (!IsActive && (reloginInitiated || (mainVM != null && !mainVM.ShowAllRedemptions)))
                {
                    e.Handled = true;
                }
            });
            log.LogMethodExit();
        }
        internal void OnKeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (IsLoadingVisible)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4877), MessageType.Warning);
                log.LogMethodExit("Another API/action in progress");
                return;
            }
            ExecuteActionWithFooter(() =>
            {
                if (!reloginInitiated && (this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption ||
                this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket))
                {
                    this.lastActivityTime = DateTime.Now;
                    if (this.IsActive && (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl)
                    || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
                    && e.Key != Key.LeftCtrl && e.Key != Key.RightCtrl)
                    {
                        if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                        {
                            shortCutKeys = shortCutKeys + e.Key.ToString().Last();
                        }
                        else
                        {
                            shortCutKeys = shortCutKeys + e.Key.ToString();
                        }
                        //AddCurrency(null, null, null, pressedkey);
                    }
                }
            });
            log.LogMethodExit();
        }
        internal void OnKeyUp(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                if (!reloginInitiated && (this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption ||
                this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket))
                {
                    this.lastActivityTime = DateTime.Now;
                    if (this.IsActive && (e.Key == Key.RightCtrl || e.Key == Key.LeftCtrl)
                    )
                    {
                        AddCurrency(null, null, null, shortCutKeys);
                        shortCutKeys = null;
                    }
                }
            });
            log.LogMethodExit();
        }
        internal void RefreshTransactionItem()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (this.redemptionDTO != null && this.RedemptionDTO.RedemptionCardsListDTO != null
                && leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket && LoadTicketRedemptionUserControlVM != null
                && LoadTicketRedemptionUserControlVM.GenericTransactionListVM != null
                && LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ItemCollection != null)
                {
                    LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Clear();
                    if (this.RedemptionDTO.RedemptionCardsListDTO.Any() && this.RedemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyId >= 0 || x.CurrencyRuleId >= 0))
                    {
                        foreach (int grouping in RedemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId!=null && x.CurrencyId >= 0 && x.ViewGroupingNumber != null).Select(x => x.ViewGroupingNumber).Distinct())
                        {
                            RedemptionCurrencyContainerDTO currencyDTO = mainVM.GetRedemptionCurrencyContainerDTOList(ExecutionContext).FirstOrDefault(c => c.CurrencyId == this.RedemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CurrencyId >= 0 && x.ViewGroupingNumber == grouping).CurrencyId);
                            LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Add(
                                new GenericTransactionListItem(ExecutionContext)
                                {
                                    ProductName = currencyDTO.CurrencyName,
                                    Ticket = (int)currencyDTO.ValueInTickets,
                                    TicketDisplayText = MessageViewContainerList.GetMessage(ExecutionContext, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets")),
                                    Count = this.RedemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0 && x.ViewGroupingNumber == grouping).Select(x => x.CurrencyQuantity == null ? 0 : (int)x.CurrencyQuantity).Sum(),
                                    RedemptionRightSectionItemType = GenericTransactionListItemType.Item,
                                    Key = (int)currencyDTO.CurrencyId,
                                    ViewGroupingNumber = grouping
                                });
                        }
                        foreach (int currencyId in this.RedemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0 && x.ViewGroupingNumber == null).Select(x => x.CurrencyId).Distinct())
                        {
                            RedemptionCurrencyContainerDTO currencyDTO = mainVM.GetRedemptionCurrencyContainerDTOList(ExecutionContext).FirstOrDefault(c => c.CurrencyId == currencyId);
                            LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Add(
                                new GenericTransactionListItem(ExecutionContext)
                                {
                                    ProductName = currencyDTO.CurrencyName,
                                    Ticket = (int)currencyDTO.ValueInTickets,
                                    TicketDisplayText = MessageViewContainerList.GetMessage(ExecutionContext, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets")),
                                    Count = this.RedemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0 && x.CurrencyId == currencyId && x.ViewGroupingNumber == null).Select(x => x.CurrencyQuantity == null ? 0 : (int)x.CurrencyQuantity).Sum(),
                                    RedemptionRightSectionItemType = GenericTransactionListItemType.Item,
                                    Key = (int)currencyDTO.CurrencyId,
                                    ViewGroupingNumber = null
                                });
                        }
                        foreach (RedemptionCardsDTO cardsDTO in this.RedemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyRuleId != null && (x.CurrencyId == null || x.CurrencyId < 0) && x.CurrencyRuleId >= 0))
                        {
                            if (cardsDTO.CurrencyRuleId != null && cardsDTO.CurrencyRuleId != -1)
                            {
                                string currencyRuleName = RedemptionCurrencyRuleViewContainerList.GetRedemptionCurrencyRuleContainerDTOList(ExecutionContext).FirstOrDefault(c => c.RedemptionCurrencyRuleId == cardsDTO.CurrencyRuleId).RedemptionCurrencyRuleName;
                                LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Add(
                                    new GenericTransactionListItem(ExecutionContext)
                                    {
                                        ProductName = currencyRuleName,
                                        Ticket = (int)(cardsDTO.TicketCount / ((cardsDTO.CurrencyQuantity == null|| cardsDTO.CurrencyQuantity ==0)?1: cardsDTO.CurrencyQuantity)) ,
                                        TicketDisplayText = MessageViewContainerList.GetMessage(ExecutionContext, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets")),
                                        Count = cardsDTO.CurrencyQuantity == null ? 0 : (int)cardsDTO.CurrencyQuantity,
                                        RedemptionRightSectionItemType = GenericTransactionListItemType.Item,
                                        Key = (int)cardsDTO.CurrencyRuleId,
                                        IsEnabled = false
                                    });
                            }
                        }
                    }
                    foreach (TicketReceiptDTO ticketReceiptDTO in this.RedemptionDTO.TicketReceiptListDTO)
                    {
                        LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Add(
                                new GenericTransactionListItem(LoadTicketRedemptionUserControlVM.ExecutionContext)
                                {
                                    RedemptionRightSectionItemType = GenericTransactionListItemType.Ticket,
                                    TicketNo = ticketReceiptDTO.ManualTicketReceiptNo,
                                    Ticket = ticketReceiptDTO.BalanceTickets,
                                    // IsEnabled = false
                                });
                    }
                    SetTotalCurrencyTickets();
                    LoadTicketRedemptionUserControlVM.LoadTotatlTicketCount = this.GetLoadTicketTotalCount();
                    if (LoadTicketRedemptionUserControlVM.LoadTotatlTicketCount >= 0)
                    {
                        LoadTicketRedemptionUserControlVM.StayInTransactionMode = true;
                    }
                }
            });
            log.LogMethodExit();
        }
        internal void SetRedemptionCardsDTO(RedemptionCurrencyContainerDTO currencyDTO, int quantity = 0, bool fromTransaction = false,int? viewGroupingNumber=null)
        {
            log.LogMethodEntry(currencyDTO, quantity);
            ExecuteActionWithFooter(() =>
            {
                if (this.RedemptionDTO != null)
                {
                    if (this.RedemptionDTO.RedemptionCardsListDTO == null)
                    {
                        this.RedemptionDTO.RedemptionCardsListDTO = new List<RedemptionCardsDTO>();
                    }
                    RedemptionCardsDTO cardsDTO = this.RedemptionDTO.RedemptionCardsListDTO.FirstOrDefault(r => r.CurrencyId == currencyDTO.CurrencyId && ((viewGroupingNumber!=null && r.ViewGroupingNumber== viewGroupingNumber) || (viewGroupingNumber == null && r.ViewGroupingNumber==null)));
                    if (cardsDTO != null)
                    {
                        updatedcardsDTO = new List<RedemptionCardsDTO>() { cardsDTO };
                        //if (quantity == 0)
                        //{
                        //    quantity = 1;
                        //    cardsDTO.CurrencyQuantity += quantity;
                        //}
                        //else
                        //{
                        //    cardsDTO.CurrencyQuantity = quantity;
                        //}
                        if (quantity == 0)
                        {
                            quantity = 1;
                        }
                        int? oldqty = cardsDTO.CurrencyQuantity;
                        if (fromTransaction)
                        {
                            cardsDTO.CurrencyQuantity = quantity;
                        }
                        else
                        {
                            cardsDTO.CurrencyQuantity += quantity;
                        }
                        cardsDTO.TicketCount = cardsDTO.CurrencyQuantity * cardsDTO.CurrencyValueInTickets;
                        this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2931, cardsDTO.CurrencyName, oldqty, cardsDTO.CurrencyQuantity), MessageType.Info);
                    }
                    else
                    {
                        if (quantity == 0)
                        {
                            quantity = 1;
                        }
                        RedemptionCardsDTO redemptionCardsDTO = new RedemptionCardsDTO(-1, redemptionDTO.RedemptionId, null, -1, (int)currencyDTO.ValueInTickets * quantity, currencyDTO.CurrencyId, quantity, (int)currencyDTO.ValueInTickets, currencyDTO.CurrencyName, 0,viewGroupingNumber);
                        updatedcardsDTO = new List<RedemptionCardsDTO>() { redemptionCardsDTO };
                        this.RedemptionDTO.RedemptionCardsListDTO.Add(redemptionCardsDTO);
                        this.SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 1393), MessageType.Info);
                    }
                }
                if (ApplyCurrencyRule())
                {
                    updatedcardsDTO = this.RedemptionDTO.RedemptionCardsListDTO;
                    this.RefreshTransactionItem();
                }
                SetTotalCurrencyTickets();
            });
            log.LogMethodExit();
        }
        internal void SetTotalCurrencyTickets()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (RedemptionDTO.RedemptionCardsListDTO != null)
                {
                    int totalCurrencyTicket = 0;
                    foreach (RedemptionCardsDTO cardsDTO in RedemptionDTO.RedemptionCardsListDTO)
                    {
                        if (cardsDTO.CurrencyId != null && cardsDTO.CurrencyId != -1)
                        {
                            if (cardsDTO.CurrencyValueInTickets != null)
                            {
                                totalCurrencyTicket += (int)cardsDTO.CurrencyQuantity * (int)cardsDTO.CurrencyValueInTickets;
                            }
                        }
                        else if (cardsDTO.CurrencyRuleId != null && cardsDTO.CurrencyRuleId != -1)
                        {
                            if (cardsDTO.CurrencyValueInTickets != null)
                            {
                                totalCurrencyTicket += (int)cardsDTO.CurrencyQuantity * (int)cardsDTO.CurrencyValueInTickets;
                            }
                            else
                            {
                                totalCurrencyTicket += (int)cardsDTO.TicketCount;
                            }
                        }
                    }
                    redemptionDTO.CurrencyTickets = totalCurrencyTicket;
                }
            });
            log.LogMethodExit();
        }
        internal void AddCurrency(string currencyName, string currencyId,
           string barcode, string shortCutKey)
        {
            log.LogMethodEntry(currencyName, currencyId, barcode, shortCutKey);
            ExecuteActionWithFooter(() =>
            {
                RedemptionCurrencyContainerDTO currencyDTO = null;
                if (mainVM.GetRedemptionCurrencyContainerDTOList(ExecutionContext) != null)
                {
                    if (!string.IsNullOrEmpty(currencyName))
                    {
                        currencyDTO = mainVM.GetRedemptionCurrencyContainerDTOList(ExecutionContext).FirstOrDefault(r => r.CurrencyName.ToLower() == currencyName.ToLower());
                    }
                    else if (!string.IsNullOrEmpty(currencyId))
                    {
                        currencyDTO = mainVM.GetRedemptionCurrencyContainerDTOList(ExecutionContext).FirstOrDefault(r => r.CurrencyId.ToString().ToLower() == currencyId.ToLower());
                    }
                    else if (!string.IsNullOrEmpty(barcode))
                    {
                        currencyDTO = mainVM.GetRedemptionCurrencyContainerDTOList(ExecutionContext).FirstOrDefault(r => r.BarCode.ToLower() == barcode.ToLower());
                        if (currencyDTO == null)
                        {
                            if (RedemptionCurrencyViewContainerList.GetRedemptionCurrencyContainerDTOList(ExecutionContext).Any(x => x.BarCode.ToLower() == barcode.ToLower()))
                            {
                                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1550, RedemptionCurrencyViewContainerList.GetRedemptionCurrencyContainerDTOList(ExecutionContext).FirstOrDefault(x => x.BarCode.ToLower() == barcode.ToLower()).CurrencyName), MessageType.Error);
                                isTicket = false;
                                return;
                            }
                            isTicket = true;
                            return;
                        }
                    }
                    else if (!string.IsNullOrEmpty(shortCutKey))
                    {
                        currencyDTO = mainVM.GetRedemptionCurrencyContainerDTOList(ExecutionContext).FirstOrDefault(r => r.ShortCutKeys.ToLower() == shortCutKey.ToLower());
                        if (currencyDTO == null)
                        {
                            if (shortCutKey != "LeftCtrl" && shortCutKey != "RightCtrl")
                            {
                                if (RedemptionCurrencyViewContainerList.GetRedemptionCurrencyContainerDTOList(ExecutionContext).Any(x => x.ShortCutKeys.ToLower() == shortCutKey.ToLower()))
                                {
                                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1550, RedemptionCurrencyViewContainerList.GetRedemptionCurrencyContainerDTOList(ExecutionContext).FirstOrDefault(x => x.ShortCutKeys.ToLower() == shortCutKey.ToLower()).CurrencyName), MessageType.Error);
                                    return;
                                }
                                else
                                {
                                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1584, shortCutKey), MessageType.Error);
                                    return;
                                }
                            }
                        }
                    }
                }
                if (currencyDTO != null)
                {
                    if (currencyDTO.ManagerApproval && !UserViewContainerList.IsSelfApprovalAllowed(this.ExecutionContext.SiteId, this.ExecutionContext.UserPKId)
                        && !this.redemptionDTO.RedemptionCardsListDTO.Any(r => r.CurrencyId == currencyDTO.CurrencyId))
                    {
                        waitingCurrencyDTO = currencyDTO;
                        OpenManagerView(ManageViewType.ShortCutKey);
                    }
                    else
                    {
                        lastScannedType = 'C';
                        lastScannedCurrency = currencyDTO;
                        lastScannedViewGrouping = GetNextViewGroupingNumber();
                        AddCurrencyToUI(currencyDTO, 0,false,lastScannedViewGrouping);
                        if (this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption && RedemptionUserControlVM != null
&& !RedemptionUserControlVM.StayInTransactionMode)
                        {
                            RedemptionUserControlVM.StayInTransactionMode = true;
                        }
                        if (this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket && LoadTicketRedemptionUserControlVM != null
&& !LoadTicketRedemptionUserControlVM.StayInTransactionMode)
                        {
                            LoadTicketRedemptionUserControlVM.StayInTransactionMode = true;
                        }
                    }
                }
            });
            log.LogMethodExit();
        }
        internal void SetUserControlFocus()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (redemptionMainView != null && !redemptionMainView.IsFocused)
                {
                    redemptionMainView.Focus();
                }
                if (redemptionMainUserControl != null && !redemptionMainUserControl.Focusable)
                {
                    redemptionMainUserControl.Focusable = true;
                }
                if (redemptionMainUserControl != null && !redemptionMainUserControl.IsFocused)
                {
                    redemptionMainUserControl.UpdateLayout();
                    redemptionMainUserControl.Focus();
                }
            });
            log.LogMethodExit();
        }
        private void OnShortCutKeyApproveManagerViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                AuthenticateManagerVM managerVM = (sender as AuthenticateManagerView).DataContext as AuthenticateManagerVM;
                if (managerVM.IsValid && waitingCurrencyDTO != null)
                {
                    lastScannedType = 'C';
                    lastScannedCurrency = waitingCurrencyDTO;
                    lastScannedViewGrouping = GetNextViewGroupingNumber();
                    AddCurrencyToUI(waitingCurrencyDTO,0,false, lastScannedViewGrouping);
                    waitingCurrencyDTO = null;
                }
                else
                {
                    this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268, null), MessageType.Error);
                    log.Debug("End currency addition -manager didnt approve");
                }
            });
            managerView = null;
            SetUserControlFocus();
            log.LogMethodExit();
        }
        internal void AddCurrencyToUI(RedemptionCurrencyContainerDTO currencyDTO, int quantity = 0, bool fromTransaction = false,int? viewGroupingNumber=null)
        {
            log.LogMethodEntry(currencyDTO, quantity, fromTransaction, viewGroupingNumber);
            ExecuteActionWithFooter(() =>
            {
                isTicket = false;
                if (IsRedemptionDelivered())
                {
                    ClearCompletedRedemption();
                }
                if (redemptionDTO!=null && redemptionDTO.RedemptionStatus!=null && redemptionDTO.RedemptionStatus== RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString())
                {
                    redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.NEW.ToString();
                }
                if (leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket && LoadTicketRedemptionUserControlVM != null
                       && LoadTicketRedemptionUserControlVM.GenericDisplayItemsVM != null)
                {
                    LoadTicketRedemptionUserControlVM.AddCurrencyToTransaction(currencyDTO, quantity,viewGroupingNumber);
                }
                if (quantity == 0)
                {
                    SetRedemptionCardsDTO(currencyDTO,0,false,viewGroupingNumber);
                }
                else
                {
                    SetRedemptionCardsDTO(currencyDTO, quantity, fromTransaction,viewGroupingNumber);
                }
                if (redemptionMainUserControlVM != null && quantity == 0)
                {
                    this.SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 1393), MessageType.Info);
                }
                if ((currencyDTO.ShowQtyPrompt &&!fromTransaction )|| allocationView != null)
                {
                    OpenTicketAllocation(true, goToLastItem: currencyDTO.ShowQtyPrompt ? true : false);
                }
                if (leftPaneSelectedItem == LeftPaneSelectedItem.Redemption && RedemptionUserControlVM != null)
                {
                    RedemptionUserControlVM.UpdateTicketValues();
                    RedemptionUserControlVM.HideContentArea();
                }
                else if (leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket && LoadTicketRedemptionUserControlVM != null)
                {
                    LoadTicketRedemptionUserControlVM.LoadTotatlTicketCount = this.GetLoadTicketTotalCount();
                    LoadTicketRedemptionUserControlVM.HideContentArea();
                }
                SetHeaderCustomerBalanceInfo(redemptionDTO.CustomerName, this.GetBalanceTickets());
                updatedcardsDTO = null;
            });
            log.LogMethodExit();
        }
        internal async Task AddProduct(ProductsContainerDTO productDTO)
        {
            log.LogMethodEntry(productDTO);
            if (productDTO != null)
            {
                lastScannedType = 'G';
                lastScannedProduct = productDTO;
                await AddProductToUI(productDTO);
            }
            log.LogMethodExit();
        }
        internal async Task AddProductToUI(ProductsContainerDTO productDTO, int quantity = 1, bool fromTransaction = false)
        {
            log.LogMethodEntry(productDTO, quantity, fromTransaction);
            if (redemptionDTO != null && redemptionDTO.RedemptionStatus != null && redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString())
            {
                redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.NEW.ToString();
            }
            if (productDTO != null)
            {
                if (this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption && RedemptionUserControlVM != null)
                {
                    if (!mainVM.AllowTransactionOnZeroStock && productDTO.InventoryItemContainerDTO != null)
                    {
                        double stock = await RedemptionUserControlVM.GetTotalStock(productDTO);
                        int totalQuantity = quantity;
                        int productId = productDTO.InventoryItemContainerDTO.ProductId;
                        if (!fromTransaction && redemptionDTO.RedemptionGiftsListDTO.Any(x => x.ProductId == productId))
                        {
                            totalQuantity += redemptionDTO.RedemptionGiftsListDTO.Where(x => x.ProductId == productId).Sum(x => x.ProductQuantity);
                        }
                        if (stock < totalQuantity)
                        {
                            this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 120), MessageType.Error);
                            log.Info("Ends-addGift(" + productDTO.InventoryItemContainerDTO.Code + ") as selected more gifts than available stock. Please crosscheck available quantity before proceeding");
                            log.LogMethodExit();
                            return;
                        }
                    }
                    RedemptionUserControlVM.SetTransactionSection(productDTO, quantity, fromTransaction);
                    RedemptionUserControlVM.UpdateTicketValues();
                    SetHeaderCustomerBalanceInfo(null, this.GetBalanceTickets());
                    SetHideTapCardEnterScanTicket();
                    RedemptionUserControlVM.HideContentArea();
                }
                else if (this.leftPaneSelectedItem == LeftPaneSelectedItem.TurnIn && TurnInUserControlVM != null)
                {
                    TurnInUserControlVM.SetTransactionSection(productDTO, quantity, fromTransaction);
                    SetTurnInTotalTicketCount();
                    SetHideTapCardEnterScanTicket();
                    TurnInUserControlVM.HideContentArea();
                }
            }
            log.LogMethodExit();
        }
        internal int SetTurnInTotalTicketCount()
        {
            log.LogMethodEntry();
            int totalTkt = 0;
            ExecuteActionWithFooter(() =>
            {
                if (redemptionDTO != null)
                {
                    if (redemptionDTO.RedemptionGiftsListDTO != null && redemptionDTO.RedemptionGiftsListDTO.Count > 0)
                    {
                        foreach (RedemptionGiftsDTO giftsDTO in redemptionDTO.RedemptionGiftsListDTO)
                        {
                            totalTkt += (giftsDTO.ProductQuantity * (int)giftsDTO.Tickets);
                        }
                    }
                    totalTkt *= -1;
                    if (TurnInUserControlVM != null)
                    {
                        TurnInUserControlVM.LoadTotatlTicketCount = totalTkt;
                    }
                    if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Count > 0)
                    {
                        foreach (RedemptionCardsDTO cardsDTO in redemptionDTO.RedemptionCardsListDTO)
                        {
                            totalTkt += (int)cardsDTO.TotalCardTickets;
                        }
                    }
                }
            });
            log.LogMethodExit();
            return totalTkt;
        }
        internal void OpenTicketAllocation(bool currencyChecked = false, bool ticketsChecked = false, bool voucherChecked = false, bool goToLastItem = false)
        {
            log.LogMethodEntry(currencyChecked, ticketsChecked);
            ExecuteActionWithFooter(() =>
            {
                RedemptionTicketAllocationVM ticketAllocationVM = null;
                if (this.AllocationView == null)
                {
                    this.AllocationView = new RedemptionTicketAllocationView();
                    ticketAllocationVM = new RedemptionTicketAllocationVM(this.ExecutionContext, this, mainVM.GetRedemptionCurrencyContainerDTOList(ExecutionContext), this.cardReader);
                    ticketAllocationVM.MultiScreenMode = MultiScreenMode;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        ticketAllocationVM.IsMultiScreenRowTwo = true;
                    }
                    this.AllocationView.DataContext = ticketAllocationVM;
                    this.AllocationView.PreviewMouseDown += this.UpdateActivityTimeOnMouseOrKeyBoardAction;
                    this.AllocationView.PreviewKeyDown += UpdateActivityTimeOnMouseOrKeyBoardAction;
                    if (this.AllocationView.KeyBoardHelper != null)
                    {
                        this.SetKeyBoardHelperColorCode();
                        this.AllocationView.KeyBoardHelper.KeypadMouseDownEvent -= redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                        this.AllocationView.KeyBoardHelper.KeypadMouseDownEvent += redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                    }
                    this.AllocationView.Loaded += this.OnWindowLoaded;
                    if (leftPaneSelectedItem == LeftPaneSelectedItem.Redemption
                        && RedemptionUserControlVM != null)
                    {
                        this.AllocationView.Closed += RedemptionUserControlVM.OnAllocationViewClosed;
                    }
                    else if (leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket
                        && LoadTicketRedemptionUserControlVM != null)
                    {
                        this.AllocationView.Closed += LoadTicketRedemptionUserControlVM.OnAllocationViewClosed;
                    }
                    this.AllocationView.Show();
                }
                else
                {
                    goToLastItem = true;
                    ticketAllocationVM = this.AllocationView.DataContext as RedemptionTicketAllocationVM;
                    ticketAllocationVM.MultiScreenMode = MultiScreenMode;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        ticketAllocationVM.IsMultiScreenRowTwo = true;
                    }
                    if (this.AllocationView.KeyBoardHelper != null)
                    {
                        this.SetKeyBoardHelperColorCode();
                        this.AllocationView.KeyBoardHelper.KeypadMouseDownEvent -= redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                        this.AllocationView.KeyBoardHelper.KeypadMouseDownEvent += redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                    }
                    ticketAllocationVM.OnToggleChecked(this.AllocationView);
                }
                if (currencyChecked)
                {
                    CustomToggleButtonItem toggleButtonItem = ticketAllocationVM.GenericToggleButtonsVM.ToggleButtonItems.FirstOrDefault(d =>
                           d.DisplayTags[0].Text.ToLower() == MessageViewContainerList.GetMessage(this.ExecutionContext, "CURRENCIES", null).ToLower());
                    if (toggleButtonItem != null)
                    {
                        toggleButtonItem.IsChecked = true;
                    }
                }
                if (!enableManualTicket)
                {
                    CustomToggleButtonItem toggleButtonItem = ticketAllocationVM.GenericToggleButtonsVM.ToggleButtonItems.FirstOrDefault(d =>
                           d.DisplayTags[0].Text.ToLower() == MessageViewContainerList.GetMessage(this.ExecutionContext, "TICKETS", null).ToLower());
                    if (toggleButtonItem != null)
                    {
                        ticketAllocationVM.GenericToggleButtonsVM.ToggleButtonItems.Remove(toggleButtonItem);
                    }
                }
                if (ticketsChecked && enableManualTicket)
                {
                    CustomToggleButtonItem toggleButtonItem = ticketAllocationVM.GenericToggleButtonsVM.ToggleButtonItems.FirstOrDefault(d =>
                           d.DisplayTags[0].Text.ToLower() == MessageViewContainerList.GetMessage(this.ExecutionContext, "TICKETS", null).ToLower());
                    if (toggleButtonItem != null)
                    {
                        toggleButtonItem.IsChecked = true;
                    }
                }
                if (voucherChecked)
                {
                    CustomToggleButtonItem toggleButtonItem = ticketAllocationVM.GenericToggleButtonsVM.ToggleButtonItems.FirstOrDefault(d =>
                    d.DisplayTags[0].Text.ToLower() == MessageViewContainerList.GetMessage(this.ExecutionContext, "VOUCHERS", null).ToLower());
                    if (toggleButtonItem != null)
                    {
                        toggleButtonItem.IsChecked = true;
                    }
                }
                if(goToLastItem && ticketAllocationVM != null)
                {
                    ticketAllocationVM.ScrollToLastItem();
                }
            });
            log.LogMethodExit();
        }
        private void OnAllocationViewContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                if (this.AllocationView != null && this.AllocationView.upDown != null &&
                this.AllocationView.upDown.TextBox != null)
                {
                    this.AllocationView.upDown.TextBox.Focus();
                }
            });
            log.LogMethodExit();
        }
        private void OnLeftPaneMenuSelected(object param)
        {
            log.LogMethodEntry(param);
            ExecuteActionWithFooter(() =>
            {
                if (DeviceErrorMessage != null)
                {
                    this.SetFooterContent(DeviceErrorMessage, MessageType.Warning);
                }
                else
                {
                    this.SetFooterContent(string.Empty, MessageType.None);
                }
                accountDTO = null;
                if (RedemptionUserControlVM != null)
                {
                    RedemptionUserControlVM.ClearSearchFields();
                    if (RedemptionUserControlVM.ShowSearchCloseIcon)
                    {
                        RedemptionUserControlVM.PerformSearch();
                    }
                    if (!RedemptionUserControlVM.GenericToggleButtonsVM.ToggleButtonItems[0].IsChecked)
                    {
                        RedemptionUserControlVM.GenericToggleButtonsVM.ToggleButtonItems[0].IsChecked = true;
                    }
                    else
                    {
                        RedemptionUserControlVM.OnToggleChecked(null);
                    }
                }
                if (TurnInUserControlVM != null)
                {
                    TurnInUserControlVM.ClearSearchFields();
                    TurnInUserControlVM.AutoShowProductMenu(redemptionMainUserControlVM, false);
                    if (TurnInUserControlVM.ShowSearchCloseIcon)
                    {
                        TurnInUserControlVM.PerformSearch();
                    }
                    if (!TurnInUserControlVM.GenericToggleButtonsVM.ToggleButtonItems[0].IsChecked)
                    {
                        TurnInUserControlVM.GenericToggleButtonsVM.ToggleButtonItems[0].IsChecked = true;
                    }
                    else
                    {
                        TurnInUserControlVM.OnToggleChecked(null);
                    }
                }
                if (LoadTicketRedemptionUserControlVM != null)
                {
                    LoadTicketRedemptionUserControlVM.ClearSearchFields();
                    LoadTicketRedemptionUserControlVM.AutoShowProductMenu(redemptionMainUserControlVM, false);
                    if (LoadTicketRedemptionUserControlVM.ShowSearchCloseIcon)
                    {
                        LoadTicketRedemptionUserControlVM.PerformSearch();
                    }
                    if (!LoadTicketRedemptionUserControlVM.GenericToggleButtonsVM.ToggleButtonItems[0].IsChecked)
                    {
                        LoadTicketRedemptionUserControlVM.GenericToggleButtonsVM.ToggleButtonItems[0].IsChecked = true;
                    }
                    else
                    {
                        LoadTicketRedemptionUserControlVM.OnToggleChecked(null);
                    }
                }
                if (VoucherUserControlVM != null)
                {
                    VoucherUserControlVM.ClearSearchFields();
                    if (VoucherUserControlVM.ShowSearchCloseIcon)
                    {
                        VoucherUserControlVM.PerformSearch();
                    }
                    if (!VoucherUserControlVM.GenericToggleButtonsVM.ToggleButtonItems[0].IsChecked)
                    {
                        VoucherUserControlVM.GenericToggleButtonsVM.ToggleButtonItems[0].IsChecked = true;
                    }
                    else
                    {
                        VoucherUserControlVM.OnToggleChecked(null);
                    }
                }
                if (ShowTextBlock)
                {
                    ShowTextBlock = false;
                }
                if (LeftPaneVM != null)
                {
                    if (multiScreenMode)
                    {
                        if (string.IsNullOrEmpty(LeftPaneVM.SelectedMenuItem))
                        {
                            ShowTextBlock = true;
                        }
                        else
                        {
                            ShowTextBlock = false;
                            if (FooterVM != null && FooterVM.SideBarContent != MessageViewContainerList.GetMessage(ExecutionContext, "Show Sidebar"))
                            {
                                FooterVM.SideBarContent = MessageViewContainerList.GetMessage(ExecutionContext, "Show Sidebar");
                            }
                        }
                    }
                    if (LeftPaneVM.SelectedMenuItem != MessageViewContainerList.GetMessage(this.ExecutionContext, "Previous View", null))
                    {
                        if (!string.IsNullOrEmpty(turnInCardInfoText))
                        {
                            TurnInCardInfoText = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(oldLeftPaneSelectedItem) && LeftPaneVM.SelectedMenuItem.ToLower() != oldLeftPaneSelectedItem.ToLower()
                            && !ShowDiscardConfirmation(true))
                        {
                            newSelectedLeftPaneItem = LeftPaneVM.SelectedMenuItem;
                            LeftPaneVM.SelectedMenuItem = oldLeftPaneSelectedItem;
                            return;
                        }
                        oldLeftPaneSelectedItem = LeftPaneVM.SelectedMenuItem;
                    }
                    if (LeftPaneVM.SelectedMenuItem == MessageViewContainerList.GetMessage(this.ExecutionContext, "Redemption", null))
                    {
                        LeftPaneSelectedItem = LeftPaneSelectedItem.Redemption;
                        if (TurnInUserControlVM != null)
                        {
                            TurnInUserControlVM.StayInTransactionMode = false;
                        }
                        if (LoadTicketRedemptionUserControlVM != null)
                        {
                            LoadTicketRedemptionUserControlVM.StayInTransactionMode = false;
                        }
                        if (RedemptionUserControlVM != null && string.IsNullOrEmpty(newSelectedLeftPaneItem))
                        {
                            RedemptionUserControlVM.AutoShowProductMenu(this, false);
                            //RedemptionUserControlVM.GenericDisplayItemsVM.UIDisplayItemModels.Clear();
                        }
                    }

                    else if (LeftPaneVM.SelectedMenuItem == MessageViewContainerList.GetMessage(this.ExecutionContext, "Load Ticket", null))
                    {
                        LeftPaneSelectedItem = LeftPaneSelectedItem.LoadTicket;
                        if (RedemptionUserControlVM != null)
                        {
                            RedemptionUserControlVM.StayInTransactionMode = false;
                        }
                        if (TurnInUserControlVM != null)
                        {
                            TurnInUserControlVM.StayInTransactionMode = false;
                        }
                        if (LoadTicketRedemptionUserControlVM != null)
                        {
                            if (mainVM != null && mainVM.GetRedemptionCurrencyContainerDTOList(ExecutionContext) != null
                                && mainVM.GetRedemptionCurrencyContainerDTOList(ExecutionContext).Count <= 0)
                            {
                                LoadTicketRedemptionUserControlVM.ShowNoCurrencyTextBlock = true;
                            }
                            else if (LoadTicketRedemptionUserControlVM.ShowNoCurrencyTextBlock)
                            {
                                LoadTicketRedemptionUserControlVM.ShowNoCurrencyTextBlock = false;
                            }
                            if (!LoadTicketRedemptionUserControlVM.IsLoadTicket)
                            {
                                LoadTicketRedemptionUserControlVM.IsLoadTicket = true;
                            }
                            if (LoadTicketRedemptionUserControlVM.GenericDisplayItemsVM != null)
                            {
                                // LoadTicketRedemptionUserControlVM.GenericDisplayItemsVM.UIDisplayItemModels.Clear();
                            }
                            if (LoadTicketRedemptionUserControlVM.TodayCompletedRedemptions == null)
                            {
                                LoadTicketRedemptionUserControlVM.TodayCompletedRedemptions = new List<RedemptionDTO>();
                            }
                            if (string.IsNullOrEmpty(newSelectedLeftPaneItem))
                            {
                                LoadTicketRedemptionUserControlVM.AutoShowProductMenu(this, false);
                            }
                        }
                    }
                    else if (LeftPaneVM.SelectedMenuItem == MessageViewContainerList.GetMessage(this.ExecutionContext, "Turn In", null))
                    {
                        LeftPaneSelectedItem = LeftPaneSelectedItem.TurnIn;
                        if (RedemptionUserControlVM != null)
                        {
                            RedemptionUserControlVM.StayInTransactionMode = false;
                        }
                        if (LoadTicketRedemptionUserControlVM != null)
                        {
                            LoadTicketRedemptionUserControlVM.StayInTransactionMode = false;
                            if (LoadTicketRedemptionUserControlVM.TodayCompletedRedemptions == null)
                            {
                                if (RedemptionUserControlVM.TodayCompletedRedemptions != null && RedemptionUserControlVM.TodayCompletedRedemptions.Any())
                                {
                                    LoadTicketRedemptionUserControlVM.TodayCompletedRedemptions = RedemptionUserControlVM.TodayCompletedRedemptions.Where(t => t.RedemptionGiftsListDTO.Count == 0).ToList();
                                }
                            }
                        }
                        if (TurnInUserControlVM != null)
                        {
                            if(string.IsNullOrEmpty(newSelectedLeftPaneItem))
                            {
                                TurnInUserControlVM.AutoShowProductMenu(this, false);
                            }
                            if (TurnInUserControlVM.TodayCompletedRedemptions == null)
                            {
                                TurnInUserControlVM.TodayCompletedRedemptions = new List<RedemptionDTO>();
                            }
                        }
                    }
                    else if (LeftPaneVM.SelectedMenuItem == MessageViewContainerList.GetMessage(this.ExecutionContext, "Voucher", null))
                    {
                        LeftPaneSelectedItem = LeftPaneSelectedItem.Voucher;
                        if (RedemptionUserControlVM != null)
                        {
                            RedemptionUserControlVM.StayInTransactionMode = false;
                        }
                        if (LoadTicketRedemptionUserControlVM != null)
                        {
                            LoadTicketRedemptionUserControlVM.StayInTransactionMode = false;
                        }
                        if (TurnInUserControlVM != null)
                        {
                            TurnInUserControlVM.StayInTransactionMode = false;
                        }
                        if (VoucherUserControlVM != null)
                        {
                            VoucherUserControlVM.SetDefaultCollections(false);
                            if (multiScreenMode && VoucherUserControlVM.GenericRightSectionContentVM != null
                                && !VoucherUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble)
                            {
                                VoucherUserControlVM.GenericRightSectionContentVM.IsScreenUserAreaVisble = true;
                            }
                        }
                    }
                    else if (LeftPaneVM.SelectedMenuItem == MessageViewContainerList.GetMessage(this.ExecutionContext, "Previous View", null))
                    {
                        if (mainVM != null && !mainVM.CheckInProgressRedemption())
                        {
                            string leftPaneSelected = string.Empty;
                            switch (leftPaneSelectedItem)
                            {
                                case LeftPaneSelectedItem.Redemption:
                                    {
                                        leftPaneSelected = MessageViewContainerList.GetMessage(this.ExecutionContext, "Redemption", null);
                                    }
                                    break;
                                case LeftPaneSelectedItem.LoadTicket:
                                    {
                                        leftPaneSelected = MessageViewContainerList.GetMessage(this.ExecutionContext, "Load Ticket", null);
                                    }
                                    break;
                                case LeftPaneSelectedItem.TurnIn:
                                    {
                                        leftPaneSelected = MessageViewContainerList.GetMessage(this.ExecutionContext, "Turn In", null);
                                    }
                                    break;
                                case LeftPaneSelectedItem.Voucher:
                                    {
                                        leftPaneSelected = MessageViewContainerList.GetMessage(this.ExecutionContext, "Voucher", null);
                                    }
                                    break;
                            }
                            if (!string.IsNullOrEmpty(leftPaneSelected))
                            {
                                LeftPaneVM.SelectedMenuItem = leftPaneSelected;
                            }
                        }
                    }
                    SetUserControlFocus();
                }
            });
            log.LogMethodExit();
        }
        internal void OnEnterTicketViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                GenericDataEntryVM dataEntryVM = (sender as GenericDataEntryView).DataContext as GenericDataEntryVM;
                if (dataEntryVM != null && dataEntryVM.ButtonClickType == ButtonClickType.Ok && dataEntryVM.DataEntryCollections != null
                    && dataEntryVM.DataEntryCollections.Count > 0 && dataEntryVM.DataEntryCollections[0].Type == DataEntryType.TextBox)
                {
                    AddTickettoUI(dataEntryVM.DataEntryCollections[0].Text);
                    if (this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket && this.ScanView != null)
                    {
                        this.ScanView.Close();
                        this.ScanView = null;
                    }
                }
            });
            SetUserControlFocus();
            log.LogMethodExit();
        }
        internal void UpdateActivityTimeOnMouseOrKeyBoardAction(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (reloginInitiated)
            {
                if (isActive && userView == null)
                {
                    return;
                }
                KeyEventArgs eventArgs = e as KeyEventArgs;
                if (eventArgs != null)
                {
                    eventArgs.Handled = true;
                }
                else
                {
                    MouseButtonEventArgs buttonEventArgs = e as MouseButtonEventArgs;
                    if (buttonEventArgs != null)
                    {
                        buttonEventArgs.Handled = true;
                    }
                }
            }
            else
            {
                UpdateActivityTimeOnAction();
                if (!this.isActive && mainVM != null)
                {
                    mainVM.OnRedemptionMouseClicked(this);
                }
                if (sender is RedemptionTicketAllocationView)
                {
                    if (isActive && redemptionMainView != null && !redemptionMainView.IsActive)
                    {
                        redemptionMainView.Activate();
                    }
                    if (isActive && !redemptionMainUserControl.IsFocused)
                    {
                        redemptionMainUserControl.Focus();
                    }
                }
            }
            log.LogMethodExit();
        }
        internal void UpdateActivityTimeOnAction()
        {
            log.LogMethodEntry();
            lastActivityTime = DateTime.Now;
            log.LogMethodExit();
        }
        internal void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                Window window = sender as Window;
                if (redemptionMainView != null && window.Owner != redemptionMainView)
                {
                    window.Owner = redemptionMainView;
                }
                window.ShowInTaskbar = false;
                if (redemptionMainUserControl == null)
                {
                    return;
                }
                if (redemptionMainUserControl.IsVisible)
                {
                    Point point = redemptionMainUserControl.PointToScreen(new Point());
                    window.Left = point.X;
                    window.Top = point.Y;
                }
                if (window is NumberKeyboardView)
                {
                    if (window.Width > redemptionMainUserControl.ActualWidth)
                    {
                        window.Width = redemptionMainUserControl.ActualWidth - 2;
                    }
                    if (window.Height > redemptionMainUserControl.ActualHeight)
                    {
                        window.Height = redemptionMainUserControl.ActualHeight - 2;
                    }
                }
                else
                {
                    window.Width = redemptionMainUserControl.ActualWidth - 2;
                    window.Height = redemptionMainUserControl.ActualHeight - GetFooterHeight();
                }
            });
            log.LogMethodExit();
        }
        private void OnAddCliked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithFooter(() =>
            {
                if (parameter != null)
                {
                    RedemptionMainUserControl redemptionMainUserControl = parameter as RedemptionMainUserControl;
                    if (redemptionMainUserControl != null)
                    {
                        redemptionMainUserControl.RaiseAddButtonClickedEvent();
                    }
                }
            });
            log.LogMethodExit();
        }
        private void OnRemoveClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithFooter(() =>
            {
                newOrCloseScreen = 'C';
                if (ShowDiscardConfirmation() && parameter != null)
                {
                    RedemptionMainUserControl redemptionMainUserControl = parameter as RedemptionMainUserControl;
                    RaiseRemoveClickedEvent(redemptionMainUserControl);
                }
            });
            log.LogMethodExit();
        }
        internal void CloseChildWindows()
        {
            log.LogMethodEntry(redemptionMainUserControl);
            ExecuteActionWithFooter(() =>
            {
                CloseKeyboarWindow();
                CloseOwnedWindows(itemInfoPopUpView);
                CloseOwnedWindows(dataEntryView);
                CloseOwnedWindows(scanView);
                CloseOwnedWindows(scanPopupView);
                CloseOwnedWindows(messagePopupView);
                CloseOwnedWindows(reverseView);
                CloseOwnedWindows(updateView);
                CloseOwnedWindows(allocationView);
                CloseOwnedWindows(userView);
                CloseOwnedWindows(enterTicketView);
                CloseOwnedWindows(managerView);
                CloseOwnedWindows(numberKeyboardView);
            });
            log.LogMethodExit();
        }

        private void CloseOwnedWindows(Window window)
        {
            ExecuteActionWithFooter(() =>
            {
                if (window != null)
                {
                    if (window.OwnedWindows != null)
                    {
                        foreach (Window childWindow in window.OwnedWindows)
                        {
                            CloseOwnedWindows(childWindow);
                        }
                    }
                    if (window == userView)
                    {
                        AuthenticateUserVM userVM = window.DataContext as AuthenticateUserVM;
                        if (userVM.CardReader != null)
                        {
                            userVM.CardReader.UnRegister();
                        }
                    }
                    if (window == managerView)
                    {
                        AuthenticateManagerVM managerVM = window.DataContext as AuthenticateManagerVM;
                        if (managerVM.CardReader != null)
                        {
                            managerVM.CardReader.UnRegister();
                        }
                    }
                    if (window.DataContext != null)
                    {
                        window.DataContext = null;
                    }
                    window.Close();
                }
            });
        }
        private void RaiseRemoveClickedEvent(RedemptionMainUserControl redemptionMainUserControl)
        {
            log.LogMethodEntry(redemptionMainUserControl);
            ExecuteActionWithFooter(() =>
            {
                if (redemptionMainUserControl != null)
                {
                    redemptionMainUserControl.RaiseRemoveButtonClickedEvent();
                }
            });
            log.LogMethodExit();
        }
        private void OnFooterMessageClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithFooter(() =>
            {
                if (FooterVM != null && FooterVM.MessagePopupView != null)
                {
                    OnWindowLoaded(FooterVM.MessagePopupView, null);
                    FooterVM.MessagePopupView.PreviewMouseDown += UpdateActivityTimeOnMouseOrKeyBoardAction;
                }
            });
            log.LogMethodExit();
        }
        internal void OnFooterSideBarButtonClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithFooter(() =>
            {
                if (!multiUserMultiscreen && !singleUserMultiscreen
                       && LeftPaneVM != null && LeftPaneVM.AddButtonVisiblity)
                {
                    LeftPaneVM.AddButtonVisiblity = false;
                }
                if (this.multiScreenMode)
                {
                    if (LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption && RedemptionUserControlVM != null)
                    {
                        RedemptionUserControlVM.SetRightSectionVMOnFooterClick();
                    }
                    else if (LeftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket && LoadTicketRedemptionUserControlVM != null)
                    {
                        LoadTicketRedemptionUserControlVM.SetRightSectionVMOnFooterClick();
                    }
                    else if (LeftPaneSelectedItem == LeftPaneSelectedItem.TurnIn && TurnInUserControlVM != null)
                    {
                        TurnInUserControlVM.SetRightSectionVMOnFooterClick();
                    }
                    else if (LeftPaneSelectedItem == LeftPaneSelectedItem.Voucher && VoucherUserControlVM != null)
                    {
                        VoucherUserControlVM.SetRightSectionVMOnFooterClick();
                    }
                }
            });
            log.LogMethodExit();
        }
        internal void RegisterDevices()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (cardReader != null)
                {
                    cardReader.Register(CardScanCompleteEventHandle);
                }
                if (barcodeReader != null)
                {
                    barcodeReader.Register(BarCodeScanCompleteEventHandle);
                }
            });
            log.LogMethodExit();
        }
        internal void UnRegisterDevices()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (cardReader != null)
                {
                    cardReader.UnRegister();
                }
                if (barcodeReader != null)
                {
                    barcodeReader.UnRegister();
                }
            });
            log.LogMethodExit();
        }
        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                lastActivityTime = DateTime.Now;
                SetFooterContent(string.Empty, MessageType.None);
                tappedCardNumber = null;
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                DeviceClass deviceClass = sender as DeviceClass;
                TagNumberView tagNumberView;
                if (tagNumberViewParser.TryParse(checkScannedEvent.Message, out tagNumberView) == false)
                {
                    string message = tagNumberViewParser.Validate(checkScannedEvent.Message);
                    this.SetFooterContent(message, MessageType.None);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }
                tappedCardNumber = tagNumberView.Value;
                //MessageBox.Show(tappedCardNumber);
                AddCardtoUI(tappedCardNumber, deviceClass.TagSiteId);
            });
            log.LogMethodExit();
        }
        private async void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            lastActivityTime = DateTime.Now;
            SetFooterContent(string.Empty, MessageType.None);
            log.Debug("Starting the barcode " + IsLoadingVisible);
            if (IsLoadingVisible)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4877), MessageType.Warning);
                log.LogMethodExit("Another API/action in progress");
                return;
            }
            DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
            string barCode = ProcessScannedBarCode(
                                checkScannedEvent.Message,
                                ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LEFT_TRIM_BARCODE", 0),
                                ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "RIGHT_TRIM_BARCODE", 0));
            //MessageBox.Show(barCode);
            if (scanPopupView != null)
            {
                scanPopupView.Close();
            }
            if (scanView != null)
            {
                scanView.Close();
            }
            await ProcessBarcode(barCode);
            log.LogMethodExit();
        }
        private string ProcessScannedBarCode(string Code, int leftTrim, int rightTrim)
        {
            log.LogMethodEntry(Code, leftTrim, rightTrim);
            try
            {
                Code = System.Text.RegularExpressions.Regex.Replace(Code, @"\W+", "");
                log.LogMethodExit((Code.Substring(leftTrim, Code.Length - rightTrim - leftTrim)));
                return (Code.Substring(leftTrim, Code.Length - rightTrim - leftTrim));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in Processing Scanned Bar Code", ex);
                log.LogMethodExit(Code);
                return Code;
            }
        }
        private void OnScanTicketMessagePopupViewClosed(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok
                    && this.LeftPaneVM != null && this.voucherUserControlVM != null)
                {
                    if (LeftPaneVM.MenuItems.Any(x => x.Equals(MessageViewContainerList.GetMessage(ExecutionContext, "Voucher"))) && EnableUnFlagVoucher)
                    {
                        LeftPaneVM.SelectedMenuItem = MessageViewContainerList.GetMessage(ExecutionContext, "Voucher");
                        if (flaggedticketReceiptDTO != null)
                        {
                            voucherUserControlVM.FlaggedTicketReceiptDTO = flaggedticketReceiptDTO;
                            flaggedticketReceiptDTO = null;
                        }
                    }
                    else
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2690), MessageType.Error);
                    }
                }
                else
                {
                    flaggedticketReceiptDTO = null;
                }
            });
            SetUserControlFocus();
            log.LogMethodExit();
        }
        private void SetHideTapCardEnterScanTicket()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                switch (leftPaneSelectedItem)
                {
                    case LeftPaneSelectedItem.Redemption:
                        {
                            if (RedemptionUserControlVM != null)
                            {
                                if (RedemptionUserControlVM.GenericTransactionListVM != null &&
                                RedemptionUserControlVM.GenericTransactionListVM.ItemCollection != null
                                && RedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Count == 0)
                                {
                                    RedemptionUserControlVM.TransactionID = "RO-";
                                }
                                RedemptionUserControlVM.StayInTransactionMode = true;
                            }
                        }
                        break;
                    case LeftPaneSelectedItem.TurnIn:
                        {
                            if (TurnInUserControlVM != null)
                            {
                                if (TurnInUserControlVM.GenericTransactionListVM != null &&
                                TurnInUserControlVM.GenericTransactionListVM.ItemCollection != null
                                && TurnInUserControlVM.GenericTransactionListVM.ItemCollection.Count == 0)
                                {
                                    TurnInUserControlVM.TransactionID = "RO-";
                                }
                                TurnInUserControlVM.StayInTransactionMode = true;
                            }
                        }
                        break;
                    case LeftPaneSelectedItem.LoadTicket:
                        {
                            if (LoadTicketRedemptionUserControlVM != null)
                            {
                                if (LoadTicketRedemptionUserControlVM.GenericTransactionListVM != null &&
                                LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ItemCollection != null
                                && LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Count == 0)
                                {
                                    LoadTicketRedemptionUserControlVM.TransactionID = "RO-";
                                    LoadTicketRedemptionUserControlVM.LoadTotatlTicketCount = 0;
                                }
                                LoadTicketRedemptionUserControlVM.StayInTransactionMode = true;
                            }
                        }
                        break;
                }
            });
            log.LogMethodExit();
        }
        internal async Task AddTickettoUI(string ticketNumber)
        {
            log.LogMethodEntry(ticketNumber);
            try
            {
                IsLoadingVisible = true;
                if (redemptionDTO != null && redemptionDTO.RedemptionStatus != null && redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString())
                {
                    redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.NEW.ToString();
                }
                isTicket = false;
                bool isTicketFlagged = false;
                if (!string.IsNullOrEmpty(ticketNumber))
                {
                    if (redemptionDTO != null && redemptionDTO.TicketReceiptListDTO != null)
                    {
                        if (redemptionDTO.TicketReceiptListDTO.Any(x => x.ManualTicketReceiptNo.ToUpper() == ticketNumber.ToUpper()))
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 112, ticketNumber), MessageType.Error);
                            isTicket = true;
                            IsLoadingVisible = false;
                            return;
                        }
                    }
                    try
                    {
                        int balancetickets;
                        ITicketReceiptUseCases iticketreceiptusecases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(ExecutionContext);
                        balancetickets = await iticketreceiptusecases.ValidateTicketReceipts(ticketNumber, null);
                        if (balancetickets > 0 && redemptionDTO != null)
                        {
                            isTicket = true;
                            if (redemptionDTO.TicketReceiptListDTO == null)
                            {
                                redemptionDTO.TicketReceiptListDTO = new List<TicketReceiptDTO>();
                            }
                            TicketReceiptDTO ticketReceiptDTO = new TicketReceiptDTO(-1, -1, ticketNumber, ExecutionContext.GetSiteId(), null, false, -1, balancetickets,
                                                                                  balancetickets, ExecutionContext.GetUserId(), DateTime.Now, false, -1, DateTime.Now, ExecutionContext.GetUserId(), DateTime.Now);
                            redemptionDTO.TicketReceiptListDTO.Add(ticketReceiptDTO);
                            redemptionDTO.ReceiptTickets = ((redemptionDTO.ReceiptTickets == null) ? 0 : redemptionDTO.ReceiptTickets) + balancetickets;
                            if (this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket && LoadTicketRedemptionUserControlVM != null
                                && LoadTicketRedemptionUserControlVM.GenericTransactionListVM != null &&
                                LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ItemCollection != null)
                            {
                                if (LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Count == 0)
                                {
                                    LoadTicketRedemptionUserControlVM.TransactionID = "RO-";
                                }
                                LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Add(
                                    new GenericTransactionListItem(LoadTicketRedemptionUserControlVM.ExecutionContext)
                                    {
                                        RedemptionRightSectionItemType = GenericTransactionListItemType.Ticket,
                                        TicketNo = ticketReceiptDTO.ManualTicketReceiptNo,
                                        Ticket = ticketReceiptDTO.BalanceTickets,
                                        //IsEnabled = false
                                    });
                                LoadTicketRedemptionUserControlVM.LoadTotatlTicketCount = this.GetLoadTicketTotalCount();
                                LoadTicketRedemptionUserControlVM.HideContentArea();
                            }
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2909, ticketReceiptDTO.ManualTicketReceiptNo), MessageType.Info);
                            if (this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption && RedemptionUserControlVM != null)
                            {
                                RedemptionUserControlVM.UpdateTicketValues();
                                RedemptionUserControlVM.HideContentArea();
                            }
                            SetHeaderCustomerBalanceInfo(null, this.GetBalanceTickets());
                            SetHideTapCardEnterScanTicket();
                        }
                    }
                    catch (Exception ex)
                    {
                        SetFooterContent(ex.Message, MessageType.Error);
                        if (ex.Message == MessageViewContainerList.GetMessage(ExecutionContext, 2321, null))
                        {
                            isTicket = false;
                            IsLoadingVisible = false;
                            return;
                        }
                        if (ex.Message == "Ticket receipt is flagged\r\n")
                        {
                            isTicket = true;
                            isTicketFlagged = true;
                        }
                    }
                    if (isTicketFlagged)
                    {
                        if (redemptionUserControlVM != null)
                        {
                            string message = string.Empty;
                            List<TicketReceiptDTO> ticketReceiptDTOList = await redemptionUserControlVM.GetVouchers(ticketNumber, null, null, null, null, null, true);
                            if (ticketReceiptDTOList != null && ticketReceiptDTOList.Count > 0)
                            {
                                flaggedticketReceiptDTO = ticketReceiptDTOList.FirstOrDefault();
                                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                                List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>> applicationRemarksSearchParams = new List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>>();
                                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.ACTIVE_FLAG, "1"));
                                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SITE_ID, ExecutionContext.SiteId.ToString()));
                                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_NAME, "ManualTicketReceipts"));
                                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_GUID, flaggedticketReceiptDTO.Guid));
                                List<ApplicationRemarksDTO> applicationRemarksDTOList = await redemptionUseCases.GetApplicationRemarks(applicationRemarksSearchParams);
                                message = MessageViewContainerList.GetMessage(ExecutionContext, 1395, ": " + ((applicationRemarksDTOList != null && applicationRemarksDTOList.Count > 0) ? applicationRemarksDTOList[0].Remarks : " " + MessageViewContainerList.GetMessage(ExecutionContext, "unknown") + "."));
                            }
                            OpenGenericMessagePopupView(
                        MessageViewContainerList.GetMessage(ExecutionContext, "Suspect"),
                        string.Empty,
                        message,
                        MessageViewContainerList.GetMessage(this.ExecutionContext, "CONFIRM", null),
                        MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                        MessageButtonsType.OkCancel);
                            if (this.MessagePopupView != null)
                            {
                                this.MessagePopupView.Closed += OnScanTicketMessagePopupViewClosed;
                                this.MessagePopupView.Show();
                            }
                        }
                    }
                }
                else
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2910), MessageType.Info);
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
        internal void ShowFlagOrEnterTicketView(bool flagTicket = false)
        {
            log.LogMethodEntry(flagTicket);
            ExecuteActionWithFooter(() =>
            {
                this.FlagOrEnterTicketView = new GenericDataEntryView();
                this.FlagOrEnterTicketView.Loaded += OnWindowLoaded;
                this.FlagOrEnterTicketView.PreviewMouseDown += this.UpdateActivityTimeOnMouseOrKeyBoardAction;
                this.FlagOrEnterTicketView.PreviewKeyDown += this.UpdateActivityTimeOnMouseOrKeyBoardAction;
                if (FlagOrEnterTicketView.KeyBoardHelper != null)
                {
                    this.SetKeyBoardHelperColorCode();
                    FlagOrEnterTicketView.KeyBoardHelper.KeypadMouseDownEvent -= redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                    FlagOrEnterTicketView.KeyBoardHelper.KeypadMouseDownEvent += redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                }
                string heading = MessageViewContainerList.GetMessage(ExecutionContext, "ENTER TICKET NUMBER");
                string defaultValue = MessageViewContainerList.GetMessage(ExecutionContext, "Enter Ticket No..");
                string subheading = string.Empty;
                bool mandatory = false;
                if (flagTicket)
                {
                    heading = MessageViewContainerList.GetMessage(ExecutionContext, "ENTER REMARKS");
                    defaultValue = MessageViewContainerList.GetMessage(ExecutionContext, "Enter Remaks..");
                    mandatory = true;
                    FlagOrEnterTicketView.Closed += OnFlagDataEntryViewClosed;
                    subheading = MessageViewContainerList.GetMessage(ExecutionContext, "Remarks");
                }
                else
                {
                    FlagOrEnterTicketView.Closed += OnEnterTicketViewClosed;
                }
                GenericDataEntryVM genericDataEntryVM = new GenericDataEntryVM(ExecutionContext)
                {
                    Heading = heading,
                    DataEntryCollections = new ObservableCollection<DataEntryElement>()
                                            {
                                                new DataEntryElement()
                                                {
                                                    Type = DataEntryType.TextBox,
                                                    DefaultValue = defaultValue,
                                                    IsMandatory=mandatory,
                                                    Heading=subheading
                                                }
                                            },
                    OkButtonContent = MessageViewContainerList.GetMessage(ExecutionContext, "CONFIRM"),
                    IsKeyboardVisible = true
                };
                this.FlagOrEnterTicketView.DataContext = genericDataEntryVM;
                this.FlagOrEnterTicketView.Show();
            });
            log.LogMethodExit();
        }
        private async Task ProcessBarcode(string barCode)
        {
            log.LogMethodEntry(barCode);
            try
            {

                if (IsRedemptionDelivered())
                {
                    ClearCompletedRedemption();
                }
                if (userView == null)
                {
                    newOrCloseScreen = 'N';
                    LastActivityTime = DateTime.Now;
                    scannedBarCode = barCode;
                    isTicket = false;
                    switch (barCode.ToUpper())
                    {
                        case "NEWRD":
                            if (RedemptionUserControlVM != null)
                            {
                                RedemptionUserControlVM.OnResetClicked(redemptionMainUserControl);
                            }
                            return;
                        case "SCTKT":
                            if (RedemptionUserControlVM != null)
                            {
                                ScanTicketGiftMode = 'T';
                                lastScannedType = ScanTicketGiftMode;
                                switch (leftPaneSelectedItem)
                                {
                                    case LeftPaneSelectedItem.Redemption:
                                        {
                                            if (RedemptionUserControlVM != null)
                                            {
                                                RedemptionUserControlVM.ScanClicked(true, GenericScanPopupVM.PopupType.SCANTICKET);
                                            }
                                        }
                                        break;
                                    case LeftPaneSelectedItem.LoadTicket:
                                        {
                                            if (LoadTicketRedemptionUserControlVM != null)
                                            {
                                                LoadTicketRedemptionUserControlVM.ScanClicked(true, GenericScanPopupVM.PopupType.SCANTICKET);
                                            }
                                        }
                                        break;
                                }
                            }
                            return;
                        case "SCGFT":
                            if (RedemptionUserControlVM != null)
                            {
                                ScanTicketGiftMode = 'G';
                                lastScannedType = ScanTicketGiftMode;
                                RedemptionUserControlVM.ScanClicked(true, GenericScanPopupVM.PopupType.SCANGIFT);
                            }
                            return;
                        case "MNLTK":
                            if (this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption && RedemptionUserControlVM != null)
                            {
                                RedemptionUserControlVM.OnAddTicketClicked();
                            }
                            if (this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket && LoadTicketRedemptionUserControlVM != null)
                            {
                                LoadTicketRedemptionUserControlVM.OnAddTicketClicked();
                            }
                            if (this.allocationView != null)
                            {
                                RedemptionTicketAllocationVM ticketAllocationVM = allocationView.DataContext as RedemptionTicketAllocationVM;
                                if (ticketAllocationVM != null)
                                {
                                    if (EnableManualTicket)
                                    {
                                        CustomToggleButtonItem toggleButtonItem = ticketAllocationVM.GenericToggleButtonsVM.ToggleButtonItems.FirstOrDefault(d =>
                                               d.DisplayTags[0].Text.ToLower() == MessageViewContainerList.GetMessage(this.ExecutionContext, "TICKETS", null).ToLower());
                                        if (toggleButtonItem != null)
                                        {
                                            if (!toggleButtonItem.IsChecked)
                                            {
                                                toggleButtonItem.IsChecked = true;
                                            }
                                            else
                                            {
                                                ticketAllocationVM.SetTicketTypeValues();
                                            }
                                        }
                                        if (this.allocationView.KeyBoardHelper != null)
                                        {
                                            this.allocationView.KeyBoardHelper.NumberKeyboardView.Show();
                                        }
                                    }
                                }
                            }
                            return;
                        case "CHQTY":
                            {
                                NumberKeyboardView numberKeyboardView = GetNumberPadView(redemptionMainView, barCode: barCode);
                                if(numberKeyboardView != null)
                                {
                                    numberKeyboardView.Closed += NumberKeyboardView_Closed;
                                    numberKeyboardView.Show();
                                }
                            }
                            return;
                        case "SAVER":
                            if (RedemptionUserControlVM != null)
                            {
                                await RedemptionUserControlVM.CompleteClicked();
                            }
                            return;
                        case "SPNDR":
                            if (RedemptionUserControlVM != null)
                            {
                                await RedemptionUserControlVM.SuspendClicked();
                            }
                            return;
                        case "PRINT":
                            if (RedemptionUserControlVM != null)
                            {
                                await RedemptionUserControlVM.PrintRedemptionClicked();
                            }
                            return;
                        case "TRNIN":
                            if (this.LeftPaneVM != null)
                            {
                                if (this.LeftPaneVM.MenuItems.Any(x => x.Equals(MessageViewContainerList.GetMessage(ExecutionContext, "Turn In"))))
                                {
                                    this.LeftPaneVM.SelectedMenuItem = MessageViewContainerList.GetMessage(ExecutionContext, "Turn In");
                                }
                            }
                            return;
                        case "SERCH":
                            if (this.LeftPaneVM.SelectedMenuItem == MessageViewContainerList.GetMessage(ExecutionContext, "Redemption"))
                            {
                                if (RedemptionUserControlVM != null)
                                {
                                    await RedemptionUserControlVM.PerformSearch();
                                }
                            }
                            if (this.LeftPaneVM.SelectedMenuItem == MessageViewContainerList.GetMessage(this.ExecutionContext, "Load Ticket", null))
                            {
                                if (LoadTicketRedemptionUserControlVM != null)
                                {
                                    await LoadTicketRedemptionUserControlVM.PerformSearch();
                                }
                            }
                            if (this.LeftPaneVM.SelectedMenuItem == MessageViewContainerList.GetMessage(ExecutionContext, "Turn In"))
                            {
                                if (TurnInUserControlVM != null)
                                {
                                    await TurnInUserControlVM.PerformSearch();
                                }
                            }
                            if (this.LeftPaneVM.SelectedMenuItem == MessageViewContainerList.GetMessage(ExecutionContext, "Voucher"))
                            {
                                if (VoucherUserControlVM != null)
                                {
                                    await VoucherUserControlVM.PerformSearch();
                                }
                            }
                            return;
                        case "LDTKT":
                            if (LoadTicketRedemptionUserControlVM != null && LoadTicketRedemptionUserControlVM.IsLoadTicket)
                            {
                                LoadTicketRedemptionUserControlVM.LoadToCardClicked();
                            }
                            if (RedemptionUserControlVM != null )
                            {
                                RedemptionUserControlVM.LoadToCardClicked();
                            }
                            return;
                        case "FLGTR":
                            if (this.LeftPaneVM != null)
                            {
                                if (this.LeftPaneVM.MenuItems.Any(x => x.Equals(MessageViewContainerList.GetMessage(ExecutionContext, "Voucher"))))
                                {
                                    this.LeftPaneVM.SelectedMenuItem = MessageViewContainerList.GetMessage(ExecutionContext, "Voucher");
                                }
                            }
                            return;
                        case "OKKEY":
                            if (this.allocationView != null)
                            {
                                GenericMessagePopupView genericMessagePopupView = Application.Current.Windows.Cast<Window>().FirstOrDefault(x => x.Owner == allocationView) as GenericMessagePopupView;
                                if (genericMessagePopupView != null)
                                {
                                    if (genericMessagePopupView.btnOk != null)
                                    {
                                        genericMessagePopupView.btnOk.Command.Execute(genericMessagePopupView.btnOk.CommandParameter);
                                    }
                                }
                                if (this.allocationView.KeyBoardHelper != null
                                    && this.allocationView.KeyBoardHelper.NumberKeyboardView != null)
                                {
                                    this.allocationView.KeyBoardHelper.NumberKeyboardView.Close();
                                }
                            }
                            if (redemptionMainUserControlVM.DataEntryView != null && redemptionMainUserControlVM.DataEntryView.KeyBoardHelper.NumberKeyboardView != null)
                            {
                                redemptionMainUserControlVM.DataEntryView.KeyBoardHelper.NumberKeyboardView.Close();
                            }
                            CloseNumberPad();
                            if (messagePopupView!=null)
                            {
                                bool isMessageWindowOpen = Application.Current.Windows.Cast<Window>().Any(x => x == messagePopupView);
                                if (isMessageWindowOpen)
                                {
                                    if (messagePopupView.btnOk != null)
                                    {
                                        messagePopupView.btnOk.Command.Execute(messagePopupView.btnOk.CommandParameter);
                                    }
                                }
                            }
                            if (managerView != null)
                            {
                                bool isManagerWindowOpen = Application.Current.Windows.Cast<Window>().Any(x => x == managerView);
                                if (isManagerWindowOpen && managerView.txtPassword != null)
                                {
                                    managerView.KeyUp -= managerView.OnKeyUp;
                                    managerView.KeyUp += ManagerView_KeyUp;
                                }
                            }
                            return;
                        case "CANCL":
                            if (this.allocationView != null)
                            {
                                if (this.allocationView != null)
                                {
                                    int count = Application.Current.Windows.Cast<Window>().Count(x => x.Owner == allocationView);
                                    if (count == 0)
                                    {
                                        this.allocationView.Close();
                                    }
                                    GenericMessagePopupView genericMessagePopupView = Application.Current.Windows.Cast<Window>().FirstOrDefault(x => x.Owner == allocationView) as GenericMessagePopupView;
                                    if (genericMessagePopupView != null)
                                    {
                                        if (genericMessagePopupView.btnCancel != null)
                                        {
                                            genericMessagePopupView.btnCancel.Command.Execute(genericMessagePopupView.btnCancel.CommandParameter);
                                        }
                                    }
                                    if (this.allocationView != null && this.allocationView.KeyBoardHelper != null
                                        && this.allocationView.KeyBoardHelper.NumberKeyboardView != null)
                                    {
                                        if (this.allocationView.KeyBoardHelper.NumberKeyboardView.DataContext != null)
                                        {
                                            NumberKeyboardVM numberTicketKeyboardVM = this.allocationView.KeyBoardHelper.NumberKeyboardView.DataContext as NumberKeyboardVM;
                                            if (numberTicketKeyboardVM != null)
                                            {
                                                numberTicketKeyboardVM.NumberText = string.Empty;
                                            }
                                        }
                                        this.allocationView.KeyBoardHelper.NumberKeyboardView.Close();
                                    }
                                }
                            }
                            if (redemptionMainUserControlVM.DataEntryView != null && redemptionMainUserControlVM.DataEntryView.KeyBoardHelper.NumberKeyboardView != null)
                            {
                                NumberKeyboardVM numberTicketKeyboardVM = redemptionMainUserControlVM.DataEntryView.KeyBoardHelper.NumberKeyboardView.DataContext as NumberKeyboardVM;
                                numberTicketKeyboardVM.NumberText = string.Empty;
                                redemptionMainUserControlVM.DataEntryView.KeyBoardHelper.NumberKeyboardView.Close();
                            }
                            if (messagePopupView != null)
                            {
                                bool isMessageWindowOpen = Application.Current.Windows.Cast<Window>().Any(x => x == messagePopupView);
                                if (isMessageWindowOpen)
                                {
                                    if (messagePopupView.btnCancel != null)
                                    {
                                        messagePopupView.btnCancel.Command.Execute(messagePopupView.btnCancel.CommandParameter);
                                    }
                                }
                            }
                            if (managerView != null)
                            {
                                bool isManagerWindowOpen = Application.Current.Windows.Cast<Window>().Any(x => x == managerView);
                                if (isManagerWindowOpen && managerView.txtPassword != null)
                                {
                                    managerView.KeyUp -= managerView.OnKeyUp;
                                    managerView.KeyUp += ManagerView_KeyUp;
                                }
                            }
                            return;
                        case "0DGIT":
                        case "1DGIT":
                        case "2DGIT":
                        case "3DGIT":
                        case "4DGIT":
                        case "5DGIT":
                        case "6DGIT":
                        case "7DGIT":
                        case "8DGIT":
                        case "9DGIT":
                            {
                                if (this.allocationView != null && this.allocationView.KeyBoardHelper != null
                                    && this.allocationView.KeyBoardHelper.NumberKeyboardView != null
                                    && this.allocationView.KeyBoardHelper.NumberKeyboardView.DataContext != null)
                                {
                                    NumberKeyboardVM numberTicketKeyboardVM = this.allocationView.KeyBoardHelper.NumberKeyboardView.DataContext as NumberKeyboardVM;
                                    if (numberTicketKeyboardVM != null)
                                    {
                                        numberTicketKeyboardVM.NumberText += barCode[0].ToString();
                                    }
                                }
                                if (redemptionMainUserControlVM.DataEntryView != null && redemptionMainUserControlVM.DataEntryView.KeyBoardHelper.NumberKeyboardView != null)
                                {
                                    NumberKeyboardVM numberTicketKeyboardVM = redemptionMainUserControlVM.DataEntryView.KeyBoardHelper.NumberKeyboardView.DataContext as NumberKeyboardVM;
                                    numberTicketKeyboardVM.NumberText += barCode[0].ToString();
                                    //redemptionMainUserControlVM.DataEntryView.KeyBoardHelper.NumberKeyboardView.Close();
                                }
                                if (numberKeyboardView != null)
                                {
                                    NumberKeyboardVM numberQtyKeyboardVM = numberKeyboardView.DataContext as NumberKeyboardVM;
                                    numberQtyKeyboardVM.NumberText += barCode[0].ToString();
                                    // numberKeyboardView.Close();
                                }
                            }
                            return;
                        case "ADDUSER":
                            if (redemptionMainView != null && mainVM != null && !singleUserMultiscreen)
                            {
                                mainVM.OnAddNewUserClicked(redemptionMainView);
                            }
                            else if (singleUserMultiscreen)
                            {
                                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2692), MessageType.Error);
                            }
                            return;
                        case "ADDSCRN":
                            if (singleUserMultiscreen || multiUserMultiscreen)
                            {
                                if (!reloginInitiated)
                                {
                                    OnAddCliked(redemptionMainUserControl);
                                }
                            }
                            else
                            {
                                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2684), MessageType.Error);
                            }
                            return;
                        case "CLOSESCRN":
                            newOrCloseScreen = 'C';
                            OnRemoveClicked(redemptionMainUserControl);
                            return;
                        case "SCREEN1":
                            SetScreen(1);
                            return;
                        case "SCREEN2":
                            SetScreen(2);
                            return;
                        case "SCREEN3":
                            SetScreen(3);
                            return;
                        case "SCREEN4":
                            SetScreen(4);
                            return;
                        case "SCREEN5":
                            SetScreen(5);
                            return;
                        case "SCREEN6":
                            SetScreen(6);
                            return;
                        case "SCREEN7":
                            SetScreen(7);
                            return;
                        case "SCREEN8":
                            SetScreen(8);
                            return;
                    }
                    LastActivityTime = DateTime.Now;
                    if (barCode.ToUpper().StartsWith("RDSPND"))
                    {
                        if (redemptionDTO.RedemptionGiftsListDTO.Count > 0 || redemptionDTO.TicketReceiptListDTO.Count > 0)
                        {
                            this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2681), MessageType.Warning);
                            log.Warn("(" + barCode + ") as REDEMPTION IN PROGRESS. CLEAR OR SAVE TO PROCEED");
                            return;
                        }
                        if (RedemptionUserControlVM != null)
                        {
                            IsLoadingVisible = true;
                            RedemptionUserControlVM.RedemptionDTOList = await RedemptionUserControlVM.GetRedemptions(barCode.ToUpper().Replace("RDSPND", ""), null, null,
                              "SUSPENDED", null, null, null);
                            if (RedemptionUserControlVM.RedemptionDTOList.Any())
                            {
                                redemptionDTO = RedemptionUserControlVM.RedemptionDTOList.FirstOrDefault();
                                if (RetreivedBackupDTO == null)
                                {
                                    RetreivedBackupDTO = new RedemptionDTO(redemptionDTO.RedemptionId, redemptionDTO.PrimaryCardNumber, redemptionDTO.ManualTickets,
                                        redemptionDTO.ETickets, redemptionDTO.RedeemedDate, redemptionDTO.CardId, redemptionDTO.OrigRedemptionId, redemptionDTO.Remarks,
                                        redemptionDTO.GraceTickets, redemptionDTO.ReceiptTickets, redemptionDTO.CurrencyTickets, redemptionDTO.LastUpdatedBy,
                                        redemptionDTO.SiteId, redemptionDTO.Guid, redemptionDTO.SynchStatus, redemptionDTO.MasterEntityId, redemptionDTO.Source,
                                        redemptionDTO.RedemptionOrderNo, redemptionDTO.LastUpdateDate, redemptionDTO.OrderCompletedDate, redemptionDTO.OrderDeliveredDate,
                                        redemptionDTO.RedemptionStatus, redemptionDTO.CreationDate, redemptionDTO.CreatedBy, redemptionDTO.RedemptionGiftsListDTO.ToList(),
                                        redemptionDTO.RedemptionCardsListDTO.ToList(), redemptionDTO.TicketReceiptListDTO.ToList(), redemptionDTO.RedemptionTicketAllocationListDTO.ToList(),
                                        redemptionDTO.CustomerName, redemptionDTO.POSMachineId, redemptionDTO.CustomerId, redemptionDTO.PosMachineName, redemptionDTO.OriginalRedemptionOrderNo);
                                }
                                RedemptionUserControlVM.GenericToggleButtonsVM.ToggleButtonItems[0].IsChecked = true;
                                RedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Clear();
                                string transactionListHeading = redemptionDTO.RedemptionOrderNo;
                                if (string.IsNullOrEmpty(redemptionDTO.RedemptionOrderNo))
                                {
                                    transactionListHeading = MessageViewContainerList.GetMessage(ExecutionContext, "RO-");
                                }
                                RedemptionUserControlVM.SetTransactionListVM(transactionListHeading);
                                foreach (RedemptionGiftsDTO giftsDTO in redemptionDTO.RedemptionGiftsListDTO)
                                {
                                    if (giftsDTO != null)
                                    {
                                        GenericTransactionListItem redemptionRightSectionItem = RedemptionUserControlVM.GenericTransactionListVM.ItemCollection.FirstOrDefault(s => s.Key == mainVM.GetProductContainerDTOList(ExecutionContext).FirstOrDefault(x => x.InventoryItemContainerDTO.ProductId == giftsDTO.ProductId).ProductId);
                                        if (redemptionRightSectionItem != null)
                                        {
                                            redemptionRightSectionItem.Count += 1;
                                        }
                                        else
                                        {
                                            RedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Add(new GenericTransactionListItem(ExecutionContext)
                                            {
                                                ProductName = mainVM.GetProductContainerDTOList(ExecutionContext).FirstOrDefault(x => x.InventoryItemContainerDTO.ProductId == giftsDTO.ProductId).ProductName,
                                                Ticket = giftsDTO.Tickets != null ? (int)giftsDTO.Tickets : 0,
                                                Count = giftsDTO.ProductQuantity,
                                                TicketDisplayText = MessageViewContainerList.GetMessage(ExecutionContext, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets")),
                                                RedemptionRightSectionItemType = GenericTransactionListItemType.Item,
                                                Key = mainVM.GetProductContainerDTOList(ExecutionContext).FirstOrDefault(x => x.InventoryItemContainerDTO.ProductId == giftsDTO.ProductId).ProductId
                                            });
                                        }
                                    }
                                }
                                RedemptionUserControlVM.StayInTransactionMode = true;
                                bool result = await RecomputeSuspendedRedemption();
                                RedemptionUserControlVM.SuspendedRedemptions.Remove(RedemptionUserControlVM.SuspendedRedemptions.FirstOrDefault(x => x.RedemptionId == redemptionDTO.RedemptionId));
                                redemptionUserControlVM.SetOtherRedemptionList(RedemptionUserControlVM.ActionType.Retrieve);
                                redemptionUserControlVM.SetCompletedSuspenedCount(RedemptionsType.Suspended, redemptionUserControlVM.SuspendedRedemptions.Where(r => (r.CreatedBy.ToLower() == this.ExecutionContext.UserId.ToLower())
                                || (r.LastUpdatedBy.ToLower() == this.ExecutionContext.UserId.ToLower())).Count());
                                redemptionUserControlVM.UpdateTicketValues();
                                SetHeaderCustomerBalanceInfo((!string.IsNullOrWhiteSpace(redemptionDTO.CustomerName) ? redemptionDTO.CustomerName : redemptionDTO.PrimaryCardNumber), GetBalanceTickets());
                                IsLoadingVisible = false;
                                return;
                            }
                            else
                            {
                                SetFooterContent(barCode.ToUpper().Replace("RDSPND", "") + "-" + MessageViewContainerList.GetMessage(ExecutionContext, 1392), MessageType.Error);
                                log.Info("Ends-retrieveSuspended - for redemption id" + barCode.ToUpper().Replace("RDSPND", "") + " as Unable to retrieve suspended redemption");
                                IsLoadingVisible = false;
                                return;
                            }
                        }
                        log.Info("(" + barCode + ") -RDSPND- ");
                    }
                    if (this.LeftPaneSelectedItem == LeftPaneSelectedItem.Voucher
                    && VoucherUserControlVM != null
                    )
                    {
                        VoucherUserControlVM.ShowSearchCloseIcon = false;
                        VoucherUserControlVM.SearchedReceiptNo = barCode;
                        await VoucherUserControlVM.PerformSearch();
                        return;
                    }
                    if (this.LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption
    && RedemptionUserControlVM != null && RedemptionUserControlVM.RedemptionsType == RedemptionsType.Completed
    )
                    {
                        RedemptionUserControlVM.ShowSearchCloseIcon = false;
                        RedemptionUserControlVM.SearchedBalanceTicketToOrPrdCodeDescBarcode = barCode;
                        await RedemptionUserControlVM.PerformSearch();
                        return;
                    }
                    ProductsContainerDTO p;
                    switch (scanTicketGiftmode)
                    {
                        case 'T':
                            if (scanPopupView != null)
                            {
                                scanPopupView.Close();
                            }
                            if (scanView != null)
                            {
                                scanView.Close();
                            }
                            AddCurrency(null, null, barCode, null);
                            if (isTicket)
                            {
                                await AddTickettoUI(barCode);
                            }
                            scanTicketGiftmode = (char)0;
                            break;
                        case 'G':
                            if (scanPopupView != null)
                            {
                                scanPopupView.Close();
                            }
                            if (scanView != null)
                            {
                                scanView.Close();
                            }
                            try
                            {
                                p = ProductViewContainerList.GetProductsContainerDTOByBarCode(ExecutionContext, ManualProductType.REDEEMABLE.ToString(), barCode);
                            }
                            catch
                            {
                                p = null;
                            }
                            if (p != null)
                            {
                                bool validProduct = mainVM.ValidateProductInclusion(ExecutionContext, p);
                                if (validProduct)
                                {
                                    await AddProduct(p);
                                }
                                else
                                {
                                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 111), MessageType.Warning);
                                }
                            }
                            else
                            {
                                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 111), MessageType.Warning);
                            }
                            scanTicketGiftmode = (char)0;
                            break;
                        case (char)0:
                            {
                                try
                                {
                                    p = ProductViewContainerList.GetProductsContainerDTOByBarCode(ExecutionContext, ManualProductType.REDEEMABLE.ToString(), barCode);
                                }
                                catch
                                {
                                    p = null;
                                }
                                if (p != null)
                                {
                                    bool validProduct = mainVM.ValidateProductInclusion(ExecutionContext, p);
                                    if (validProduct)
                                    {
                                        await AddProduct(p);
                                        return;
                                    }
                                }
                                AddCurrency(null, null, barCode, null);
                                if (isTicket)
                                {
                                    await AddTickettoUI(barCode);
                                }
                            }
                            break;
                    }
                    LastActivityTime = DateTime.Now;
                }
                else
                {
                    //if (barCode == "ADDSCRN")
                    //{
                    //    OnAddCliked(redemptionMainUserControl);
                    //}
                    //else 
                    if (barCode.ToUpper() == "CLOSESCRN")
                    {
                        newOrCloseScreen = 'C';
                        foreach (RedemptionMainUserControlVM mainusercontrol in mainVM.RedemptionUserControlVMs.Where(x => x.UserName == this.UserName))
                        {
                            OnRemoveClicked(mainusercontrol);
                        }
                    }
                    else
                    {
                        this.SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 2673), MessageType.Warning);
                    }
                }
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
                ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                SetFooterContent(pax.Message.ToString(), MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again."), MessageType.Error);
            }
            finally
            {
                log.Debug("IsLoadingVisible process barcode" + IsLoadingVisible);
                IsLoadingVisible = false;
            }
            log.LogMethodExit();
        }
        private void ManagerView_KeyUp(object sender, KeyEventArgs e)
        {
            managerView.KeyUp -= ManagerView_KeyUp;
            managerView.KeyUp += managerView.OnKeyUp;
            e.Handled = true;
        }
        internal async Task<bool> RecomputeSuspendedRedemption()
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                if (redemptionDTO != null && redemptionDTO.RedemptionGiftsListDTO != null)
                {
                    if (!ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ALLOW_TRANSACTION_ON_ZERO_STOCK", false))
                    {
                        foreach (RedemptionGiftsDTO rgifts in redemptionDTO.RedemptionGiftsListDTO)
                        {
                            ProductsContainerDTO productDTO = ProductViewContainerList.GetActiveProductsContainerDTOList(ExecutionContext,
                                   ManualProductType.REDEEMABLE.ToString()).FirstOrDefault(x => x.InventoryItemContainerDTO.ProductId == rgifts.ProductId);
                            if(productDTO != null && productDTO.InventoryItemContainerDTO != null)
                            {
                                double stock = await RedemptionUserControlVM.GetTotalStock(productDTO);
                                int productId = productDTO.InventoryItemContainerDTO.ProductId;
                                if (redemptionDTO.RedemptionGiftsListDTO.Any(x => x.ProductId == productId))
                                {
                                    if (stock < redemptionDTO.RedemptionGiftsListDTO.Where(x => x.ProductId == productId).Sum(x => x.ProductQuantity))
                                    {
                                        this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 120), MessageType.Error);
                                        log.Info("Ends-addGift(" + productDTO.InventoryItemContainerDTO.Code + ") as selected more gifts than available stock. Please crosscheck available quantity before proceeding");
                                        log.LogMethodExit();
                                        result = false;
                                    }
                                }
                            }
                        }
                    }
                }
                if (redemptionDTO != null && redemptionDTO.TicketReceiptListDTO != null)
                {
                    List<TicketReceiptDTO> validateTicketList = new List<TicketReceiptDTO>();
                    validateTicketList.AddRange(redemptionDTO.TicketReceiptListDTO); ;
                    foreach (TicketReceiptDTO tickets in validateTicketList)
                    {
                        try
                        {
                            ITicketReceiptUseCases iticketreceiptusecases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(ExecutionContext);
                            int balancetickets = await iticketreceiptusecases.ValidateTicketReceipts(tickets.ManualTicketReceiptNo, null);
                            if (balancetickets > 0)
                            {
                                result = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            SetFooterContent(ex.Message, MessageType.Error);
                            redemptionDTO.TicketReceiptListDTO.Remove(tickets);
                        }
                    }
                }
                if (redemptionDTO != null && redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
                {
                    foreach (RedemptionCardsDTO rcards in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0))
                    {
                        accountDTO = null;
                        AccountDTOCollection accountDTOCollection;
                        try
                        {
                            IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(ExecutionContext);
                            accountDTOCollection = await accountUseCases.GetAccounts(accountId: rcards.CardId, tagSiteId: ExecutionContext.GetSiteId(), buildChildRecords: true, activeRecordsOnly: true);

                            if (accountDTOCollection != null && accountDTOCollection.data != null)
                            {
                                accountDTO = accountDTOCollection.data[0];
                            }
                        }
                        catch (ValidationException vex)
                        {
                            log.Error(vex);
                            SetFooterContent(vex.Message.ToString(), MessageType.Error);
                            result = false;
                        }
                        catch (UnauthorizedException uaex)
                        {
                            log.Info("unauthroized exception while retreiving card info for suspended redemption - show relogin");
                            ShowRelogin(this.ExecutionContext.GetUserId());
                            result = false;
                        }
                        catch (ParafaitApplicationException pax)
                        {
                            log.Error(pax);
                            SetFooterContent(pax.Message.ToString(), MessageType.Error);
                            result = false;
                        }
                        catch (Exception ex)
                        {
                            string message = "Error occurred while retrieving card from server.";
                            log.Error(message, ex);
                            this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                            result = false;
                        }
                        if (accountDTO != null && accountDTO.AccountId >= 0)
                        {
                            result = await AddCardtoRedemption(rcards.RedemptionCardsId);
                        }
                    }
                }
                result = true;
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                SetFooterContent(vex.Message.ToString(), MessageType.Error);
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                SetFooterContent(pax.Message.ToString(), MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again."), MessageType.Error);
            }
            log.LogMethodExit(result);
            return result;
        }
        private void NumberKeyboardView_Closed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                if (sender != null)
                {
                    NumberKeyboardView numberkeyboardView = sender as NumberKeyboardView;
                    NumberKeyboardVM numberkeyboardVM = numberkeyboardView.DataContext as NumberKeyboardVM;
                    if (numberkeyboardVM != null && !string.IsNullOrWhiteSpace(numberkeyboardVM.NumberText))
                    {
                        int qty = Convert.ToInt32(numberkeyboardVM.NumberText);
                        if (lastScannedType == 'C')
                        {
                            this.AddCurrencyToUI(lastScannedCurrency, qty, true,lastScannedViewGrouping);
                        }
                        else if (lastScannedType == 'G')
                        {
                            this.AddProductToUI(lastScannedProduct, qty, true);
                        }
                    }
                }
            });
            SetUserControlFocus();
            log.LogMethodExit();
        }
        internal void SetHeaderCustomerBalanceInfo(string customerinfo, double balance = 0)
        {
            log.LogMethodEntry(customerinfo, balance);
            ExecuteActionWithFooter(() =>
            {
                if (mainVM != null && mainVM.RedemptionHeaderTagsVM != null && mainVM.RedemptionHeaderTagsVM.HeaderGroups != null)
                {
                    foreach (RedemptionHeaderGroup g in mainVM.RedemptionHeaderTagsVM.HeaderGroups.Where(x => x.UserName == UserName))
                    {
                        foreach (RedemptionHeaderTag r in g.RedemptionHeaderTags.Where(x => x.ScreenNumber == screenNumber))
                        {
                            if (customerinfo != null)
                            {
                                r.CardNumberText = customerinfo;
                            }
                            r.BalanceTicket = balance;
                        }
                    }
                    if (this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption && redemptionUserControlVM != null)
                    {
                        redemptionUserControlVM.UpdateTicketValues();
                    }
                }
            });
            log.LogMethodExit();
        }
        private void SetScreen(int screenNumber)
        {
            log.LogMethodEntry(screenNumber);
            ExecuteActionWithFooter(() =>
            {
                if (mainVM != null)
                {
                    if (!mainVM.RedemptionHeaderTagsVM.HeaderGroups.Any(x => x.RedemptionHeaderTags.Any(y => y.ScreenNumber == screenNumber)))
                    {
                        this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2678, screenNumber), MessageType.Warning);
                        return;
                    }
                    else
                    {
                        foreach (RedemptionHeaderGroup g in mainVM.RedemptionHeaderTagsVM.HeaderGroups)
                        {
                            foreach (RedemptionHeaderTag r in g.RedemptionHeaderTags)
                            {
                                if (g.UserName == this.UserName)
                                {
                                    if (r.ScreenNumber == screenNumber)
                                    {

                                        if (mainVM.RedemptionUserControlVMs != null && mainVM.RedemptionUserControlVMs.Any(x => x.ScreenNumber == screenNumber && x.UserName == this.UserName && this.ReloginInitiated))
                                        {
                                            return;
                                        }
                                        mainVM.SetasActiveScreen(this.UserName, screenNumber);
                                        r.IsActive = true;
                                    }
                                    else
                                    {
                                        r.IsActive = false;
                                    }
                                }
                                else if (r.ScreenNumber == screenNumber && g.UserName != redemptionMainUserControlVM.UserName)
                                {
                                    this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2679, screenNumber, redemptionMainUserControlVM.UserName), MessageType.Warning);
                                    return;
                                }
                            }
                        }
                    }
                }
            });
            log.LogMethodExit();
        }
        internal bool ShowDiscardConfirmation(bool fromLeftPaneOrReset = false)
        {
            log.LogMethodEntry();
            bool result = false;
            ExecuteActionWithFooter(() =>
            {
                if (fromLeftPaneOrReset)
                {
                    newOrCloseScreen = 'N';
                }
                if ((redemptionDTO != null && redemptionDTO.RedemptionStatus != "DELIVERED" && (redemptionDTO.TicketReceiptListDTO.Count > 0 || redemptionDTO.RedemptionGiftsListDTO.Count > 0
                || redemptionDTO.RedemptionCardsListDTO.Count > 0 || redemptionDTO.ManualTickets > 0)))
                {
                    string content = string.Empty;
                    string heading = string.Empty;
                    string subHeading = string.Empty;
                    if (accountDTO != null && this.leftPaneSelectedItem == LeftPaneSelectedItem.TurnIn)
                    {
                        isTurnInSecondCardTapped = true;
                    }
                    if ((accountDTO != null && redemptionDTO != null && this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket
                    && redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0) && redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0).Count() > 0) || isTurnInSecondCardTapped)
                    {
                        heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "DISCARD", null) + "!";
                        content = MessageViewContainerList.GetMessage(ExecutionContext, 2912, this.LeftPaneVM.SelectedMenuItem);
                    }
                    else
                    {
                        if ((redemptionDTO.TicketReceiptListDTO.Count > 0 || redemptionDTO.RedemptionGiftsListDTO.Count > 0)
                        || ((this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket || this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption) &&
                        redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyId >= 0) && redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId != null && x.CurrencyId >= 0).Count() > 0)
                        || redemptionDTO.ManualTickets > 0 || (redemptionDTO.RedemptionCardsListDTO != null &&
                        redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0) && redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0).Count() > 0))
                        {
                            heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "DISCARD", null);
                            //subHeading = MessageViewContainerList.GetMessage(this.ExecutionContext, "CLOSE SCREEN", null);
                            content = MessageViewContainerList.GetMessage(ExecutionContext, 2740, ExecutionContext.UserId);
                            if (!fromLeftPaneOrReset)
                            {
                                content += MessageViewContainerList.GetMessage(ExecutionContext, "Close Redemption") + MessageViewContainerList.GetMessage(ExecutionContext, 2693, screenNumber);
                            }
                        }
                        else
                        {
                            result = true;
                        }
                    }
                    if (!result)
                    {
                        OpenGenericMessagePopupView(heading, subHeading, content,
                                                    MessageViewContainerList.GetMessage(this.ExecutionContext, "DISCARD", null),
                                                    MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                                                    MessageButtonsType.OkCancel);
                        if (this.messagePopupView != null)
                        {
                            this.messagePopupView.Closed += OnConfirmCloseScreen;
                            this.messagePopupView.Show();
                        }
                        result = false;
                    }
                }
                else
                {
                    result = true;
                }
                log.LogMethodExit();
            });
            return result;
        }

        private async void OnConsolidateCardClosed(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null)
                {
                    if (messagePopupVM.ButtonClickType == ButtonClickType.Ok)
                    {
                        await AddCardtoRedemption();
                    }
                    else
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1435, tappedCardNumber), MessageType.Warning);
                        log.Info("Ends-addCard(User declined to consolidate " + tappedCardNumber + " with current redemption");
                    }
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                SetFooterContent(vex.Message.ToString(), MessageType.Error);
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                ShowRelogin(ExecutionContext.GetUserId());
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
            SetUserControlFocus();
            log.LogMethodExit();
        }
        private async Task<bool> AddCardtoRedemption(int? redemptionCardsId = null)
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                if (redemptionDTO != null && redemptionDTO.RedemptionStatus != null && redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString())
                {
                    redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.NEW.ToString();
                }
                recalculateproductprice = false;
                CustomerDTO customerDTO = new CustomerDTO();
                if (accountDTO != null)
                {
                    // get membership details, change for factory
                    if (accountDTO.CustomerId >= 0)
                    {
                        customerDTO = await GetCustomerInfo(accountDTO.CustomerId);
                    }
                    if (customerDTO.Id >= 0)
                    {
                        if (!CardIDcustomerIDList.ContainsKey(accountDTO.AccountId))
                        {
                            CardIDcustomerIDList.Add(accountDTO.AccountId, customerDTO.Id);
                        }
                        cardInfoText = customerDTO.ProfileDTO.FirstName + ((customerDTO.ProfileDTO.FirstName != null) ? " " : "") + customerDTO.ProfileDTO.LastName;
                    }
                    else
                    {
                        cardInfoText = accountDTO.TagNumber;
                    }
                    if (!CustomerIDcustomerInfoList.ContainsKey(accountDTO.AccountId))
                    {
                        CustomerIDcustomerInfoList.Add(accountDTO.AccountId, cardInfoText);
                    }
                    if (customerDTO.Id >= 0 && customerDTO.MembershipId >= 0)
                    {
                        MembershipIDCardIDList.Add(accountDTO.AccountId, customerDTO.MembershipId);
                        if (!membershipIDList.Any(x => x == customerDTO.MembershipId))
                        {
                            if (MembershipViewContainerList.GetMembershipContainerDTOCollection(ExecutionContext.GetSiteId(), null).MembershipContainerDTOList.Any(x => x.MembershipId == customerDTO.MembershipId && x.RedemptionDiscount > 0))
                            {
                                recalculateproductprice = true;
                            }
                            membershipIDList.Add(customerDTO.MembershipId);
                        }
                    }
                    int? totalcardtickets = (accountDTO.TicketCount == null ? 0 : accountDTO.TicketCount);
                    if (accountDTO.AccountSummaryDTO != null && accountDTO.AccountSummaryDTO.CreditPlusTickets != null)
                    {
                        totalcardtickets += Convert.ToInt32(accountDTO.AccountSummaryDTO.CreditPlusTickets.HasValue ? accountDTO.AccountSummaryDTO.CreditPlusTickets : 0);
                    }
                    if (redemptionCardsId == null)
                    {
                        RedemptionCardsDTO redemptionCardsDTO = new RedemptionCardsDTO(-1, -1,
                                                        accountDTO.TagNumber,
                                                        accountDTO.AccountId,
                                                        null
                                                        , null, null, null, null,
                                                        totalcardtickets,null);
                        if (redemptionDTO.RedemptionCardsListDTO == null)
                        {
                            redemptionDTO.RedemptionCardsListDTO = new List<RedemptionCardsDTO>();
                        }
                        redemptionDTO.RedemptionCardsListDTO.Add(redemptionCardsDTO);
                        redemptionDTO.ETickets = ((redemptionDTO.ETickets == null) ? 0 : redemptionDTO.ETickets) + ((totalcardtickets == null) ? 0 : totalcardtickets);
                    }
                    if (redemptionCardsId >= 0 && redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.RedemptionCardsId == redemptionCardsId))
                    {
                        redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.RedemptionCardsId == redemptionCardsId).TotalCardTickets = totalcardtickets;
                    }
                    if (redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0).Count() == 1)
                    {
                        redemptionDTO.CardId = redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardId >= 0).CardId;
                        redemptionDTO.PrimaryCardNumber = redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardId >= 0).CardNumber;
                        if (leftPaneSelectedItem == LeftPaneSelectedItem.TurnIn)
                        {
                            TurnInCardInfoText = redemptionDTO.PrimaryCardNumber;
                            SetHeaderCustomerBalanceInfo(cardInfoText, SetTurnInTotalTicketCount());
                        }
                        else
                        {
                            SetHeaderCustomerBalanceInfo(cardInfoText, GetBalanceTickets());
                        }
                    }
                    else
                    {
                        SetHeaderCustomerBalanceInfo(null, GetBalanceTickets());
                    }
                    if (redemptionCardsId == null)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2913, accountDTO.TagNumber), MessageType.Info);
                    }
                    SetDefaultTransaction();
                    if (recalculateproductprice)
                    {
                        CancellationTokenSource.Cancel();
                        ResetRecalculateFlags();
                        CancellationTokenSource = new System.Threading.CancellationTokenSource();
                        CallRecalculatePriceandStock();
                    }
                    result = true;
                    accountDTO = null;
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                SetFooterContent(vex.Message.ToString(), MessageType.Error);
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                ShowRelogin(ExecutionContext.GetUserId());
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
            log.LogMethodExit(result);
            return result;
        }
        private void SetDefaultTransaction()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (RedemptionUserControlVM != null && redemptionDTO != null)
                {
                    if (!RedemptionUserControlVM.StayInTransactionMode)
                    {
                        RedemptionUserControlVM.StayInTransactionMode = true;
                    }
                    if (!string.IsNullOrEmpty(redemptionDTO.RedemptionOrderNo))
                    {
                        RedemptionUserControlVM.TransactionID = redemptionDTO.RedemptionOrderNo;
                    }
                    else
                    {
                        RedemptionUserControlVM.TransactionID = "RO-";
                    }
                }
            });
            log.LogMethodExit();
        }
        internal int GetETickets(RedemptionDTO localredemptionDTO = null)
        {
            log.LogMethodEntry();
            int result = 0;
            ExecuteActionWithFooter(() =>
            {
                if (localredemptionDTO == null)
                {
                    localredemptionDTO = redemptionDTO;
                }
                if (localredemptionDTO != null && localredemptionDTO.RedemptionCardsListDTO != null &&
                localredemptionDTO.RedemptionCardsListDTO.Count > 0)
                {
                    if (localredemptionDTO.RedemptionCardsListDTO.Any(x => x.CardId >= 0))
                    {
                        foreach (RedemptionCardsDTO cardsDTO in localredemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0))
                        {
                            result += (int)((cardsDTO.TotalCardTickets == null) ? 0 : cardsDTO.TotalCardTickets);
                        }
                    }
                }
            });
            log.LogMethodExit(result);
            return result;
        }
        internal int GetReceiptTickets(RedemptionDTO localredemptionDTO = null)
        {
            log.LogMethodEntry();
            int result = 0;
            ExecuteActionWithFooter(() =>
            {
                if (localredemptionDTO == null)
                {
                    localredemptionDTO = redemptionDTO;
                }
                if (localredemptionDTO != null && localredemptionDTO.TicketReceiptListDTO != null &&
    localredemptionDTO.TicketReceiptListDTO.Count > 0)
                {
                    foreach (TicketReceiptDTO ticketDTO in localredemptionDTO.TicketReceiptListDTO)
                    {
                        result += (int)(ticketDTO.BalanceTickets);
                    }
                }
            });
            log.LogMethodExit(result);
            return result;
        }
        internal int GetCurrencyTickets(RedemptionDTO localredemptionDTO = null)
        {
            log.LogMethodEntry();
            int result = 0;
            ExecuteActionWithFooter(() =>
            {
                if (localredemptionDTO == null)
                {
                    localredemptionDTO = redemptionDTO;
                }
                if (localredemptionDTO != null && localredemptionDTO.RedemptionCardsListDTO != null &&
                localredemptionDTO.RedemptionCardsListDTO.Count > 0)
                {
                    if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyId >= 0 || x.CurrencyRuleId >= 0))
                    {
                        int prevRuleId = -1;
                        foreach (RedemptionCardsDTO cardsDTO in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0 || x.CurrencyRuleId >= 0).OrderBy(x => x.CurrencyRuleId))
                        {
                            if (cardsDTO.CurrencyRuleId >= 0 && prevRuleId != cardsDTO.CurrencyRuleId)
                            {
                                if (redemptionDTO.RedemptionTicketAllocationListDTO != null && redemptionDTO.RedemptionTicketAllocationListDTO.Any())
                                {
                                    if (redemptionDTO.RedemptionTicketAllocationListDTO.Any(x => x.RedemptionCurrencyRuleId == cardsDTO.CurrencyRuleId))
                                    {
                                        foreach (RedemptionTicketAllocationDTO ticketAllocationDTO in redemptionDTO.RedemptionTicketAllocationListDTO.Where(x => x.RedemptionCurrencyRuleId == cardsDTO.CurrencyRuleId))
                                        {
                                            result += (int)(ticketAllocationDTO.RedemptionCurrencyRuleTicket == null ? 0 : ticketAllocationDTO.RedemptionCurrencyRuleTicket);
                                        }
                                    }
                                }
                                else
                                {
                                    result += (int)(cardsDTO.TicketCount == null ? 0 : cardsDTO.TicketCount);
                                }
                                prevRuleId = (int)cardsDTO.CurrencyRuleId;
                            }
                            else
                            {
                                result +=  (int)(cardsDTO.TicketCount == null ? 0 : cardsDTO.TicketCount);
                            }

                        }
                    }
                }
            });
            log.LogMethodExit(result);
            return result;
        }
        internal int GetTotalTickets(RedemptionDTO localredemptionDTO = null)
        {
            log.LogMethodEntry();
            int result = 0;
            ExecuteActionWithFooter(() =>
            {
                if (localredemptionDTO == null)
                {
                    localredemptionDTO = redemptionDTO;
                }
                result += GetETickets(localredemptionDTO);
                result += GetCurrencyTickets(localredemptionDTO);
                result += GetReceiptTickets(localredemptionDTO);
                if (localredemptionDTO != null && localredemptionDTO.ManualTickets != null && localredemptionDTO.ManualTickets > 0)
                {
                    result += (int)localredemptionDTO.ManualTickets;
                }
            });
            log.LogMethodExit(result);
            return result;
        }
        internal int GetLoadTicketTotalCount()
        {
            log.LogMethodEntry();
            int result = 0;
            ExecuteActionWithFooter(() =>
            {
                if (redemptionDTO != null && redemptionDTO.RedemptionCardsListDTO != null &&
redemptionDTO.RedemptionCardsListDTO.Count > 0)
                {
                    if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyId >= 0 || x.CurrencyRuleId >= 0))
                    {
                        foreach (RedemptionCardsDTO cardsDTO in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0 || x.CurrencyRuleId >= 0))
                        {
                            result += (int)(cardsDTO.CurrencyValueInTickets == null?cardsDTO.TicketCount: (cardsDTO.CurrencyQuantity == null ? 1 : cardsDTO.CurrencyQuantity) * (int)(cardsDTO.CurrencyValueInTickets == null ? 0 : cardsDTO.CurrencyValueInTickets));
                        }
                    }
                }
                if (redemptionDTO != null && redemptionDTO.TicketReceiptListDTO != null &&
    redemptionDTO.TicketReceiptListDTO.Count > 0)
                {
                    foreach (TicketReceiptDTO ticketDTO in redemptionDTO.TicketReceiptListDTO)
                    {
                        result += (int)(ticketDTO.BalanceTickets);
                    }
                }
                if (redemptionDTO != null && redemptionDTO.ManualTickets != null && redemptionDTO.ManualTickets > 0)
                {
                    result += (int)redemptionDTO.ManualTickets;
                }
            });
            log.LogMethodExit(result);
            return result;
        }
        internal int GetTotalPhysicalTickets()
        {
            log.LogMethodEntry();
            int result = 0;
            ExecuteActionWithFooter(() =>
            {
                if (redemptionDTO != null && redemptionDTO.RedemptionCardsListDTO != null &&
redemptionDTO.RedemptionCardsListDTO.Count > 0)
                {
                    if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyId >= 0))
                    {
                        foreach (RedemptionCardsDTO cardsDTO in redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0 || x.CurrencyRuleId >= 0))
                        {
                            result += (int)cardsDTO.TicketCount;
                        }
                    }
                }
                if (redemptionDTO != null && redemptionDTO.TicketReceiptListDTO != null &&
    redemptionDTO.TicketReceiptListDTO.Count > 0)
                {
                    foreach (TicketReceiptDTO ticketDTO in redemptionDTO.TicketReceiptListDTO)
                    {
                        result += (int)(ticketDTO.BalanceTickets);
                    }
                }
            });
            log.LogMethodExit(result);
            return result;
        }

        internal int GetTotalRedeemed()
        {
            log.LogMethodEntry();
            int result = 0;
            ExecuteActionWithFooter(() =>
            {
                if (redemptionDTO != null && redemptionDTO.RedemptionGiftsListDTO != null &&
                redemptionDTO.RedemptionGiftsListDTO.Count > 0)
                {
                    foreach (RedemptionGiftsDTO g in redemptionDTO.RedemptionGiftsListDTO)
                    {
                        result += (int)((g.Tickets == null) ? 0 : g.Tickets * g.ProductQuantity);
                    }
                }
            });
            log.LogMethodExit(result);
            return result;
        }
        internal int GetGraceTickets()
        {
            log.LogMethodEntry();
            int result = 0;
            ExecuteActionWithFooter(() =>
            {
                int graceTickets = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "REDEMPTION_GRACE_TICKETS");
                int graceticketpercent = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "REDEMPTION_GRACE_TICKETS_PERCENTAGE");
                if (graceTickets > 0)
                {
                    result = graceTickets;
                }
                else if (graceticketpercent > 0)
                {
                    result = Convert.ToInt32(Math.Round((double)(GetTotalTickets() * graceticketpercent) / 100));
                }
            });
            log.LogMethodExit(result);
            return result;
        }
        internal int GetBalanceTickets()
        {
            log.LogMethodEntry();
            int result = 0;
            ExecuteActionWithFooter(() =>
            {
                result = GetTotalTickets() - GetTotalRedeemed();
            });
            log.LogMethodExit(result);
            return result;
        }
        private void OnConfirmCloseScreen(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok)
                {
                    if (newOrCloseScreen == 'C')
                    {
                        RaiseRemoveClickedEvent(redemptionMainUserControl);
                    }
                    if (newOrCloseScreen == 'N')
                    {
                        Discard();
                    }
                }
                isTurnInSecondCardTapped = false;
            });
            log.LogMethodExit();
        }
        public void Discard()
        {
            switch (this.leftPaneSelectedItem)
            {
                case LeftPaneSelectedItem.Redemption:
                    {
                        if (RedemptionUserControlVM != null)
                        {
                            RedemptionUserControlVM.TransactionID = string.Empty;
                            RedemptionUserControlVM.StayInTransactionMode = false;
                            if (RedemptionUserControlVM.GenericTransactionListVM != null)
                            {
                                RedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Clear();
                                RedemptionUserControlVM.GenericTransactionListVM.SelectedItem = null;
                            }
                            RedemptionUserControlVM.AddRetreivedSuspendBackup();
                            RedemptionUserControlVM.UpdateTicketValues();
                            SetNewRedemption();
                            RedemptionUserControlVM.UpdateTicketValues();
                            this.SetHeaderCustomerBalanceInfo(string.Empty, 0);
                        }
                    }
                    break;
                case LeftPaneSelectedItem.LoadTicket:
                    {
                        if (LoadTicketRedemptionUserControlVM != null)
                        {
                            LoadTicketRedemptionUserControlVM.TransactionID = string.Empty;
                            LoadTicketRedemptionUserControlVM.StayInTransactionMode = false;
                            LoadTicketRedemptionUserControlVM.LoadTotatlTicketCount = 0;
                            if (LoadTicketRedemptionUserControlVM.GenericTransactionListVM != null)
                            {
                                LoadTicketRedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Clear();
                                LoadTicketRedemptionUserControlVM.GenericTransactionListVM.SelectedItem = null;
                            }
                            this.redemptionDTO.RedemptionCardsListDTO.Clear();
                            redemptionDTO.CurrencyTickets = 0;
                            SetNewRedemption();
                            this.SetHeaderCustomerBalanceInfo(string.Empty, 0);
                            AddSecondTappedCard();
                        }
                    }
                    break;
                case LeftPaneSelectedItem.TurnIn:
                    {
                        if (TurnInUserControlVM != null)
                        {
                            if (isTurnInSecondCardTapped)
                            {
                                AddSecondTappedCard();
                                SetHeaderCustomerBalanceInfo(null, SetTurnInTotalTicketCount());
                            }
                            else
                            {
                                TurnInUserControlVM.SelectedTargetLocation = null;
                                TurnInUserControlVM.TransactionID = string.Empty;
                                TurnInUserControlVM.StayInTransactionMode = false;
                                if (TurnInUserControlVM.GenericTransactionListVM != null)
                                {
                                    TurnInUserControlVM.GenericTransactionListVM.ItemCollection.Clear();
                                    TurnInUserControlVM.GenericTransactionListVM.SelectedItem = null;
                                    TurnInUserControlVM.LoadTotatlTicketCount = 0;
                                }
                                this.redemptionDTO.RedemptionGiftsListDTO.Clear();
                                this.redemptionDTO.TicketReceiptListDTO.Clear();
                                SetNewRedemption();
                                this.SetHeaderCustomerBalanceInfo(string.Empty, 0);
                                AddSecondTappedCard();
                                this.TurnInCardInfoText = string.Empty;
                            }
                        }
                    }
                    break;
            }
            if (newSelectedLeftPaneItem != null && LeftPaneVM != null)
            {
                LeftPaneVM.SelectedMenuItem = newSelectedLeftPaneItem;
                newSelectedLeftPaneItem = string.Empty;
            }
        }
        private async void AddSecondTappedCard()
        {
            log.LogMethodEntry();
            try
            {
                if (accountDTO != null)
                {
                    this.redemptionDTO.RedemptionCardsListDTO.Clear();
                    await AddCardtoRedemption();
                    if (leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket)
                    {
                        SetHideTapCardEnterScanTicket();
                    }
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                SetFooterContent(vex.Message.ToString(), MessageType.Error);
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                ShowRelogin(ExecutionContext.GetUserId());
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
            log.LogMethodExit();
        }
        private void OnAddUserConfirmation(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok)
                {
                    LoginRequest loginRequest = new LoginRequest();
                    loginRequest.TagNumber = accountDTO.TagNumber;
                    loginRequest.MachineName = Environment.MachineName;
                    loginRequest.SiteId = ExecutionContext.GetSiteId().ToString();
                    Semnox.Core.Utilities.ExecutionContext systemuserExecutioncontext = SystemUserExecutionContextBuilder.GetSystemUserExecutionContext();
                    IAuthenticationUseCases authenticateUseCases = AuthenticationUseCaseFactory.GetAuthenticationUseCases(systemuserExecutioncontext);
                    Semnox.Core.Utilities.ExecutionContext localusercontext;
                    using (NoSynchronizationContextScope.Enter())
                    {
                        Task<Semnox.Core.Utilities.ExecutionContext> task = authenticateUseCases.LoginUser(loginRequest);
                        task.Wait();
                        localusercontext = task.Result;
                    }
                    // ExecutionContext localusercontext = new ExecutionContext(newloginid, ExecutionContext.SiteId, ExecutionContext.MachineId, -1, ExecutionContext.IsCorporate, ExecutionContext.LanguageId);
                    mainVM.AddNewUser(localusercontext);
                }
                else
                {
                    string message = MessageViewContainerList.GetMessage(ExecutionContext, 197, tappedCardNumber);
                    SetFooterContent(message, MessageType.Error);
                    log.Info(message);
                }
            });
            log.LogMethodExit();
        }
        private void CheckReloginRequired(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                int inactivityPeriodSec = (int)(DateTime.Now - lastActivityTime).TotalSeconds;
                if (inactivityPeriodSec > posInActivityTime)
                {
                    timer.Stop();
                    ShowRelogin(ExecutionContext.UserId);
                }
            });
            log.LogMethodExit();
        }
        internal void ShowRelogin(string loginID, System.ComponentModel.CancelEventArgs e=null)
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if ( IsActive)
                {
                    if (userView == null)
                    {
                        olduserContext = new Semnox.Core.Utilities.ExecutionContext(ExecutionContext.UserId, ExecutionContext.SiteId, ExecutionContext.MachineId, ExecutionContext.UserPKId, ExecutionContext.IsCorporate, ExecutionContext.LanguageId);
                        olduserContext.WebApiToken = ExecutionContext.WebApiToken;
                        userView = new AuthenticateUserView(true);
                        this.SetKeyBoardHelperColorCode();
                        userView.Loaded += OnWindowLoaded;
                        userView.CloseButtonClicked += OnAuthenticateUserViewClosed;
                        userView.Closing += OnAuthenticateUserViewClosing;
                        userView.Closed += OnUserViewClosed;
                        AuthenticateUserVM authenticateUserVM = new AuthenticateUserVM(ExecutionContext, "", "POS", loginStyle.PopUp, false, CardReader);
                        authenticateUserVM.MultiScreenMode = this.MultiScreenMode;
                        if (mainVM != null && mainVM.RowCount > 1)
                        {
                            authenticateUserVM.IsMultiScreenRowTwo = true;
                        }
                        SetReloginInitiated(loginID);
                        userView.DataContext = authenticateUserVM;
                        userView.Show();
                        if (mainVM != null && mainVM.authenticateUserView != null)
                        {
                            mainVM.authenticateUserView.Topmost = true;
                        }
                    }
                    else
                    {
                        if (e != null)
                        {
                            e.Cancel = true;
                        }
                    }
                }
                else
                {
                    AuthenticateUserVM userVM = userView.DataContext as AuthenticateUserVM;
                    if (mainVM != null && mainVM.RowCount > 1)
                    {
                        userVM.IsMultiScreenRowTwo = true;
                    }
                    else if (userVM.IsMultiScreenRowTwo)
                    {
                        userVM.IsMultiScreenRowTwo = false;
                    }
                    userVM.MultiScreenMode = this.MultiScreenMode;
                    OnWindowLoaded(userView, null);
                }
            });
            log.LogMethodExit();
        }
        private void SetReloginInitiated(string loginID)
        {
            log.LogMethodEntry(loginID);
            if (mainVM != null && mainVM.RedemptionUserControlVMs.Count > 0)
            {
                foreach (RedemptionMainUserControlVM redemptionMainUserControlVM in mainVM.RedemptionUserControlVMs.Where(x => x.UserName == loginID))
                {
                    //if (redemptionMainUserControlVM.ScreenNumber!=this.ScreenNumber)
                    //{
                    redemptionMainUserControlVM.ReloginInitiated = true;
                    //}
                }
            }
            log.LogMethodExit();
        }
        private void SetReloginandLastActivityTime(string loginID)
        {
            log.LogMethodEntry(loginID);
            if (mainVM != null && mainVM.RedemptionUserControlVMs.Count > 0)
            {
                foreach (RedemptionMainUserControlVM redemptionMainUserControlVM in mainVM.RedemptionUserControlVMs.Where(x => x.UserName == loginID))
                {
                    redemptionMainUserControlVM.ReloginInitiated = false;
                    redemptionMainUserControlVM.lastActivityTime = DateTime.Now;
                }
            }
            log.LogMethodExit();
        }
        private void OnUserViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            userView = null;
            redemptionMainView.Focus();
            log.LogMethodExit();
        }
        internal GenericMessagePopupView OpenGenericMessagePopupView(string heading, string subHeading, string content,
            string okButtonText, string cancelButtonText, MessageButtonsType messageButtonsType)
        {
            log.LogMethodEntry(heading, subHeading, content, okButtonText, cancelButtonText, messageButtonsType);
            ExecuteActionWithFooter(() =>
            {
                this.messagePopupView = new GenericMessagePopupView();
                this.messagePopupView.PreviewMouseDown += this.UpdateActivityTimeOnMouseOrKeyBoardAction;
                this.messagePopupView.PreviewKeyDown += UpdateActivityTimeOnMouseOrKeyBoardAction;
                this.messagePopupView.Loaded += OnWindowLoaded;
                GenericMessagePopupVM genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                {
                    OkButtonText = okButtonText,
                    CancelButtonText = cancelButtonText,
                    MessageButtonsType = messageButtonsType,
                    SubHeading = subHeading,
                    Heading = heading,
                    Content = content
                };
                messagePopupView.DataContext = genericMessagePopupVM;
            });
            log.LogMethodExit();
            return messagePopupView;
        }
        private void OnAuthenticateUserViewClosed(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                AuthenticateUserView authenticateUserView = sender as AuthenticateUserView;
                authenticateUserView.Closing -= OnAuthenticateUserViewClosing;
                authenticateUserView.Close();
                //RegisterDevices();
                OpenGenericMessagePopupView(
                    MessageViewContainerList.GetMessage(this.ExecutionContext, "LOGIN CANCEL", null),
                    MessageViewContainerList.GetMessage(this.ExecutionContext, "CLOSE SCREEN", null),
                    MessageViewContainerList.GetMessage(ExecutionContext, 2677, ExecutionContext.UserId),
                    MessageViewContainerList.GetMessage(this.ExecutionContext, "CONFIRM", null),
                    MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                    MessageButtonsType.OkCancel);
                if (this.messagePopupView != null)
                {
                    this.messagePopupView.Closed += OnGenericMessagePopupViewClosed;
                    this.messagePopupView.Show();
                }
            });
            SetUserControlFocus();
            log.LogMethodExit();
        }
        private void OnAuthenticateUserViewClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                AuthenticateUserView authenticateUserView = sender as AuthenticateUserView;
                AuthenticateUserVM authenticateUserVM = authenticateUserView.DataContext as AuthenticateUserVM;
                if (authenticateUserVM != null)
                {
                    if (!authenticateUserVM.IsValid)
                    {
                        ShowRelogin(olduserContext.UserId,e);
                    }
                    else
                    {
                        if (authenticateUserVM.IsValid)
                        {
                            if (olduserContext.UserId == authenticateUserVM.ExecutionContext.UserId)
                            {
                                ExecutionContext = authenticateUserVM.ExecutionContext;
                                SetChildViewsExecutionContext(ExecutionContext);
                                SetReloginandLastActivityTime(ExecutionContext.UserId);
                                if (timer != null)
                                {
                                    timer.Start();
                                }
                            }
                            else
                            {
                                ExecutionContext = olduserContext;
                                OpenGenericMessagePopupView(
                                   MessageViewContainerList.GetMessage(this.ExecutionContext, "LOGIN FAILED", null),
                                   string.Empty,
                                   MessageViewContainerList.GetMessage(ExecutionContext, 2680, ExecutionContext.UserId),
                                   MessageViewContainerList.GetMessage(this.ExecutionContext, "OK", null),
                                   MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                                   MessageButtonsType.OkCancel);
                                if (this.messagePopupView != null)
                                {
                                    this.messagePopupView.Closed += OnWrongLoginClosed;
                                    this.messagePopupView.Topmost = true;
                                    this.messagePopupView.Show();
                                }
                            }
                        }
                    }
                }
            });
            log.LogMethodExit();
        }
        private void OnWrongLoginClosed(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                ShowRelogin(olduserContext.UserId);
            });
            SetUserControlFocus();
            log.LogMethodExit();
        }
        private void OnGenericMessagePopupViewClosed(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok)
                {
                    RedemptionMainVM redemptionMainVM = this.redemptionMainView.DataContext as RedemptionMainVM;
                    ObservableCollection<RedemptionMainUserControlVM> backupRedemptionUserControlVMs = new ObservableCollection<RedemptionMainUserControlVM>();
                    foreach (RedemptionMainUserControlVM b in redemptionMainVM.RedemptionUserControlVMs)
                    {
                        backupRedemptionUserControlVMs.Add(b);
                    }
                    bool isMaximumScreens = mainVM.RedemptionUserControlVMs.Count == 8 ? true : false;
                    foreach (RedemptionMainUserControlVM r in backupRedemptionUserControlVMs.Where(x => x.ExecutionContext.UserId == ExecutionContext.UserId).OrderByDescending(x => x.ScreenNumber))
                    {
                        mainVM.RemoveScreen(r, true);
                    }
                    mainVM.SetAddUserButtonEnabled();
                    foreach (RedemptionMainUserControlVM mainUserControlVM in mainVM.RedemptionUserControlVMs)
                    {
                        if (isMaximumScreens && mainUserControlVM.IsActive && !mainUserControlVM.LeftPaneVM.AddButtonVisiblity)
                        {
                            mainUserControlVM.LeftPaneVM.AddButtonVisiblity = true;
                        }
                        mainUserControlVM.OnSizeChanged(mainUserControlVM, null);
                    }
                    if (mainVM != null && mainVM.RedemptionUserControlVMs != null
                        && mainVM.RedemptionUserControlVMs.Count <= 0 && redemptionMainView != null)
                    {
                        mainVM.DisposeAllDevices();
                        redemptionMainView.Close();
                    }
                }
                else
                {
                    ShowRelogin(ExecutionContext.UserId);
                }
            });
            SetUserControlFocus();
            log.LogMethodExit();
        }
        private void SetChildViewsExecutionContext(Semnox.Core.Utilities.ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            if (redemptionUserControlVM != null)
            {
                redemptionUserControlVM.ExecutionContext = executionContext;
            }
            if (turnInUserControlVM != null)
            {
                turnInUserControlVM.ExecutionContext = executionContext;
            }
            if (loadTicketRedemptionUserControlVM != null)
            {
                loadTicketRedemptionUserControlVM.ExecutionContext = executionContext;
            }
            if (voucherUserControlVM != null)
            {
                voucherUserControlVM.ExecutionContext = executionContext;
            }
            if (itemInfoPopUpView != null)
            {
                GenericItemInfoPopUpVM itemInfoPopupVM = itemInfoPopUpView.DataContext as GenericItemInfoPopUpVM;
                if (itemInfoPopupVM != null)
                {
                    itemInfoPopupVM.ExecutionContext = executionContext;
                }
            }
            if (dataEntryView != null)
            {
                GenericDataEntryVM dataEntryVM = dataEntryView.DataContext as GenericDataEntryVM;
                if (dataEntryVM != null)
                {
                    dataEntryVM.ExecutionContext = executionContext;
                }
            }
            if (scanView != null)
            {
                RedemptionScanVM scanVM = scanView.DataContext as RedemptionScanVM;
                if (scanVM != null)
                {
                    scanVM.ExecutionContext = executionContext;
                }
            }
            if (messagePopupView != null)
            {
                GenericMessagePopupVM messagePopupVM = messagePopupView.DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null)
                {
                    messagePopupVM.ExecutionContext = executionContext;
                }
            }
            if (reverseView != null)
            {
                RedemptionReverseVM reverseVM = reverseView.DataContext as RedemptionReverseVM;
                if (reverseVM != null)
                {
                    reverseVM.ExecutionContext = executionContext;
                }
            }
            if (updateView != null)
            {
                RedemptionUpdateVM updateVM = updateView.DataContext as RedemptionUpdateVM;
                if (updateVM != null)
                {
                    updateVM.ExecutionContext = executionContext;
                }
            }
            if (allocationView != null)
            {
                RedemptionTicketAllocationVM updateVM = allocationView.DataContext as RedemptionTicketAllocationVM;
                if (updateVM != null)
                {
                    updateVM.ExecutionContext = executionContext;
                }
            }
            if (enterTicketView != null)
            {
                GenericDataEntryVM dataEntryVM = enterTicketView.DataContext as GenericDataEntryVM;
                if (dataEntryVM != null)
                {
                    dataEntryVM.ExecutionContext = executionContext;
                }
            }
            if (managerView != null)
            {
                AuthenticateManagerVM managerVM = managerView.DataContext as AuthenticateManagerVM;
                if (managerVM != null)
                {
                    managerVM.ExecutionContext = executionContext;
                }
            }
            if (changePasswordView != null)
            {
                ChangePasswordVM changePasswordVM = changePasswordView.DataContext as ChangePasswordVM;
                if (changePasswordVM != null)
                {
                    changePasswordVM.ExecutionContext = executionContext;
                }
            }
            if (datePickerView != null)
            {
                DatePickerVM datePickerVM = datePickerView.DataContext as DatePickerVM;
                if (datePickerVM != null)
                {
                    datePickerVM.ExecutionContext = executionContext;
                }
            }
            if (numberKeyboardView != null)
            {
                NumberKeyboardVM numberKeyboardVM = numberKeyboardView.DataContext as NumberKeyboardVM;
                if (numberKeyboardVM != null)
                {
                    numberKeyboardVM.ExecutionContext = executionContext;
                }
            }
            if (FooterVM != null && FooterVM.MessagePopupView != null)
            {
                GenericMessagePopupVM messagePopupVM = FooterVM.MessagePopupView.DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null)
                {
                    messagePopupVM.ExecutionContext = executionContext;
                }
            }
            log.LogMethodExit();
        }
        private async void AddCardtoUI(string cardNumber, int cardSiteId)
        {
            log.LogMethodEntry(cardNumber, cardSiteId);
            try
            {
                if (IsLoadingVisible)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4877), MessageType.Warning);
                    log.LogMethodExit("Another API/action inprogress");
                    return;
                }
                tappedCardNumber = cardNumber;
                newloginid = null;
                accountDTO = null;
                string message;
                IsLoadingVisible = true;
                AccountDTOCollection accountDTOCollection;
                try
                {
                    IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(ExecutionContext);
                    accountDTOCollection = await accountUseCases.GetAccounts(accountNumber: cardNumber, tagSiteId: cardSiteId);

                    if (accountDTOCollection != null && accountDTOCollection.data != null)
                    {
                        accountDTO = accountDTOCollection.data[0];
                        if (IsRedemptionDelivered())
                        {
                            ClearCompletedRedemption();
                        }
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
                    log.Info("unauthroized exception while retreiving card info - show relogin");
                    IsLoadingVisible = false;
                    ShowRelogin(this.ExecutionContext.GetUserId());
                    return;
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
                    message = MessageViewContainerList.GetMessage(ExecutionContext, 2914, cardNumber);
                    log.Error(message, ex);
                    IsLoadingVisible = false;
                    this.SetFooterContent(message + ex.Message, MessageType.Error);
                    return;
                }
                finally
                {
                    IsLoadingVisible = false;
                }
                if (accountDTO.AccountId >= 0)
                {
                    // check for suspended redemptions
                    if (RedemptionUserControlVM != null)
                    {
                        IsLoadingVisible = true;
                        List<RedemptionDTO> suspendedDTOs = await RedemptionUserControlVM.GetRedemptions(null, null, cardNumber,
                            "SUSPENDED", null, null, null); // change to suspended

                        if (suspendedDTOs.Any())
                        {
                            string redemptionText = MessageViewContainerList.GetMessage(ExecutionContext, "Redemption");
                            if (this.leftPaneSelectedItem != LeftPaneSelectedItem.Redemption && LeftPaneVM != null && LeftPaneVM.MenuItems.Contains(redemptionText))
                            {
                                this.LeftPaneVM.SelectedMenuItem = redemptionText;
                            }
                            RedemptionUserControlVM.GenericToggleButtonsVM.ToggleButtonItems[1].IsChecked = true;
                            RedemptionUserControlVM.RedemptionDTOList = suspendedDTOs;
                            RedemptionUserControlVM.SetCustomDataGridVM(completedOrSuspendedRedemptions: RedemptionUserControlVM.RedemptionDTOList);
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2685, cardNumber), MessageType.None);
                            RedemptionUserControlVM.SetCompletedSuspenedCount(RedemptionsType.Suspended, RedemptionUserControlVM.RedemptionDTOList.Count);
                            SetHideTapCardEnterScanTicket();
                            redemptionUserControlVM.UpdateTicketValues();
                            SetHeaderCustomerBalanceInfo((!string.IsNullOrWhiteSpace(redemptionDTO.CustomerName) ? redemptionDTO.CustomerName : redemptionDTO.PrimaryCardNumber), GetBalanceTickets());
                            IsLoadingVisible = false;
                            return;
                        }
                        SetHideTapCardEnterScanTicket();
                    }
                    if (accountDTO.TechnicianCard == "Y")
                    {
                        if (UserViewContainerList.GetUserContainerDTOList(ExecutionContext) != null && UserViewContainerList.GetUserContainerDTOList(ExecutionContext).Any(x => x.UserIdentificationTagContainerDTOList.Exists(y => y.CardId == accountDTO.AccountId)))
                        {
                            newloginid = UserViewContainerList.GetUserContainerDTOList(ExecutionContext).FirstOrDefault(x => x.UserIdentificationTagContainerDTOList.Exists(y => y.CardId == accountDTO.AccountId)).LoginId;
                        }
                        if (newloginid != null)
                        {
                            if (mainVM != null)
                            {
                                if (mainVM.RedemptionUserControlVMs != null)
                                {
                                    if (mainVM.RedemptionUserControlVMs.Any(x => x.UserName == newloginid))
                                    {
                                        message = MessageViewContainerList.GetMessage(ExecutionContext, 2672, newloginid);
                                        log.Info(message);
                                        SetFooterContent(message, MessageType.Warning);
                                        IsLoadingVisible = false;
                                        return;
                                    }
                                    if (mainVM.RedemptionUserControlVMs.Count == 8)
                                    {
                                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2671, null), MessageType.Error);
                                        IsLoadingVisible = false;
                                        return;
                                    }
                                }
                                if (singleUserMultiscreen)
                                {
                                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2692), MessageType.Error);
                                    IsLoadingVisible = false;
                                    return;
                                }
                                else
                                {
                                    OpenGenericMessagePopupView(
                                        MessageViewContainerList.GetMessage(this.ExecutionContext, "ADD NEW USER", null),
                                        MessageViewContainerList.GetMessage(this.ExecutionContext, "TECHNICIAN CARD", null),
                                        MessageViewContainerList.GetMessage(ExecutionContext, 2749, newloginid) + MessageViewContainerList.GetMessage(ExecutionContext, 2693, screenNumber),
                                        MessageViewContainerList.GetMessage(this.ExecutionContext, "OK", null),
                                        MessageViewContainerList.GetMessage(this.ExecutionContext, "CANCEL", null),
                                        MessageButtonsType.OkCancel);
                                    if (this.messagePopupView != null)
                                    {
                                        this.messagePopupView.Closed += OnAddUserConfirmation;
                                        this.messagePopupView.Show();
                                    }
                                    IsLoadingVisible = false;
                                    return;
                                }
                            }
                            IsLoadingVisible = false;
                            return;
                        }
                        else
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2915, cardNumber), MessageType.Error); // create new message
                            IsLoadingVisible = false;
                            return;
                        }
                    }
                    else
                    {
                        if (this.LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption
                            && RedemptionUserControlVM != null && RedemptionUserControlVM.RedemptionsType == RedemptionsType.Completed
                            )
                        {
                            RedemptionUserControlVM.ShowSearchCloseIcon = false;
                            RedemptionUserControlVM.SearchedCardNo = cardNumber;
                            RedemptionUserControlVM.PerformSearch();
                            IsLoadingVisible = false;
                            return;
                        }
                        if (this.LeftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket
                        && LoadTicketRedemptionUserControlVM != null && LoadTicketRedemptionUserControlVM.RedemptionsType == RedemptionsType.Completed
                        )
                        {
                            LoadTicketRedemptionUserControlVM.ShowSearchCloseIcon = false;
                            LoadTicketRedemptionUserControlVM.SearchedCardNo = cardNumber;
                            LoadTicketRedemptionUserControlVM.PerformSearch();
                            IsLoadingVisible = false;
                            return;
                        }
                        if (this.LeftPaneSelectedItem == LeftPaneSelectedItem.TurnIn
                        && TurnInUserControlVM != null && TurnInUserControlVM.RedemptionsType == RedemptionsType.Completed
                        )
                        {
                            TurnInUserControlVM.ShowSearchCloseIcon = false;
                            TurnInUserControlVM.SearchedCardNo = cardNumber;
                            TurnInUserControlVM.PerformSearch();
                            IsLoadingVisible = false;
                            return;
                        }
                        if (this.LeftPaneSelectedItem == LeftPaneSelectedItem.Voucher
                        && VoucherUserControlVM != null
                        )
                        {
                            VoucherUserControlVM.ShowSearchCloseIcon = false;
                            VoucherUserControlVM.SearchedCardNo = cardNumber;
                            VoucherUserControlVM.PerformSearch();
                            IsLoadingVisible = false;
                            return;
                        }
                        else
                        {
                            if (ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "REGISTRATION_MANDATORY_FOR_REDEMPTION") && accountDTO.CustomerId < 0)
                            {
                                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 515), MessageType.Error);
                                IsLoadingVisible = false;
                                return;
                            }
                            else if (redemptionDTO.RedemptionCardsListDTO != null)
                            {
                                if (redemptionDTO.RedemptionCardsListDTO.Count > 0)
                                {
                                    if (redemptionDTO.RedemptionCardsListDTO.Exists(x => x.CardId == accountDTO.AccountId))
                                    {
                                        message = MessageViewContainerList.GetMessage(ExecutionContext, 59, null);
                                        this.SetFooterContent(message, MessageType.Warning);
                                        log.Info("Ends-addCard(" + cardNumber + "," + message + ") as Card already added");
                                        IsLoadingVisible = false;
                                        return;
                                    }
                                    else
                                    {
                                        if (redemptionDTO.RedemptionCardsListDTO.Where(x => x.CardId >= 0).Count() > 0)
                                        {
                                            if (leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket
                                                                                || leftPaneSelectedItem == LeftPaneSelectedItem.TurnIn)
                                            {
                                                ShowDiscardConfirmation();
                                                IsLoadingVisible = false;
                                                return;
                                            }
                                            else if (leftPaneSelectedItem == LeftPaneSelectedItem.Redemption)
                                            {
                                                OpenGenericMessagePopupView(
                                                    MessageViewContainerList.GetMessage(this.ExecutionContext, 4201, null),
                                                    MessageViewContainerList.GetMessage(this.ExecutionContext, "ADD CARD", null),
                                                    MessageViewContainerList.GetMessage(ExecutionContext, 1434, tappedCardNumber),
                                                    MessageViewContainerList.GetMessage(this.ExecutionContext, "YES", null),
                                                    MessageViewContainerList.GetMessage(this.ExecutionContext, "NO", null),
                                                    MessageButtonsType.OkCancel);
                                                if (this.messagePopupView != null)
                                                {
                                                    this.messagePopupView.Closed += OnConsolidateCardClosed;
                                                    this.messagePopupView.Show();
                                                }
                                            }
                                            IsLoadingVisible = false;
                                            return;
                                        }
                                    }
                                }
                            }

                        }
                        IsLoadingVisible = false;
                        await AddCardtoRedemption();
                        return;
                    }
                }
                message = MessageViewContainerList.GetMessage(ExecutionContext, 110, tappedCardNumber);
                SetFooterContent(message, MessageType.Error);
                log.Info(message);
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
                ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                SetFooterContent(pax.Message.ToString(), MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again."), MessageType.Error);
            }
            finally
            {
                IsLoadingVisible = false;
            }
            log.LogMethodExit();
        }
        private async Task<CustomerDTO> GetCustomerInfo(int customerId)
        {
            log.LogMethodEntry(customerId);
            CustomerDTO customerDTO = new CustomerDTO();
            try
            {
                IsLoadingVisible = true;
                // change to use factory
                ICustomerUseCases iCustomerUseCases = CustomerUseCaseFactory.GetCustomerUseCases(ExecutionContext);
                List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerSearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CUSTOMER_ID, customerId.ToString()));
                List<CustomerDTO> customers = await iCustomerUseCases.GetCustomerDTOList(searchParameters, true);
                customerDTO = customers.FirstOrDefault();
                //CustomerBL customers = new CustomerBL(ExecutionContext, customerId);
                //customerDTO = customers.CustomerDTO;
                if (customerDTO == null)
                {
                    string message = MessageViewContainerList.GetMessage(ExecutionContext, 2916, customerId);
                    log.Error(message);
                    this.SetFooterContent(message, MessageType.Error);
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                SetFooterContent(vex.Message.ToString(), MessageType.Error);
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                SetFooterContent(pax.Message.ToString(), MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again."), MessageType.Error);
            }
            finally
            {
                IsLoadingVisible = false;
            }
            log.LogMethodExit(customerDTO);
            return customerDTO;
        }
        internal bool ApplyCurrencyRule()
        {
            log.LogMethodEntry();
            bool isAnyCurrencyRuleAdded = false;
            ExecuteActionWithFooter(() =>
            {
                RedemptionCurrencyRuleViewProvider redemptionCurrencyRuleViewProvider = new RedemptionCurrencyRuleViewProvider(ExecutionContext);
                RedemptionCurrencyRuleCalculator redemptionCurrencyRuleCalculator = new RedemptionCurrencyRuleCalculator(redemptionCurrencyRuleViewProvider);
                List<RedemptionCardsDTO> currencyListforRule = this.RedemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyRuleId >= 0).ToList();
                if (currencyListforRule != null && currencyListforRule.Count > 0)
                {
                    foreach (RedemptionCardsDTO currencyforRule in currencyListforRule)
                    {
                        this.RedemptionDTO.RedemptionCardsListDTO.Remove(currencyforRule);
                    }
                    currencyListforRule.Clear();
                    isAnyCurrencyRuleAdded = true;
                }
                else
                {
                    currencyListforRule = new List<RedemptionCardsDTO>();
                }
                foreach (RedemptionCardsDTO currencyforRule in this.RedemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId >= 0))
                {
                    RedemptionCardsDTO currencyDTOforRule = new RedemptionCardsDTO(currencyforRule);
                    currencyListforRule.Add(currencyDTOforRule);
                }
                currencyListforRule = redemptionCurrencyRuleCalculator.Calculate(currencyListforRule);
                if (currencyListforRule.Any(x => x.CurrencyRuleId >= 0))
                {
                    foreach (RedemptionCardsDTO rewardcurrency in currencyListforRule.Where(x => x.CurrencyRuleId >= 0))
                    {
                        this.RedemptionDTO.RedemptionCardsListDTO.Add(rewardcurrency);
                        isAnyCurrencyRuleAdded = true;
                    }
                }
                log.LogMethodExit();
            });
            return isAnyCurrencyRuleAdded;
        }
        internal int? GetNextViewGroupingNumber()
        {
            log.LogMethodEntry();
            int? result = null;
            if (!ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(this.ExecutionContext, "GROUP_REDEMPTION_CURRENCY"))
            {
                if (redemptionDTO != null && redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any(x => x.CurrencyId > -1 && x.ViewGroupingNumber != null))
                {
                    try
                    {
                        result = Convert.ToInt32(redemptionDTO.RedemptionCardsListDTO.Where(x => x.CurrencyId > -1 && x.ViewGroupingNumber != null).Max(x => x.ViewGroupingNumber)) + 1;
                    }
                    catch (Exception ex) { result = null; }
                }
                else
                {
                    result = 0;
                }
            }
            log.LogMethodExit(result);
            return result;
        }
        internal void SetDiscountPrice()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (leftPaneSelectedItem == LeftPaneSelectedItem.Redemption && RedemptionUserControlVM != null &&
                RedemptionUserControlVM.GenericDisplayItemsVM!=null && RedemptionUserControlVM.GenericDisplayItemsVM.BackupDisplayItemModels != null
                && RedemptionUserControlVM.GenericDisplayItemsVM.BackupDisplayItemModels.Any()
                && leftPaneSelectedItem == LeftPaneSelectedItem.Redemption)
                {
                    string ticketMessage = " " + MessageViewContainerList.GetMessage(ExecutionContext,
                        ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT", "Tickets"));
                    string numberFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT");
                    foreach (GenericDisplayItemModel displayItemModel in RedemptionUserControlVM.GenericDisplayItemsVM.BackupDisplayItemModels)
                    {
                        decimal discountedPrice = RedemptionPriceViewContainerList.GetLeastPriceInTickets(ExecutionContext.SiteId, Convert.ToInt32(displayItemModel.Key), redemptionMainUserControlVM.MembershipIDList);
                        int price = Convert.ToInt32(Math.Round(discountedPrice));
                        string priceText = price >= 0 ? price.ToString(numberFormat) : price.ToString();
                        if (priceText + ticketMessage != displayItemModel.ItemsSource[0].OriginalValue)
                        {
                            redemptionMainUserControl.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                displayItemModel.ItemsSource[0].DiscountValue = priceText;
                            }));
                        }
                        else if (!string.IsNullOrEmpty(displayItemModel.ItemsSource[0].DiscountValue))
                        {
                            redemptionMainUserControl.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                displayItemModel.ItemsSource[0].DiscountValue = string.Empty;
                            }));
                        }
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    }
                }
            });
            log.LogMethodExit();
        }
        internal void ResetRecalculateFlags()
        {
            log.LogMethodEntry();
            reCalculatePriceInProgress = false;
            reCalculateStockInProgress = false;
            log.LogMethodEntry();
        }
        internal void RecalculateProductPriceandStock(bool fromScroll = false,bool recalculateStock=false,bool skipPriceUpdate=false)
        {
            log.LogMethodEntry();
            if (!reCalculatePriceInProgress)
            {
                ExecuteActionWithFooter(() =>
                {
                    reCalculatePriceInProgress = true;
                    List<Task> priceTasks = new List<Task>();
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    priceTasks.Add(Task.Factory.StartNew(() => { ReCalculatePrice(fromScroll, recalculateStock, skipPriceUpdate); }, cancellationTokenSource.Token));
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    Task.WaitAll(priceTasks.ToArray());
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    redemptionMainUserControl.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        redemptionMainUserControlVM.PostPriceUpdate();
                    }));
                    reCalculatePriceInProgress = false;
                });
            }
            log.LogMethodExit();
        }
        internal void ReCalculatePrice(bool fromScroll = false, bool recalculateStock = false, bool skipPriceUpdate = false)
        {
            log.LogMethodEntry(fromScroll, recalculateStock, skipPriceUpdate);
            if (!skipPriceUpdate)
            {
                if (leftPaneSelectedItem == LeftPaneSelectedItem.Redemption && RedemptionUserControlVM != null)
                {
                    if (!fromScroll)
                    {
                        bool callDiscount = false;
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                        if (MembershipIDList != null && MembershipIDList.Any())
                        {
                            foreach (int i in MembershipIDList)
                            {
                                if (MembershipViewContainerList.GetMembershipContainerDTOCollection(ExecutionContext.GetSiteId(), null).MembershipContainerDTOList.Any(x=>x.MembershipId==i && x.RedemptionDiscount>0))
                                {
                                    callDiscount = true;
                                    continue;
                                }
                                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                            }
                            if (callDiscount)
                            {
                                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                                SetDiscountPrice();
                            }
                            else
                            {
                                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                                redemptionMainUserControl.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    if (RedemptionUserControlVM != null && RedemptionUserControlVM.GenericDisplayItemsVM != null && RedemptionUserControlVM.GenericDisplayItemsVM.BackupDisplayItemModels != null
                                    && RedemptionUserControlVM.GenericDisplayItemsVM.BackupDisplayItemModels.Any())
                                    {
                                        RedemptionUserControlVM.GenericDisplayItemsVM.BackupDisplayItemModels.ToList().ForEach(x => x.ItemsSource[0].DiscountValue = string.Empty);
                                    }
                                }));
                            }
                        }
                        else
                        {
                            cancellationTokenSource.Token.ThrowIfCancellationRequested();
                            redemptionMainUserControl.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (RedemptionUserControlVM != null && RedemptionUserControlVM.GenericDisplayItemsVM != null && RedemptionUserControlVM.GenericDisplayItemsVM.BackupDisplayItemModels != null
                                && RedemptionUserControlVM.GenericDisplayItemsVM.BackupDisplayItemModels.Any())
                                {
                                    RedemptionUserControlVM.GenericDisplayItemsVM.BackupDisplayItemModels.ToList().ForEach(x => x.ItemsSource[0].DiscountValue = string.Empty);
                                }
                            }));
                        }
                    }
                    else
                    {
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                        redemptionMainUserControl.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (RedemptionUserControlVM != null && RedemptionUserControlVM.GenericDisplayItemsVM != null && RedemptionUserControlVM.GenericDisplayItemsVM.BackupDisplayItemModels != null
                            && RedemptionUserControlVM.GenericDisplayItemsVM.BackupDisplayItemModels.Any())
                            {
                                RedemptionUserControlVM.GenericDisplayItemsVM.BackupDisplayItemModels.ToList().ForEach(x => x.ItemsSource[0].DiscountValue = string.Empty);
                            }
                        }));
                    }
                }
            }
            log.LogMethodExit();
        }
        internal void PostPriceUpdate()
        {
            log.LogMethodEntry();
            bool isUpdated = false;
            if (this.redemptionDTO != null && redemptionDTO.RedemptionGiftsListDTO != null && redemptionDTO.RedemptionGiftsListDTO.Count > 0)
            {
                foreach (RedemptionGiftsDTO giftsDTO in redemptionDTO.RedemptionGiftsListDTO)
                {
                    ProductsContainerDTO containerDTO = mainVM.GetProductContainerDTOList(ExecutionContext).FirstOrDefault(p =>
                    p.InventoryItemContainerDTO.ProductId == giftsDTO.ProductId);
                    if (containerDTO != null)
                    {
                        decimal discountedPrice = Math.Round(RedemptionPriceViewContainerList.GetLeastPriceInTickets(ExecutionContext.SiteId, containerDTO.ProductId, redemptionMainUserControlVM.MembershipIDList));
                        if (giftsDTO != null)
                        {
                            int price = Convert.ToInt32(discountedPrice);
                            if (price != giftsDTO.Tickets)
                            {
                                isUpdated = true;
                                giftsDTO.Tickets = price;
                                if (RedemptionUserControlVM.GenericTransactionListVM != null &&
                                    RedemptionUserControlVM.GenericTransactionListVM.ItemCollection != null
                                    && RedemptionUserControlVM.GenericTransactionListVM.ItemCollection.Count > 0)
                                {
                                    GenericTransactionListItem transactionListItem = RedemptionUserControlVM.GenericTransactionListVM.ItemCollection.FirstOrDefault(t => t.Key == containerDTO.ProductId);
                                    if (transactionListItem != null)
                                    {
                                        transactionListItem.Ticket = price;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (isUpdated)
            {
                RedemptionUserControlVM.UpdateTicketValues();
                this.SetHeaderCustomerBalanceInfo(null, this.GetBalanceTickets());
            }
            log.LogMethodExit();
        }
        private void OnAddTicketManagerViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                AuthenticateManagerVM managerVM = (sender as AuthenticateManagerView).DataContext as AuthenticateManagerVM;
                if (managerVM.IsValid)
                {
                    redemptionMainUserControlVM.RedemptionActivityManualTicketDTO.ManagerToken = managerVM.ExecutionContext.WebApiToken;
                    if (this.AllocationView != null && this.AllocationView.DataContext != null)
                    {
                        RedemptionTicketAllocationVM ticketAllocationVM = this.AllocationView.DataContext as RedemptionTicketAllocationVM;
                        if (ticketAllocationVM != null)
                        {
                            ticketAllocationVM.SetTicketTypeValues();
                        }
                    }
                    else
                    {
                        OpenTicketAllocation(false, true);
                    }
                }
                else
                {
                    if (this.AllocationView != null)
                    {
                        this.AllocationView.Close();
                    }
                    this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268, null), MessageType.Error);
                    log.Debug("End manual ticket add -manager didnt approve");
                }
            });
            managerView = null;
            SetUserControlFocus();
            log.LogMethodExit();
        }
        internal void OpenManagerView(ManageViewType viewType)
        {
            log.LogMethodEntry(viewType);
            ExecuteActionWithFooter(() =>
            {
                managerView = new AuthenticateManagerView();
                if (managerView.KeyboardHelper != null)
                {
                    this.SetKeyBoardHelperColorCode();
                    managerView.KeyboardHelper.KeypadMouseDownEvent -= UpdateActivityTimeOnAction;
                    managerView.KeyboardHelper.KeypadMouseDownEvent += UpdateActivityTimeOnAction;
                }
                managerView.PreviewMouseDown += UpdateActivityTimeOnMouseOrKeyBoardAction;
                managerView.PreviewKeyDown += UpdateActivityTimeOnMouseOrKeyBoardAction;
                managerView.Loaded += OnWindowLoaded;
                switch (viewType)
                {
                    case ManageViewType.ShortCutKey:
                        {
                            managerView.Closed += OnShortCutKeyApproveManagerViewClosed;
                        }
                        break;
                    case ManageViewType.AddTicket:
                        {
                            managerView.Closed += OnAddTicketManagerViewClosed;
                        }
                        break;
                    case ManageViewType.Flag:
                        {
                            managerView.Closed += OnFlagManagerViewClosed;
                        }
                        break;
                    case ManageViewType.RePrint:
                        {
                            managerView.Closed += OnFlagRePrintManagerViewClosed;
                        }
                        break;
                    case ManageViewType.RedemptionLimit:
                        {
                            if (this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption
                            && RedemptionUserControlVM != null)
                            {
                                managerView.Closed += redemptionUserControlVM.OnManagerViewTicketLimitClosed;
                            }
                            else if (this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket
                            && LoadTicketRedemptionUserControlVM != null)
                            {
                                managerView.Closed += LoadTicketRedemptionUserControlVM.OnManagerViewTicketLimitClosed;
                            }
                            else if (this.leftPaneSelectedItem == LeftPaneSelectedItem.TurnIn
                            && TurnInUserControlVM != null)
                            {
                                managerView.Closed += TurnInUserControlVM.OnManagerViewTicketLimitClosed;
                            }
                        }
                        break;
                    case ManageViewType.LoadBalanceTicketLimit:
                        {
                            if (this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption
                            && RedemptionUserControlVM != null)
                            {
                                managerView.Closed += redemptionUserControlVM.OnManagerViewLoadBalanceLimitClosed;
                            }
                            else if (this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket
                            && LoadTicketRedemptionUserControlVM != null)
                            {
                                managerView.Closed += LoadTicketRedemptionUserControlVM.OnManagerViewLoadBalanceLimitClosed;
                            }
                            else if (this.leftPaneSelectedItem == LeftPaneSelectedItem.TurnIn
                            && TurnInUserControlVM != null)
                            {
                                managerView.Closed += TurnInUserControlVM.OnManagerViewLoadBalanceLimitClosed;
                            }
                        }
                        break;
                    case ManageViewType.PrintBalanceTicketLimit:
                        {
                            if (this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption
                            && RedemptionUserControlVM != null)
                            {
                                managerView.Closed += redemptionUserControlVM.OnManagerViewPrintBalanceLimitClosed;
                            }
                            else if (this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket
                            && LoadTicketRedemptionUserControlVM != null)
                            {
                                managerView.Closed += LoadTicketRedemptionUserControlVM.OnManagerViewPrintBalanceLimitClosed;
                            }
                            else if (this.leftPaneSelectedItem == LeftPaneSelectedItem.TurnIn
                            && TurnInUserControlVM != null)
                            {
                                managerView.Closed += TurnInUserControlVM.OnManagerViewPrintBalanceLimitClosed;
                            }
                        }
                        break;
                    case ManageViewType.LoadtoCardTicketLimit:
                        {
                            if (this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption
                            && RedemptionUserControlVM != null)
                            {
                                managerView.Closed += redemptionUserControlVM.OnManagerViewLoadtoCardLimitClosed;
                            }
                            else if (this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket
                            && LoadTicketRedemptionUserControlVM != null)
                            {
                                managerView.Closed += LoadTicketRedemptionUserControlVM.OnManagerViewLoadtoCardLimitClosed;
                            }
                            else if (this.leftPaneSelectedItem == LeftPaneSelectedItem.TurnIn
                            && TurnInUserControlVM != null)
                            {
                                managerView.Closed += TurnInUserControlVM.OnManagerViewLoadtoCardLimitClosed;
                            }
                        }
                        break;
                    case ManageViewType.PrintConsolidateTicketLimit:
                        {
                            if (this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption
                            && RedemptionUserControlVM != null)
                            {
                                managerView.Closed += redemptionUserControlVM.OnManagerViewPrintConsolidateLimitClosed;
                            }
                            else if (this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket
                            && LoadTicketRedemptionUserControlVM != null)
                            {
                                managerView.Closed += LoadTicketRedemptionUserControlVM.OnManagerViewPrintConsolidateLimitClosed;
                            }
                            else if (this.leftPaneSelectedItem == LeftPaneSelectedItem.TurnIn
                            && TurnInUserControlVM != null)
                            {
                                managerView.Closed += TurnInUserControlVM.OnManagerViewPrintConsolidateLimitClosed;
                            }
                        }
                        break;
                    case ManageViewType.TurninLimit:
                        {
                            if (this.leftPaneSelectedItem == LeftPaneSelectedItem.Redemption
                            && RedemptionUserControlVM != null)
                            {
                                managerView.Closed += redemptionUserControlVM.OnTurninLimitManagerViewClosed;
                            }
                            else if (this.leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket
                            && LoadTicketRedemptionUserControlVM != null)
                            {
                                managerView.Closed += LoadTicketRedemptionUserControlVM.OnTurninLimitManagerViewClosed;
                            }
                            else if (this.leftPaneSelectedItem == LeftPaneSelectedItem.TurnIn
                         && TurnInUserControlVM != null)
                            {
                                managerView.Closed += TurnInUserControlVM.OnTurninLimitManagerViewClosed;
                            }
                        }
                        break;
                }
                AuthenticateManagerVM managerVM = new AuthenticateManagerVM(this.ExecutionContext, this.cardReader)
                {

                };
                managerView.DataContext = managerVM;
                managerView.Show();
            });
            log.LogMethodExit();
        }
        private void OnFlagRePrintManagerViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                AuthenticateManagerVM managerVM = (sender as AuthenticateManagerView).DataContext as AuthenticateManagerVM;
                if (managerVM.IsValid)
                {
                    redemptionActivityDTO.ManagerToken = managerVM.ExecutionContext.WebApiToken;
                    RePrint();
                }
                else
                {
                    this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268, null), MessageType.Error);
                    log.Debug("End reprint task -manager didnt approve");
                }
            });
            managerView = null;
            SetUserControlFocus();
            log.LogMethodExit();
        }
        internal async void RePrint()
        {
            log.LogMethodEntry();
            try
            {
                if (leftPaneSelectedItem == LeftPaneSelectedItem.Voucher && voucherUserControlVM != null)
                {
                    IsLoadingVisible = true;
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                    clsTicket clsTicket = await redemptionUseCases.ReprintManualTicketReceipt((voucherUserControlVM.CustomDataGridVM.SelectedItem as TicketReceiptDTO).Id, redemptionActivityDTO);
                    bool result = POSPrintHelper.PrintTicketsToPrinter(ExecutionContext, new List<clsTicket>() { clsTicket }, redemptionMainUserControlVM.ScreenNumber.ToString());
                    if (!result)
                    {
                        redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Print Error"), MessageType.Error);
                        IsLoadingVisible = false;
                        return;
                    }
                    else
                    {
                        int index = -1;
                        voucherUserControlVM.ReceiptDTO = voucherUserControlVM.GetSelectedVoucher(ref index);
                        voucherUserControlVM.ReceiptDTO.ReprintCount += 1;
                        voucherUserControlVM.AddToVoucherList(index);
                        voucherUserControlVM.SetOtherRedemptionList();
                    }
                    log.LogMethodExit();
                }
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
                ShowRelogin(ExecutionContext.GetUserId());
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                SetFooterContent(pax.Message.ToString(), MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + ex.Message, MessageType.Error);
            }
            finally
            {
                IsLoadingVisible = false;
            }
            log.LogMethodExit();
        }
        private void OnFlagManagerViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                AuthenticateManagerVM managerVM = (sender as AuthenticateManagerView).DataContext as AuthenticateManagerVM;
                if (managerVM.IsValid)
                {
                    this.ShowFlagOrEnterTicketView(true);
                }
                else
                {
                    this.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268, null), MessageType.Error);
                    log.Debug("End flag voucher -manager didnt approve");
                }
            });
            managerView = null;
            SetUserControlFocus();
            log.LogMethodExit();
        }
        private async void OnFlagDataEntryViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            GenericDataEntryVM dataEntryVM = (sender as GenericDataEntryView).DataContext as GenericDataEntryVM;
            if (dataEntryVM != null && dataEntryVM.ButtonClickType == ButtonClickType.Ok && dataEntryVM.DataEntryCollections != null
                && dataEntryVM.DataEntryCollections.Count > 0 && dataEntryVM.DataEntryCollections[0].Type == DataEntryType.TextBox
                && this.leftPaneSelectedItem == LeftPaneSelectedItem.Voucher)
            {
                string text = dataEntryVM.DataEntryCollections[0].Text;
                await voucherUserControlVM.UpdateVoucherUI(text);
            }
            SetUserControlFocus();
            log.LogMethodExit();
        }
        internal void SetNewRedemption()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                redemptionDTO = new RedemptionDTO();
                NewRedemptionDTO = new RedemptionDTO();
                retreivedBackupDTO = null;
                redemptionActivityDTO = new RedemptionActivityDTO();
                redemptionActivityManualTicketDTO = new RedemptionActivityDTO();
                redemptionLoadToCardRequestDTO = new RedemptionLoadToCardRequestDTO();
                membershipIDList = new List<int>();
                membershipIDcardIDList = new Dictionary<int, int>();
                CardIDcustomerIDList = new Dictionary<int, int>();
                customerIDcustomerInfoList = new Dictionary<int, string>();
                scanTicketGiftmode = (char)0;
                redemptionDTO.Source = "POS Redemption";
                redemptionActivityDTO.Source = "POS Redemption";
                redemptionActivityManualTicketDTO.Source = "POS Redemption";
                redemptionLoadToCardRequestDTO.Source = "POS Redemption";
                CallRecalculatePriceandStock(true);
                if (turnInUserControlVM != null && turnInUserControlVM.IsTurnIn)
                {
                    redemptionDTO.Remarks = "TURNINREDEMPTION";
                }
                else
                {
                    redemptionDTO.Remarks = string.Empty;
                }
            });
            log.LogMethodExit();
        }
        private void SetLoadTicket()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                int siteId = ExecutionContext.GetSiteId();
                bool enableLoadTicketMenu = false;
                UserContainerDTO userContainerDTO = UserViewContainerList.GetUserContainerDTO(siteId, ExecutionContext.UserId);
                if(userContainerDTO != null)
                {
                    int roleId = userContainerDTO.RoleId;
                    enableLoadTicketMenu = UserRoleViewContainerList.CheckAccess(siteId, roleId, "Load Ticket Menu");
                    //enableLoadToCard = UserRoleViewContainerList.GetUserRoleContainerDTOList(ExecutionContext).FirstOrDefault(x => x.RoleId == UserViewContainerList.GetUserContainerDTO(ExecutionContext.GetSiteId(), ExecutionContext.UserId).RoleId).ManagementFormAccessContainerDTOList.FirstOrDefault(y => y.FormName == "Load To Card in Redemption").AccessAllowed;
                    enableLoadToCard = UserRoleViewContainerList.CheckAccess(siteId, roleId, "Load To Card in Redemption");
                    //enableConsolidatePrint = UserRoleViewContainerList.GetUserRoleContainerDTOList(ExecutionContext).FirstOrDefault(x => x.RoleId == UserViewContainerList.GetUserContainerDTO(ExecutionContext.GetSiteId(), ExecutionContext.UserId).RoleId).ManagementFormAccessContainerDTOList.FirstOrDefault(y => y.FormName == "Consolidated Ticket Print").AccessAllowed;
                    enableConsolidatePrint = UserRoleViewContainerList.CheckAccess(siteId, roleId, "Consolidated Ticket Print");
                }
                if (!enableLoadTicketMenu || (!enableLoadToCard && !enableConsolidatePrint))
                {
                    log.LogMethodExit("User does not have access to load ticket.");
                    return;
                }
                LeftPaneVM.MenuItems.Add(MessageViewContainerList.GetMessage(ExecutionContext, "Load Ticket"));
                AutoShowLoadTicketProductMenu = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AUTO_SHOW_CURRENCY_MENU_IN_SINGLE_SCREEN", false);
                loadTicketRedemptionUserControlVM = new RedemptionUserControlVM(this.ExecutionContext)
                {
                    IsLoadTicket = true,
                    GenericToggleButtonsVM = new GenericToggleButtonsVM()
                    {
                        ToggleButtonItems = new ObservableCollection<CustomToggleButtonItem>()
                        {
                            new CustomToggleButtonItem()
                            {
                                DisplayTags = new ObservableCollection<DisplayTag>()
                                {
                                    new DisplayTag(){ Text = MessageViewContainerList.GetMessage(ExecutionContext,"New")},
                                },
                                Key = "New"
                            },
                            new CustomToggleButtonItem()
                            {
                                DisplayTags = new ObservableCollection<DisplayTag>()
                                {
                                    new DisplayTag()
                                    {
                                        Text = MessageViewContainerList.GetMessage(ExecutionContext,"Completed"),
                                        FontWeight = FontWeights.Bold,
                                    },
                                    new DisplayTag()
                                    {
                                        Text = "0",
                                        TextSize = TextSize.Medium,
                                    },
                                },
                                Key = "Completed"
                            },
                        }
                    },
                };
            });
            log.LogMethodExit();
        }
        private void SetTurnIn()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                //enableTurnIn =
                //UserRoleViewContainerList.GetUserRoleContainerDTOList(ExecutionContext).FirstOrDefault(x => x.RoleId == UserViewContainerList.GetUserContainerDTO(ExecutionContext.GetSiteId(), ExecutionContext.UserId).RoleId).ManagementFormAccessContainerDTOList.FirstOrDefault(y => y.FormName == "Turn-In Redemption").AccessAllowed;
                enableTurnIn = UserRoleViewContainerList.CheckAccess(ExecutionContext.GetSiteId(), UserViewContainerList.GetUserContainerDTO(ExecutionContext.GetSiteId(), ExecutionContext.UserId).RoleId, "Turn-In Redemption");
                if (enableTurnIn)
                {
                    LeftPaneVM.MenuItems.Add(MessageViewContainerList.GetMessage(ExecutionContext, "Turn In"));
                    turnInUserControlVM = new RedemptionUserControlVM(this.ExecutionContext)
                    {
                        IsTurnIn = true,
                        GenericToggleButtonsVM = new GenericToggleButtonsVM()
                        {
                            ToggleButtonItems = new ObservableCollection<CustomToggleButtonItem>()
                        {
                            new CustomToggleButtonItem()
                            {
                                DisplayTags = new ObservableCollection<DisplayTag>()
                                {
                                     new DisplayTag(){ Text = MessageViewContainerList.GetMessage(ExecutionContext,"New")},
                                },
                                Key = "New"
                            },
                            new CustomToggleButtonItem()
                            {
                                DisplayTags = new ObservableCollection<DisplayTag>()
                                {
                                    new DisplayTag()
                                    {
                                        Text = MessageViewContainerList.GetMessage(ExecutionContext,"Completed"),
                                        FontWeight = FontWeights.Bold,
                                    },
                                    new DisplayTag()
                                    {
                                        Text = "0",
                                        TextSize = TextSize.Medium,
                                    },
                                },
                                Key = "Completed"
                            },
                        }
                        },
                    };
                    turnInUserControlVM.TargetLocations = new ObservableCollection<LocationContainerDTO>(LocationViewContainerList.GetLocationContainerDTOList(this.ExecutionContext).OrderBy(x => x.Name));
                }
            });
            log.LogMethodExit();
        }
        private void SetVouchers()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                enableRePrint = UserRoleViewContainerList.CheckAccess(ExecutionContext.GetSiteId(), UserViewContainerList.GetUserContainerDTO(ExecutionContext.GetSiteId(), ExecutionContext.UserId).RoleId, "Reprint Ticket Receipt");

                enableUnflagVoucher = UserRoleViewContainerList.CheckAccess(ExecutionContext.GetSiteId(), UserViewContainerList.GetUserContainerDTO(ExecutionContext.GetSiteId(), ExecutionContext.UserId).RoleId, "Flag Voucher");
                //enableRePrint =
                //UserRoleViewContainerList.GetUserRoleContainerDTOList(ExecutionContext).FirstOrDefault(x => x.RoleId == UserViewContainerList.GetUserContainerDTO(ExecutionContext.GetSiteId(), ExecutionContext.UserId).RoleId).ManagementFormAccessContainerDTOList.FirstOrDefault(y => y.FormName == "Reprint Ticket Receipt").AccessAllowed;
                //enableRePrint =
                //UserRoleViewContainerList.GetUserRoleContainerDTOList(ExecutionContext).FirstOrDefault(x => x.RoleId == UserViewContainerList.GetUserContainerDTO(ExecutionContext.GetSiteId(), ExecutionContext.UserId).RoleId).ManagementFormAccessContainerDTOList.FirstOrDefault(y => y.FormName == "Flag Voucher").AccessAllowed;
                if (!enableRePrint && !enableUnflagVoucher)
                {
                    return;
                }
                LeftPaneVM.MenuItems.Add(MessageViewContainerList.GetMessage(ExecutionContext, "Voucher"));
                voucherUserControlVM = new RedemptionUserControlVM(this.ExecutionContext)
                {
                    IsVoucher = true,
                    GenericToggleButtonsVM = new GenericToggleButtonsVM()
                    {
                        ToggleButtonItems = new ObservableCollection<CustomToggleButtonItem>()
                        {
                            new CustomToggleButtonItem()
                            {
                                DisplayTags = new ObservableCollection<DisplayTag>()
                                {
                                    new DisplayTag()
                                    {
                                        Text = MessageViewContainerList.GetMessage(ExecutionContext,"Vouchers")
                                    },
                                },
                                Key = "Vouchers"
                            },
                        }
                    },
                };
                if (enableUnflagVoucher)
                {
                    voucherUserControlVM.GenericToggleButtonsVM.ToggleButtonItems.Add(new CustomToggleButtonItem()
                    {
                        DisplayTags = new ObservableCollection<DisplayTag>()
                                    {
                                        new DisplayTag()
                                        {
                                            Text = MessageViewContainerList.GetMessage(ExecutionContext,"Flagged"),
                                            FontWeight = FontWeights.Bold,
                                        },
                                        new DisplayTag()
                                        {
                                            Text = "0",
                                            TextSize = TextSize.Medium,
                                        },
                                    },
                        Key = "Flagged"
                    });
                }
                voucherUserControlVM.SetDefaultCollections(false);
            });
            log.LogMethodExit();
        }
        internal void SetKeyboardWindow()
        {
            log.LogMethodEntry();
            //OnFooterLoaded(footerUserControl);
            if (footerUserControl != null && redemptionMainUserControl != null)
            {
                redemptionMainUserControl.KeyboardHelper.ShowKeyBoard(this.redemptionMainUserControl, new List<Control>() { footerUserControl.btnKeyboard },
                    ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                SetKeyBoardHelperColorCode();
            }
            log.LogMethodExit();
        }
        private void OnFooterLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithFooter(() =>
            {
                if (parameter != null)
                {
                    footerUserControl = parameter as FooterUserControl;
                }
            });
            log.LogMethodExit();
        }
        internal void SetKeyBoardHelperColorCode()
        {
            log.LogMethodEntry();
            if (mainVM != null)
            {
                bool moreUsers = mainVM.RedemptionUserControlVMs != null && mainVM.RedemptionUserControlVMs.Select(r => r.UserName).Distinct().ToList().Count > 1;
                if (redemptionMainUserControl != null && redemptionMainUserControl.KeyboardHelper != null)
                {
                    redemptionMainUserControl.KeyboardHelper.NumericUpDownAssigning = numericUpDownAssigning;
                    if (moreUsers)
                    {
                        redemptionMainUserControl.KeyboardHelper.MultiScreenMode = multiScreenMode;
                        redemptionMainUserControl.KeyboardHelper.ColorCode = colorCode;
                    }
                    else if (redemptionMainUserControl.KeyboardHelper.MultiScreenMode && !multiScreenMode)
                    {
                        redemptionMainUserControl.KeyboardHelper.MultiScreenMode = false;
                    }
                }
                if (this.AllocationView != null && this.AllocationView.KeyBoardHelper != null)
                {
                    this.AllocationView.KeyBoardHelper.NumericUpDownAssigning = numericUpDownAssigning;
                    if (moreUsers)
                    {
                        this.AllocationView.KeyBoardHelper.MultiScreenMode = multiScreenMode;
                        this.AllocationView.KeyBoardHelper.ColorCode = colorCode;
                    }
                    else if (AllocationView.KeyBoardHelper.MultiScreenMode && !multiScreenMode)
                    {
                        AllocationView.KeyBoardHelper.MultiScreenMode = false;
                    }
                }
                if (this.FlagOrEnterTicketView != null && FlagOrEnterTicketView.KeyBoardHelper != null)
                {
                    this.FlagOrEnterTicketView.KeyBoardHelper.NumericUpDownAssigning = numericUpDownAssigning;
                    if (moreUsers)
                    {
                        this.FlagOrEnterTicketView.KeyBoardHelper.MultiScreenMode = multiScreenMode;
                        this.FlagOrEnterTicketView.KeyBoardHelper.ColorCode = colorCode;
                    }
                    else if (FlagOrEnterTicketView.KeyBoardHelper.MultiScreenMode && !multiScreenMode)
                    {
                        FlagOrEnterTicketView.KeyBoardHelper.MultiScreenMode = false;
                    }
                }
                if (this.userView != null && userView.KeyboardHelper != null)
                {
                    userView.KeyboardHelper.NumericUpDownAssigning = numericUpDownAssigning;
                    if (moreUsers)
                    {
                        userView.KeyboardHelper.MultiScreenMode = multiScreenMode;
                        userView.KeyboardHelper.ColorCode = colorCode;
                    }
                    else if (userView.KeyboardHelper.MultiScreenMode && !multiScreenMode)
                    {
                        userView.KeyboardHelper.MultiScreenMode = false;
                    }
                }
                if (this.managerView != null && managerView.KeyboardHelper != null)
                {
                    managerView.KeyboardHelper.NumericUpDownAssigning = numericUpDownAssigning;
                    if (moreUsers)
                    {
                        managerView.KeyboardHelper.MultiScreenMode = multiScreenMode;
                        managerView.KeyboardHelper.ColorCode = colorCode;
                    }
                    else if (managerView.KeyboardHelper.MultiScreenMode && !multiScreenMode)
                    {
                        managerView.KeyboardHelper.MultiScreenMode = false;
                    }
                }
                if (this.ItemInfoPopUpView != null && ItemInfoPopUpView.KeyBoardHelper != null)
                {
                    if (moreUsers)
                    {
                        this.ItemInfoPopUpView.KeyBoardHelper.MultiScreenMode = multiScreenMode;
                        this.ItemInfoPopUpView.KeyBoardHelper.ColorCode = colorCode;
                    }
                    else if (ItemInfoPopUpView.KeyBoardHelper.MultiScreenMode && !multiScreenMode)
                    {
                        ItemInfoPopUpView.KeyBoardHelper.MultiScreenMode = false;
                    }
                }
                if (this.DataEntryView != null && DataEntryView.KeyBoardHelper != null)
                {
                    if (moreUsers)
                    {
                        this.DataEntryView.KeyBoardHelper.MultiScreenMode = multiScreenMode;
                        this.DataEntryView.KeyBoardHelper.ColorCode = colorCode;
                    }
                    else if (DataEntryView.KeyBoardHelper.MultiScreenMode && !multiScreenMode)
                    {
                        DataEntryView.KeyBoardHelper.MultiScreenMode = false;
                    }
                }
                if (this.UpdateView != null && UpdateView.KeyboardHelper != null)
                {
                    this.UpdateView.KeyboardHelper.NumericUpDownAssigning = numericUpDownAssigning;
                    if (moreUsers)
                    {
                        this.UpdateView.KeyboardHelper.MultiScreenMode = multiScreenMode;
                        this.UpdateView.KeyboardHelper.ColorCode = colorCode;
                    }
                    else if (UpdateView.KeyboardHelper.MultiScreenMode && !multiScreenMode)
                    {
                        UpdateView.KeyboardHelper.MultiScreenMode = false;
                    }
                }
            }
            log.LogMethodExit();
        }
        internal new void ExecuteActionWithFooter(Action method)
        {
            log.LogMethodEntry();
            try
            {
                method();
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                SetFooterContent(vex.Message.ToString(), MessageType.Error);
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                ShowRelogin(ExecutionContext.GetUserId());
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
                log.LogMethodExit();
            }
        }
        internal NumberKeyboardView GetNumberPadView(Window parentWindow, int quantity = 0, string barCode = "")
        {
            log.LogMethodEntry(quantity);
            if (!string.IsNullOrEmpty(barCode) && barCode.ToLower() == "CHQTY".ToLower())
            {
                switch (lastScannedType)
                {
                    case 'G':
                        if (allocationView != null)
                        {
                            log.Info("Closing the page");
                            RedemptionTicketAllocationVM ticketAllocationVM = allocationView.DataContext as RedemptionTicketAllocationVM;
                            if (ticketAllocationVM != null)
                            {
                                ticketAllocationVM.CloseCommand.Execute(allocationView);
                            }
                        }
                        break;
                    case 'C':
                        log.Info("Already numberpad is opened.");
                        break;
                }
            }
            CloseNumberPad();
            numberKeyboardView = new NumberKeyboardView();
            NumberKeyboardVM numberKeyboardVM = new NumberKeyboardVM()
            {
                NumberText = quantity > 0 ? quantity.ToString() : string.Empty,
                NumberKeyboardView = numberKeyboardView
            };
            numberKeyboardView.DataContext = numberKeyboardVM;
            numberKeyboardView.PreviewMouseDown += UpdateActivityTimeOnMouseOrKeyBoardAction;
            numberKeyboardView.PreviewKeyDown += UpdateActivityTimeOnMouseOrKeyBoardAction;
            OnWindowLoaded(numberKeyboardView, null);
            if (parentWindow != null)
            {
                numberKeyboardView.Owner = parentWindow;
            }
            log.LogMethodExit(numberKeyboardView);
            return numberKeyboardView;
        }
        internal void CloseNumberPad()
        {
            log.LogMethodEntry();
            if (numberKeyboardView != null)
            {
                numberKeyboardView.Close();
                numberKeyboardView = null;
            }
            log.LogMethodExit();
        }
        private double GetFooterHeight()
        {
            log.LogMethodEntry();
            double footerHeight = multiScreenMode ? 36 : 72;
            if (redemptionMainUserControl != null && redemptionMainUserControl.FooterContentControl != null)
            {
                footerHeight = redemptionMainUserControl.FooterContentControl.ActualHeight;
            }
            log.LogMethodExit(footerHeight);
            return footerHeight;
        }
        private void HookFooterMouseDownEvent()
        {
            log.LogMethodEntry();
            if (redemptionMainUserControl == null)
            {
                log.LogMethodExit("redemptionMainUserControl is null");
                return;
            }
            if (redemptionMainUserControl.FooterContentControl == null)
            {
                log.LogMethodExit("redemptionMainUserControl.FooterContentControl is null");
                return;
            }
            redemptionMainUserControl.FooterContentControl.PreviewMouseDown -= OnFooterContentControlPreviewMouseDown;
            redemptionMainUserControl.FooterContentControl.PreviewMouseDown += OnFooterContentControlPreviewMouseDown;
            log.LogMethodExit();
        }
        private void OnFooterContentControlPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (IsWindowVisible(userView) || IsWindowVisible(managerView) || IsWindowVisible(reverseView) || IsWindowVisible(updateView)
                || IsWindowVisible(allocationView) || IsWindowVisible(scanView) || IsWindowVisible(messagePopupView)
                || IsWindowVisible(dataEntryView) || IsWindowVisible(scanPopupView) || IsWindowVisible(itemInfoPopUpView)
                || IsWindowVisible(changePasswordView) || IsWindowVisible(datePickerView) || IsWindowVisible(enterTicketView))
            {
                e.Handled = true;
            }
            log.LogMethodExit();
        }
        private bool IsWindowVisible(Window window)
        {
            log.LogMethodEntry(window);
            bool isWindowVisible = window != null && window.IsVisible;
            log.LogMethodExit(isWindowVisible);
            return isWindowVisible;
        }
        #endregion

        #region Constructor
        public RedemptionMainUserControlVM(Semnox.Core.Utilities.ExecutionContext executionContext, bool isActive, string userName, int screenNumber, DeviceClass cardReader, DeviceClass barcodeReader, int carddeviceaddress, int barcodedeviceaddress,
            bool singleuserMultiscreen, bool multiuserMultiscreen, ColorCode colorCode)
        {
            log.LogMethodEntry();
            this.ExecutionContext = executionContext;
            newSelectedLeftPaneItem = string.Empty;
            this.userName = userName;
            this.screenNumber = screenNumber;
            this.cardReader = cardReader;
            this.barcodeReader = barcodeReader;
            this.carddeviceaddress = carddeviceaddress;
            this.barcodedeviceaddress = barcodedeviceaddress;
            this.singleUserMultiscreen = singleuserMultiscreen;
            this.multiUserMultiscreen = multiuserMultiscreen;
            this.ColorCode = colorCode;
            this.IsActive = isActive;
            this.lastActivityTime = DateTime.Now;
            this.newOrCloseScreen = 'N';
            PropertyChanged += OnPropertyChanged;
            tagNumberViewParser = new TagNumberViewParser(ExecutionContext);
            ExecuteActionWithFooter(() =>
            {
                enableSuspend = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ENABLE_SUSPEND_IN_REDEMPTION", true);
                posInActivityTime = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "POS_INACTIVE_TIMEOUT", 0) * 60;
                reLoginRequired = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "RELOGIN_USER_AFTER_INACTIVE_TIMEOUT", false);
                enableManualTicket = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ENABLE_MANUAL_TICKET_IN_REDEMPTION", false);
                if (reLoginRequired)
                {
                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += CheckReloginRequired;
                    timer.Start();
                }
                bool hideScroll = false;
                if (SystemParameters.PrimaryScreenWidth >= 1000)
                {
                    hideScroll = ParafaitDefaultViewContainerList.GetParafaitDefault(this.ExecutionContext, "FULL_SCREEN_POS") == "Y" ? false : true;
                }
                LeftPaneVM = new LeftPaneVM(executionContext)
                {
                    SearchVisibility = Visibility.Collapsed,
                    RemoveButtonVisiblity = true,
                    ScreenName = this.screenNumber.ToString(),
                    ModuleName = this.userName,
                    ColorCode = this.ColorCode,
                    HideScroll = hideScroll
                };
                LeftPaneVM.MenuItems.Add(MessageViewContainerList.GetMessage(ExecutionContext, "Redemption"));
                if (isActive)
                {
                    LeftPaneVM.AddButtonVisiblity = isActive;
                }
                if (!singleuserMultiscreen && !multiuserMultiscreen && LeftPaneVM.AddButtonVisiblity)
                {
                    LeftPaneVM.AddButtonVisiblity = false;
                }
                LeftPaneMenuSelectedCommand = new DelegateCommand(OnLeftPaneMenuSelected);
                sidebarClickedCommand = new DelegateCommand(OnFooterSideBarButtonClicked);
                loadedCommand = new DelegateCommand(OnLoaded);
                footerMessageClickedCommand = new DelegateCommand(OnFooterMessageClicked);
                FooterVM = new FooterVM(executionContext)
                {
                    MessageType = MessageType.None,
                };
                multiScreenMode = false;
                redemptionUserControlVM = new RedemptionUserControlVM(this.ExecutionContext)
                {
                    GenericToggleButtonsVM = new GenericToggleButtonsVM()
                    {
                        ToggleButtonItems = new ObservableCollection<CustomToggleButtonItem>()
                        {
                            new CustomToggleButtonItem()
                            {
                                DisplayTags = new ObservableCollection<DisplayTag>()
                                {
                                    new DisplayTag()
                                    {
                                        Text = MessageViewContainerList.GetMessage(ExecutionContext,"New")
                                    },
                                },
                                Key = "New",
                            },
                            new CustomToggleButtonItem()
                            {
                                DisplayTags = new ObservableCollection<DisplayTag>()
                                {
                                    new DisplayTag()
                                    {
                                        Text = MessageViewContainerList.GetMessage(ExecutionContext,"Suspended"),
                                        FontWeight = FontWeights.Bold,
                                    },
                                    new DisplayTag(){
                                        Text = "0",
                                        TextSize = TextSize.Medium,
                                    },
                                },
                                Key = "Suspended"
                            },
                            new CustomToggleButtonItem()
                            {
                                DisplayTags = new ObservableCollection<DisplayTag>()
                                {
                                    new DisplayTag()
                                    {
                                        Text = MessageViewContainerList.GetMessage(ExecutionContext,"Completed"),
                                        FontWeight = FontWeights.Bold,
                                    },
                                    new DisplayTag()
                                    {
                                        Text = "0",
                                        TextSize = TextSize.Medium,
                                    },
                                },
                                Key = "Completed"
                            },
                        }
                    },

                    RightSectionDisplayTagsVM = new DisplayTagsVM()
                    {
                        DisplayTags = new ObservableCollection<ObservableCollection<DisplayTag>>()
                        {
                             new ObservableCollection<DisplayTag>()
                             {
                                 new DisplayTag()
                                 {
                                     Text = MessageViewContainerList.GetMessage(ExecutionContext,"Total Tkt"),
                                     Type = DisplayTagType.Button
                                 },
                                 new DisplayTag()
                                 {
                                     Text ="0",
                                     FontWeight = FontWeights.Bold,
                                     Type = DisplayTagType.Button
                                 }
                             },
                             new ObservableCollection<DisplayTag>()
                             {
                                 new DisplayTag()
                                 {
                                     Text = MessageViewContainerList.GetMessage(ExecutionContext,"Redeemed")
                                 },
                                 new DisplayTag()
                                 {
                                     Text ="0", FontWeight = FontWeights.Bold
                                 }
                             },
                             new ObservableCollection<DisplayTag>()
                             {
                                 new DisplayTag()
                                 {
                                     Text = MessageViewContainerList.GetMessage(ExecutionContext,"Balance")
                                 },
                                 new DisplayTag()
                                 {
                                     Text ="0", FontWeight = FontWeights.Bold
                                 }
                             },
                        },
                    }
                };
                AutoShowRedemptionProductMenu = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AUTO_SHOW_PRODUCT_MENU_IN_SINGLE_SCREEN", false);
                if (ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "REDEMPTION_GRACE_TICKETS", 0) > 0 ||
                ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "REDEMPTION_GRACE_TICKETS_PERCENTAGE", 0) > 0)
                {
                    redemptionUserControlVM.RightSectionDisplayTagsVM.DisplayTags.Add(new ObservableCollection<DisplayTag>()
                             {
                                 new DisplayTag()
                                 {
                                     Text = MessageViewContainerList.GetMessage(ExecutionContext,"Grace")
                                 },
                                 new DisplayTag()
                                 {
                                     Text ="0", FontWeight = FontWeights.Bold
                                 }
                             });
                }
                redemptionUserControlVM.GenericToggleButtonsVM = new GenericToggleButtonsVM()
                {
                    ToggleButtonItems = new ObservableCollection<CustomToggleButtonItem>()
                };
                redemptionUserControlVM.GenericToggleButtonsVM.ToggleButtonItems.Add(new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                                {
                                    new DisplayTag()
                                    {
                                        Text = MessageViewContainerList.GetMessage(ExecutionContext,"New")
                                    },
                                },
                    Key = "New",
                });
                if (enableSuspend)
                {
                    redemptionUserControlVM.GenericToggleButtonsVM.ToggleButtonItems.Add(new CustomToggleButtonItem()
                    {
                        DisplayTags = new ObservableCollection<DisplayTag>()
                                    {
                                        new DisplayTag()
                                        {
                                            Text = MessageViewContainerList.GetMessage(ExecutionContext,"Suspended"),
                                            FontWeight = FontWeights.Bold,
                                        },
                                        new DisplayTag(){
                                            Text = "0",
                                            TextSize = TextSize.Medium,
                                        },
                                    },
                        Key = "Suspended",
                    });
                }
                redemptionUserControlVM.GenericToggleButtonsVM.ToggleButtonItems.Add(new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                                {
                                    new DisplayTag()
                                    {
                                        Text = MessageViewContainerList.GetMessage(ExecutionContext,"Completed"),
                                        FontWeight = FontWeights.Bold,
                                    },
                                    new DisplayTag()
                                    {
                                        Text = "0",
                                        TextSize = TextSize.Medium,
                                    },
                                },
                    Key = "Completed"
                });
                redemptionUserControlVM.SetDefaultCollections(true);
                SetLoadTicket();
                SetTurnIn();
                SetVouchers();
                addButtonClickedCommand = new DelegateCommand(OnAddCliked);
                footerLoadedCommand = new DelegateCommand(OnFooterLoaded);
                removeButtonClickedCommand = new DelegateCommand(OnRemoveClicked);
                cancellationTokenSource = new CancellationTokenSource();
                SetNewRedemption();
            });
            log.LogMethodExit();
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!string.IsNullOrWhiteSpace(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case "IsLoadingVisible":
                        RaiseCanExecuteChanged();
                        break;
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
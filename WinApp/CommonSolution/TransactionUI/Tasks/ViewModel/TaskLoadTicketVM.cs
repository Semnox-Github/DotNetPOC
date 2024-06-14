/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Load tickets View model
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Threading;
using Semnox.Parafait.Printer;
using Semnox.Core.Utilities;
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.TransactionUI
{
    public class TaskLoadTicketVM : TaskBaseViewModel
    {

        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CardDetailsVM cardDetailsVM;
        private CustomDataGridVM customDataGridVM;
        private ICommand loaded;
        private ICommand cardAddedCommand;
        private ICommand navigationClickCommand;
        private ICommand deleteCommand;
        private ICommand deleteAllCommand;
        private ICommand rboScanTicketClick;
        private ICommand manualTicketEntryClicked;
        private int totalTickets;
        private int manualTickets;
        private bool isRboEnterManualTicketsChecked;
        private bool isRboScanTicketsChecked;
        private string rboEnterTicketContent;
        private string rboScanTicketContent;
        private DeviceClass barcodeReader;
        private int totalScannedTickets;
        private List<TicketReceiptDTO> ticketReceiptDTOs;
        private TaskLoadTicketView taskLoadTicketView;
        private bool showTicketSourceScreen;
        private Dispatcher dispatcher;
        private bool isCardValid;
        private bool isEnabledManualTicketEntry;
        private string ticketNameVariant;
        private GenericScanPopupView genericScanPopupView;
        private string managerToken = string.Empty;
        private bool considerForLoyalty;
        #endregion

        #region Properties       
        public bool ConsiderForLoyalty
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(considerForLoyalty);
                return considerForLoyalty;
            }
            set
            {
                log.LogMethodEntry(considerForLoyalty, value);
                SetProperty(ref considerForLoyalty, value);
                log.LogMethodExit(considerForLoyalty);
            }
        }
        public string RboEnterTicketContent
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rboEnterTicketContent);
                return rboEnterTicketContent;
            }
            set
            {
                log.LogMethodEntry(rboEnterTicketContent, value);
                SetProperty(ref rboEnterTicketContent, value);
                log.LogMethodExit(rboEnterTicketContent);
            }
        }
        public string RboScanTicketContent
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rboScanTicketContent);
                return rboScanTicketContent;
            }
            set
            {
                log.LogMethodEntry(rboScanTicketContent, value);
                SetProperty(ref rboScanTicketContent, value);
                log.LogMethodExit(rboScanTicketContent);
            }
        }
        public ICommand ManualTicketEntryClicked
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(manualTicketEntryClicked);
                return manualTicketEntryClicked;
            }
            set
            {
                log.LogMethodEntry(manualTicketEntryClicked, value);
                SetProperty(ref manualTicketEntryClicked, value);
                log.LogMethodExit(manualTicketEntryClicked);
            }
        }
        public string TicketNameVariant
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ticketNameVariant);
                return ticketNameVariant;
            }
            set
            {
                log.LogMethodEntry(ticketNameVariant, value);
                SetProperty(ref ticketNameVariant, value);
                log.LogMethodExit(ticketNameVariant);
            }
        }
        public bool ShowTicketSourceScreen
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showTicketSourceScreen);
                return showTicketSourceScreen;
            }
            set
            {
                log.LogMethodEntry(showTicketSourceScreen, value);
                SetProperty(ref showTicketSourceScreen, value);
                log.LogMethodExit(showTicketSourceScreen);
            }
        }

        public int TotalScannedTickets
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(totalScannedTickets);
                return totalScannedTickets;
            }
            set
            {
                log.LogMethodEntry(totalScannedTickets, value);
                SetProperty(ref totalScannedTickets, value);
                log.LogMethodExit(totalScannedTickets);
            }
        }
        public bool IsRboEnterManualTicketsChecked
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isRboEnterManualTicketsChecked);
                return isRboEnterManualTicketsChecked;
            }
            set
            {
                log.LogMethodEntry(isRboEnterManualTicketsChecked, value);
                SetProperty(ref isRboEnterManualTicketsChecked, value);
                if(CustomDataGridVM != null && value)
                {
                    if(ticketReceiptDTOs == null)
                    {
                        ticketReceiptDTOs = new List<TicketReceiptDTO>();
                    }
                    ticketReceiptDTOs.Clear();
                    CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>();
                    TotalScannedTickets = 0;
                    SetFooterContent(string.Empty, MessageType.None);
                }
            }
        }
        public bool IsRboScanTicketsChecked
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isRboScanTicketsChecked);
                return isRboScanTicketsChecked;
            }
            set
            {
                log.LogMethodEntry(isRboScanTicketsChecked, value);
                SetProperty(ref isRboScanTicketsChecked, value);
                log.LogMethodExit(isRboScanTicketsChecked);
            }
        }

        public ICommand RboScanTicketClick
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rboScanTicketClick);
                return rboScanTicketClick;
            }
            set
            {
                log.LogMethodEntry(rboScanTicketClick, value);
                SetProperty(ref rboScanTicketClick, value);
                log.LogMethodExit(rboScanTicketClick);
            }
        }
        public int ManualTickets
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(manualTickets);
                return manualTickets;
            }
            set
            {
                log.LogMethodEntry(manualTickets, value);
                SetProperty(ref manualTickets, value);
                totalTickets = manualTickets;
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
        public ICommand NavigationClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(navigationClickCommand);
                return navigationClickCommand;
            }
            set
            {
                log.LogMethodEntry(navigationClickCommand,value);
                SetProperty(ref navigationClickCommand, value);
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
                log.LogMethodEntry(cardDetailsVM, value);
                SetProperty(ref cardDetailsVM, value);
                log.LogMethodExit(cardDetailsVM);
            }
        }
        public ICommand Loaded
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loaded);
                return loaded;
            }
            set
            {
                log.LogMethodEntry(loaded, value);
                SetProperty(ref loaded, value);
                log.LogMethodExit(loaded);
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

        public ICommand DeleteCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(deleteCommand);
                return deleteCommand;
            }
            set
            {
                log.LogMethodEntry(deleteCommand, value);
                SetProperty(ref deleteCommand, value);
                log.LogMethodExit(deleteCommand);
            }
        }
        public ICommand DeleteAllCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(deleteAllCommand);
                return deleteAllCommand;
            }
            set
            {
                log.LogMethodEntry(deleteAllCommand, value);
                SetProperty(ref deleteAllCommand, value);
                log.LogMethodExit(deleteAllCommand);
            }
        }
        public bool IsEnabledManualTicketEntry
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isEnabledManualTicketEntry);
                return isEnabledManualTicketEntry;
            }
            set
            {
                log.LogMethodEntry(isEnabledManualTicketEntry, value);
                SetProperty(ref isEnabledManualTicketEntry, value);
                log.LogMethodExit(isEnabledManualTicketEntry);
            }
        }
        #endregion

        #region Constructor
        public TaskLoadTicketVM(ExecutionContext executionContext, DeviceClass cardReader, DeviceClass barcodeReader) : base(executionContext, cardReader)
        {
            this.PropertyChanged += TaskLoadTicketVM_PropertyChanged;
            this.barcodeReader = barcodeReader;
            managerToken = ExecutionContext.WebApiToken;
            RegisterDevices();
            TicketNameVariant = MessageViewContainerList.GetMessage(ExecutionContext, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT"));
            RboEnterTicketContent = MessageViewContainerList.GetMessage(ExecutionContext, "Manual") + " " + MessageViewContainerList.GetMessage(ExecutionContext, TicketNameVariant);
            RboScanTicketContent = MessageViewContainerList.GetMessage(ExecutionContext, "Scan") + " " + MessageViewContainerList.GetMessage(ExecutionContext, TicketNameVariant);
            TotalScannedTickets = 0;
            totalTickets = 0;
            ShowTicketSourceScreen = false;
            isCardValid = false;
            Loaded = new DelegateCommand(OnLoaded);
            RboScanTicketClick = new DelegateCommand(OnRboScanTicketClick);
            CardAddedCommand = new DelegateCommand(OnCardAdded);
            OkCommand = new DelegateCommand(OnOkClickedCommand, ButtonEnable);
            ClearCommand = new DelegateCommand(OnClearClickedCommand, ButtonEnable);
            DeleteCommand = new DelegateCommand(OnDeleteClicked, ButtonEnable);
            DeleteAllCommand = new DelegateCommand(OnDeleteAllClicked, ButtonEnable);
            ManualTicketEntryClicked = new DelegateCommand(OnManualTicketEntryClicked, ButtonEnable);
            NavigationClickCommand = new DelegateCommand(OnNavigationClickedCommand, ButtonEnable);
            CardDetailsVM = new CardDetailsVM(ExecutionContext);
            CardTappedEvent += HandleCardRead;
            dispatcher = Dispatcher.CurrentDispatcher;
            CustomDataGridVM = new CustomDataGridVM(ExecutionContext)
            {
                IsComboAndSearchVisible = false,
                VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
                SelectOption = SelectOption.Delete,
                
                HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                {
                    {"ManualTicketReceiptNo", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Ticket Number")} },
                    {"Tickets", new CustomDataGridColumnElement(){Heading = MessageViewContainerList.GetMessage(ExecutionContext, TicketNameVariant) } },
                },
            };
            if (ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ENABLE_MANUAL_TICKET_ENTRY_IN_REDEMPTION"))
            {
                IsEnabledManualTicketEntry = true;
            }
            else
            {
                IsEnabledManualTicketEntry = false;
            }
        }

        private void TaskLoadTicketVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetFooterContent(string.Empty, MessageType.None);
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                switch (e.PropertyName.ToLower())
                {
                    case "manualtickets":
                        {
                            if (ManualTickets < 0)
                            {
                                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5089), MessageType.Warning);
                            }
                        }
                        break;
                    case "isloadingvisible":
                        RaiseCanExecuteChanged();
                        break;
                }
            }
        }
        #endregion

        #region Methods
        private void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            (NavigationClickCommand as DelegateCommand).RaiseCanExecuteChanged();
            (OkCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ClearCommand as DelegateCommand).RaiseCanExecuteChanged();
            (DeleteCommand as DelegateCommand).RaiseCanExecuteChanged();
            (DeleteAllCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ManualTicketEntryClicked as DelegateCommand).RaiseCanExecuteChanged();
            log.LogMethodExit();
        }
        private void OnDeleteAllClicked(object parameter)
        {
            if (parameter != null)
            {
                log.LogMethodEntry(parameter);
                SetFooterContent(string.Empty, MessageType.None);
                TotalScannedTickets = 0;
                totalTickets = 0;
                ticketReceiptDTOs.Clear();
                CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(ticketReceiptDTOs);
            }
            log.LogMethodExit();
        }
        private void OnRboScanTicketClick(object parameter)
        {
            if(parameter != null)
            {
                log.LogMethodEntry(parameter);
                taskLoadTicketView.ManualTicketCustomNumericUpDown.Text = "0";
                if (IsRboScanTicketsChecked)
                {                   
                    ShowScanPopUpView();
                }
            }
            log.LogMethodExit();
        }
        private void OnDeleteClicked(object parameter)
        {
            if (parameter != null)
            {
                log.LogMethodEntry(parameter);
                SetFooterContent(string.Empty, MessageType.None);
                TicketReceiptDTO ticketReceiptDTO = parameter as TicketReceiptDTO;
                if (ticketReceiptDTO != null)
                {
                    TotalScannedTickets -= ticketReceiptDTO.Tickets;
                    totalTickets = TotalScannedTickets;
                    ticketReceiptDTOs.Remove(ticketReceiptDTO);
                    CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(ticketReceiptDTOs);
                }
            }
            log.LogMethodExit();
        }
        private void ShowScanPopUpView()
        {
            log.LogMethodEntry();
            genericScanPopupView = new GenericScanPopupView();
            GenericScanPopupVM genericScanPopupVM = new GenericScanPopupVM(ExecutionContext, BarcodeReader: barcodeReader);
            genericScanPopupVM.ScanPopupType = GenericScanPopupVM.PopupType.SCANTICKET;
            genericScanPopupVM.Title = MessageViewContainerList.GetMessage(ExecutionContext, "SCAN TICKET NOW");
            genericScanPopupVM.IsTimerRequired = false;
            genericScanPopupView.DataContext = genericScanPopupVM;
            genericScanPopupView.Owner = taskLoadTicketView;
            genericScanPopupView.ShowDialog();
            log.LogMethodExit();
        }
        private new void PerformClose(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(parameter != null)
            {
                UnRegisterDevices();
                base.PerformClose(parameter);
            }
            log.LogMethodExit();
        }

        private void OnNavigationClickedCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            PerformClose(parameter);
            log.LogMethodExit();
        }
        private async void OnOkClickedCommand(object parameter)
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);
            if (!isCardValid)
            {
                return;
            }
            if(totalTickets == 0)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5151), MessageType.Warning);
                return;
            }
            if (totalTickets < 0)
            {
                if (ConsiderForLoyalty)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5089), MessageType.Warning);
                    return;
                }
                decimal ticketsAvailable = 0;
                if (CardDetailsVM.AccountDTO.AccountSummaryDTO != null)
                {
                    if (CardDetailsVM.AccountDTO.AccountSummaryDTO.TotalTicketsBalance != null)
                    {
                        ticketsAvailable = ticketsAvailable + (decimal)CardDetailsVM.AccountDTO.AccountSummaryDTO.TotalTicketsBalance;
                    }
                }
                if ((totalTickets + ticketsAvailable) < 0)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1647), MessageType.Warning);
                    return;
                }
            }
            bool managerApprovalRequired = false;
            if (IsRboEnterManualTicketsChecked)
            {
                bool managerApprovalToAddManualTicket = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "MANAGER_APPROVAL_TO_ADD_MANUAL_TICKET");
                int addTicketLimitForManagerApprovalRedemption = ParafaitDefaultContainerList.GetParafaitDefault<int>(ExecutionContext, "ADD_TICKET_LIMIT_FOR_MANAGER_APPROVAL_REDEMPTION", 0);
                if (managerApprovalToAddManualTicket || addTicketLimitForManagerApprovalRedemption > 0 && totalTickets > 0 && totalTickets > addTicketLimitForManagerApprovalRedemption)
                {
                    managerApprovalRequired = true;
                }
            }
            int loadTicketLimitForManagerApproval = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(this.ExecutionContext, "LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL", 0);
            int loadTicketDeductionLimitForManagerApproval = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(this.ExecutionContext, "LOAD_TICKET_DEDUCTION_LIMIT_FOR_MANAGER_APPROVAL", 0);
            int redemptionLimitForManagerApproval = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "REDEMPTION_LIMIT_FOR_MANAGER_APPROVAL", 0);
            if ((loadTicketLimitForManagerApproval > 0 && totalTickets > 0 && totalTickets > loadTicketLimitForManagerApproval) ||
                (loadTicketDeductionLimitForManagerApproval > 0 && totalTickets < 0 && (-1 * totalTickets) > loadTicketDeductionLimitForManagerApproval) ||
                (redemptionLimitForManagerApproval > 0 && totalTickets > 0 && totalTickets > redemptionLimitForManagerApproval) ||
                ProductViewContainerList.GetSystemProductContainerDTO(ExecutionContext.GetSiteId(), ManualProductType.REDEEMABLE.ToString(), "LOADTICKETS", "LOADTICKETS").ManagerApprovalRequired == "Y" ||
                ProductViewContainerList.GetSystemProductContainerDTO(ExecutionContext.GetSiteId(), ManualProductType.REDEEMABLE.ToString(), "LOADTICKETS", "LOADTICKETS_NOLOYALTY").ManagerApprovalRequired == "Y")
            {
                managerApprovalRequired = true;
            }
            if (managerApprovalRequired)
            {
                bool valid = GetManagerApproval();
                if (!valid)
                {
                    return;
                }
            }
            if (IsRboEnterManualTicketsChecked)
            {
                int maxManualTicketsPerRedemption = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "MAX_MANUAL_TICKETS_PER_REDEMPTION", 0);
                if (maxManualTicketsPerRedemption > 0 && totalTickets > 0 && totalTickets > maxManualTicketsPerRedemption)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2495, ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, "MAX_MANUAL_TICKETS_PER_REDEMPTION")), MessageType.Warning);
                    return;
                }

                IsLoadingVisible = true;
                try
                {
                    ITicketReceiptUseCases iTicketReceiptUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(ExecutionContext);
                    await iTicketReceiptUseCases.PerDayTicketLimitCheck(totalTickets);
                }
                catch (ValidationException vex)
                {
                    log.Error(vex.Message);
                    IsLoadingVisible = false;
                    SetFooterContent(vex.Message, MessageType.Error);
                    return;
                }
                catch (UnauthorizedException uaex)
                {
                    log.Error(uaex.Message);
                    IsLoadingVisible = false;
                    throw;
                }
                catch (ParafaitApplicationException pax)
                {
                    log.Error(pax.Message);
                    IsLoadingVisible = false;
                    SetFooterContent(pax.Message, MessageType.Error);
                    return;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    IsLoadingVisible = false;
                    SetFooterContent(ex.Message, MessageType.Error);
                    return;
                }

            }
            int loadTicketLimit = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LOAD_TICKETS_LIMIT", 0);
            if (loadTicketLimit > 0 && totalTickets > 0 && totalTickets > loadTicketLimit)
            {
                IsLoadingVisible = false;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2830, totalTickets.ToString(), TicketNameVariant, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "LOAD_TICKETS_LIMIT").ToString()), MessageType.Error);               
                return;
            }
            int loadTicketDeductionLimit = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LOAD_TICKETS_DEDUCTION_LIMIT", 0);
            if (loadTicketDeductionLimit > 0 && totalTickets < 0 && (-1 * totalTickets) > loadTicketDeductionLimit)
            {
                IsLoadingVisible = false;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5094,  ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "LOAD_TICKETS_DEDUCTION_LIMIT").ToString(), TicketNameVariant), MessageType.Error);               
                return;
            }           
            int redemptionTransactionTicketLimit = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "REDEMPTION_TRANSACTION_TICKET_LIMIT", 0);
            if (redemptionTransactionTicketLimit > 0 && totalTickets > 0 && totalTickets > redemptionTransactionTicketLimit)
            {
                IsLoadingVisible = false;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1438, totalTickets, redemptionTransactionTicketLimit), MessageType.Error);               
                return;
            }


            RedemptionDTO redemptionDTO = new RedemptionDTO();
            
            IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
            
            RedemptionCardsDTO redemptionCardsDTO = new RedemptionCardsDTO(-1,redemptionDTO.RedemptionId,CardDetailsVM.AccountDTO.TagNumber, CardDetailsVM.AccountDTO.AccountId
                                                                          ,null,null,null,null,string.Empty,null,null);
            List<RedemptionCardsDTO> redemptionCardsDTOs = new List<RedemptionCardsDTO>() { redemptionCardsDTO};
            
            try
            {
                redemptionDTO = await redemptionUseCases.AddCard(redemptionDTO.RedemptionId, redemptionCardsDTOs);
                if (IsRboEnterManualTicketsChecked)
                {
                    RedemptionActivityDTO redemptionActivityDTO = new RedemptionActivityDTO();
                    redemptionActivityDTO.ManagerToken = managerToken;
                    redemptionActivityDTO.Tickets = ManualTickets;
                    await redemptionUseCases.AddManualTickets(redemptionDTO.RedemptionId, redemptionActivityDTO);
                }
                else
                {
                    foreach (TicketReceiptDTO ticketReceiptDTO in ticketReceiptDTOs)
                    {
                        ticketReceiptDTO.RedemptionId = redemptionDTO.RedemptionId;
                    }
                    await redemptionUseCases.AddTicket(redemptionDTO.RedemptionId, ticketReceiptDTOs);
                }
                RedemptionLoadToCardRequestDTO redemptionLoadToCardRequestDTO = new RedemptionLoadToCardRequestDTO();
                redemptionLoadToCardRequestDTO.AccountId = CardDetailsVM.AccountDTO.AccountId;
                redemptionLoadToCardRequestDTO.TotalTickets = totalTickets;
                redemptionLoadToCardRequestDTO.ManagerToken = managerToken;
                redemptionLoadToCardRequestDTO.Source = "POS Redemption";
                redemptionLoadToCardRequestDTO.Remarks = string.IsNullOrWhiteSpace(Remarks) ? "Load Tickets Task" : Remarks;
                redemptionLoadToCardRequestDTO.ConsiderForLoyalty = ConsiderForLoyalty;
                redemptionDTO = await redemptionUseCases.LoadTicketsToCard(redemptionDTO.RedemptionId, redemptionLoadToCardRequestDTO);

                bool result = false;
                if (ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AUTO_PRINT_LOAD_TICKETS") == "Y")
                {
                    ReceiptClass receiptClass = await redemptionUseCases.GetRedemptionOrderPrint(redemptionDTO.RedemptionId);
                    result = POSPrintHelper.PrintReceiptToPrinter(ExecutionContext, receiptClass, MessageViewContainerList.GetMessage(ExecutionContext, "Load Ticket Receipt"),string.Empty);
                    if (!result)
                    {
                        IsLoadingVisible = false;
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Print Error"), MessageType.Error);                      
                        return;
                    }
                }
                else if (ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AUTO_PRINT_LOAD_TICKETS") == "A")
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
                    if (taskLoadTicketView != null)
                    {
                        messagePopupView.Owner = taskLoadTicketView;
                    }
                    messagePopupView.ShowDialog();
                    if (messagePopupVM.ButtonClickType == ButtonClickType.Ok)
                    {
                        ReceiptClass receiptClass = await redemptionUseCases.GetRedemptionOrderPrint(redemptionDTO.RedemptionId);
                        result = POSPrintHelper.PrintReceiptToPrinter(ExecutionContext, receiptClass, MessageViewContainerList.GetMessage(ExecutionContext, "Load Ticket Receipt"), string.Empty);
                        ManagerId = -1;
                        if (!result)
                        {
                            IsLoadingVisible = false;
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Print Error"), MessageType.Error);                           
                            return;
                        }
                    }
                }               
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                IsLoadingVisible = false;
                SetFooterContent(vex.Message, MessageType.Error);              
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
                SetFooterContent(pax.ToString(), MessageType.Error);
                return;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                return;
            }
            IsLoadingVisible = false;
            SuccessMessage = MessageViewContainerList.GetMessage(ExecutionContext, 36, MessageViewContainerList.GetMessage(ExecutionContext, ticketNameVariant));
            PerformClose(taskLoadTicketView);
            PoleDisplay.executionContext = ExecutionContext;
            PoleDisplay.writeSecondLine(totalTickets.ToString() + " " + TicketNameVariant + " Loaded");
            log.LogMethodExit();
        }
        internal bool GetManagerApproval()
        {
            log.LogMethodEntry();
            if (!UserViewContainerList.IsSelfApprovalAllowed(this.ExecutionContext.SiteId, this.ExecutionContext.UserPKId))
            {
                AuthenticateManagerVM managerVM = new AuthenticateManagerVM(this.ExecutionContext, this.CardReader);
                AuthenticateManagerView managerView = new AuthenticateManagerView();
                managerView.DataContext = managerVM;
                managerView.Owner = taskLoadTicketView;
                managerView.ShowDialog();
                if (managerVM.IsValid)
                {
                    managerToken = managerVM.ExecutionContext.WebApiToken;
                    log.LogMethodExit();
                    return true;
                }
                else
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268), MessageType.Error);
                    log.LogMethodExit();
                    return false;
                }
            }
            else
            {
                managerToken = ExecutionContext.WebApiToken;
                log.LogMethodExit();
                return true;
            }
        }
        private void OnClearClickedCommand(object parameter)
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);
            CardDetailsVM.AccountDTO = null;
            Remarks = string.Empty;
            totalTickets = 0;
            TotalScannedTickets = 0;
            taskLoadTicketView.ManualTicketCustomNumericUpDown.Text = "0";
            if(ticketReceiptDTOs != null)
            {
                ticketReceiptDTOs.Clear();
            }
            if(CustomDataGridVM.CollectionToBeRendered != null)
            {
                CustomDataGridVM.CollectionToBeRendered.Clear();
            }
            ShowTicketSourceScreen = false;
            ConsiderForLoyalty = true;
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            taskLoadTicketView = parameter as TaskLoadTicketView;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            if (!ManagerApprovalCheck(TaskType.LOADTICKETS, parameter))
            {
                ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 268);
                PerformClose(parameter);
                return;
            }
            taskLoadTicketView.txtTotal.Heading = MessageViewContainerList.GetMessage(ExecutionContext, TicketNameVariant) + " to Load";
            ConsiderForLoyalty = true;
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
            if (TappedAccountDTO != null)
            {
                CardDetailsVM.AccountDTO = TappedAccountDTO;
            }
            if (CardDetailsVM.AccountDTO == null)
            {
                ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 257, null);
                PerformClose(taskLoadTicketView);
            }
            if (CardDetailsVM.AccountDTO.TechnicianCard == "Y")
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 197, CardDetailsVM.AccountDTO.TagNumber),MessageType.Warning);
                isCardValid = false;
                return;
            }
            if(CardDetailsVM.AccountDTO.AccountId < 0)
            {
                isCardValid = false;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 459), MessageType.Warning);
                return;
            }
            isCardValid = true;
            taskLoadTicketView.rboEnterTicket.IsChecked = true;
            ShowTicketSourceScreen = true;
            log.LogMethodExit();
        }

        internal void RegisterDevices()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (this.barcodeReader != null)
                {
                    this.barcodeReader.Register(new EventHandler(BarCodeScanCompleteEventHandle));
                    
                }
            });
            log.LogMethodExit();
        }
        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExecuteActionWithFooter(() =>
            {
                SetFooterContent(string.Empty, MessageType.None);
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                string barCode = ProcessScannedBarCode(
                                    checkScannedEvent.Message,
                                    ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LEFT_TRIM_BARCODE", 0),
                                    ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "RIGHT_TRIM_BARCODE", 0));

                ProcessBarcode(barCode);
            });
            
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
        internal async void ProcessBarcode(string barCode)
        {
            log.LogMethodEntry(barCode);

            if (!string.IsNullOrEmpty(barCode))
            {
                string ticketReceiptNumber = barCode;
                await dispatcher.BeginInvoke(new Action(() =>
                {
                    if (IsRboEnterManualTicketsChecked)
                    {
                        IsRboEnterManualTicketsChecked = false;
                        taskLoadTicketView.ManualTicketCustomNumericUpDown.Text = "0";
                        IsRboScanTicketsChecked = true;
                    }
                    
                    if (genericScanPopupView != null)
                    {
                        genericScanPopupView.Close();
                    }
                    GetTicketReceiptDetails(ticketReceiptNumber);
                }));
            }
            log.LogMethodExit();
        }

        internal async void GetTicketReceiptDetails(string receiptNumber)
        {
            log.LogMethodEntry(receiptNumber);
            ITicketReceiptUseCases ticketReceiptUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(ExecutionContext);
            try
            {
                IsLoadingVisible = true;
                bool isValid = false;
                int balanceTickets = await ticketReceiptUseCases.ValidateTicketReceipts(receiptNumber);
                if(balanceTickets > 0)
                {
                    isValid = true;
                }
                if (isValid)
                {
                    List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                    searchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_TICKET_RECEIPT_NO, receiptNumber));
                    List<TicketReceiptDTO> ticketReceipts = await ticketReceiptUseCases.GetTicketReceipts(searchParams);
                    IsLoadingVisible = false;
                    TicketReceiptDTO ticketReceiptDTO = null;
                    if (ticketReceipts != null && ticketReceipts.Any())
                    {
                        ticketReceiptDTO = ticketReceipts[0];               
                    }
                    else
                    {
                        TicketStationContainerDTO ticketStationContainerDTO = TicketStationViewContainerList.GetTicketStation(ExecutionContext, receiptNumber);
                        int balancetickets = Convert.ToInt32(receiptNumber.Substring(receiptNumber.Length - (ticketStationContainerDTO.CheckDigit ? ticketStationContainerDTO.TicketLength + 1 : ticketStationContainerDTO.TicketLength), ticketStationContainerDTO.TicketLength));
                        ticketReceiptDTO = new TicketReceiptDTO(-1, -1, receiptNumber, ExecutionContext.GetSiteId(), null, false, -1, balancetickets,
                                                                              balancetickets, ExecutionContext.GetUserId(), DateTime.Now, false, -1, DateTime.Now, ExecutionContext.GetUserId(), DateTime.Now);                        
                    }
                    if (ticketReceiptDTOs == null)
                    {
                        ticketReceiptDTOs = new List<TicketReceiptDTO>();
                    }
                    bool found = ticketReceiptDTOs.Exists(x => x.Id == ticketReceiptDTO.Id);
                    if (found)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 113) + " : " + MessageViewContainerList.GetMessage(ExecutionContext, 112), MessageType.Warning);
                    }
                    else
                    {
                        ticketReceiptDTOs.Add(ticketReceiptDTO);
                        int scannedTicketValue = ticketReceiptDTO.Tickets;
                        TotalScannedTickets += scannedTicketValue;
                        totalTickets = TotalScannedTickets;
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 114, scannedTicketValue.ToString(), TicketNameVariant), MessageType.Info);
                    }
                    CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(ticketReceiptDTOs);
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex.Message);
                IsLoadingVisible = false;
                SetFooterContent(vex.Message, MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                IsLoadingVisible = false;
                SetFooterContent(ex.Message, MessageType.Error);
            }
            finally
            {
                IsLoadingVisible = false;
            }
        }

       

        internal void UnRegisterDevices()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (barcodeReader != null)
                {
                    barcodeReader.UnRegister();
                }
            });
            log.LogMethodExit();
        }
        internal void ExecuteActionWithFooter(Action method)
        {
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
                SetFooterContent(uaex.Message.ToString(), MessageType.Error);
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

        private void OnManualTicketEntryClicked(object parameter)
        {
            if(parameter != null)
            {
                log.LogMethodEntry(parameter);
                GenericDataEntryView genericDataEntryView = new GenericDataEntryView();
                GenericDataEntryVM genericDataEntryVM = new GenericDataEntryVM(ExecutionContext);
                genericDataEntryVM.Heading = MessageViewContainerList.GetMessage(ExecutionContext, "ENTER TICKET NUMBER");
                genericDataEntryVM.DataEntryCollections = new ObservableCollection<DataEntryElement>()
                {
                    new DataEntryElement()
                    {
                        Type = DataEntryType.TextBox,
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Ticket Number")
                    }
                };
                genericDataEntryView.DataContext = genericDataEntryVM;
                genericDataEntryView.Owner = taskLoadTicketView;
                genericDataEntryView.ShowDialog();
                if(genericDataEntryVM.ButtonClickType == ButtonClickType.Ok)
                {
                    string ticketNumber = genericDataEntryVM.DataEntryCollections[0].Text;
                    if (!string.IsNullOrEmpty(ticketNumber))
                    {
                        GetTicketReceiptDetails(ticketNumber);
                    }
                }
            }
        }
        #endregion
    }
}

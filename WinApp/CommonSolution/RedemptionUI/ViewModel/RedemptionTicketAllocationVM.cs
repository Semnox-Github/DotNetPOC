/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - view model for redemption ticket allocation
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Semnox.Parafait.CommonUI;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.ViewContainer;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace Semnox.Parafait.RedemptionUI
{
    public enum TicketType
    {
        CARDS = 0,
        VOUCHERS = 1,
        CURRENCIES = 2,
        TICKETS = 3
    }

    public class RedemptionTicketAllocationVM : ViewModelBase
    {
        #region Members
        private int maxManualTicket;
        private int managerApprovalRequired;
        private TicketType ticketType;
        private List<RedemptionCurrencyContainerDTO> currencyContainerDTOs;
        List<RedemptionCardsDTO> ticketAllocationRedemptionCurrencies;
        private bool isApplyButtonEnable;
        private bool multiScreenMode;
        private bool ismultiScreenRowTwo;
        private int manualTicket;
        private bool isCurrency;
        private bool fromDeleteClicked;
        private DeviceClass cardReader;
        private bool disableDelete = false;

        private string title;
        private string heading;
        private string manualTicketAppliedContent;

        private CustomDataGridVM customDataGridVM;
        private RedemptionCardsDTO selectedCardsDTO;
        private GenericToggleButtonsVM genericToggleButtonsVM;
        private RedemptionDTO redemptionDTO;
        private RedemptionMainUserControlVM redemptionMainUserControlVM;
        private ObservableCollection<string> headings;
        private RedemptionTicketAllocationView ticketAllocationView;
        private GenericDataEntryView dataEntryView;
        private GenericMessagePopupView messagePopupView;
        private AuthenticateManagerView managerView;

        private ICommand loadedCommand;
        private ICommand itemClickedCommand;
        private ICommand deleteAllCommand;
        private ICommand applyManualTicketCommand;
        private ICommand closeCommand;
        private ICommand deleteCommand;
        private ICommand toggleCheckedCommand;
        private ICommand toggleUncheckedCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public CustomDataGridVM CustomDataGridVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(customDataGridVM);
                if (customDataGridVM == null)
                {
                    customDataGridVM = new CustomDataGridVM(this.ExecutionContext) { IsComboAndSearchVisible = false, SelectOption = SelectOption.Delete };
                }
                return customDataGridVM;
            }
            set
            {
                log.LogMethodEntry(customDataGridVM, value);
                SetProperty(ref customDataGridVM, value);
                log.LogMethodExit(customDataGridVM);
            }
        }
        internal RedemptionMainUserControlVM RedemptionMainUserControlVM
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
        public int ManualTicket
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(manualTicket);
                return manualTicket;
            }
            set
            {
                log.LogMethodEntry(manualTicket, value);
                SetProperty(ref manualTicket, value);
                if (!isApplyButtonEnable)
                {
                    IsApplyButtonEnable = !isApplyButtonEnable;
                }
                SetRedemptionMainUserControlVMFocus();
                log.LogMethodExit(manualTicket);
            }
        }
        public TicketType TicketType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ticketType);
                return ticketType;
            }
            set
            {
                log.LogMethodEntry(ticketType, value);
                SetProperty(ref ticketType, value);
                log.LogMethodExit(ticketType);
            }
        }
        public bool IsMultiScreenRowTwo
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ismultiScreenRowTwo);
                return ismultiScreenRowTwo;
            }
            set
            {
                log.LogMethodEntry(ismultiScreenRowTwo, value);
                SetProperty(ref ismultiScreenRowTwo, value);
                CustomDataGridVM.MultiScreenMode = multiScreenMode;
                CustomDataGridVM.IsMultiScreenRowTwo = ismultiScreenRowTwo;
                if (ticketType != TicketType.TICKETS)
                {
                    OnToggleChecked(ticketAllocationView);
                }
                log.LogMethodExit(ismultiScreenRowTwo);
            }
        }
        public bool IsApplyButtonEnable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isApplyButtonEnable);
                return isApplyButtonEnable;
            }
            set
            {
                log.LogMethodEntry(isApplyButtonEnable, value);
                SetProperty(ref isApplyButtonEnable, value);
                log.LogMethodExit(isApplyButtonEnable);
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
                log.LogMethodExit(multiScreenMode);
            }
        }
        public string Title
        {
            get
            {
                log.LogMethodEntry();
                return title;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref title, value);
            }
        }
        public string ManualTicketAppliedContent
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(manualTicketAppliedContent);
                return manualTicketAppliedContent;
            }
            set
            {
                log.LogMethodEntry(manualTicketAppliedContent, value);
                SetProperty(ref manualTicketAppliedContent, value);
                log.LogMethodExit(manualTicketAppliedContent);
            }
        }
        public string Heading
        {
            get
            {
                log.LogMethodEntry();
                return heading;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref heading, value);
            }
        }
        public ObservableCollection<string> Headings
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(headings);
                return headings;
            }
            set
            {
                log.LogMethodEntry(headings, value);
                SetProperty(ref headings, value);
                log.LogMethodExit(headings);
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
        public ICommand ApplyManualTicketCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(applyManualTicketCommand);
                return applyManualTicketCommand;
            }
            set
            {
                log.LogMethodEntry(applyManualTicketCommand, value);
                SetProperty(ref applyManualTicketCommand, value);
                log.LogMethodExit(applyManualTicketCommand);
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
            set
            {
                log.LogMethodEntry(itemClickedCommand, value);
                SetProperty(ref itemClickedCommand, value);
                log.LogMethodExit(itemClickedCommand);
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
        public ICommand CloseCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(closeCommand);
                return closeCommand;
            }
            set
            {
                log.LogMethodEntry(closeCommand, value);
                SetProperty(ref closeCommand, value);
                log.LogMethodExit(closeCommand);
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

        #endregion

        #region Methods
        internal void OnNumberpadClosedEvent()
        {
            log.LogMethodEntry();
            redemptionMainUserControlVM.ExecuteActionWithFooter(() =>
            {
                if (this.ticketType == TicketType.TICKETS)
                {
                    if (isApplyButtonEnable)
                    {
                        IsApplyButtonEnable = !isApplyButtonEnable;
                    }
                    if (this.redemptionDTO.ManualTickets == null || this.manualTicket != this.redemptionDTO.ManualTickets)
                    {
                        OnApplyManualTicketClicked(null);
                    }
                }
            });
            log.LogMethodExit();
        }
        private void OnSelectedItemChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            redemptionMainUserControlVM.ExecuteActionWithFooter(() =>
            {
                if (customDataGridVM.SelectedItem != null && isCurrency)
                {
                    RedemptionCardsDTO cardsDTO = customDataGridVM.SelectedItem as RedemptionCardsDTO;
                    if (cardsDTO != null && (cardsDTO.CurrencyRuleId == null || cardsDTO.CurrencyRuleId == -1))
                    {
                        NumberKeyboardView numberKeyboardView = redemptionMainUserControlVM.GetNumberPadView(ticketAllocationView, (int)cardsDTO.CurrencyQuantity);
                        selectedCardsDTO = cardsDTO;
                        if (numberKeyboardView != null)
                        {
                            numberKeyboardView.Closed += OnTicketViewNumberpadClosed;
                            numberKeyboardView.Show();
                        }
                    }
                }
            });
            log.LogMethodExit();
        }
        internal void ScrollToLastItem()
        {
            log.LogMethodEntry();
            if(customDataGridVM != null && customDataGridVM.UICollectionToBeRendered != null && customDataGridVM.UICollectionToBeRendered.Any()
                && ticketAllocationView != null && ticketAllocationView.CustomDatagridUserControl != null
                && ticketAllocationView.CustomDatagridUserControl.dataGrid != null)
            {
                ticketAllocationView.CustomDatagridUserControl.dataGrid.ScrollIntoView(customDataGridVM.UICollectionToBeRendered.LastOrDefault());
            }
            log.LogMethodExit();
        }
        internal void OnToggleChecked(object parameter)
        {
            log.LogMethodEntry(parameter);
            redemptionMainUserControlVM.ExecuteActionWithFooter(() =>
            {
                if (ticketAllocationView == null)
                {
                    ticketAllocationView = parameter as RedemptionTicketAllocationView;
                }
                if (GenericToggleButtonsVM != null && GenericToggleButtonsVM.SelectedToggleButtonItem != null)
                {
                    if (isCurrency)
                    {
                        isCurrency = false;
                    }
                    switch (GenericToggleButtonsVM.SelectedToggleButtonItem.Key.ToLower())
                    {
                        case "cards":
                            {
                                TicketType = TicketType.CARDS;
                                CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(redemptionDTO.RedemptionCardsListDTO.Where(c => (c.CurrencyId == null || c.CurrencyId == -1)
                                && (c.CurrencyRuleId == null || c.CurrencyRuleId == -1)));
                                CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                                {
                                    {"CardNumber", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Card Number") } },
                                    {"TotalCardTickets", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Tickets"),
                                        DataGridColumnStringFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT")} }
                                };
                            }
                            break;
                        case "vouchers":
                            {
                                TicketType = TicketType.VOUCHERS;
                                CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(redemptionDTO.TicketReceiptListDTO);
                                CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                                {
                                    {"ManualTicketReceiptNo", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Voucher") } },
                                    {"BalanceTickets", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Tickets"),
                                    DataGridColumnStringFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT")} }
                                };
                            }
                            break;
                        case "currencies":
                            {
                                TicketType = TicketType.CURRENCIES;
                                isCurrency = true;
                                SetCurrencyDetails();
                                CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(ticketAllocationRedemptionCurrencies);
                                //CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(redemptionDTO.RedemptionCardsListDTO.Where(c => (c.CurrencyId != null && c.CurrencyId >= 0)||(c.CurrencyRuleId != null && c.CurrencyRuleId >= 0)));
                                CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                                {
                                    {"CurrencyName", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Currency Name"),
                                    SecondarySource = new ObservableCollection<object>(currencyContainerDTOs), ChildOrSecondarySourcePropertyName = "CurrencyId", SourcePropertyName = "CurrencyId" } },
                                    {"CurrencyQuantity", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Quantity")} },
                                    {"CurrencyValueInTickets", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Tkt Value"),
                                    DataGridColumnStringFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT")} },
                                    {"TicketCount", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Total Tkt"),
                                    DataGridColumnStringFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT") } },
                                    {"RedemptionCurrencyRuleName", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Currency Rule"),
                                    SecondarySource = new ObservableCollection<object>(RedemptionCurrencyRuleViewContainerList.GetRedemptionCurrencyRuleContainerDTOList(ExecutionContext)),
                                    ChildOrSecondarySourcePropertyName = "RedemptionCurrencyRuleId", SourcePropertyName = "CurrencyRuleId"} },
                                };
                            }
                            break;
                        case "tickets":
                            {
                                if (TicketType != TicketType.TICKETS)
                                {
                                    bool managerApprovalRequiredForAddManualTicket = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(this.ExecutionContext, "MANAGER_APPROVAL_TO_ADD_MANUAL_TICKET");
                                    if (managerApprovalRequiredForAddManualTicket)
                                    {
                                        if (!UserViewContainerList.IsSelfApprovalAllowed(this.ExecutionContext.SiteId, this.ExecutionContext.UserPKId))
                                        {
                                            redemptionMainUserControlVM.OpenManagerView(ManageViewType.AddTicket);
                                        }
                                        else
                                        {
                                            redemptionMainUserControlVM.RedemptionActivityManualTicketDTO.ManagerToken = this.ExecutionContext.WebApiToken;
                                            SetTicketTypeValues();
                                        }
                                    }
                                    else
                                    {
                                        SetTicketTypeValues();
                                    }
                                }
                            }
                            break;
                    }
                }
                if (ticketAllocationView != null && ticketAllocationView.KeyBoardHelper != null && ticketAllocationView.KeyBoardHelper.NumberKeyboardView != null
                && this.ticketType != TicketType.TICKETS)
                {
                    ticketAllocationView.KeyBoardHelper.NumberKeyboardView.Close();
                }
                SetRedemptionMainUserControlVMFocus();
            });
            log.LogMethodExit();
        }
        private void SetCurrencyDetails()
        {
            log.LogMethodEntry();
            if (ticketAllocationRedemptionCurrencies == null)
            {
                ticketAllocationRedemptionCurrencies = new List<RedemptionCardsDTO>();
            }
            else
            {
                ticketAllocationRedemptionCurrencies.Clear();
            }
            foreach (int grouping in redemptionDTO.RedemptionCardsListDTO.Where(c => (c.CurrencyId != null && c.CurrencyId >= 0 && c.ViewGroupingNumber != null)).OrderBy(c => c.ViewGroupingNumber).Select(c => c.ViewGroupingNumber).Distinct())
            {
                ticketAllocationRedemptionCurrencies.Add(new RedemptionCardsDTO(-1, redemptionDTO.RedemptionId, string.Empty, -1, redemptionDTO.RedemptionCardsListDTO.Where(c => c.CurrencyId != null && c.CurrencyId >= 0 && c.ViewGroupingNumber != null && c.ViewGroupingNumber == grouping).Sum(c => c.TicketCount),
                    redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(c => c.CurrencyId != null && c.CurrencyId >= 0 && c.ViewGroupingNumber != null && c.ViewGroupingNumber == grouping).CurrencyId,
                    redemptionDTO.RedemptionCardsListDTO.Where(c => c.CurrencyId != null && c.CurrencyId >= 0 && c.ViewGroupingNumber != null && c.ViewGroupingNumber == grouping).Sum(c => c.CurrencyQuantity)
                    , redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(c => c.CurrencyId != null && c.CurrencyId >= 0 && c.ViewGroupingNumber != null && c.ViewGroupingNumber == grouping).CurrencyValueInTickets
                    , redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(c => c.CurrencyId != null && c.CurrencyId >= 0 && c.ViewGroupingNumber != null && c.ViewGroupingNumber == grouping).CurrencyName
                    , null, grouping));
            }
            foreach (int currencyId in redemptionDTO.RedemptionCardsListDTO.Where(c => (c.CurrencyId != null && c.CurrencyId >= 0 && c.ViewGroupingNumber == null)).Select(c => c.CurrencyId).Distinct())
            {
                ticketAllocationRedemptionCurrencies.Add(new RedemptionCardsDTO(-1, redemptionDTO.RedemptionId, string.Empty, -1, redemptionDTO.RedemptionCardsListDTO.Where(c => c.CurrencyId != null && c.CurrencyId >= 0 && c.ViewGroupingNumber == null && c.CurrencyId == currencyId).Sum(c => c.TicketCount),
                    redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(c => c.CurrencyId != null && c.CurrencyId >= 0 && c.ViewGroupingNumber == null && c.CurrencyId == currencyId).CurrencyId,
                    redemptionDTO.RedemptionCardsListDTO.Where(c => c.CurrencyId != null && c.CurrencyId >= 0 && c.ViewGroupingNumber == null && c.CurrencyId == currencyId).Sum(c => c.CurrencyQuantity)
                    , redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(c => c.CurrencyId != null && c.CurrencyId >= 0 && c.ViewGroupingNumber == null && c.CurrencyId == currencyId).CurrencyValueInTickets
                    , redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(c => c.CurrencyId != null && c.CurrencyId >= 0 && c.ViewGroupingNumber == null && c.CurrencyId == currencyId).CurrencyName
                    , null, null));
            }
            foreach (int currencyRuleId in redemptionDTO.RedemptionCardsListDTO.Where(c => c.CurrencyRuleId != null && c.CurrencyRuleId >= 0).Select(c => c.CurrencyRuleId).Distinct())
            {
                RedemptionCardsDTO currencyRuleDTO = new RedemptionCardsDTO(-1, redemptionDTO.RedemptionId, string.Empty, -1, redemptionDTO.RedemptionCardsListDTO.Where(c => c.CurrencyRuleId != null && c.CurrencyRuleId >= 0 && c.CurrencyRuleId==currencyRuleId).Sum(c=>c.TicketCount),
                    redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(c => c.CurrencyRuleId != null && c.CurrencyRuleId >= 0 && c.CurrencyRuleId == currencyRuleId).CurrencyId,
                    redemptionDTO.RedemptionCardsListDTO.Where(c => c.CurrencyRuleId != null && c.CurrencyRuleId >= 0 && c.CurrencyRuleId == currencyRuleId).Sum(c => c.CurrencyQuantity)
                    , redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(c => c.CurrencyRuleId != null && c.CurrencyRuleId >= 0 && c.CurrencyRuleId == currencyRuleId).CurrencyValueInTickets!=null? redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(c => c.CurrencyRuleId != null && c.CurrencyRuleId >= 0 && c.CurrencyRuleId == currencyRuleId).CurrencyValueInTickets:
                    (redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(c => c.CurrencyRuleId != null && c.CurrencyRuleId >= 0 && c.CurrencyRuleId == currencyRuleId).TicketCount/
                    (redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(c => c.CurrencyRuleId != null && c.CurrencyRuleId >= 0 && c.CurrencyRuleId == currencyRuleId).CurrencyQuantity!=null?
                    redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(c => c.CurrencyRuleId != null && c.CurrencyRuleId >= 0 && c.CurrencyRuleId == currencyRuleId).CurrencyQuantity:1)
                    )
                    , redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(c => c.CurrencyRuleId != null && c.CurrencyRuleId >= 0 && c.CurrencyRuleId == currencyRuleId).CurrencyName
                    , null, null);
                currencyRuleDTO.CurrencyRuleId = currencyRuleId;
                ticketAllocationRedemptionCurrencies.Add(currencyRuleDTO);
            }
            log.LogMethodExit();
        }
        internal void SetTicketTypeValues()
        {
            log.LogMethodEntry();
            TicketType = TicketType.TICKETS;
            if (redemptionDTO != null)
            {
                ManualTicket = redemptionDTO.ManualTickets != null ? (int)redemptionDTO.ManualTickets : 0;
            }
            if (ticketAllocationView != null && ticketAllocationView.upDown != null)
            {
                if (ticketAllocationView.upDown.DecreaseButton == null || ticketAllocationView.upDown.IncreaseButton == null
                || ticketAllocationView.upDown.TextBox == null)
                {
                    ticketAllocationView.upDown.ApplyTemplate();
                }
                if (ismultiScreenRowTwo)
                {
                    if (ticketAllocationView.upDown.DecreaseButton != null)
                    {
                        ticketAllocationView.upDown.DecreaseButton.Width = 36;
                        ticketAllocationView.upDown.DecreaseButton.Height = 36;
                    }
                    if (ticketAllocationView.upDown.IncreaseButton != null)
                    {
                        ticketAllocationView.upDown.IncreaseButton.Width = 36;
                        ticketAllocationView.upDown.IncreaseButton.Height = 36;
                    }
                    if (ticketAllocationView.btnApply != null)
                    {
                        ticketAllocationView.btnApply.Height = 60;
                    }
                }
            }
            OpenManualTicketNumberpad();
            log.LogMethodExit();
        }
        internal void OpenManualTicketNumberpad()
        {
            log.LogMethodEntry();
            if (ticketAllocationView != null && ticketAllocationView.KeyBoardHelper != null)
            {
                ticketAllocationView.KeyBoardHelper.ShowKeyBoard(ticketAllocationView, new List<Control>(), ParafaitDefaultViewContainerList.GetParafaitDefault(this.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                if ((this.RedemptionMainUserControlVM != null && !string.IsNullOrEmpty(this.RedemptionMainUserControlVM.ScannedBarCode)
                    && this.RedemptionMainUserControlVM.ScannedBarCode.ToLower() == "MNLTK".ToLower())
                    || ticketType == TicketType.TICKETS)
                {
                    if (ticketAllocationView.upDown != null && ticketAllocationView.upDown.TextBox != null)
                    {
                        ticketAllocationView.KeyBoardHelper.OnNumericUpDownMouseDown(ticketAllocationView.upDown.TextBox, null);
                    }
                    if (!string.IsNullOrEmpty(this.RedemptionMainUserControlVM.ScannedBarCode))
                    {
                        this.RedemptionMainUserControlVM.ScannedBarCode = string.Empty;
                    }
                }
            }
            log.LogMethodExit();
        }
        private void OnToggleUnchecked(object parameter)
        {
            log.LogMethodEntry(parameter);
            log.LogMethodExit();
        }
        private void SetManualTicketContentAsEmpty()
        {
            log.LogMethodEntry();
            if (!string.IsNullOrEmpty(this.manualTicketAppliedContent))
            {
                manualTicketAppliedContent = string.Empty;
            }
            log.LogMethodExit();
        }
        private void OnCancelClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            redemptionMainUserControlVM.ExecuteActionWithFooter(() =>
            {
                if (parameter != null)
                {
                    RedemptionTicketAllocationView redemptionTicketAllocationView = parameter as RedemptionTicketAllocationView;
                    if (redemptionTicketAllocationView != null)
                    {
                        ClearSelectedCardsDTO();
                        SetManualTicketContentAsEmpty();
                        redemptionTicketAllocationView.Close();
                    }
                }
            });
            log.LogMethodExit();
        }
        private void OnDeleteClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            redemptionMainUserControlVM.ExecuteActionWithFooter(() =>
            {
                if (redemptionDTO != null && redemptionDTO.RedemptionStatus != null && redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString())
                {
                    redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.NEW.ToString();
                }
                if (parameter != null)
                {
                    if (redemptionDTO != null)
                    {
                        bool performcurrencyrule = false;
                        bool recalculateprice = false;
                        switch (ticketType)
                        {
                            case TicketType.CARDS:
                            case TicketType.CURRENCIES:
                                {
                                    if (redemptionDTO.RedemptionCardsListDTO != null)
                                    {
                                        RedemptionCardsDTO cardsDTO = customDataGridVM.SelectedItem as RedemptionCardsDTO;
                                        List<RedemptionCardsDTO> cardstobeRemoved = new List<RedemptionCardsDTO>();
                                        if (cardsDTO != null)
                                        {
                                            if (cardsDTO == selectedCardsDTO)
                                            {
                                                ClearSelectedCardsDTO();
                                            }
                                            if (ticketType == TicketType.CARDS)
                                            {
                                                redemptionDTO.ETickets = redemptionDTO.ETickets - cardsDTO.TotalCardTickets;
                                                if (redemptionMainUserControlVM.MembershipIDCardIDList.ContainsKey(cardsDTO.CardId))
                                                {
                                                    int removedmembershipId = redemptionMainUserControlVM.MembershipIDCardIDList[cardsDTO.CardId];
                                                    redemptionMainUserControlVM.MembershipIDCardIDList.Remove(cardsDTO.CardId);
                                                    if (!redemptionMainUserControlVM.MembershipIDCardIDList.Values.Any(x => x == removedmembershipId))
                                                    {
                                                        redemptionMainUserControlVM.MembershipIDList.Remove(removedmembershipId);
                                                    }
                                                }
                                                redemptionDTO.RedemptionCardsListDTO.Remove(cardsDTO);
                                            }
                                            else if (ticketType == TicketType.CURRENCIES)
                                            {
                                                if (cardsDTO.CurrencyRuleId != null && cardsDTO.CurrencyRuleId > -1)
                                                {
                                                    return;
                                                }
                                                redemptionDTO.CurrencyTickets = redemptionDTO.CurrencyTickets - cardsDTO.CurrencyValueInTickets * cardsDTO.CurrencyQuantity;
                                                performcurrencyrule = true;
                                            }
                                            if (redemptionDTO.RedemptionCardsListDTO.Any(x => cardsDTO.CurrencyId > -1 && x.CurrencyId == cardsDTO.CurrencyId ))
                                            {
                                                if (cardsDTO.ViewGroupingNumber == null)
                                                {
                                                    List<RedemptionCardsDTO> redemptionCardsDTOs = redemptionDTO.RedemptionCardsListDTO.Where(x => cardsDTO.CurrencyId > -1 && x.CurrencyId == cardsDTO.CurrencyId && x.ViewGroupingNumber == null).ToList();
                                                    if (redemptionCardsDTOs != null)
                                                    {
                                                        cardstobeRemoved.AddRange(redemptionCardsDTOs);
                                                    }
                                                }
                                                else
                                                {
                                                    List<RedemptionCardsDTO> redemptionCardsDTOs = redemptionDTO.RedemptionCardsListDTO.Where(x => cardsDTO.CurrencyId > -1 && x.CurrencyId == cardsDTO.CurrencyId && x.ViewGroupingNumber == cardsDTO.ViewGroupingNumber).ToList();
                                                    if (redemptionCardsDTOs != null)
                                                    {
                                                        cardstobeRemoved.AddRange(redemptionCardsDTOs);
                                                    }
                                                }
                                                foreach (RedemptionCardsDTO cardDTOtobeRemoved in cardstobeRemoved)
                                                {
                                                    redemptionDTO.RedemptionCardsListDTO.Remove(cardDTOtobeRemoved);
                                                }
                                            }
                                            recalculateprice = true;
                                            if (redemptionMainUserControlVM.CardIDcustomerIDList.ContainsKey(cardsDTO.CardId))
                                            {
                                                redemptionMainUserControlVM.CardIDcustomerIDList.Remove(cardsDTO.CardId);
                                            }
                                            if (redemptionMainUserControlVM.CustomerIDcustomerInfoList.ContainsKey(cardsDTO.CardId))
                                            {
                                                redemptionMainUserControlVM.CustomerIDcustomerInfoList.Remove(cardsDTO.CardId);
                                            }

                                            if (cardsDTO.CardNumber != null && cardsDTO.CardNumber == redemptionDTO.PrimaryCardNumber)
                                            {
                                                if (redemptionDTO.RedemptionCardsListDTO.Any(x => x.CardNumber != null))
                                                {
                                                    redemptionDTO.PrimaryCardNumber = redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardNumber != null).CardNumber;
                                                    redemptionDTO.CardId = redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => x.CardNumber != null).CardId;
                                                    if (redemptionMainUserControlVM.CardIDcustomerIDList.ContainsKey(redemptionDTO.CardId))
                                                    {
                                                        redemptionDTO.CustomerId = redemptionMainUserControlVM.CardIDcustomerIDList[redemptionDTO.CardId];
                                                    }
                                                    else
                                                    {
                                                        redemptionDTO.CustomerId = -1;

                                                    }
                                                }
                                                else
                                                {
                                                    redemptionDTO.PrimaryCardNumber = null;
                                                    redemptionDTO.CardId = -1;
                                                    redemptionDTO.CustomerId = -1;
                                                }
                                                if (redemptionMainUserControlVM.CustomerIDcustomerInfoList.ContainsKey(redemptionDTO.CardId))
                                                {
                                                    redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(redemptionMainUserControlVM.CustomerIDcustomerInfoList[redemptionDTO.CardId], redemptionMainUserControlVM.GetBalanceTickets());
                                                }
                                                else
                                                {
                                                    redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(string.Empty, redemptionMainUserControlVM.GetBalanceTickets());
                                                }
                                            }
                                            if (performcurrencyrule)
                                            {
                                                PerformCurrencyRule();
                                                performcurrencyrule = false;
                                            }
                                            if (recalculateprice)
                                            {
                                                redemptionMainUserControlVM.CancellationTokenSource.Cancel();
                                                redemptionMainUserControlVM.ResetRecalculateFlags();
                                                redemptionMainUserControlVM.CancellationTokenSource = new System.Threading.CancellationTokenSource();
                                                redemptionMainUserControlVM.CallRecalculatePriceandStock();
                                                recalculateprice = false;
                                            }
                                            OnToggleChecked(ticketAllocationView);
                                            redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(null, redemptionMainUserControlVM.GetBalanceTickets());
                                        }
                                    }
                                }
                                break;
                            case TicketType.VOUCHERS:
                                {
                                    if (redemptionDTO.RedemptionTicketAllocationListDTO != null)
                                    {
                                        TicketReceiptDTO ticketDTO = customDataGridVM.SelectedItem as TicketReceiptDTO;//redemptionDTO.TicketReceiptListDTO.FirstOrDefault(d => d.Id == id);
                                        if (ticketDTO != null)
                                        {
                                            redemptionDTO.TicketReceiptListDTO.Remove(ticketDTO);
                                            redemptionDTO.ReceiptTickets = redemptionDTO.ReceiptTickets - ticketDTO.Tickets;
                                        }
                                    }
                                    OnToggleChecked(ticketAllocationView);
                                    redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(null, redemptionMainUserControlVM.GetBalanceTickets());
                                }
                                break;
                        }
                        RefreshRedemptionTicketUI();
                    }
                }
                SetRedemptionMainUserControlVMFocus();
            });
            log.LogMethodExit();
        }
        private void OnWindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Window window = sender as Window;
            if (ticketAllocationView != null)
            {
                window.Owner = this.ticketAllocationView;
                window.Width = this.ticketAllocationView.ActualWidth;
                window.Height = this.ticketAllocationView.ActualHeight;
                window.Top = this.ticketAllocationView.Top;
                window.Left = this.ticketAllocationView.Left;
                if (window is AuthenticateManagerView)
                {
                    managerView.KeyboardHelper.MultiScreenMode = ticketAllocationView.KeyBoardHelper.MultiScreenMode;
                    managerView.KeyboardHelper.ColorCode = ticketAllocationView.KeyBoardHelper.ColorCode;
                }
            }
            log.LogMethodExit();
        }       
        private void PerformCurrencyRule(RedemptionCardsDTO cardsDTO = null)
        {
            log.LogMethodEntry();
            redemptionMainUserControlVM.ExecuteActionWithFooter(() =>
            {
                if (redemptionMainUserControlVM != null && redemptionMainUserControlVM.ApplyCurrencyRule())
                {
                    UpdateCurrencyUI();
                }
                else
                {
                    if (cardsDTO != null)
                    {
                        AddCurrencyToList(cardsDTO);
                    }
                }
                RefreshRedemptionTicketUI();
            });
            log.LogMethodExit();
        }
        internal void UpdateCurrencyUI()
        {
            log.LogMethodEntry();
            redemptionMainUserControlVM.ExecuteActionWithFooter(() =>
            {
                foreach (RedemptionCardsDTO redemptioncardsDTO in redemptionMainUserControlVM.RedemptionDTO.RedemptionCardsListDTO)
                {
                    AddCurrencyToList(redemptioncardsDTO);
                }
            });
            log.LogMethodExit();
        }
        private void AddCurrencyToList(RedemptionCardsDTO cardsDTO)
        {
            log.LogMethodEntry();
            redemptionMainUserControlVM.ExecuteActionWithFooter(() =>
            {
                if (cardsDTO != null && cardsDTO.CurrencyId != null && cardsDTO.CurrencyId >= 0
                )
                {
                    string currencyName = string.Empty;
                    int totalValue = 0;
                    int totalqty = 0;
                    if (currencyContainerDTOs != null && currencyContainerDTOs.Count > 0)
                    {
                        currencyName = currencyContainerDTOs.FirstOrDefault(c => c.CurrencyId == cardsDTO.CurrencyId).CurrencyName;
                    }
                    if (cardsDTO.CurrencyQuantity != null && cardsDTO.CurrencyValueInTickets != null)
                    {
                        totalValue += (int)cardsDTO.CurrencyQuantity * (int)cardsDTO.CurrencyValueInTickets;
                        totalqty += (int)cardsDTO.CurrencyQuantity;
                    }
                }
                if (cardsDTO != null && cardsDTO.CurrencyRuleId != null && cardsDTO.CurrencyRuleId >= 0)
                {
                    string currencyRuleName = string.Empty;
                    int totalValue = 0;
                    currencyRuleName = RedemptionCurrencyRuleViewContainerList.GetRedemptionCurrencyRuleContainerDTOList(ExecutionContext).FirstOrDefault(c => c.RedemptionCurrencyRuleId == cardsDTO.CurrencyRuleId).RedemptionCurrencyRuleName;
                    totalValue = (int)(cardsDTO.CurrencyValueInTickets == null ? cardsDTO.TicketCount : (cardsDTO.CurrencyQuantity == null ? 1 : cardsDTO.CurrencyQuantity) * (int)(cardsDTO.CurrencyValueInTickets == null ? 0 : cardsDTO.CurrencyValueInTickets));

                }
            });
            log.LogMethodExit();
        }
        private void RefreshRedemptionTicketUI()
        {
            log.LogMethodEntry();
            if (redemptionMainUserControlVM != null)
            {
                int totalTicket = 0;
                redemptionMainUserControlVM.ExecuteActionWithFooter(() =>
                {
                    switch (redemptionMainUserControlVM.LeftPaneSelectedItem)
                    {
                        case LeftPaneSelectedItem.Redemption:
                            {
                                if (redemptionMainUserControlVM.RedemptionUserControlVM != null)
                                {
                                    redemptionMainUserControlVM.SetTotalCurrencyTickets();
                                    totalTicket = redemptionMainUserControlVM.GetTotalTickets();
                                    redemptionMainUserControlVM.RedemptionUserControlVM.UpdateTicketValues();
                                }
                                break;
                            }
                        case LeftPaneSelectedItem.LoadTicket:
                            {
                                if (redemptionMainUserControlVM.LoadTicketRedemptionUserControlVM != null)
                                {
                                    redemptionMainUserControlVM.RefreshTransactionItem();
                                    totalTicket = redemptionMainUserControlVM.GetLoadTicketTotalCount();
                                    redemptionMainUserControlVM.LoadTicketRedemptionUserControlVM.LoadTotatlTicketCount = totalTicket;
                                }
                                break;
                            }
                    }
                    redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(redemptionDTO.CustomerName, redemptionMainUserControlVM.GetBalanceTickets());
                });
            }
            SetRedemptionMainUserControlVMFocus();
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                ticketAllocationView = parameter as RedemptionTicketAllocationView;
                if (ticketAllocationView != null)
                {
                    ticketAllocationView.SizeChanged += OnTicketAllocationViewSizeChanged;
                }
            }
            log.LogMethodExit();
        }
        private void SetRedemptionMainUserControlVMFocus()
        {
            log.LogMethodEntry();
            if (redemptionMainUserControlVM != null && redemptionMainUserControlVM.RedemptionMainUserControl != null)
            {
                redemptionMainUserControlVM.RedemptionMainUserControl.Focus();
            }
            log.LogMethodExit();
        }
        private void OnTicketAllocationViewSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (ticketAllocationView != null)
            {
                if (dataEntryView != null)
                {
                    this.OnWindowLoaded(dataEntryView, null);
                }
                if (messagePopupView != null)
                {
                    this.OnWindowLoaded(messagePopupView, null);
                }
                if (managerView != null)
                {
                    this.OnWindowLoaded(managerView, null);
                }
            }
        }
        private void OnApplyManualTicketClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            redemptionMainUserControlVM.ExecuteActionWithFooter(() =>
            {
                if (redemptionDTO != null && redemptionDTO.RedemptionStatus != null && redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString())
                {
                    redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.NEW.ToString();
                }
                if (redemptionDTO != null && ticketType == TicketType.TICKETS)
                {
                    if (maxManualTicket < this.manualTicket)
                    {
                        OpenMessagePopupView(true, MessageViewContainerList.GetMessage(ExecutionContext, 2495, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "MAX_MANUAL_TICKETS_PER_REDEMPTION")));
                    }
                    else if (managerApprovalRequired > 0 && managerApprovalRequired < manualTicket)
                    {
                        if (!UserViewContainerList.IsSelfApprovalAllowed(this.ExecutionContext.SiteId, this.ExecutionContext.UserPKId))
                        {
                            managerView = new AuthenticateManagerView();
                            managerView.Loaded += OnWindowLoaded;
                            managerView.Closed += OnManagerViewClosed;
                            AuthenticateManagerVM managerVM = new AuthenticateManagerVM(this.ExecutionContext, this.cardReader)
                            {

                            };
                            managerView.DataContext = managerVM;
                            managerView.Show();
                        }
                        else
                        {
                            redemptionMainUserControlVM.RedemptionActivityManualTicketDTO.ManagerToken = this.ExecutionContext.WebApiToken;
                            SetManualTickets();
                        }
                    }
                    else
                    {
                        SetManualTickets();
                    }
                }
                SetRedemptionMainUserControlVMFocus();
            });
            log.LogMethodExit();
        }
        private async void SetManualTickets()
        {
            log.LogMethodEntry();
            if (this.ManualTicket != ((redemptionDTO.ManualTickets == null) ? 0 : redemptionDTO.ManualTickets))
            {
                try
                {
                    ITicketReceiptUseCases iTicketReceiptUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(ExecutionContext);
                    await iTicketReceiptUseCases.PerDayTicketLimitCheck(this.ManualTicket);
                    string manualTicket = this.ManualTicket.ToString();
                    ManualTicketAppliedContent = MessageViewContainerList.GetMessage(ExecutionContext, 2920,
                        redemptionDTO.ManualTickets != null ? ((int)redemptionDTO.ManualTickets).ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT")) : "0",
                        this.ManualTicket.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT"))
                        );
                    redemptionDTO.ManualTickets = Int32.Parse(manualTicket);
                    if (redemptionMainUserControlVM != null)
                    {
                        switch (redemptionMainUserControlVM.LeftPaneSelectedItem)
                        {
                            case LeftPaneSelectedItem.Redemption:
                                {
                                    if (redemptionMainUserControlVM.RedemptionUserControlVM != null
                                        && !redemptionMainUserControlVM.RedemptionUserControlVM.StayInTransactionMode)
                                    {
                                        redemptionMainUserControlVM.RedemptionUserControlVM.StayInTransactionMode = true;
                                    }
                                }
                                break;
                            case LeftPaneSelectedItem.LoadTicket:
                                {
                                    if (redemptionMainUserControlVM.LoadTicketRedemptionUserControlVM != null)
                                    {
                                        redemptionMainUserControlVM.LoadTicketRedemptionUserControlVM.LoadTotatlTicketCount = redemptionMainUserControlVM.GetLoadTicketTotalCount();
                                        if (!redemptionMainUserControlVM.LoadTicketRedemptionUserControlVM.StayInTransactionMode)
                                        {
                                            redemptionMainUserControlVM.LoadTicketRedemptionUserControlVM.StayInTransactionMode = true;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    RefreshRedemptionTicketUI();
                    if (ticketType == TicketType.TICKETS && !string.IsNullOrEmpty(manualTicketAppliedContent)
                    && this.ticketAllocationView != null)
                    {
                        ticketAllocationView.Close();
                    }
                }
                catch (ValidationException vex)
                {
                    log.Error(vex);
                    if (redemptionMainUserControlVM != null)
                    {
                        OpenMessagePopupView(true, vex.Message);
                        //if (redemptionMainUserControlVM.AllocationView !=null)
                        //{
                        //    redemptionMainUserControlVM.AllocationView.Close();
                        //}
                        redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                    }
                }
                catch (UnauthorizedException uaex)
                {
                    log.Error(uaex);
                    if (redemptionMainUserControlVM != null)
                    {
                        if (redemptionMainUserControlVM.AllocationView != null)
                        {
                            SetManualTicketContentAsEmpty();
                            redemptionMainUserControlVM.AllocationView.Close();
                        }
                        redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
                    }
                }
                catch (ParafaitApplicationException pax)
                {
                    log.Error(pax);
                    if (redemptionMainUserControlVM != null)
                    {
                        if (redemptionMainUserControlVM.AllocationView != null)
                        {
                            SetManualTicketContentAsEmpty();
                            redemptionMainUserControlVM.AllocationView.Close();
                        }
                        redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    if (redemptionMainUserControlVM != null)
                    {
                        if (redemptionMainUserControlVM.AllocationView != null)
                        {
                            SetManualTicketContentAsEmpty();
                            redemptionMainUserControlVM.AllocationView.Close();
                        }
                        redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                    }
                }
            }
            log.LogMethodExit();
        }
        private void OnManagerViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            redemptionMainUserControlVM.ExecuteActionWithFooter(() =>
            {
                AuthenticateManagerVM managerVM = (sender as AuthenticateManagerView).DataContext as AuthenticateManagerVM;
                if (managerVM != null && managerVM.IsValid)
                {
                    redemptionMainUserControlVM.RedemptionActivityManualTicketDTO.ManagerToken = managerVM.ExecutionContext.WebApiToken;
                    SetManualTickets();
                }
                else
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 268), MessageType.Error);
                    this.ManualTicket = redemptionDTO.ManualTickets != null ? (int)redemptionDTO.ManualTickets : 0;
                }
                SetRedemptionMainUserControlVMFocus();
            });
            log.LogMethodExit();
        }
        private void OnDeleteAll(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (redemptionDTO != null && redemptionDTO.RedemptionStatus != null && redemptionDTO.RedemptionStatus == RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString())
            {
                redemptionDTO.RedemptionStatus = RedemptionDTO.RedemptionStatusEnum.NEW.ToString();
            }
            if (customDataGridVM != null && customDataGridVM.UICollectionToBeRendered != null &&
                customDataGridVM.UICollectionToBeRendered.Count > 0)
            {
                OpenMessagePopupView();
            }
            log.LogMethodExit();
        }
        private void OpenMessagePopupView(bool isMaxTicket = false, string message = null)
        {
            log.LogMethodEntry();
            messagePopupView = new GenericMessagePopupView();
            messagePopupView.Loaded += OnWindowLoaded;
            messagePopupView.Closed += OnMessagePopupViewClosed;
            GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext);
            if (isMaxTicket)
            {
                messagePopupVM.MessageButtonsType = MessageButtonsType.OK;
                messagePopupVM.CancelButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "OK");
                messagePopupVM.Heading = MessageViewContainerList.GetMessage(ExecutionContext, 2921);
                messagePopupVM.Content = message;
            }
            else
            {
                messagePopupVM.MessageButtonsType = MessageButtonsType.OkCancel;
                messagePopupVM.OkButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "CONFIRM");
                messagePopupVM.Heading = MessageViewContainerList.GetMessage(ExecutionContext, 2922);
                messagePopupVM.Content = MessageViewContainerList.GetMessage(ExecutionContext, 2923);
            }
            messagePopupView.DataContext = messagePopupVM;
            messagePopupView.Show();
            log.LogMethodExit();
        }
        private void OnMessagePopupViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            redemptionMainUserControlVM.ExecuteActionWithFooter(() =>
            {
                GenericMessagePopupVM messagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
                if (messagePopupVM != null && messagePopupVM.ButtonClickType == ButtonClickType.Ok
                    && redemptionDTO != null)
                {
                    switch (ticketType)
                    {
                        case TicketType.TICKETS:
                            {
                                ManualTicket = redemptionDTO.ManualTickets != null ? (int)redemptionDTO.ManualTickets : 0;
                            }
                            break;
                        case TicketType.CARDS:
                        case TicketType.CURRENCIES:
                            {
                                ClearSelectedCardsDTO();
                                if (redemptionDTO.RedemptionCardsListDTO != null && redemptionDTO.RedemptionCardsListDTO.Any())
                                {
                                    bool recalculateprice = false;
                                    List<RedemptionCardsDTO> cardstobeRemoved = new List<RedemptionCardsDTO>();
                                    foreach (RedemptionCardsDTO cardsDTO in redemptionDTO.RedemptionCardsListDTO)
                                    {
                                        if (ticketType == TicketType.CARDS)
                                        {
                                            redemptionDTO.ETickets = redemptionDTO.ETickets - cardsDTO.TotalCardTickets;
                                            if (redemptionMainUserControlVM.MembershipIDCardIDList.ContainsKey(cardsDTO.CardId))
                                            {
                                                int removedmembershipId = redemptionMainUserControlVM.MembershipIDCardIDList[cardsDTO.CardId];
                                                redemptionMainUserControlVM.MembershipIDCardIDList.Remove(cardsDTO.CardId);
                                                if (!redemptionMainUserControlVM.MembershipIDCardIDList.Values.Any(x => x == removedmembershipId))
                                                {
                                                    redemptionMainUserControlVM.MembershipIDList.Remove(removedmembershipId);
                                                    recalculateprice = true;
                                                }
                                            }
                                            //redemptionDTO.RedemptionCardsListDTO.Remove(cardsDTO);
                                            cardstobeRemoved.Add(cardsDTO);

                                            if (redemptionMainUserControlVM.CardIDcustomerIDList.ContainsKey(cardsDTO.CardId))
                                            {
                                                redemptionMainUserControlVM.CardIDcustomerIDList.Remove(cardsDTO.CardId);
                                            }
                                            if (redemptionMainUserControlVM.CustomerIDcustomerInfoList.ContainsKey(cardsDTO.CardId))
                                            {
                                                redemptionMainUserControlVM.CustomerIDcustomerInfoList.Remove(cardsDTO.CardId);
                                            }
                                            redemptionDTO.PrimaryCardNumber = null;
                                            redemptionDTO.CardId = -1;
                                            redemptionDTO.CustomerId = -1;
                                        }
                                        else if (ticketType == TicketType.CURRENCIES)
                                        {
                                            if (cardsDTO != null &&( cardsDTO.CurrencyId > -1 || cardsDTO.CurrencyRuleId>-1))
                                            {
                                                redemptionDTO.CurrencyTickets = redemptionDTO.CurrencyTickets - (cardsDTO.CurrencyValueInTickets == null ? cardsDTO.TicketCount : (cardsDTO.CurrencyValueInTickets * cardsDTO.CurrencyQuantity));
                                                if (cardsDTO.CurrencyId > -1)
                                                {
                                                    if (cardsDTO.ViewGroupingNumber == null)
                                                    {
                                                        if (!cardstobeRemoved.Any(x => cardsDTO.CurrencyId > -1 && x.CurrencyId == cardsDTO.CurrencyId && x.ViewGroupingNumber == null))
                                                        {
                                                            cardstobeRemoved.AddRange(redemptionDTO.RedemptionCardsListDTO.Where(x => cardsDTO.CurrencyId > -1 && x.CurrencyId == cardsDTO.CurrencyId && x.ViewGroupingNumber == null).ToList());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!cardstobeRemoved.Any(x => cardsDTO.CurrencyId > -1 && x.CurrencyId == cardsDTO.CurrencyId && x.ViewGroupingNumber == cardsDTO.ViewGroupingNumber))
                                                        {
                                                            cardstobeRemoved.AddRange(redemptionDTO.RedemptionCardsListDTO.Where(x => cardsDTO.CurrencyId > -1 && x.CurrencyId == cardsDTO.CurrencyId && x.ViewGroupingNumber == cardsDTO.ViewGroupingNumber).ToList());
                                                        }
                                                    }
                                                }
                                                if (cardsDTO.CurrencyRuleId > -1)
                                                 {
                                                    if (cardsDTO.ViewGroupingNumber == null)
                                                    {
                                                        if (!cardstobeRemoved.Any(x => cardsDTO.CurrencyRuleId > -1 && x.CurrencyRuleId == cardsDTO.CurrencyRuleId && x.ViewGroupingNumber == null))
                                                        {
                                                            cardstobeRemoved.AddRange(redemptionDTO.RedemptionCardsListDTO.Where(x => cardsDTO.CurrencyRuleId > -1 && x.CurrencyRuleId == cardsDTO.CurrencyRuleId && x.ViewGroupingNumber == null).ToList());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!cardstobeRemoved.Any(x => cardsDTO.CurrencyRuleId > -1 && x.CurrencyRuleId == cardsDTO.CurrencyRuleId && x.ViewGroupingNumber == cardsDTO.ViewGroupingNumber))
                                                        {
                                                            cardstobeRemoved.AddRange(redemptionDTO.RedemptionCardsListDTO.Where(x => cardsDTO.CurrencyRuleId > -1 && x.CurrencyRuleId == cardsDTO.CurrencyRuleId && x.ViewGroupingNumber == cardsDTO.ViewGroupingNumber).ToList());
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (cardstobeRemoved != null && cardstobeRemoved.Any())
                                    {
                                        foreach (RedemptionCardsDTO cardDTOtobeRemoved in cardstobeRemoved)
                                        {
                                            redemptionDTO.RedemptionCardsListDTO.Remove(cardDTOtobeRemoved);
                                        }
                                    }
                                    if (recalculateprice)
                                    {
                                        redemptionMainUserControlVM.CancellationTokenSource.Cancel();
                                        redemptionMainUserControlVM.ResetRecalculateFlags();
                                        redemptionMainUserControlVM.CancellationTokenSource = new System.Threading.CancellationTokenSource();
                                        redemptionMainUserControlVM.CallRecalculatePriceandStock();
                                        recalculateprice = false;
                                    }
                                }
                            }
                            break;
                        case TicketType.VOUCHERS:
                            {
                                if (redemptionDTO.TicketReceiptListDTO != null)
                                {
                                    redemptionDTO.TicketReceiptListDTO.Clear();
                                }
                            }
                            break;
                    }
                    this.OnToggleChecked(ticketAllocationView);
                    redemptionMainUserControlVM.SetHeaderCustomerBalanceInfo(null, redemptionMainUserControlVM.GetBalanceTickets());
                    //     this.DisplayRedemptionTickets.Clear();
                    RefreshRedemptionTicketUI();
                }
            });
            log.LogMethodExit();
        }
        private void OnScrollChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                ticketAllocationView = parameter as RedemptionTicketAllocationView;
            }
            log.LogMethodExit();
        }
        private void ClearSelectedCardsDTO()
        {
            log.LogMethodEntry();
            selectedCardsDTO = null;
            if (redemptionMainUserControlVM != null)
            {
                redemptionMainUserControlVM.CloseNumberPad();
            }
            log.LogMethodExit();
        }
        internal void RefreshContentArea(int quantity)
        {
            log.LogMethodEntry();
            if (isCurrency && selectedCardsDTO != null)
            {
                if (selectedCardsDTO != null)
                {
                    if (quantity <= 0)
                    {
                        if (redemptionDTO.RedemptionCardsListDTO.Any(x => selectedCardsDTO.CurrencyId > -1 && x.CurrencyId == selectedCardsDTO.CurrencyId))
                        {
                            List<RedemptionCardsDTO> cardstobeRemoved = new List<RedemptionCardsDTO>();
                            if (selectedCardsDTO.ViewGroupingNumber == null)
                            {
                                List<RedemptionCardsDTO> redemptionCardsDTOs = redemptionDTO.RedemptionCardsListDTO.Where(x => selectedCardsDTO.CurrencyId > -1 && x.CurrencyId == selectedCardsDTO.CurrencyId && x.ViewGroupingNumber == null).ToList();
                                if (redemptionCardsDTOs != null)
                                {
                                    cardstobeRemoved.AddRange(redemptionCardsDTOs);
                                }
                            }
                            else
                            {
                                List<RedemptionCardsDTO> redemptionCardsDTOs = redemptionDTO.RedemptionCardsListDTO.Where(x => selectedCardsDTO.CurrencyId > -1 && x.CurrencyId == selectedCardsDTO.CurrencyId && x.ViewGroupingNumber == selectedCardsDTO.ViewGroupingNumber).ToList();
                                if (redemptionCardsDTOs != null)
                                {
                                    cardstobeRemoved.AddRange(redemptionCardsDTOs);
                                }
                            }
                            foreach (RedemptionCardsDTO cardDTOtobeRemoved in cardstobeRemoved)
                            {
                                redemptionDTO.RedemptionCardsListDTO.Remove(cardDTOtobeRemoved);
                            }
                        }
                        RefreshRedemptionTicketUI();
                        selectedCardsDTO = null;
                    }
                    else
                    {
                        redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => (selectedCardsDTO.CurrencyId > -1 && x.CurrencyId == selectedCardsDTO.CurrencyId && (selectedCardsDTO.ViewGroupingNumber == null || selectedCardsDTO.ViewGroupingNumber == x.ViewGroupingNumber)) || (selectedCardsDTO.CurrencyRuleId > -1 && selectedCardsDTO.CurrencyRuleId == x.CurrencyRuleId)).CurrencyQuantity = quantity;
                        redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => (selectedCardsDTO.CurrencyId > -1 && x.CurrencyId == selectedCardsDTO.CurrencyId && (selectedCardsDTO.ViewGroupingNumber == null || selectedCardsDTO.ViewGroupingNumber == x.ViewGroupingNumber)) || (selectedCardsDTO.CurrencyRuleId > -1 && selectedCardsDTO.CurrencyRuleId == x.CurrencyRuleId)).TicketCount = quantity* redemptionDTO.RedemptionCardsListDTO.FirstOrDefault(x => (selectedCardsDTO.CurrencyId > -1 && x.CurrencyId == selectedCardsDTO.CurrencyId && (selectedCardsDTO.ViewGroupingNumber == null || selectedCardsDTO.ViewGroupingNumber == x.ViewGroupingNumber)) || (selectedCardsDTO.CurrencyRuleId > -1 && selectedCardsDTO.CurrencyRuleId == x.CurrencyRuleId)).CurrencyValueInTickets;
                        selectedCardsDTO.CurrencyQuantity = quantity;
                        selectedCardsDTO.TicketCount = quantity* selectedCardsDTO.CurrencyValueInTickets;
                    }
                    PerformCurrencyRule(selectedCardsDTO);
                    OnToggleChecked(ticketAllocationView);
                }
            }
            selectedCardsDTO = null;
            log.LogMethodExit();
        }
        private void OnTicketViewNumberpadClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (sender != null)
            {
                NumberKeyboardView numberkeyboardView = sender as NumberKeyboardView;
                NumberKeyboardVM numberkeyboardVM = numberkeyboardView.DataContext as NumberKeyboardVM;
                int qty = -1;
                if (numberkeyboardVM != null && numberkeyboardVM.ButtonClickType == ButtonClickType.Ok
                    && !string.IsNullOrWhiteSpace(numberkeyboardVM.NumberText) && int.TryParse(numberkeyboardVM.NumberText, out qty)
                    && qty > -1)
                {
                    RefreshContentArea(qty);
                }
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public RedemptionTicketAllocationVM(ExecutionContext executionContext, RedemptionMainUserControlVM redemptionMainUserControlVM, List<RedemptionCurrencyContainerDTO> currencyContainerDTOs, DeviceClass cardReader)
        {
            log.LogMethodEntry();
            this.cardReader = cardReader;
            redemptionMainUserControlVM.ExecuteActionWithFooter(() =>
            {
                this.ExecutionContext = executionContext;
                maxManualTicket = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(this.ExecutionContext, "MAX_MANUAL_TICKETS_PER_REDEMPTION");
                managerApprovalRequired = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(this.ExecutionContext, "ADD_TICKET_LIMIT_FOR_MANAGER_APPROVAL_REDEMPTION");
                multiScreenMode = false;
                ismultiScreenRowTwo = false;
                isApplyButtonEnable = true;
                if (redemptionMainUserControlVM != null)
                {
                    this.redemptionMainUserControlVM = redemptionMainUserControlVM;
                    this.redemptionDTO = redemptionMainUserControlVM.RedemptionDTO;
                    if (this.redemptionDTO != null && this.redemptionDTO.RedemptionStatus != null && this.redemptionDTO.RedemptionStatus.ToString().ToLower() == "delivered")
                    {
                        disableDelete = true;
                    }
                    this.ticketAllocationView = redemptionMainUserControlVM.AllocationView;
                }
                this.currencyContainerDTOs = currencyContainerDTOs;
                deleteAllCommand = new DelegateCommand(OnDeleteAll);
                deleteCommand = new DelegateCommand(OnDeleteClicked);
                applyManualTicketCommand = new DelegateCommand(OnApplyManualTicketClicked);
                loadedCommand = new DelegateCommand(OnLoaded);
                itemClickedCommand = new DelegateCommand(OnSelectedItemChanged);
                toggleCheckedCommand = new DelegateCommand(OnToggleChecked);
                closeCommand = new DelegateCommand(OnCancelClicked);
                title = MessageViewContainerList.GetMessage(executionContext, "Redemption Ticket Details");
                heading = (redemptionDTO.RedemptionOrderNo == null) ? MessageViewContainerList.GetMessage(executionContext, "RO - ") : redemptionDTO.RedemptionOrderNo;

                manualTicketAppliedContent = string.Empty;

                ObservableCollection<CustomToggleButtonItem> toggleButtonItems = new ObservableCollection<CustomToggleButtonItem>();

                toggleButtonItems.Add(new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag() { Text = MessageViewContainerList.GetMessage(this.ExecutionContext, "Cards",null) }
                    },
                    Key = "Cards"
                });
                toggleButtonItems.Add(new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag() { Text = MessageViewContainerList.GetMessage(this.ExecutionContext,"Vouchers",null) }
                    },
                    Key = "Vouchers"
                });
                toggleButtonItems.Add(new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag() { Text = MessageViewContainerList.GetMessage(this.ExecutionContext,"Currencies",null) }
                    },
                    Key = "Currencies"
                });
                toggleButtonItems.Add(new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag() { Text = MessageViewContainerList.GetMessage(this.ExecutionContext,"Tickets",null) }
                    },
                    Key = "Tickets"
                });
                genericToggleButtonsVM = new GenericToggleButtonsVM()
                {
                    ToggleButtonItems = toggleButtonItems,
                    IsVerticalOrientation = true
                };

                headings = new ObservableCollection<string>();
            });
            log.LogMethodExit();
        }
        #endregion
    }
}

/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - view model for redemption revserse view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Windows;

using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.Printer;


namespace Semnox.Parafait.RedemptionUI
{
    public class RedemptionReverseVM : ViewModelBase
    {
        #region Members
        private RedemptionDTO redemptionDTO;
        private bool isReverseEnabled;
        private bool ismultiScreenRowTwo;
        private bool multiScreenMode;
        private string screenNumber;
        private DeviceClass cardReader;

        private string title;
        private string heading;
        private string cancelButtonText;
        private string reverseButtonText;
        private int totalTicketsForReversal;

        private CustomDataGridVM customDataGridVM;
        private RedemptionActivityDTO redemptionActivityDTO;
        private RedemptionReverseView reverseView;
        private GenericDataEntryView genericDataEntryView;
        private AuthenticateManagerView managerView;
        private GenericMessagePopupView messagePopupView;
        private RedemptionMainUserControlVM redemptionMainUserControlVM;
        private RedemptionDTO NewRedemptionDTO;

        private ICommand cancelCommand;
        private ICommand reverseCommand;
        private ICommand loadedCommand;
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
        public bool IsReverseEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isReverseEnabled);
                return isReverseEnabled;
            }
            set
            {
                log.LogMethodEntry(isReverseEnabled, value);
                SetProperty(ref isReverseEnabled, value);
                log.LogMethodExit(isReverseEnabled);
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
                SetCustomDataGridVM();
                log.LogMethodExit(ismultiScreenRowTwo);
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
                log.LogMethodExit(title);
                return title;
            }
            set
            {
                log.LogMethodEntry(title, value);
                SetProperty(ref title, value);
                log.LogMethodExit(title);
            }
        }

        public string Heading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(heading);
                return heading;
            }
            set
            {
                log.LogMethodEntry(heading, value);
                SetProperty(ref heading, value);
                log.LogMethodExit(heading);
            }
        }

        public string CancelButtonText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cancelButtonText);
                return cancelButtonText;
            }
            set
            {
                log.LogMethodEntry(cancelButtonText, value);
                SetProperty(ref cancelButtonText, value);
                log.LogMethodExit(cancelButtonText);
            }
        }

        public string ReverseButtonText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(reverseButtonText);
                return reverseButtonText;
            }
            set
            {
                log.LogMethodEntry(reverseButtonText, value);
                SetProperty(ref reverseButtonText, value);
                log.LogMethodExit(reverseButtonText);
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cancelCommand);
                return cancelCommand;
            }
            set
            {
                log.LogMethodEntry(cancelCommand, value);
                SetProperty(ref cancelCommand, value);
                log.LogMethodExit(cancelCommand);
            }
        }

        public ICommand ReverseCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(reverseCommand);
                return reverseCommand;
            }
            set
            {
                log.LogMethodEntry(reverseCommand, value);
                SetProperty(ref reverseCommand, value);
                log.LogMethodExit(reverseCommand);
            }
        }

        #endregion

        #region Methods
        private void SetCustomDataGridVM()
        {
            log.LogMethodEntry();
            CustomDataGridVM.IsComboAndSearchVisible = false;
            CustomDataGridVM.SelectOption = SelectOption.CheckBox;
            CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(redemptionDTO.RedemptionGiftsListDTO);
            CustomDataGridVM.IsMultiScreenRowTwo = ismultiScreenRowTwo;
            CustomDataGridVM.MultiScreenMode = multiScreenMode;
            CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                    {
                        {"ProductName", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Code") } },
                        {"ProductDescription", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Description") } },
                        {"Tickets", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Price"),
                        DataGridColumnStringFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT") } },
                        {"GiftLineIsReversed", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Reversed"),
                        Type = DataEntryType.CheckBox, IsEnable = false} },
                    };
            CustomDataGridVM.DataGridRowEnableProperty = "GiftLineIsReversed";
            CustomDataGridVM.DataGridRowEnableWorksInReverse = true;
            log.LogMethodExit();
        }
        private void OnWindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Window window = sender as Window;
            if (reverseView != null)
            {
                window.Owner = this.reverseView;
                window.Width = this.reverseView.ActualWidth;
                window.Height = this.reverseView.ActualHeight;
                window.Top = this.reverseView.Top;
                window.Left = this.reverseView.Left;
            }
            log.LogMethodExit();
        }

        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                reverseView = parameter as RedemptionReverseView;
                if (reverseView != null)
                {
                    reverseView.SizeChanged += OnReverseViewSizeChanged;
                }
            }
            log.LogMethodExit();
        }

        private void OnReverseViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (reverseView != null)
            {
                if (genericDataEntryView != null)
                {
                    this.OnWindowLoaded(genericDataEntryView, null);
                }
                if (managerView != null)
                {
                    this.OnWindowLoaded(managerView, null);
                }
                if (messagePopupView != null)
                {
                    this.OnWindowLoaded(messagePopupView, null);
                }
            }
            log.LogMethodExit();
        }
        private void OnCancelClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                reverseView = parameter as RedemptionReverseView;
                if (reverseView != null)
                {
                    reverseView.Close();
                }
            }
            log.LogMethodExit();
        }

        private async void OnReverseClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            try
            {
                if (parameter != null)
                {
                    redemptionActivityDTO = new RedemptionActivityDTO();
                    redemptionActivityDTO.Source = "POS Redemption";
                    RedemptionReverseView redemptionReverseView = parameter as RedemptionReverseView;
                    totalTicketsForReversal = 0;
                    List<RedemptionGiftsDTO> selectedGiftLinesForReversal = new List<RedemptionGiftsDTO>();
                    if (redemptionDTO.OrigRedemptionId > -1)
                    {
                        OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(this.ExecutionContext, "Reversal") + MessageViewContainerList.GetMessage(ExecutionContext, 2693, screenNumber),
                        string.Empty, MessageViewContainerList.GetMessage(this.ExecutionContext, 128),
                        string.Empty,
                        MessageViewContainerList.GetMessage(this.ExecutionContext, "OK"),
                        MessageButtonsType.OK);
                        log.LogMethodExit("reverseRedemption() - Selected Redemption is a reversal, not allowed to proceed.");
                        return;
                    }
                    foreach (object data in CustomDataGridVM.SelectedItems)
                    {
                        RedemptionGiftsDTO giftsDTO = data as RedemptionGiftsDTO;
                        if (giftsDTO != null)
                        {
                            selectedGiftLinesForReversal.Add(giftsDTO);
                            totalTicketsForReversal = totalTicketsForReversal + Convert.ToInt32(giftsDTO.Tickets);
                        }
                    }
                    if (selectedGiftLinesForReversal.Count() == 0)
                    {
                        OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(this.ExecutionContext, "Reversal") + MessageViewContainerList.GetMessage(ExecutionContext, 2693, screenNumber),
                            string.Empty, MessageViewContainerList.GetMessage(this.ExecutionContext, 2666),
                            string.Empty,
                            MessageViewContainerList.GetMessage(this.ExecutionContext, "OK"), MessageButtonsType.OK);
                        log.LogMethodExit("No rows selected for reversal");
                        return;
                    }
                    int mgrApprovalLimit = 0;
                    redemptionActivityDTO.ReversalRedemptionGiftDTOList = selectedGiftLinesForReversal;

                    mgrApprovalLimit = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "REDEMPTION_REVERSAL_LIMIT_FOR_MANAGER_APPROVAL", 0);
                    if ((totalTicketsForReversal > mgrApprovalLimit && mgrApprovalLimit != 0) || mgrApprovalLimit == 0)
                    {
                        int mgrId = -1;
                        if (!UserViewContainerList.IsSelfApprovalAllowed(this.ExecutionContext.SiteId, this.ExecutionContext.UserPKId))
                        {
                            managerView = new AuthenticateManagerView();
                            if (managerView != null)
                            {
                                managerView.Owner = reverseView;
                            }
                            managerView.PreviewMouseDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                            managerView.PreviewKeyDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
                            if (managerView.KeyboardHelper != null)
                            {
                                if (redemptionMainUserControlVM.MainVM != null && redemptionMainUserControlVM.MainVM.RedemptionHeaderTagsVM != null &&
                                    redemptionMainUserControlVM.MainVM.RedemptionHeaderTagsVM.HeaderGroups.Count > 1)
                                {
                                    managerView.KeyboardHelper.MultiScreenMode = redemptionMainUserControlVM.MultiScreenMode;
                                    managerView.KeyboardHelper.ColorCode = redemptionMainUserControlVM.ColorCode;
                                }
                                managerView.KeyboardHelper.KeypadMouseDownEvent -= redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                                managerView.KeyboardHelper.KeypadMouseDownEvent += redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                            }
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
                            mgrId = this.ExecutionContext.UserPKId;
                            redemptionActivityDTO.ManagerToken = ExecutionContext.WebApiToken;
                            ShowConfirmationPopup();
                        }
                    }
                    else
                    {
                        ShowConfirmationPopup();
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
        }
        private void SetRedemptionMainFocus()
        {
            log.LogMethodEntry();
            if (redemptionMainUserControlVM != null)
            {
                redemptionMainUserControlVM.SetUserControlFocus();
            }
            log.LogMethodExit();
        }
        private async void PrintMessagePopupView_Closed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                GenericMessagePopupVM genericMessagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
                if (genericMessagePopupVM != null && genericMessagePopupVM.ButtonClickType == ButtonClickType.Ok)
                {
                    redemptionActivityDTO.PrintBalanceTicket = true;
                }
                else
                {
                    redemptionActivityDTO.PrintBalanceTicket = false;
                }
                bool result = await ReverseRedemptionRefreshUI();
                if (result)
                {
                    redemptionMainUserControlVM.RedemptionUserControlVM.TodayCompletedRedemptions.Insert(0, NewRedemptionDTO);
                    reverseView.Close();
                    if (redemptionMainUserControlVM.RedemptionUserControlVM.ShowSearchCloseIcon)
                    {
                        if (redemptionMainUserControlVM.RedemptionUserControlVM.RedemptionDTOList != null)
                        {
                            redemptionMainUserControlVM.RedemptionUserControlVM.SetCustomDataGridVM(completedOrSuspendedRedemptions: redemptionMainUserControlVM.RedemptionUserControlVM.RedemptionDTOList);
                        }
                    }
                    else
                    {
                        redemptionMainUserControlVM.RedemptionUserControlVM.OnToggleChecked(null);
                    }
                    redemptionMainUserControlVM.RedemptionUserControlVM.SetOtherRedemptionList(RedemptionUserControlVM.ActionType.Complete, NewRedemptionDTO);
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 137), MessageType.Info);
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
            SetRedemptionMainFocus();
            log.LogMethodExit();
        }

        private async void OnManagerViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                AuthenticateManagerVM managerVM = (sender as AuthenticateManagerView).DataContext as AuthenticateManagerVM;
                if (managerVM != null && managerVM.IsValid)
                {
                    redemptionActivityDTO.ManagerToken = managerVM.ExecutionContext.WebApiToken;
                    ShowConfirmationPopup();
                }
                else
                {
                    OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(this.ExecutionContext, "Reversal") + MessageViewContainerList.GetMessage(ExecutionContext, 2693, screenNumber),
                            string.Empty, MessageViewContainerList.GetMessage(this.ExecutionContext, 268),
                            string.Empty, MessageViewContainerList.GetMessage(this.ExecutionContext, "OK"), MessageButtonsType.OK);
                    log.Error("Authentication Error");
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
            SetRedemptionMainFocus();
            log.LogMethodExit();
        }

        private async Task<bool> ReverseRedemption()
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
                IsLoadingVisible = true;
                NewRedemptionDTO = await redemptionUseCases.ReverseRedemption(redemptionDTO.RedemptionId, redemptionActivityDTO); // change to return redemptionDTO
                result = true;
                if (redemptionMainUserControlVM != null && redemptionMainUserControlVM.RedemptionUserControlVM != null)
                {
                    try
                    {
                        Task.Factory.StartNew(redemptionMainUserControlVM.RedemptionUserControlVM.UpdateStock, redemptionMainUserControlVM.CancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException ex)
                    {
                        redemptionMainUserControlVM.ResetRecalculateFlags();
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
            IsLoadingVisible = false;
            log.LogMethodExit(result);
            return result;
        }
        private async Task<List<RedemptionDTO>> GetRedemption(string redemptionId)
        {
            log.LogMethodEntry(redemptionId);
            List<RedemptionDTO> searchedRedemptionList = new List<RedemptionDTO>();
            IRedemptionUseCases RedemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
            List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchparams = new List<KeyValuePair<RedemptionDTO.SearchByParameters, string>>();
            searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.SITE_ID, Convert.ToString(ExecutionContext.GetSiteId())));
            if (!string.IsNullOrEmpty(redemptionId))
            {
                searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEPTION_ID, redemptionId));
            }
            searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.LOAD_GIFT_CARD_TICKET_ALLOCATION_DETAILS, "Y"));
            Task<List<RedemptionDTO>> taskGetRedemption = RedemptionUseCases.GetRedemptionOrders(searchparams);
            searchedRedemptionList = await taskGetRedemption;
            if (searchedRedemptionList == null)
            {
                searchedRedemptionList = new List<RedemptionDTO>();
            }
            log.LogMethodExit(searchedRedemptionList);
            return searchedRedemptionList;
        }
        private async Task<bool> ReverseRedemptionRefreshUI()
        {
            bool result = false;
            try
            {
                result = await ReverseRedemption();
                if (result)
                {
                    List<RedemptionDTO> reversedRedemptionDTO = new List<RedemptionDTO>();
                    try
                    {
                        if (redemptionMainUserControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption)
                        {
                            reversedRedemptionDTO = await GetRedemption(redemptionDTO.RedemptionId.ToString());
                        }
                        else if (redemptionMainUserControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.TurnIn)
                        {
                            reversedRedemptionDTO = await GetRedemption(redemptionDTO.RedemptionId.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug("Unable to fetch redemption after reversal for redemption " + redemptionDTO.RedemptionId + " " + ex.Message);
                    }
                    if (reversedRedemptionDTO != null)
                    {
                        RedemptionDTO updatedRedemptionDTO = reversedRedemptionDTO.FirstOrDefault();
                        if (redemptionMainUserControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption
                            && redemptionMainUserControlVM.RedemptionUserControlVM != null &&
                            redemptionMainUserControlVM.RedemptionUserControlVM.TodayCompletedRedemptions != null
                            && updatedRedemptionDTO != null)
                        {
                            RedemptionDTO redemption = redemptionMainUserControlVM.RedemptionUserControlVM.TodayCompletedRedemptions.FirstOrDefault(x => x.RedemptionId == redemptionDTO.RedemptionId);
                            if(redemption != null)
                            {
                                redemption.RedemptionGiftsListDTO = updatedRedemptionDTO.RedemptionGiftsListDTO;
                            }
                        }
                        else if (redemptionMainUserControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.TurnIn
                            && redemptionMainUserControlVM.TurnInUserControlVM != null && redemptionMainUserControlVM.TurnInUserControlVM.TodayCompletedRedemptions != null
                            && updatedRedemptionDTO != null)
                        {
                            RedemptionDTO redemption = redemptionMainUserControlVM.TurnInUserControlVM.TodayCompletedRedemptions.FirstOrDefault(x => x.RedemptionId == redemptionDTO.RedemptionId);
                            if (redemption != null)
                            {
                                redemption.RedemptionGiftsListDTO = updatedRedemptionDTO.RedemptionGiftsListDTO;
                            }
                        }
                    }
                    if (redemptionActivityDTO.PrintBalanceTicket && redemptionMainUserControlVM.LeftPaneSelectedItem == LeftPaneSelectedItem.Redemption)
                    {
                        result = await PrintTicket();
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
            return result;
        }

        internal async Task<bool> PrintTicket()
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                ITicketReceiptUseCases ticketUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(ExecutionContext);
                List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchparams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, Convert.ToString(ExecutionContext.GetSiteId())));
                if (NewRedemptionDTO != null && NewRedemptionDTO.RedemptionId >= 0)
                {
                    searchparams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID, NewRedemptionDTO.RedemptionId.ToString()));
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
                        OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(this.ExecutionContext, "Reversal") + MessageViewContainerList.GetMessage(ExecutionContext, 2693, screenNumber),
                        string.Empty, MessageViewContainerList.GetMessage(ExecutionContext, "Print Error"),
                        string.Empty, MessageViewContainerList.GetMessage(this.ExecutionContext, "OK"), MessageButtonsType.OK);
                        if (messagePopupView != null)
                        {
                            messagePopupView.Closed += OnPrintErrorMessagePopupViewClosed;
                        }
                    }
                }
                else
                {
                    log.Debug("No tickets receipt found for the redemption " + redemptionDTO.RedemptionId);
                    OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(this.ExecutionContext, "Reversal") + MessageViewContainerList.GetMessage(ExecutionContext, 2693, screenNumber),
                        string.Empty, MessageViewContainerList.GetMessage(ExecutionContext, "Print Error"),
                        string.Empty, MessageViewContainerList.GetMessage(this.ExecutionContext, "OK"), MessageButtonsType.OK);
                    if (messagePopupView != null)
                    {
                        messagePopupView.Closed += OnPrintErrorMessagePopupViewClosed;
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(this.ExecutionContext, "Reversal") + MessageViewContainerList.GetMessage(ExecutionContext, 2693, screenNumber),
                                        string.Empty, MessageViewContainerList.GetMessage(ExecutionContext, "Print Error"),
                                         string.Empty, MessageViewContainerList.GetMessage(this.ExecutionContext, "OK"), MessageButtonsType.OK);
                if (messagePopupView != null)
                {
                    messagePopupView.Closed += OnPrintErrorMessagePopupViewClosed;
                }
            }
            log.LogMethodExit();
            return result;
        }
        private void OnPrintErrorMessagePopupViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.reverseView != null)
            {
                foreach (Window w in reverseView.OwnedWindows)
                {
                    if (w == managerView && managerView != null)
                    {
                        if (managerView.DataContext != null)
                        {
                            AuthenticateManagerVM managerVM = managerView.DataContext as AuthenticateManagerVM;
                            if (managerVM != null)
                            {
                                managerVM.CardReader.UnRegister();
                            }
                        }
                        managerView.Close();
                    }
                    else
                    {
                        w.Close();
                    }
                }
                reverseView.Close();
            }
            SetRedemptionMainFocus();
            log.LogMethodExit();
        }
        private void ShowConfirmationPopup()
        {
            log.LogMethodEntry();
            OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(ExecutionContext, "Reversal Confirmation") + MessageViewContainerList.GetMessage(ExecutionContext, 2693, screenNumber),
                                        Heading, MessageViewContainerList.GetMessage(ExecutionContext, 130),
                                        MessageViewContainerList.GetMessage(this.ExecutionContext, "OK"),
                                        MessageViewContainerList.GetMessage(ExecutionContext, "CANCEL"),
                                        MessageButtonsType.OkCancel);
            if (messagePopupView != null)
            {
                messagePopupView.Closed += OnConfirmationPopupViewClosed;
            }
            log.LogMethodExit();
        }

        private void OnConfirmationPopupViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender);
            GenericMessagePopupVM genericMessagePopupVM = (sender as GenericMessagePopupView).DataContext as GenericMessagePopupVM;
            if (genericMessagePopupVM != null && genericMessagePopupVM.ButtonClickType == ButtonClickType.Ok)
            {
                GenericDataEntryVM dataEntryVM = new GenericDataEntryVM(ExecutionContext)
                {
                    Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Redemption Reversal Remarks"),
                    DataEntryCollections = new ObservableCollection<DataEntryElement>()
                    {
                        new DataEntryElement()
                        {
                            Type=DataEntryType.TextBox,
                            DefaultValue = MessageViewContainerList.GetMessage(ExecutionContext, "Enter"),
                            IsMandatory = true,
                            Heading =  MessageViewContainerList.GetMessage(ExecutionContext, 132),
                            ValidationType = ValidationType.None
                        },
                    }
                };
                genericDataEntryView = new GenericDataEntryView();
                genericDataEntryView.DataContext = dataEntryVM;
                genericDataEntryView.Closed += OnDataEntryiewClosed;
                genericDataEntryView.Loaded += this.OnWindowLoaded;
                if (genericDataEntryView.KeyBoardHelper != null)
                {
                    genericDataEntryView.KeyBoardHelper.MultiScreenMode = redemptionMainUserControlVM.MultiScreenMode;
                    genericDataEntryView.KeyBoardHelper.ColorCode = redemptionMainUserControlVM.ColorCode;
                    genericDataEntryView.KeyBoardHelper.KeypadMouseDownEvent -= redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                    genericDataEntryView.KeyBoardHelper.KeypadMouseDownEvent += redemptionMainUserControlVM.UpdateActivityTimeOnAction;
                }
                genericDataEntryView.Show();
            }
            SetRedemptionMainFocus();
            log.LogMethodExit();
        }

        private async void OnDataEntryiewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender);
            GenericDataEntryVM dataEntryVM = (sender as GenericDataEntryView).DataContext as GenericDataEntryVM;
            if (dataEntryVM != null)
            {
                if (dataEntryVM.ButtonClickType == ButtonClickType.Ok)
                {
                    redemptionActivityDTO.Remarks = dataEntryVM.DataEntryCollections.Where(x => x.Type == DataEntryType.TextBox && x.Heading == MessageViewContainerList.GetMessage(ExecutionContext, 132)).FirstOrDefault().Text;
                    if (redemptionDTO.CardId >= 0)
                    {
                        redemptionActivityDTO.LoadToCard = true;
                        bool result = await ReverseRedemptionRefreshUI();
                        if (result)
                        {
                            redemptionMainUserControlVM.RedemptionUserControlVM.TodayCompletedRedemptions.Insert(0, NewRedemptionDTO);
                            reverseView.Close();
                            if (redemptionMainUserControlVM.RedemptionUserControlVM.ShowSearchCloseIcon)
                            {
                                if (redemptionMainUserControlVM.RedemptionUserControlVM.RedemptionDTOList != null)
                                {
                                    redemptionMainUserControlVM.RedemptionUserControlVM.SetCustomDataGridVM(completedOrSuspendedRedemptions: redemptionMainUserControlVM.RedemptionUserControlVM.RedemptionDTOList);
                                }
                            }
                            else
                            {
                                redemptionMainUserControlVM.RedemptionUserControlVM.OnToggleChecked(null);
                            }
                            redemptionMainUserControlVM.RedemptionUserControlVM.SetOtherRedemptionList(RedemptionUserControlVM.ActionType.Complete, NewRedemptionDTO);
                            redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 135), MessageType.Info);
                        }
                    }
                    else
                    {
                        if (totalTicketsForReversal > 0)
                        {
                            OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(this.ExecutionContext, "Reversed") + " " + ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT") + MessageViewContainerList.GetMessage(ExecutionContext, 2693, screenNumber),
                                            string.Empty, MessageViewContainerList.GetMessage(this.ExecutionContext, 138, totalTicketsForReversal, ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REDEMPTION_TICKET_NAME_VARIANT")),
                                            MessageViewContainerList.GetMessage(this.ExecutionContext, "OK"),
                                            MessageViewContainerList.GetMessage(ExecutionContext, "CANCEL"),
                                            MessageButtonsType.OkCancel);
                            if (messagePopupView != null)
                            {
                                messagePopupView.Closed += PrintMessagePopupView_Closed;
                            }
                        }
                        else
                        {
                            redemptionActivityDTO.LoadToCard = true;
                            bool result = await ReverseRedemptionRefreshUI();
                            if (result)
                            {
                                redemptionMainUserControlVM.RedemptionUserControlVM.TodayCompletedRedemptions.Insert(0, NewRedemptionDTO);
                                reverseView.Close();
                                if (redemptionMainUserControlVM.RedemptionUserControlVM.ShowSearchCloseIcon)
                                {
                                    if (redemptionMainUserControlVM.RedemptionUserControlVM.RedemptionDTOList != null)
                                    {
                                        redemptionMainUserControlVM.RedemptionUserControlVM.SetCustomDataGridVM(completedOrSuspendedRedemptions: redemptionMainUserControlVM.RedemptionUserControlVM.RedemptionDTOList);
                                    }
                                }
                                else
                                {
                                    redemptionMainUserControlVM.RedemptionUserControlVM.OnToggleChecked(null);
                                }
                                redemptionMainUserControlVM.RedemptionUserControlVM.SetOtherRedemptionList(RedemptionUserControlVM.ActionType.Complete, NewRedemptionDTO);
                                redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 135), MessageType.Info);
                            }
                        }
                    }
                }
            }
            else
            {
                OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(ExecutionContext, "Reversal") + MessageViewContainerList.GetMessage(ExecutionContext, 2693, screenNumber),
                    Heading, MessageViewContainerList.GetMessage(ExecutionContext, 134),
                    MessageViewContainerList.GetMessage(ExecutionContext, "OK"),
                    MessageViewContainerList.GetMessage(ExecutionContext, "CANCEL"),
                    MessageButtonsType.OkCancel);
                if (messagePopupView != null)
                {
                    messagePopupView.Closed += OnConfirmationPopupViewClosed;
                }
                log.LogMethodExit("Ends-reverseRedemption() as Remarks was not entered for Reversal");
                return;
            }
            SetRedemptionMainFocus();
            log.LogMethodExit();
        }
        private void CloseWindow()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        private void OpenGenericMessagePopupView(string heading, string subHeading, string content,
            string okButtonText, string cancelButtonText, MessageButtonsType messageButtonsType)
        {
            log.LogMethodEntry(heading, subHeading, content, okButtonText, cancelButtonText, messageButtonsType);
            this.messagePopupView = new GenericMessagePopupView();
            this.messagePopupView.PreviewMouseDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
            this.messagePopupView.PreviewKeyDown += redemptionMainUserControlVM.UpdateActivityTimeOnMouseOrKeyBoardAction;
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
            messagePopupView.Show();
            log.LogMethodExit();
        }
        internal new void ExecuteAction(Action method)
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
        }
        #endregion

        #region Constructor
        public RedemptionReverseVM(ExecutionContext executionContext, RedemptionDTO redemptionDTO, string screenNumber, DeviceClass cardReader, RedemptionMainUserControlVM redemptionMainUserControlVM)
        {
            log.LogMethodEntry();
            ExecuteAction(() =>
            {
                this.ExecutionContext = executionContext;
                this.screenNumber = screenNumber;
                this.cardReader = cardReader;
                this.redemptionMainUserControlVM = redemptionMainUserControlVM;
                cancelCommand = new DelegateCommand(OnCancelClicked);
                reverseCommand = new DelegateCommand(OnReverseClicked);
                loadedCommand = new DelegateCommand(OnLoaded);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<RedemptionDTO>> task = GetRedemption(redemptionDTO.RedemptionId.ToString());
                    task.Wait();
                    if (task.Result != null)
                    {
                        this.redemptionDTO = task.Result.FirstOrDefault();
                    }
                    else
                    {
                        this.redemptionDTO = redemptionDTO;
                    }
                }
                multiScreenMode = false;
                ismultiScreenRowTwo = false;
                isReverseEnabled = true;

                title = MessageViewContainerList.GetMessage(executionContext, "Redemption Reverse Details");
                heading = "RO-";
                if (!string.IsNullOrEmpty(redemptionDTO.RedemptionOrderNo))
                {
                    heading = redemptionDTO.RedemptionOrderNo;
                }
                cancelButtonText = MessageViewContainerList.GetMessage(executionContext, "CANCEL");
                reverseButtonText = MessageViewContainerList.GetMessage(executionContext, "REVERSE");
                SetCustomDataGridVM();
                if (redemptionDTO.RedemptionGiftsListDTO.All(g => g.GiftLineIsReversed == true))
                {
                    isReverseEnabled = false;
                }
            });
            log.LogMethodExit();
        }
        #endregion
    }
}

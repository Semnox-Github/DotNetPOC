/********************************************************************************************
 * Project Name - Transaction UI
 * Description  - PaymentSettlement VM
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     17-Sep-2021    Fiona                  Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.KDS;
using Semnox.Parafait.User;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.ViewContainer;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Authentication;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Customer;
using Semnox.Parafait.DeliveryIntegration;

namespace Semnox.Parafait.TransactionUI
{
    public class PaymentSettlementVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string dateTimeFormat;
        private string moduleName;
        private string transactionId;
        private bool showSearchArea;
        private string fromDate;
        private string toDate;
        private PaymentSettlementView paymentSettlementView;
        private ICommand navigationClickCommand;
        private ICommand loadedCommand;
        private ICommand actionsCommand;
        private CustomDataGridVM customDataGridVM;
        private ObservableCollection<TransactionPaymentSummaryDTO> transactionPaymentSummaryDTOCollection;
        private ObservableCollection<PaymentModeDTO> paymentModes;
        private ObservableCollection<DeliveryChannelDTO> channels;
        private PaymentModeDTO selectedPaymentMode;
        private DeliveryChannelDTO selectedChannel;
        private bool settled;
        private bool isLoadingVisble;
        private bool enableOrderShareAccrossPos;
        private bool enableOrderShareAccrossPosUsers;
        private bool enableOrderShareAccrossPosCounters;
        private Utilities utilities;
        private string oldMode;
        private Dictionary<int, TransactionPaymentsDTO> transactionPaymentDictionary;
        private List<TransactionPaymentsDTO> unsettledTransactionPaymentsDTOList;
        private DeviceClass cardReader;
        private int managerId;
        private string approvedBy;



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
                if (!object.Equals(moduleName, value))
                {
                    moduleName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string TransactionId
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(transactionId);
                return transactionId;
            }
            set
            {
                if (!object.Equals(transactionId, value))
                {
                    transactionId = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<DeliveryChannelDTO> Channels
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(channels);
                return channels;
            }
            set
            {
                if (!object.Equals(channels, value))
                {
                    channels = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsLoadingVisble
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isLoadingVisble);
                return isLoadingVisble;
            }
            set
            {
                if (!object.Equals(isLoadingVisble, value))
                {
                    isLoadingVisble = value;
                    OnPropertyChanged();
                }
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
                if (!object.Equals(navigationClickCommand, value))
                {
                    navigationClickCommand = value;
                    OnPropertyChanged();
                }
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
                if (!object.Equals(loadedCommand, value))
                {
                    loadedCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand ActionsCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(actionsCommand);
                return actionsCommand;
            }
            set
            {
                if (!object.Equals(actionsCommand, value))
                {
                    actionsCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool ShowSearchArea
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showSearchArea);
                return showSearchArea;
            }
            set
            {
                if (!object.Equals(showSearchArea, value))
                {
                    showSearchArea = value;
                    OnPropertyChanged();
                }
            }
        }
        public PaymentModeDTO SelectedPaymentMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedPaymentMode);
                return selectedPaymentMode;
            }
            set
            {
                if (!object.Equals(selectedPaymentMode, value))
                {
                    selectedPaymentMode = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<TransactionPaymentSummaryDTO> TransactionPaymentSummaryDTOCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(transactionPaymentSummaryDTOCollection);
                return transactionPaymentSummaryDTOCollection;
            }
            set
            {
                if (!object.Equals(transactionPaymentSummaryDTOCollection, value))
                {
                    transactionPaymentSummaryDTOCollection = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<PaymentModeDTO> PaymentModes
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(paymentModes);
                return paymentModes;
            }
            set
            {
                if (!object.Equals(paymentModes, value))
                {
                    paymentModes = value;
                    OnPropertyChanged();
                }
            }
        }
        public CustomDataGridVM TransactionPaymentsCustomDataGridVM
        {
            get
            {
                log.LogMethodEntry();
                if (customDataGridVM == null)
                {
                    customDataGridVM = new CustomDataGridVM(ExecutionContext)
                    {
                        IsComboAndSearchVisible = false,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    };
                }
                log.LogMethodExit(customDataGridVM);
                return customDataGridVM;
            }
            set
            {
                if (!object.Equals(customDataGridVM, value))
                {
                    customDataGridVM = value;
                    OnPropertyChanged();
                }
            }
        }

        public DeliveryChannelDTO SelectedChannel
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return selectedChannel;
            }
            set
            {
                if (!object.Equals(selectedChannel, value))
                {
                    selectedChannel = value;
                    OnPropertyChanged();
                }
            }
        }
        public string FromDate
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(fromDate);
                return fromDate;
            }
            set
            {
                if (!object.Equals(fromDate, value))
                {
                    fromDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ToDate
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(toDate);
                return toDate;
            }
            set
            {
                if (!object.Equals(toDate, value))
                {
                    toDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool Settled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(settled);
                return settled;
            }
            set
            {
                if (!object.Equals(settled, value))
                {
                    settled = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
        #region Methods
        private void OnNavigationClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (paymentSettlementView != null)
            {
                paymentSettlementView.Close();
            }
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                paymentSettlementView = parameter as PaymentSettlementView;
                IsLoadingVisble = true;
                PerformDBSearch();
                IsLoadingVisble = false;
                ShowSearchArea = true;
            }
            log.LogMethodExit();
        }
        private void OnActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            IsLoadingVisible = true;
            if (parameter != null)
            {
                Button button = parameter as Button;
                if (button != null && !string.IsNullOrEmpty(button.Name))
                {
                    switch (button.Name)
                    {
                        case "btnSearch":
                            {
                                PerformDBSearch();
                            }
                            break;
                        case "btnClear":
                            {
                                SetDefaultValues();
                            }
                            break;
                        case "btnSeachVisible":
                            {
                                ShowSearchArea = !showSearchArea;
                            }
                            break;
                        case "btnSettlements":
                            {
                                PerformSettlements();
                            }
                            break;
                        case "btnEdit":
                            {
                                PerformEdit();
                            }
                            break;
                        case "btnShowPrevious":
                            {
                                PerformPreviousView();
                            }
                            break;

                    }
                }
            }
            IsLoadingVisible = false;
            log.LogMethodExit();
        }

        private void PerformPreviousView()
        {
            log.LogMethodEntry();
            OldMode = "Y";
            if (paymentSettlementView != null)
            {
                paymentSettlementView.Close();
            }
            log.LogMethodExit();
        }

        private void GetPaymentModes()
        {
            log.LogMethodEntry();
            IPaymentModesUseCases paymentModesUseCases = PaymentModesUseCaseFactory.GetPaymentModesUseCases(ExecutionContext);
            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
            List<PaymentModeDTO> paymentModeDTOList = null;
            try
            {
                // await deliveryChannelUseCases.GetDeliveryChannel(searchParameters);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<PaymentModeDTO>> task = paymentModesUseCases.GetPaymentModes(searchParameters, false);
                    task.Wait();
                    paymentModeDTOList = task.Result;
                }
                if (paymentModeDTOList != null)
                {
                    PaymentModes = new ObservableCollection<PaymentModeDTO>(paymentModeDTOList);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
            }

            log.LogMethodExit();
        }
        private void GetChannels()
        {
            log.LogMethodEntry();
            IDeliveryChannelUseCases deliveryChannelUseCases = DeliveryChannelUseCaseFactory.GetDeliveryChannelUseCases(ExecutionContext);
            List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>(DeliveryChannelDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<DeliveryChannelDTO> deliveryChannelDTOList = null;
            try
            {
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<DeliveryChannelDTO>> task = deliveryChannelUseCases.GetDeliveryChannel(searchParameters);
                    task.Wait();
                    deliveryChannelDTOList = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
            }
            if (deliveryChannelDTOList != null)
            {
                Channels = new ObservableCollection<DeliveryChannelDTO>(deliveryChannelDTOList);
            }
            log.LogMethodExit();

            log.LogMethodExit();
        }


        private void PerformEdit()
        {
            log.LogMethodEntry();
            EditPaymentsView editPaymentsView = new EditPaymentsView();
            EditPaymentsVM editPaymentsVM = new EditPaymentsVM(ExecutionContext);
            editPaymentsView.DataContext = editPaymentsVM;
            if(paymentSettlementView!=null)
            {
                editPaymentsView.Owner = paymentSettlementView;
            }
            editPaymentsView.ShowDialog();
            log.LogMethodExit();
        }

        private void ShowSelectedSettledTrxMessage(string settledpaymentsMessage)
        {
            log.LogMethodEntry();
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            //messagePopupView.Loaded += OnWindowLoaded;
            if (paymentSettlementView != null)
            {
                messagePopupView.Owner = this.paymentSettlementView;
            }
            GenericMessagePopupVM genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
            {
                OkButtonText = MessageViewContainerList.GetMessage(ExecutionContext,"Yes"),
                CancelButtonText = MessageViewContainerList.GetMessage(ExecutionContext,"No"),
                MessageButtonsType = MessageButtonsType.OK,
                SubHeading = "",
                Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Already Settled Payments"),
                Content = settledpaymentsMessage
            };
            messagePopupView.DataContext = genericMessagePopupVM;
            messagePopupView.ShowDialog();
            
            log.LogMethodExit();
        }

       
        public bool ShowManagerApproval()
        {
            bool result = false;
            log.LogMethodEntry();
            if (!UserViewContainerList.IsSelfApprovalAllowed(this.ExecutionContext))
            {
                AuthenticateManagerView managerView = new AuthenticateManagerView();
                AuthenticateManagerVM managerVM = new AuthenticateManagerVM(this.ExecutionContext, cardReader);
                managerView.DataContext = managerVM;
                if (paymentSettlementView != null)
                {
                    managerView.Owner = paymentSettlementView;
                }
                managerView.ShowDialog();
                if (managerVM.IsValid)
                {
                    managerId = Convert.ToInt32(managerVM.ManagerId);
                    approvedBy = UserViewContainerList.GetUserContainerDTO(ExecutionContext.SiteId, managerId).LoginId;
                    result = true;
                }
                else
                {
                    managerId = -1;
                    approvedBy = string.Empty;
                    result = false;
                }
            }
            else
            {
                managerId = Convert.ToInt32(this.ExecutionContext.GetUserPKId());
                approvedBy = UserViewContainerList.GetUserContainerDTO(ExecutionContext.SiteId, managerId).LoginId;
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }

        private void PerformSettlements()
        {
            log.LogMethodEntry();
            //IsLoadingVisble = true;
            if (TransactionPaymentsCustomDataGridVM.SelectedItems != null && TransactionPaymentsCustomDataGridVM.SelectedItems.Any())
            {
                List<TransactionPaymentsDTO> transactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
                List<TransactionPaymentSummaryDTO> transactionPaymentSummaryDTOList = new List<TransactionPaymentSummaryDTO>();
                StringBuilder stringB = new StringBuilder("");
                
                foreach (TransactionPaymentSummaryDTO selectedTransactionPaymentSummaryDTO in TransactionPaymentsCustomDataGridVM.SelectedItems)
                {
                    if (selectedTransactionPaymentSummaryDTO.Settled == false)
                    {
                        transactionPaymentSummaryDTOList.Add(selectedTransactionPaymentSummaryDTO);
                        transactionPaymentsDTOList.Add(transactionPaymentDictionary[selectedTransactionPaymentSummaryDTO.PaymentId]);
                    }
                    else
                    {
                        stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, 4127, selectedTransactionPaymentSummaryDTO.PaymentId));
                        //Transaction Payment with Id &1 is already settled.
                        stringB.Append(Environment.NewLine);
                    }

                }
                if(transactionPaymentSummaryDTOList.Count == 0)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4126), MessageType.Info);
                    //"All Selected Transactions are settled"
                    return;
                }
                if(!string.IsNullOrEmpty(stringB.ToString()))
                {
                    ShowSelectedSettledTrxMessage(stringB.ToString());
                }


                GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                if (paymentSettlementView != null)
                {
                    messagePopupView.Owner = this.paymentSettlementView;
                }
                GenericMessagePopupVM genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                {
                    OkButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "Yes"),
                    CancelButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "No"),
                    MessageButtonsType = MessageButtonsType.OkCancel,
                    SubHeading = string.Empty,
                    Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Enter tip before settlement"),
                    Content = MessageViewContainerList.GetMessage(ExecutionContext, "Do you want to update tip before settlement?")
                };
                messagePopupView.DataContext = genericMessagePopupVM;
                messagePopupView.ShowDialog();
                messagePopupView.Loaded += OnWindowLoaded;
                if (genericMessagePopupVM.ButtonClickType == ButtonClickType.Ok)
                {
                    bool canupdateTip = true;
                    if (ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "MANAGER_APPROVAL_REQUIRED_FOR_TIP_ADJUSTMENT"))
                    {
                        canupdateTip = ShowManagerApproval();
                    }
                    if (canupdateTip == false)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Manager Approval Required"), MessageType.Info);
                        return;
                    }
                    TipUpdateView tipUpdateView = new TipUpdateView();
                    TipUpdateVM tipUpdateVM = new TipUpdateVM(ExecutionContext, transactionPaymentSummaryDTOList);
                    tipUpdateView.DataContext = tipUpdateVM;
                    if (paymentSettlementView != null)
                    {
                        tipUpdateView.Owner = paymentSettlementView;
                        tipUpdateView.ShowDialog();
                        if (tipUpdateVM.ButtonClickType == ButtonClickType.Cancel)
                        {
                            return;
                        }
                    }
                }
                foreach (TransactionPaymentsDTO transactionPaymentsDTO in transactionPaymentsDTOList)
                {
                    transactionPaymentsDTO.TipAmount = transactionPaymentSummaryDTOList.FirstOrDefault(x => x.PaymentId == transactionPaymentsDTO.PaymentId).TipAmount;
                    transactionPaymentsDTO.ApprovedBy = approvedBy;
                }
                managerId = -1;
                approvedBy = string.Empty;
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    try
                    {
                        Task<List<KeyValuePair<TransactionPaymentsDTO, string>>> task = transactionUseCases.SettleTransactionPayments(transactionPaymentsDTOList);
                        task.Wait();
                        List<KeyValuePair<TransactionPaymentsDTO, string>> keyValuePairs = task.Result;
                        if (keyValuePairs != null && keyValuePairs.Any())
                        {
                            string successMessage = MessageViewContainerList.GetMessage(ExecutionContext, 2206);
                            ShowMessages(keyValuePairs, successMessage);
                        }
                    }
                    catch (ValidationException vex)
                    {
                        log.Error(vex);
                        SetFooterContent(vex.ToString(), MessageType.Error);
                    }
                    catch (UnauthorizedException uaex)
                    {
                        log.Error(uaex);
                        SetFooterContent(uaex.ToString(), MessageType.Error); ;
                    }
                    catch (ParafaitApplicationException pax)
                    {
                        log.Error(pax);
                        SetFooterContent(pax.ToString(), MessageType.Error);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
                    }
                }
            }
            PerformDBSearch();
            //IsLoadingVisble = false;
            log.LogMethodExit();
        }
        private void ShowMessages(List<KeyValuePair<TransactionPaymentsDTO, string>> keyValuePairs, string successMsg)
        {
            log.LogMethodEntry(keyValuePairs, successMsg);
            List<KeyValuePair<TransactionPaymentsDTO, string>> errorRecordList = keyValuePairs.Where(key => key.Value != null).ToList();
            if (errorRecordList != null && errorRecordList.Any())
            {
                StringBuilder stringB = new StringBuilder("");
                for (int i = 0; i < errorRecordList.Count; i++)
                {
                    stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Trxn Id: ") + errorRecordList[i].Key.TransactionId + ", ");
                    stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Payment Id: ") + errorRecordList[i].Key.PaymentId + ", ");
                    stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Message: " + errorRecordList[i].Value));
                    stringB.Append(System.Environment.NewLine);
                }
                OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(ExecutionContext, "Message"), string.Empty,
                stringB.ToString(), MessageViewContainerList.GetMessage(ExecutionContext, "OK"), MessageViewContainerList.GetMessage(ExecutionContext, "OK"),
                MessageButtonsType.OK);
            }
            //else
            //{
            //    OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(ExecutionContext, "Message"), string.Empty,
            //    successMsg, MessageViewContainerList.GetMessage(ExecutionContext, "OK"), MessageViewContainerList.GetMessage(ExecutionContext, "OK"),
            //    MessageButtonsType.OK);
            //    //if (tipUpdatetView != null)
            //    //{
            //    //    tipUpdatetView.Close();
            //    //}
            //}
            log.LogMethodExit();
        }

        private void OpenGenericMessagePopupView(string heading, string subHeading, string content,
            string okButtonText, string cancelButtonText, MessageButtonsType messageButtonsType)
        {
            log.LogMethodEntry(heading, subHeading, content, okButtonText, cancelButtonText, messageButtonsType);
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            //messagePopupView.Loaded += OnWindowLoaded;
            if (paymentSettlementView != null)
            {
                messagePopupView.Owner = this.paymentSettlementView;
            }
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
            //messagePopupView.Closed += OnWindowClosed;
            messagePopupView.ShowDialog();
            //messagePopupView.Loaded += OnWindowLoaded;
            log.LogMethodExit();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private void OnWindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Window window = sender as Window;
            if (paymentSettlementView != null)
            {
                window.Owner = this.paymentSettlementView;
                window.Width = this.paymentSettlementView.Width;
                window.Height = this.paymentSettlementView.Height;
                window.Top = this.paymentSettlementView.Top;
                window.Left = this.paymentSettlementView.Left;
            }
            log.LogMethodExit();
        }

        private void SetInitialtValues()
        {
            log.LogMethodEntry();
            PaymentModes = new ObservableCollection<PaymentModeDTO>();
            TransactionPaymentSummaryDTOCollection = new ObservableCollection<TransactionPaymentSummaryDTO>();
            transactionPaymentDictionary = new Dictionary<int, TransactionPaymentsDTO>();
            ShowSearchArea = true;
            Channels = new ObservableCollection<DeliveryChannelDTO>();
            enableOrderShareAccrossPos = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ENABLE_ORDER_SHARE_ACROSS_POS");
            enableOrderShareAccrossPosUsers = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ENABLE_ORDER_SHARE_ACROSS_USERS");
            enableOrderShareAccrossPosCounters = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS");
            utilities = GetUtility();
            dateTimeFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT");
            unsettledTransactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
            log.LogMethodExit();
        }

        private void SetTransactionPaymentsCustomDataGridVM()
        {
            log.LogMethodEntry();
            TransactionPaymentsCustomDataGridVM = new CustomDataGridVM(ExecutionContext)
            {
                IsComboAndSearchVisible = false,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                SelectOption = SelectOption.CheckBox
            };
            TransactionPaymentsCustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(transactionPaymentSummaryDTOCollection);
            TransactionPaymentsCustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
            {
                { "Settled",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Settled?"),
                        Type=DataEntryType.CheckBox,
                        IsReadOnly = true
                    }
                },
                { "TransactionId",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Id")
                    }
                },
                { "TransactionNumber",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Number")
                    }
                },
                { "TransactionDate",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Date"),
                        DataGridColumnStringFormat = dateTimeFormat
                    }
                },
                { "Customer",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Customer")
                    }
                },
                { "PaymentMode",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Payment Mode")
                    }
                },
                { "PaymentAmount",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Amount")
                    }
                },
                { "TipAmount",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Tip")
                    }
                },
                { "PaymentReference",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Reference")
                    }
                }
            };
            log.LogMethodExit();
        }
        //private void GeTransactionPaymentSummaryDTOList()
        //{
        //    transactionPaymentSummaryDTOCollection = new ObservableCollection<TransactionPaymentSummaryDTO>();
        //    TransactionPaymentSummaryDTO transactionPaymentSummaryDTO = new TransactionPaymentSummaryDTO(1, "11111", DateTime.Now, "Suresh", "Cash", 1000, 20, "pr1", true,"111",100,1);
        //    transactionPaymentSummaryDTOCollection.Add(new TransactionPaymentSummaryDTO(1, "11111", DateTime.Now, "Suresh", "Cash", 1000, 20, "pr1", true,"111",100,2));
        //    transactionPaymentSummaryDTOCollection.Add(new TransactionPaymentSummaryDTO(2, "22222", DateTime.Now, "Mahesh", "Cash", 1000, 20, "pr1", true, "111",100,3));
        //    transactionPaymentSummaryDTOCollection.Add(new TransactionPaymentSummaryDTO(3, "33333", DateTime.Now, "Mahesh", "Cash", 1000, 20, "pr1", true, "111", 100,4));
        //    transactionPaymentSummaryDTOCollection.Add(new TransactionPaymentSummaryDTO(3, "33333", DateTime.Now, "Mahesh", "Cash", 1000, 20, "pr1", true, "111",100,5));
        //}
        private void PerformDBSearch()
        {
            log.LogMethodEntry();

            //IsLoadingVisble = true;
            ShowSearchArea = false;
            try
            {
                DateTime fromDateTime;
                DateTime toDateTime;
                List<TransactionDTO> transactions = null;
                ExecutionContext execution = SystemUserExecutionContextBuilder.GetSystemUserExecutionContext();
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();

                if (enableOrderShareAccrossPos == false)
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_NAME, ExecutionContext.POSMachineName));
                }
                if (enableOrderShareAccrossPosUsers == false)
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.USER_ID, ExecutionContext.UserPKId.ToString()));
                }
                if (enableOrderShareAccrossPosCounters == false)
                {
                    POSMachineContainerDTO pOSMachineContainerDTO = POSMachineViewContainerList.GetPOSMachineContainerDTO(ExecutionContext);
                    int posTypeId = pOSMachineContainerDTO.POSTypeId;
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_TYPE_ID, posTypeId.ToString()));
                }
                if (!string.IsNullOrEmpty(transactionId))
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, transactionId));
                }
                else
                {
                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        fromDateTime = DateTime.Parse(fromDate);
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, fromDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                    if (!string.IsNullOrEmpty(toDate))
                    {
                        toDateTime = DateTime.Parse(toDate);
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, toDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                    if (selectedPaymentMode != null)
                    {

                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRX_PAYMENT_MODE_ID, selectedPaymentMode.PaymentModeId.ToString()));
                    }

                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, Transaction.Transaction.TrxStatus.OPEN.ToString() + ","
                                                                                                                                             + Transaction.Transaction.TrxStatus.INITIATED.ToString() + ","
                                                                                                                                             + Transaction.Transaction.TrxStatus.BOOKING.ToString() + ","
                                                                                                                                             + Transaction.Transaction.TrxStatus.ORDERED.ToString() + ","
                                                                                                                                             + Transaction.Transaction.TrxStatus.PENDING.ToString() + ","
                                                                                                                                             + Transaction.Transaction.TrxStatus.PREPARED.ToString()));


                }
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<TransactionDTO>> task = transactionUseCases.GetTransactionDTOList(searchParameters, utilities, null, 0, 1000, true, false, false);
                    task.Wait();
                    transactions = task.Result;
                }

                if (transactions == null)
                {
                    transactions = new List<TransactionDTO>();
                }
                if (selectedChannel != null)
                {
                    transactions = transactions.Where(t => t.TransctionOrderDispensingDTO.DeliveryChannelId == selectedChannel.DeliveryChannelId).ToList();
                }
                //if(settled)
                //{
                //    transactions = transactions.Where(t => t.TrxPaymentsDTOList.Any(x => !string.IsNullOrEmpty(x.CreditCardNumber) && !string.IsNullOrEmpty(x.CreditCardAuthorization))).ToList();
                //}
                GetUnsettledTransactionPayments();
                GetTransactionPaymentSummaryDTOCollection(transactions);
                SetFooterContent(string.Empty, MessageType.None);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4032), MessageType.Error);
            }
            //IsLoadingVisble = false;
            log.LogMethodExit();
         }


        private void GetUnsettledTransactionPayments()
        {
            log.LogMethodEntry();
            try
            {
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
                int trxId = -1;
                int paymentModeId = -1;
                int deliveryChannelId = -1;
                DateTime? fromDateTime = null;
                DateTime? toDateTime = null;
                if (!string.IsNullOrEmpty(transactionId))
                {
                    trxId = Convert.ToInt32(transactionId);
                }
                else
                {
                    if (selectedPaymentMode != null)
                    {
                        paymentModeId = selectedPaymentMode.PaymentModeId;
                    }
                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        fromDateTime= DateTime.Parse(fromDate);
                    }
                    if (selectedChannel!=null)
                    {
                        deliveryChannelId = selectedChannel.DeliveryChannelId;
                    }
                    if (!string.IsNullOrEmpty(toDate))
                    {
                        toDateTime = DateTime.Parse(toDate);
                    }
                }
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<TransactionPaymentsDTO>> task = transactionUseCases.GetUnsettledTransactionPayments(trxId, paymentModeId, deliveryChannelId, fromDateTime,toDateTime);
                    task.Wait();
                    unsettledTransactionPaymentsDTOList = task.Result;
                }
                if(unsettledTransactionPaymentsDTOList==null)
                {
                    unsettledTransactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4032), MessageType.Error);
            }
            log.LogMethodExit();
        }

        private void GetTransactionPaymentSummaryDTOCollection(List<TransactionDTO> transactions)
        {
            log.LogMethodEntry();
            List<TransactionPaymentSummaryDTO> transactionPaymentSummaryDTOList = new List<TransactionPaymentSummaryDTO>();
            transactionPaymentDictionary.Clear();
            for (int i = 0; i < transactions.Count; i++)
            {
                double totalAmount = (transactions[i].TrxPaymentsDTOList != null) ? transactions[i].TrxPaymentsDTOList.Sum(x => x.Amount) : 0.0;
                //string paymentMode = paymentModes.First(x => x.PaymentModeId == transactions[i].TrxPaymentsDTOList.FirstOrDefault().PaymentModeId).PaymentMode;
                foreach (TransactionPaymentsDTO transactionPaymentsDTO in transactions[i].TrxPaymentsDTOList)
                {
                    if (transactionPaymentsDTO.ParentPaymentId > -1 || transactions[i].TrxPaymentsDTOList.Exists(x=>x.ParentPaymentId == transactionPaymentsDTO.PaymentId))
                    {
                        continue;
                    }
                    bool isTipUpdateAllowed = transactionPaymentsDTO.TipAmount == 0 ? true : false;
                    bool isSettled = !unsettledTransactionPaymentsDTOList.Any(x => x.PaymentId == transactionPaymentsDTO.PaymentId);
                    TransactionPaymentSummaryDTO transactionPaymentSummaryDTO = new TransactionPaymentSummaryDTO(transactions[i].TransactionId, transactions[i].TransactionNumber,
                    transactions[i].TransactionDate, transactions[i].CustomerName, transactionPaymentsDTO.paymentModeDTO.PaymentMode, transactionPaymentsDTO.Amount, transactionPaymentsDTO.TipAmount,
                    transactionPaymentsDTO.Reference, isSettled, transactionPaymentsDTO.CreditCardNumber, totalAmount, transactionPaymentsDTO.PaymentId, isTipUpdateAllowed);
                    transactionPaymentSummaryDTOList.Add(transactionPaymentSummaryDTO);
                    transactionPaymentDictionary.Add(transactionPaymentsDTO.PaymentId, transactionPaymentsDTO);
                }
            }
            if (settled)
            {
                if(transactionPaymentSummaryDTOList.Exists(x => x.Settled == true))
                {
                    transactionPaymentSummaryDTOList = transactionPaymentSummaryDTOList.Where(x => x.Settled == true).ToList();
                }
                else
                {
                    transactionPaymentSummaryDTOList = new List<TransactionPaymentSummaryDTO>();
                }
            }
            transactionPaymentSummaryDTOCollection = new ObservableCollection<TransactionPaymentSummaryDTO>(transactionPaymentSummaryDTOList);
            TransactionPaymentsCustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(transactionPaymentSummaryDTOCollection);
            log.LogMethodExit();
        }

        private void InitializeCommands()
        {
            log.LogMethodEntry();
            NavigationClickCommand = new DelegateCommand(OnNavigationClicked);
            LoadedCommand = new DelegateCommand(OnLoaded);
            ActionsCommand = new DelegateCommand(OnActionsClicked);
            log.LogMethodExit();
        }
        private void SetDefaultValues()
        {
            log.LogMethodEntry();
            Settled = false;
            TransactionId = string.Empty;
            SelectedChannel = null;
            SelectedPaymentMode = null;
            transactionPaymentSummaryDTOCollection.Clear();
            double businessStart = Convert.ToDouble(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "BUSINESS_DAY_START_TIME"));
            FromDate = DateTime.Today.AddHours(businessStart).ToString(dateTimeFormat);
            ToDate = DateTime.Today.AddDays(1).AddHours(businessStart).ToString(dateTimeFormat);
            
            log.LogMethodExit();
        }
        private Utilities GetUtility()
        {
            log.LogMethodEntry();
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.IsCorporate = ExecutionContext.GetIsCorporate();
            utilities.ParafaitEnv.LoginID = ExecutionContext.GetUserId();
            utilities.ParafaitEnv.POSMachineId = ExecutionContext.GetMachineId();
            POSMachineContainerDTO pOSMachineContainerDTO = POSMachineViewContainerList.GetPOSMachineContainerDTO(ExecutionContext.SiteId, ExecutionContext.GetMachineId());
            if (pOSMachineContainerDTO != null)
            {
                utilities.ParafaitEnv.SetPOSMachine("", pOSMachineContainerDTO.POSName);
            }
            else
            {
                utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            }
            utilities.ParafaitEnv.IsCorporate = ExecutionContext.GetIsCorporate();
            utilities.ParafaitEnv.SiteId = ExecutionContext.GetSiteId();
            log.Debug("executionContext - siteId" + ExecutionContext.GetSiteId());
            utilities.ExecutionContext.SetIsCorporate(ExecutionContext.GetIsCorporate());
            utilities.ExecutionContext.SetSiteId(ExecutionContext.GetSiteId());
            utilities.ExecutionContext.SetUserId(ExecutionContext.GetUserId());
            UserContainerDTO user = UserViewContainerList.GetUserContainerDTO(ExecutionContext.GetSiteId(), ExecutionContext.GetUserId());
            utilities.ParafaitEnv.User_Id = user.UserId;
            utilities.ParafaitEnv.RoleId = user.RoleId;
            utilities.ExecutionContext.SetUserId(user.LoginId);
            utilities.ParafaitEnv.Initialize();
            log.LogMethodExit(utilities);
            return utilities;
        }
        
        #endregion

        #region Constructor
        public PaymentSettlementVM(ExecutionContext executionContext, DeviceClass deviceClass)
        {
            log.LogMethodEntry(executionContext);
            this.ExecutionContext = executionContext;
            OldMode = "N";
            managerId = -1;
            approvedBy = string.Empty;
            ModuleName = MessageViewContainerList.GetMessage(ExecutionContext, "Payment Settlements");
            this.cardReader = deviceClass;
            //if (this.cardReader != null)
            //{
            //    log.Debug("Card Reader: " + cardReader);
            //    cardReader.Register(new EventHandler(CardScanCompleteEventHandle));
            //}
            InitializeCommands();
            SetInitialtValues();
            SetDefaultValues();
            GetPaymentModes();
            GetChannels();


            SetTransactionPaymentsCustomDataGridVM();
            FooterVM = new FooterVM(executionContext)
            {
                MessageType = MessageType.None,
                Message = string.Empty,
                HideSideBarVisibility = Visibility.Collapsed
            };
            log.LogMethodExit();
        }


        #endregion
    }
}

/********************************************************************************************
 * Project Name - Delivery UI
 * Description  - DeliveryOrder VM
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     22-Jun-2021    Fiona                  Created 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Authentication;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Customer;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.KDS;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Reflection;
using Semnox.Parafait.DeliveryIntegration;

namespace Semnox.Parafait.DeliveryUI
{
    public class DeliveryOrderVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool activeOrderChecked;
        private bool showSearchArea;

        private string dateTimeFormat;
        private string transactionId;
        private string moduleName;
        private string referenceNo;
        private string customer;
        private DeliveryChannelDTO selectedChannel;
        private LookupValuesContainerDTO selectedDeliveryType;
        private string fromDate;
        private string toDate;

        private ObservableCollection<TransactionDTO> searchedDeliveries;
        private ObservableCollection<DeliveryChannelDTO> channels;
        private ObservableCollection<LookupValuesContainerDTO> deliveryTypes;

        private CustomDataGridVM customDataGridVM;
        private CustomTextBoxDatePicker selectedDatePicker;
        private DeliveryOrderView deliveryOrderView;

        private ICommand loadedCommand;
        private ICommand actionsCommand;
        private ICommand navigationClickCommand;
        private ICommand datePickerLoadedCommand;
        private ICommand displayTagClickedCommand;
        private ICommand selectedItemChanged;
        private int received;
        private int processing;
        private int delivered;
        private int cancelled;
        private int newTransactions;
        private bool autoRefreshNewDeliveryCount;
        private int frequencyForNewDelivery;
        private DispatcherTimer newTrxtimer;
        private DispatcherTimer timeLeftUpdateTimer;
        private DateTime lastActivityTime;

        private bool enableOrderShareAccrossPos;
        private bool enableOrderShareAccrossPosUsers;
        private bool enableOrderShareAccrossPosCounters;
        private int fetchNewTransactionsInLastXSeconds;
        private List<int> listOfExistingNewTransactions;
        private Utilities utilities;
        private Visibility acceptButtonVisibility;
        private ICommand urbanPiperStatusChanngedCommand;
        private Dictionary<TransactionDTO, string> transactionCancellationDictionary = new Dictionary<TransactionDTO, string>();
        List<LookupValuesContainerDTO> lookupValuesOfUrbanPiperCancellationRequestCodes = new List<LookupValuesContainerDTO>();
        private GenericMessagePopupVM genericMessagePopupVM;
        private ICommand selectedItemschanged;
        private TransactionDTO rightSectionDisplayedTransaction = null;
        List<TransactionDTO> selectedTransactionDTOList = new List<TransactionDTO>();
        private bool enableActionButtons;
        private int deliveryOrdersRefreshFrequency;
        private SoundPlayer SoundPlayer;

        #endregion

        #region Properties 
        public bool ShowSearchArea
        {
            get
            {
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
        public string ReferenceNo
        {
            get
            {
                return referenceNo;
            }
            set
            {
                if (!object.Equals(referenceNo, value))
                {
                    referenceNo = value;
                    OnPropertyChanged();
                }
            }
        }
        public string TransactionId
        {
            get
            {
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
        public string Customer
        {
            get
            {
                return customer;
            }
            set
            {
                if (!object.Equals(customer, value))
                {
                    customer = value;
                    OnPropertyChanged();
                }
            }
        }
        public DeliveryChannelDTO SelectedChannel
        {
            get
            {
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
        public LookupValuesContainerDTO SelectedDeliveryType
        {
            get
            {
                return selectedDeliveryType;
            }
            set
            {
                if (!object.Equals(selectedDeliveryType, value))
                {
                    selectedDeliveryType = value;
                    OnPropertyChanged();
                }
            }
        }
        public string FromDate
        {
            get
            {
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
        public bool ActiveOrderChecked
        {
            get
            {
                return activeOrderChecked;
            }
            set
            {
                if (!object.Equals(activeOrderChecked, value))
                {
                    activeOrderChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ModuleName
        {
            get
            {
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
        public ObservableCollection<DeliveryChannelDTO> Channels
        {
            get
            {
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
        public ObservableCollection<LookupValuesContainerDTO> DeliveryTypes
        {
            get
            {
                return deliveryTypes;
            }
            set
            {
                if (!object.Equals(deliveryTypes, value))
                {
                    deliveryTypes = value;
                    OnPropertyChanged();
                }
            }
        }
        public CustomDataGridVM CustomDataGridVM
        {
            get
            {
                if (customDataGridVM == null)
                {
                    customDataGridVM = new CustomDataGridVM(ExecutionContext)
                    {
                        IsComboAndSearchVisible = false,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    };
                }
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
        public ICommand NavigationClickCommand
        {
            get
            {
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
        public ICommand ActionsCommand
        {
            get
            {
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
        public ICommand LoadedCommand
        {
            get
            {
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
        public ICommand DatePickerLoadedCommand
        {
            get
            {
                return datePickerLoadedCommand;
            }
            set
            {
                if (!object.Equals(datePickerLoadedCommand, value))
                {
                    datePickerLoadedCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand UrbanPiperStatusChanngedCommand
        {
            get
            {
                return urbanPiperStatusChanngedCommand;
            }
            set
            {
                if (!object.Equals(urbanPiperStatusChanngedCommand, value))
                {
                    urbanPiperStatusChanngedCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand DisplayTagClickedCommand
        {
            get
            {
                return displayTagClickedCommand;
            }
            set
            {
                if (!object.Equals(displayTagClickedCommand, value))
                {
                    displayTagClickedCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand SelectedItemChanged
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedItemChanged);
                return selectedItemChanged;
            }
            set
            {
                if (!object.Equals(selectedItemChanged, value))
                {
                    selectedItemChanged = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility AcceptButtonVisibility
        {
            get
            {
                return acceptButtonVisibility;
            }
            set
            {
                if (!object.Equals(acceptButtonVisibility, value))
                {
                    acceptButtonVisibility = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand SelectedItemsChangedCommand
        {
            get
            {
                return selectedItemschanged;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref selectedItemschanged, value);
                log.LogMethodExit(selectedItemschanged);
            }
        }
        public bool EnableActionButtons
        {
            get
            {
                return enableActionButtons;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref enableActionButtons, value);
                log.LogMethodExit(enableActionButtons);
            }
        }

        #endregion

        #region Methods

        private void InitializeDisplayTagsVM()
        {
            log.LogMethodEntry();
            received = 0;
            processing = 0;
            delivered = 0;
            cancelled = 0;
            newTransactions = 0;
            if (DisplayTagsVM == null || (DisplayTagsVM != null && DisplayTagsVM.DisplayTags.Count == 0))
            {
                DisplayTagsVM = new DisplayTagsVM()
                {
                    DisplayTags = new ObservableCollection<ObservableCollection<DisplayTag>>()
                    {
                        new ObservableCollection<DisplayTag>()
                        {
                            new DisplayTag()
                            {
                                Text = MessageViewContainerList.GetMessage(ExecutionContext, "Received")
                            },
                            new DisplayTag()
                            {
                                Text = received.ToString(),
                                TextSize = TextSize.Medium,
                                FontWeight = FontWeights.Bold
                            }
                        },
                        new ObservableCollection<DisplayTag>()
                        {
                            new DisplayTag()
                            {
                                Text = MessageViewContainerList.GetMessage(ExecutionContext, "Processing")
                            },
                            new DisplayTag()
                            {
                                Text = processing.ToString(),
                                TextSize = TextSize.Medium,
                                FontWeight = FontWeights.Bold
                            }
                        },
                        new ObservableCollection<DisplayTag>()
                        {
                            new DisplayTag()
                            {
                                Text = MessageViewContainerList.GetMessage(ExecutionContext, "Delivered")
                            },
                            new DisplayTag()
                            {
                                Text = delivered.ToString(),
                                TextSize = TextSize.Medium,
                                FontWeight = FontWeights.Bold
                            }
                        },
                        //new ObservableCollection<DisplayTag>()
                        //{
                        //    new DisplayTag()
                        //    {
                        //        Text = MessageViewContainerList.GetMessage(ExecutionContext, "Rejected")
                        //    },
                        //    new DisplayTag()
                        //    {
                        //        Text = cancelled.ToString(),
                        //        TextSize = TextSize.Medium,
                        //        FontWeight = FontWeights.Bold
                        //    }
                        //},
                        new ObservableCollection<DisplayTag>()
                        {
                            new DisplayTag()
                            {
                                Text =  MessageViewContainerList.GetMessage(ExecutionContext, "New Transactions")
                            },
                            new DisplayTag()
                            {
                               Text =newTransactions.ToString(),
                               FontWeight = FontWeights.Bold,
                               Type = DisplayTagType.Button
                            }
                        },
                    }
                };
            }
            log.LogMethodExit();
        }

        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                deliveryOrderView = parameter as DeliveryOrderView;
                //PerformSearch();
            }
            log.LogMethodExit();
        }
        private void OnActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            EnableActionButtons = false;
            newTrxtimer.Stop();
            SetFooterContent(string.Empty, MessageType.None);
            timeLeftUpdateTimer.Stop();
            try
            {
                if (parameter != null)
                {
                    Button button = parameter as Button;
                    if (button != null && !string.IsNullOrEmpty(button.Name))
                    {
                        switch (button.Name)
                        {
                            case "btnSearch":
                                {
                                    IsLoadingVisible = true;
                                    PerformDBSearch();
                                    PerformFilterSearch();
                                    SetCustomDataGridVM();
                                    IsLoadingVisible = false;
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
                            case "btnAccept":
                                {
                                    IsLoadingVisible = true;
                                    PerformAccept();
                                    IsLoadingVisible = false;
                                }
                                break;
                            case "btnReject":
                                {
                                    IsLoadingVisible = true;
                                    PerformReject();
                                    IsLoadingVisible = false;
                                    //NewTransactionsRefresh();
                                }
                                break;
                            case "btnMarkAsPrepared":
                                {
                                    IsLoadingVisible = true;
                                    PerformMarkAsPREPARED();
                                    IsLoadingVisible = false;
                                    ///NewTransactionsRefresh();
                                }
                                break;
                            case "btnReconfirmOrder":
                                {
                                    IsLoadingVisible = true;
                                    PerformReconfirmOrder();
                                    IsLoadingVisible = false;
                                }
                                break;
                            case "btnReconfirmPreparation":
                                {
                                    IsLoadingVisible = true;
                                    PerformReconfirmPreparation();
                                    IsLoadingVisible = false;
                                }
                                break;
                            case "btnRefresh":
                                {
                                    IsLoadingVisible = true;
                                    PerformRefresh();
                                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4033), MessageType.Info);
                                    IsLoadingVisible = false;
                                    //Order details are Refreshed
                                }
                                break;
                            case "btnView":
                                {
                                    PerformView();
                                }
                                break;
                            case "btnKOT":
                                {
                                    PerformKOT();
                                }
                                break;
                            case "btnRiderAssignment":
                                {
                                    PerformRiderAssignment();
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            timeLeftUpdateTimer.Start();
            newTrxtimer.Start();
            EnableActionButtons = true;
            log.LogMethodExit();
        }
        private void PerformRefresh()
        {
            log.LogMethodEntry();
            PerformDBSearch();
            PerformFilterSearch();
            SetCustomDataGridVM();
            NewTransactionsRefresh();
            UpdateDisplayTags();
            log.LogMethodExit();
        }
        private void PerformView()
        {
            log.LogMethodEntry();
            if (customDataGridVM.SelectedItems != null && customDataGridVM.SelectedItems.Count == 1)
            {
                TransactionDTO selectedTransactionDTO = customDataGridVM.SelectedItems[0] as TransactionDTO;
                Type type = Type.GetType("Parafait_POS.OrderListView,Parafait POS");
                object orderListView = null;
                if (type != null)
                {
                    ConstructorInfo constructorN = type.GetConstructor(new Type[] { });
                    orderListView = constructorN.Invoke(new object[] { });
                }
                else
                {
                    throw new Exception(MessageViewContainerList.GetMessage(ExecutionContext, 1479, "POS OrderListView"));
                }
                if (orderListView != null)
                {

                    Utilities utilities = GetUtility(ExecutionContext);//POSUtils.ParafaitMessageBox
                    Func<string, string, System.Windows.Forms.MessageBoxButtons, System.Windows.Forms.DialogResult> parafaitMessageBox =
                        new Func<string, string, System.Windows.Forms.MessageBoxButtons, System.Windows.Forms.DialogResult>(ParafaitMessageBox);
                    Action<string, string> displayMessageLine = new Action<string, string>(DisplayMessageLine);
                    type.GetMethod("Initialize").Invoke(orderListView, new object[] { utilities, parafaitMessageBox, displayMessageLine });
                    type.GetMethod("SetTransactionId").Invoke(orderListView, new object[] { selectedTransactionDTO.TransactionId.ToString() });
                    type.GetMethod("RefreshData").Invoke(orderListView, new object[] { });
                    System.Windows.Forms.Button btnCloseWinForm = (System.Windows.Forms.Button)type.GetMethod("GetClearButtonProperties").Invoke(orderListView, new object[] { });
                    List<string> menuItemsToDisable = new string[] {  "selectOrderToolStripMenuItem", "mergeOrderToolStripMenuItem", "cancelOrderToolStripMenuItem",
                                                                     "cancelCardsToolStripMenuItem", "moveToTableToolStripMenuItem", "splitToolStripMenuItem", "changeStaffToolStripMenuItem",
                                                                     "mapTransactionWaiverToolStripMenuItem" }.ToList();
                    type.GetMethod("DisableMenuItem").Invoke(orderListView, new object[] { menuItemsToDisable });
                    type.GetMethod("SetDoForcedEnableDisableOfMenuItemsFlag").Invoke(orderListView, new object[] { });

                    using (System.Windows.Forms.Form form = new System.Windows.Forms.Form())
                    {
                        form.Text = MessageViewContainerList.GetMessage(ExecutionContext, "View"); ;
                        form.WindowState = System.Windows.Forms.FormWindowState.Normal;
                        form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
                        System.Windows.Forms.Panel panel = new System.Windows.Forms.Panel();
                        form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                        panel.AutoSize = true;
                        form.Margin = new System.Windows.Forms.Padding(5);
                        panel.Margin = new System.Windows.Forms.Padding(5);
                        panel.Controls.Add((System.Windows.Forms.Control)orderListView);
                        panel.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left));
                        if (btnCloseWinForm == null)
                        { btnCloseWinForm = new System.Windows.Forms.Button(); }
                        btnCloseWinForm.Text = MessageViewContainerList.GetMessage(ExecutionContext, "Close");
                        btnCloseWinForm.Click += BtnCloseWinFormClick;
                        btnCloseWinForm.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
                        form.Controls.Add(panel);
                        form.Size = new System.Drawing.Size(panel.Width + 50, panel.Height + btnCloseWinForm.Height + 60);
                        btnCloseWinForm.Location = new System.Drawing.Point((form.Width / 2 - btnCloseWinForm.Width / 2), panel.Height + 10);
                        form.Controls.Add(btnCloseWinForm);
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1448), MessageType.Info);
                        form.ShowDialog();
                    }
                    IsLoadingVisible = true;

                    PerformRefresh();
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4033), MessageType.Info);
                    IsLoadingVisible = false;
                }
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2460), MessageType.Info);
            }
            log.LogMethodExit();
        }

        private void BtnCloseWinFormClick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            System.Windows.Forms.Button btnCloseWinForm = sender as System.Windows.Forms.Button;
            if (btnCloseWinForm != null)
            {
                System.Windows.Forms.Form form = btnCloseWinForm.Parent as System.Windows.Forms.Form;
                if (form != null)
                {
                    form.Close();
                }
            }
            log.LogMethodExit();
        }

        private void DisplayMessageLine(string message, string msgType)
        {
            log.LogMethodEntry(message, msgType);
            try
            {
                SetFooterContent(message, MessageType.Info);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        public System.Windows.Forms.DialogResult ParafaitMessageBox(string message, string Title,
            System.Windows.Forms.MessageBoxButtons msgboxButtons = System.Windows.Forms.MessageBoxButtons.YesNo)
        {
            log.LogMethodEntry(message, Title);
            System.Windows.Forms.DialogResult retValue = System.Windows.Forms.DialogResult.No;
            Type type = Type.GetType("Parafait_POS.POSUtils,Parafait POS");
            if (type != null)
            {
                //MethodInfo parafaitMsgBox = type.GetMethod("ParafaitMessageBox", BindingFlags.Static | BindingFlags.Public);
                MethodInfo parafaitMsgBox = type.GetMethod("ParafaitMessageBox", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder,
                        new[] { typeof(string), typeof(string), typeof(System.Windows.Forms.MessageBoxButtons) }, null);
                if (parafaitMsgBox != null)
                {
                    retValue = (System.Windows.Forms.DialogResult)parafaitMsgBox.Invoke(null, new object[] { message, Title, msgboxButtons });
                }
                else
                {
                    throw new Exception(MessageViewContainerList.GetMessage(ExecutionContext, 1479, "Parafait_POS.POSUtils.ParafaitMessageBox"));
                }
            }
            else
            {
                throw new Exception(MessageViewContainerList.GetMessage(ExecutionContext, 1479, "Parafait_POS.POSUtils"));
            }
            log.LogMethodExit(retValue);
            return retValue;
        }

        private void PerformKOT()
        {
            log.LogMethodEntry();
            if (customDataGridVM.SelectedItems != null && customDataGridVM.SelectedItems.Count == 1)
            {
                TransactionDTO selectedTransactionDTO = customDataGridVM.SelectedItems[0] as TransactionDTO;
                if (selectedTransactionDTO.TransactionLinesDTOList != null && selectedTransactionDTO.TransactionLinesDTOList.Any())
                {
                    TransactionKOTVM transactionKOTVM = new TransactionKOTVM(ExecutionContext, selectedTransactionDTO);
                    TransactionKOTView transactionKOTView = new TransactionKOTView();
                    transactionKOTView.DataContext = transactionKOTVM;
                    transactionKOTView.Closed += OnWindowClosed;
                    if (deliveryOrderView != null)
                    {
                        transactionKOTView.Owner = deliveryOrderView;
                    }
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1448), MessageType.Info);
                    transactionKOTView.ShowDialog();
                }
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2460), MessageType.Info);
            }
            log.LogMethodExit();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            IsLoadingVisible = true;
            PerformRefresh();
            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4033), MessageType.Info);
            //Order details are Refreshed

            IsLoadingVisible = false;
            log.LogMethodExit();
        }

        private void PerformRiderAssignment()
        {
            log.LogMethodEntry();

            if (customDataGridVM.SelectedItems != null && customDataGridVM.SelectedItems.Count == 1)
            {
                TransactionDTO selectedTransactionDTO = customDataGridVM.SelectedItems[0] as TransactionDTO;
                if (selectedTransactionDTO.Status == Transaction.Transaction.TrxStatus.INITIATED.ToString() || selectedTransactionDTO.Status == Transaction.Transaction.TrxStatus.BOOKING.ToString())
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4034), MessageType.Info);
                    //Cannot Update Rider details. Order is not yet Accepted
                    return;
                }
                if (selectedTransactionDTO.Status == Transaction.Transaction.TrxStatus.CANCELLED.ToString() || selectedTransactionDTO.Status == Transaction.Transaction.TrxStatus.SYSTEMABANDONED.ToString())
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4035), MessageType.Info);
                    //Cannot Update Rider details. Order has been Cancelled
                    return;
                }
                if (selectedTransactionDTO.Status == Transaction.Transaction.TrxStatus.CLOSED.ToString())
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4036), MessageType.Info);
                    // "Cannot Update Rider details. Order has been Closed"
                    return;
                }
                if (selectedTransactionDTO.TransctionOrderDispensingDTO.DeliveryChannelId == -1)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4037), MessageType.Info);
                    //Rider cannnot be assigned for PickUP order
                    return;
                }

                RiderAssignmentVM riderAssignmentVM = new RiderAssignmentVM(ExecutionContext, selectedTransactionDTO.TransctionOrderDispensingDTO, selectedTransactionDTO.ExternalSystemReference);
                RiderAssignmentView riderAssignmentView = new RiderAssignmentView();
                riderAssignmentView.DataContext = riderAssignmentVM;
                riderAssignmentView.Closed += OnWindowClosed;
                if (deliveryOrderView != null)
                {
                    riderAssignmentView.Owner = deliveryOrderView;
                }
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1448), MessageType.Info);
                riderAssignmentView.ShowDialog();

            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2460), MessageType.Info);
            }
            log.LogMethodExit();
        }
        private void OpenGenericMessagePopupView(string heading, string subHeading, string content,
            string okButtonText, string cancelButtonText, MessageButtonsType messageButtonsType, PopupType popupType = PopupType.Normal)
        {
            log.LogMethodEntry(heading, subHeading, content, okButtonText, cancelButtonText, messageButtonsType);
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            if (deliveryOrderView != null)
            {
                messagePopupView.Owner = this.deliveryOrderView;
            }
            //messagePopupView.Loaded += OnWindowLoaded;
            genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
            {
                OkButtonText = okButtonText,
                CancelButtonText = cancelButtonText,
                MessageButtonsType = messageButtonsType,
                SubHeading = subHeading,
                Heading = heading,
                Content = content,
            };
            if (popupType == PopupType.Timer)
            {
                genericMessagePopupVM.TimerMilliSeconds = 5000;
            }
            messagePopupView.DataContext = genericMessagePopupVM;
            messagePopupView.ShowDialog();
            log.LogMethodExit();
        }
        private void OnWindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Window window = sender as Window;
            if (deliveryOrderView != null)
            {
                window.Owner = this.deliveryOrderView;
            }
            log.LogMethodExit();
        }
        private void PerformAccept()
        {
            log.LogMethodEntry();
            ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
            if (customDataGridVM.SelectedItems != null && customDataGridVM.SelectedItems.Any())
            {
                List<TransactionDTO> selectedTransactionDTOList = GetSelectedRecords();
                List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = GetParafaitMessageQueueDTOList(selectedTransactionDTOList);
                for (int i = 0; i < selectedTransactionDTOList.Count; i++)
                {
                    selectedTransactionDTOList[i].Status = Transaction.Transaction.TrxStatus.ORDERED.ToString();
                }
                //TransactionDTO selectedTransactionDTO =  customDataGridVM.SelectedItem as TransactionDTO;
                using (NoSynchronizationContextScope.Enter())
                {
                    try
                    {
                        Task<List<KeyValuePair<TransactionDTO, string>>> task = transactionUseCases.UpdateTransactionStatus(selectedTransactionDTOList);
                        task.Wait();
                        List<KeyValuePair<TransactionDTO, string>> keyValuePairs = task.Result;
                        if (keyValuePairs != null && keyValuePairs.Any())
                        {
                            string successMessage = MessageViewContainerList.GetMessage(ExecutionContext, 4027);
                            //Order/Orders have been Accepted
                            ShowErrorMessages(keyValuePairs, successMessage);
                            List<KeyValuePair<TransactionDTO, string>> recordList = keyValuePairs.Where(key => key.Value == null).ToList();
                            for (int i = 0; i < recordList.Count; i++)
                            {
                                if (listOfExistingNewTransactions.Contains(recordList[i].Key.TransactionId))
                                {
                                    listOfExistingNewTransactions.Remove(recordList[i].Key.TransactionId);
                                }
                            }
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
                PerformRefresh();
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2460), MessageType.Info);
            }
            log.LogMethodExit();
        }

        private void ShowErrorMessages(List<KeyValuePair<TransactionDTO, string>> keyValuePairs, string successMsg)
        {
            log.LogMethodEntry(keyValuePairs, successMsg);
            List<KeyValuePair<TransactionDTO, string>> errorRecordList = keyValuePairs.Where(key => key.Value != null).ToList();
            if (errorRecordList != null && errorRecordList.Any())
            {
                if (errorRecordList.Count == 1)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, errorRecordList[0].Value), MessageType.Error);
                }
                else
                {
                    StringBuilder stringB = new StringBuilder("");
                    for (int i = 0; i < errorRecordList.Count; i++)
                    {
                        stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Trxn Id: ") + errorRecordList[i].Key.TransactionId + " ");
                        stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Ref No: ") + errorRecordList[i].Key.ExternalSystemReference + " ");
                        stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Message: " + errorRecordList[i].Value));
                        stringB.Append(System.Environment.NewLine);
                    }
                    OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(ExecutionContext, "Message"), string.Empty,
                    stringB.ToString(), MessageViewContainerList.GetMessage(ExecutionContext, "OK"), MessageViewContainerList.GetMessage(ExecutionContext, "OK"),
                    MessageButtonsType.OK);
                }
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, successMsg), MessageType.Info);
            }
            log.LogMethodExit();
        }

        private List<TransactionDTO> GetSelectedRecords()
        {
            log.LogMethodEntry();
            List<TransactionDTO> selectedTransactionDTOList = new List<TransactionDTO>();
            try
            {
                foreach (object data in CustomDataGridVM.SelectedItems)
                {
                    TransactionDTO selectedTransactionDTO = data as TransactionDTO;
                    if (selectedTransactionDTO != null)
                    {
                        selectedTransactionDTOList.Add(selectedTransactionDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit(selectedTransactionDTOList);
            return selectedTransactionDTOList;
        }
        private List<TransactionDTO> HandleSwiggyOrderCancellation(List<TransactionDTO> transactionDTOList)
        {
            log.LogMethodEntry(transactionDTOList);
            List<TransactionDTO> finalTransactionDTOList = new List<TransactionDTO>();
            finalTransactionDTOList.AddRange(transactionDTOList);
            if (transactionDTOList != null && transactionDTOList.Any())
            {
                foreach (TransactionDTO transactionDTO in transactionDTOList)
                {
                    if (transactionDTO.TransctionOrderDispensingDTO != null)
                    {
                        int deliveryChannelId = transactionDTO.TransctionOrderDispensingDTO.DeliveryChannelId;
                        if (Channels != null && Channels.Any(x => x.DeliveryChannelId == deliveryChannelId))
                        {
                            string channelName = Channels.FirstOrDefault(x => x.DeliveryChannelId == deliveryChannelId).ChannelName;
                            if (channelName.ToUpper() == "SWIGGY")
                            {
                                GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                                GenericMessagePopupVM genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                                {
                                    OkButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "PROCEED"),
                                    CancelButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "CANCEL"),
                                    MessageButtonsType = MessageButtonsType.OkCancel,
                                    SubHeading = "",
                                    Heading = MessageViewContainerList.GetMessage(ExecutionContext, "SWIGGY ORDER CANCELLATION"),
                                    Content = MessageViewContainerList.GetMessage(ExecutionContext, 4228, transactionDTO.TransactionId)
                                    //'Do you want to raise order cancellation request with Swiggy for Transaction ID &1 ?
                                };
                                messagePopupView.DataContext = genericMessagePopupVM;
                                messagePopupView.Owner = deliveryOrderView;
                                messagePopupView.Width = this.deliveryOrderView.Width;
                                messagePopupView.Height = this.deliveryOrderView.Height;
                                messagePopupView.Top = this.deliveryOrderView.Top;
                                messagePopupView.Left = this.deliveryOrderView.Left;
                                messagePopupView.ShowDialog();
                                if (genericMessagePopupVM.ButtonClickType == ButtonClickType.Cancel)
                                {
                                    finalTransactionDTOList.Remove(transactionDTO);
                                }

                            }
                        }
                    }

                }
            }
            log.LogMethodExit(finalTransactionDTOList);
            return finalTransactionDTOList;
        }
        private List<LookupValuesContainerDTO> GetlookupValuesOfUrbanPiperCancellationRequestCodes()
        {
            log.LogMethodEntry();
            List<LookupValuesContainerDTO> lookupValuesContainerDTOList = new List<LookupValuesContainerDTO>();
            LookupsContainerDTO lookupsContainerDTO = LookupsViewContainerList.GetLookupsContainerDTO(ExecutionContext.SiteId, "DELIVERY_INTEGRATOR_CANCELLATION_REQUEST_CODES");
            if (lookupsContainerDTO != null && lookupsContainerDTO.LookupValuesContainerDTOList != null)
            {
                lookupValuesContainerDTOList = lookupsContainerDTO.LookupValuesContainerDTOList;
            }
            log.LogMethodEntry(lookupValuesContainerDTOList);
            return lookupValuesContainerDTOList;
        }
        private List<TransactionDTO> HandleReasonforCancellation(List<TransactionDTO> transactionDTOList)
        {
            log.LogMethodEntry(transactionDTOList);
            transactionCancellationDictionary.Clear();
            List<LookupValuesContainerDTO> lookupValuesContainerDTOList = lookupValuesOfUrbanPiperCancellationRequestCodes;
            List<TransactionDTO> finalTransactionDTOList = new List<TransactionDTO>();
            List<int> cancelledTrxIds = new List<int>();
            for (int i = 0; i < transactionDTOList.Count; i++)
            {
                TransactionDTO transactionDTO = transactionDTOList[i];
                GenericDataEntryView dataEntryView = new GenericDataEntryView();
                GenericDataEntryVM GenericDataEntryVM = new GenericDataEntryVM(ExecutionContext)
                {
                    Heading = MessageViewContainerList.GetMessage(ExecutionContext, 4582, transactionDTO.TransactionId),
                    //Please select Cancellation Reason for TrxId &1
                    IsKeyboardVisible = false,
                    DataEntryCollections = new ObservableCollection<DataEntryElement>()
                    {
                        new DataEntryElement()
                        {
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Cancellation Reason"),
                            Type=DataEntryType.ComboBox,
                            Options=new ObservableCollection<object>(lookupValuesContainerDTOList),
                            DisplayMemberPath="Description",
                            IsEditable=false,
                            IsMandatory = true
                        }
                    }
                };
                dataEntryView.Width = SystemParameters.PrimaryScreenWidth;
                dataEntryView.Height = SystemParameters.PrimaryScreenHeight;
                dataEntryView.MainGrid.MinWidth = 700;
                dataEntryView.DataContext = GenericDataEntryVM;
                if (deliveryOrderView != null)
                {
                    dataEntryView.Owner = deliveryOrderView;
                }
                dataEntryView.ShowDialog();

                if (GenericDataEntryVM.ButtonClickType == ButtonClickType.Ok)
                {
                    if (GenericDataEntryVM.DataEntryCollections[0].SelectedItem != null)
                    {
                        LookupValuesContainerDTO selectedLookupValue = GenericDataEntryVM.DataEntryCollections[0].SelectedItem as LookupValuesContainerDTO;
                        transactionCancellationDictionary.Add(transactionDTO, selectedLookupValue.LookupValue);
                        finalTransactionDTOList.Add(transactionDTO);
                        if (transactionDTOList.Count > 1 && i == 0)
                        {
                            OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(ExecutionContext, "Message"), string.Empty,
                                MessageViewContainerList.GetMessage(ExecutionContext, 4583),
                                //Do you want to select the same Cancellation Reason for the remaining Orders?
                                MessageViewContainerList.GetMessage(ExecutionContext, "OK"), MessageViewContainerList.GetMessage(ExecutionContext, "CANCEL"), MessageButtonsType.OkCancel);
                            if (genericMessagePopupVM.ButtonClickType == ButtonClickType.Ok)
                            {
                                for (int j = i + 1; j < transactionDTOList.Count; j++)
                                {
                                    transactionDTO = transactionDTOList[j];
                                    transactionCancellationDictionary.Add(transactionDTO, selectedLookupValue.LookupValue);
                                    finalTransactionDTOList.Add(transactionDTO);
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    cancelledTrxIds.Add(transactionDTO.TransactionId);
                }
            }

            if (cancelledTrxIds.Any())
            {
                string trxIds = string.Join(",", cancelledTrxIds);
                OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(ExecutionContext, "Error"), MessageViewContainerList.GetMessage(ExecutionContext, 4585),// Cancellation reason not selected
                               MessageViewContainerList.GetMessage(ExecutionContext, 4584, trxIds),
                               //Cannot proceed with Cancellation for Transaction Id''s &1
                               string.Empty, MessageViewContainerList.GetMessage(ExecutionContext, "OK"), MessageButtonsType.OK, PopupType.Timer);
            }
            log.LogMethodExit(finalTransactionDTOList);
            return finalTransactionDTOList;
        }
        private void PerformReject()
        {
            log.LogMethodEntry();
            transactionCancellationDictionary.Clear();
            ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
            if (customDataGridVM.SelectedItems != null && customDataGridVM.SelectedItems.Any())
            {
                List<TransactionDTO> selectedTransactionDTOList = GetSelectedRecords();
                selectedTransactionDTOList = HandleSwiggyOrderCancellation(selectedTransactionDTOList);
                if (selectedTransactionDTOList == null || selectedTransactionDTOList.Any() == false)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "No Records to process"), MessageType.Info);
                    log.Debug("No Records to process");
                    return;
                }
                selectedTransactionDTOList = HandleReasonforCancellation(selectedTransactionDTOList);
                if (selectedTransactionDTOList == null || selectedTransactionDTOList.Any() == false || transactionCancellationDictionary.Any() == false)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "No Records to process"), MessageType.Info);
                    log.Debug("No Records to process");
                    return;
                }

                using (NoSynchronizationContextScope.Enter())
                {
                    try
                    {
                        Task<List<KeyValuePair<TransactionDTO, string>>> task = transactionUseCases.SubmitUrbanPiperOrderCancellationRequest(transactionCancellationDictionary);
                        task.Wait();
                        List<KeyValuePair<TransactionDTO, string>> keyValuePairs = task.Result;
                        if (keyValuePairs != null && keyValuePairs.Any())
                        {
                            string successMessage = MessageViewContainerList.GetMessage(ExecutionContext, 4030);
                            //Order/Orders have been cancelled
                            ShowErrorMessages(keyValuePairs, successMessage);
                            List<KeyValuePair<TransactionDTO, string>> errorRecordList = keyValuePairs.Where(key => key.Value == null).ToList();
                            for (int i = 0; i < errorRecordList.Count; i++)
                            {
                                if (listOfExistingNewTransactions.Contains(errorRecordList[i].Key.TransactionId))
                                {
                                    listOfExistingNewTransactions.Remove(errorRecordList[i].Key.TransactionId);
                                }
                            }
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
                }
                PerformRefresh();
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2460), MessageType.Info);
            }
            log.LogMethodExit();
        }
        private void PerformReconfirmPreparation()
        {
            log.LogMethodEntry();
            ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
            if (customDataGridVM.SelectedItems != null && customDataGridVM.SelectedItems.Any())
            {
                List<TransactionDTO> selectedTransactionDTOList = GetSelectedRecords();
                using (NoSynchronizationContextScope.Enter())
                {
                    try
                    {
                        Task<List<KeyValuePair<TransactionDTO, string>>> task = transactionUseCases.SetAsPreparationReconfirmedOrder(selectedTransactionDTOList);
                        task.Wait();
                        List<KeyValuePair<TransactionDTO, string>> keyValuePairs = task.Result;
                        if (keyValuePairs != null && keyValuePairs.Any())
                        {
                            string successMessage = MessageViewContainerList.GetMessage(ExecutionContext, 4028);
                            //Marked as Reconfirmed Preparation
                            ShowErrorMessages(keyValuePairs, successMessage);
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
                }
                PerformRefresh();
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2460), MessageType.Info);
            }
            log.LogMethodExit();
        }
        private void PerformReconfirmOrder()
        {
            log.LogMethodEntry();
            ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
            if (customDataGridVM.SelectedItems != null && customDataGridVM.SelectedItems.Any())
            {
                List<TransactionDTO> selectedTransactionDTOList = GetSelectedRecords();

                using (NoSynchronizationContextScope.Enter())
                {
                    try
                    {
                        Task<List<KeyValuePair<TransactionDTO, string>>> task = transactionUseCases.SetAsCustomerReconfirmedOrder(selectedTransactionDTOList);
                        task.Wait();
                        List<KeyValuePair<TransactionDTO, string>> keyValuePairs = task.Result;
                        if (keyValuePairs != null && keyValuePairs.Any())
                        {
                            string successMessage = MessageViewContainerList.GetMessage(ExecutionContext, 4029);
                            //Marked as Reconfirmed Order
                            ShowErrorMessages(keyValuePairs, successMessage);
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
                }
                PerformRefresh();
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2460), MessageType.Info);
            }

            log.LogMethodExit();
        }
        private void PerformMarkAsPREPARED()
        {
            log.LogMethodEntry();
            ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
            if (customDataGridVM.SelectedItems != null && customDataGridVM.SelectedItems.Any())
            {
                List<TransactionDTO> selectedTransactionDTOList = GetSelectedRecords();
                foreach (TransactionDTO selectedTransactionDTO in selectedTransactionDTOList)
                {
                    if (selectedTransactionDTO.Status == Transaction.Transaction.TrxStatus.PREPARED.ToString())
                    {
                        string message = MessageViewContainerList.GetMessage(ExecutionContext, 4041);
                        //Order/Orders are already in Prepared status
                        log.Error(message);
                        SetFooterContent(message, MessageType.Error);
                        return;
                    }
                    if (selectedTransactionDTO.Status == Transaction.Transaction.TrxStatus.CANCELLED.ToString())
                    {
                        string message = MessageViewContainerList.GetMessage(ExecutionContext, 4030);
                        //Order/Orders have been cancelled
                        log.Error(message);
                        SetFooterContent(message, MessageType.Error);
                        return;
                    }
                    if (selectedTransactionDTO.TransactionLinesDTOList.TrueForAll(trx => (trx.KDSOrderLineDTOList == null || trx.KDSOrderLineDTOList.Any() == false)))
                    {
                        log.Error(MessageViewContainerList.GetMessage(ExecutionContext, 4038));
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4038), MessageType.Error);
                        //Order not sent for KOT/KDS
                        return;
                    }

                    foreach (TransactionLineDTO transactionLineDTO in selectedTransactionDTO.TransactionLinesDTOList)
                    {
                        if (transactionLineDTO.KDSOrderLineDTOList == null || transactionLineDTO.KDSOrderLineDTOList.Any() == false)
                        {
                            continue;
                        }
                        KDSOrderLineDTO kDSOrderLineDTO = transactionLineDTO.KDSOrderLineDTOList.OrderByDescending(x => x.Id).FirstOrDefault();

                        if (kDSOrderLineDTO.EntryType == KDSOrderLineDTO.KDSKOTEntryType.KOT && kDSOrderLineDTO.DeliveredTime == null)
                        {
                            log.Error(MessageViewContainerList.GetMessage(ExecutionContext, 4039));
                            //Order not yet Prepared
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4039), MessageType.Error);
                            return;
                        }
                        else if (kDSOrderLineDTO.EntryType == KDSOrderLineDTO.KDSKOTEntryType.KDS && kDSOrderLineDTO.PreparedTime == null)
                        {
                            log.Error(MessageViewContainerList.GetMessage(ExecutionContext, 4039));
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4039), MessageType.Error);
                            return;
                        }

                    }

                    if (selectedTransactionDTO.TransctionOrderDispensingDTO.ReConfirmPreparation == TransactionOrderDispensingDTO.ReConformationStatus.YES)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4040), MessageType.Error);
                        //Order Preparation not yet Reconfirmed
                        return;
                    }
                }

                for (int i = 0; i < selectedTransactionDTOList.Count; i++)
                {
                    selectedTransactionDTOList[i].Status = Transaction.Transaction.TrxStatus.PREPARED.ToString();
                }

                using (NoSynchronizationContextScope.Enter())
                {
                    try

                    {
                        Task<List<KeyValuePair<TransactionDTO, string>>> task = transactionUseCases.UpdateTransactionStatus(selectedTransactionDTOList);
                        task.Wait();
                        List<KeyValuePair<TransactionDTO, string>> keyValuePairs = task.Result;
                        if (keyValuePairs != null && keyValuePairs.Any())
                        {
                            string successMessage = MessageViewContainerList.GetMessage(ExecutionContext, 4031);
                            //Order/Orders have been Marked as Prepared
                            ShowErrorMessages(keyValuePairs, successMessage);
                            List<KeyValuePair<TransactionDTO, string>> recordList = keyValuePairs.Where(key => key.Value == null).ToList();
                            for (int i = 0; i < recordList.Count; i++)
                            {
                                if (listOfExistingNewTransactions.Contains(recordList[i].Key.TransactionId))
                                {
                                    listOfExistingNewTransactions.Remove(recordList[i].Key.TransactionId);
                                }
                            }
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
                }
                PerformRefresh();

            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2460), MessageType.Info);
            }
            log.LogMethodExit();

        }
        public static Utilities GetUtility(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ParafaitEnv.POSMachineId = executionContext.GetMachineId();
            try
            {
                POSMachineContainerDTO pOSMachineContainerDTO = POSMachineViewContainerList.GetPOSMachineContainerDTO(executionContext.SiteId, executionContext.GetMachineId());
                if (pOSMachineContainerDTO != null)
                {
                    utilities.ParafaitEnv.SetPOSMachine("", pOSMachineContainerDTO.POSName);
                }
                else
                {
                    utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
                }
            }
            catch (Exception)
            {

                utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            }

            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            log.Debug("executionContext - siteId" + executionContext.GetSiteId());
            utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            UserContainerDTO user = UserViewContainerList.GetUserContainerDTO(executionContext.GetSiteId(), executionContext.GetUserId());
            utilities.ParafaitEnv.User_Id = user.UserId;
            utilities.ParafaitEnv.RoleId = user.RoleId;
            utilities.ExecutionContext.SetUserId(user.LoginId);
            utilities.ParafaitEnv.Initialize();
            log.LogMethodExit(utilities);
            return utilities;
        }

        private void PerformDBSearch()
        {
            log.LogMethodEntry();
            ShowSearchArea = false;
            try
            {
                DateTime fromDateTime;
                DateTime toDateTime;
                List<TransactionDTO> transactions = null;
                ExecutionContext execution = SystemUserExecutionContextBuilder.GetSystemUserExecutionContext();
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.NEEDS_ORDER_DISPENSING, "1"));
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
                else if (!string.IsNullOrEmpty(referenceNo))
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, referenceNo));
                }
                else
                {
                    if (string.IsNullOrEmpty(transactionId) && string.IsNullOrEmpty(referenceNo) && !string.IsNullOrEmpty(customer))
                    {
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_ID, customer));
                    }
                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        log.LogVariableState("fromDate", fromDate);
                        fromDateTime = DateTime.Parse(fromDate, CultureInfo.InvariantCulture);
                        log.LogVariableState("fromDateTime", fromDateTime);
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, fromDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                    if (!string.IsNullOrEmpty(toDate))
                    {
                        log.LogVariableState("toDate", toDate);
                        toDateTime = DateTime.Parse(toDate, CultureInfo.InvariantCulture);
                        log.LogVariableState("toDateTime", toDateTime);
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, toDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                    if (activeOrderChecked)
                    {
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, Transaction.Transaction.TrxStatus.OPEN.ToString() + ","
                                                                                                                                                 + Transaction.Transaction.TrxStatus.INITIATED.ToString() + ","
                                                                                                                                                 + Transaction.Transaction.TrxStatus.BOOKING.ToString() + ","
                                                                                                                                                 + Transaction.Transaction.TrxStatus.ORDERED.ToString() + ","
                                                                                                                                                  + Transaction.Transaction.TrxStatus.PREPARED.ToString()));
                    }
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
                transactions = transactions.FindAll(trx => trx.TransctionOrderDispensingDTO != null);
                searchedDeliveries = new ObservableCollection<TransactionDTO>(transactions);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4032), MessageType.Error);
            }
            log.LogMethodExit();
        }
        private void PerformFilterSearch()
        {
            log.LogMethodEntry();
            List<TransactionDTO> transactionDTOList = searchedDeliveries.ToList();

            if (selectedChannel != null)
            {
                transactionDTOList = transactionDTOList.Where(t => t.TransctionOrderDispensingDTO.DeliveryChannelId == selectedChannel.DeliveryChannelId).ToList();
            }
            if (selectedDeliveryType != null)
            {
                transactionDTOList = transactionDTOList.Where(t => t.TransctionOrderDispensingDTO.OrderDispensingTypeId == selectedDeliveryType.LookupValueId).ToList();
            }
            searchedDeliveries = new ObservableCollection<TransactionDTO>(transactionDTOList);

            log.LogMethodExit();
        }
        private void UpdateDisplayTags()
        {
            log.LogMethodEntry();
            ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
            List<TransactionDTO> transactions = null;


            double businessStart = Convert.ToDouble(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "BUSINESS_DAY_START_TIME"));
            DateTime siteDateTime = SiteDateTime.GetSiteDateTime(ExecutionContext);
            DateTime fromDateTime = siteDateTime.Date.AddHours(businessStart);
            DateTime toDateTime = siteDateTime.Date.AddDays(1).AddHours(businessStart);

            List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.NEEDS_ORDER_DISPENSING, "1"));

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
            searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, fromDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));

            searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, toDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));

            searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, Transaction.Transaction.TrxStatus.OPEN.ToString() + ","
                                                                                                                                            + Transaction.Transaction.TrxStatus.INITIATED.ToString() + ","
                                                                                                                                             + Transaction.Transaction.TrxStatus.BOOKING.ToString() + ","
                                                                                                                                             + Transaction.Transaction.TrxStatus.ORDERED.ToString() + ","
                                                                                                                                              + Transaction.Transaction.TrxStatus.PREPARED.ToString() + ","
                                                                                                                                             + Transaction.Transaction.TrxStatus.CANCELLED.ToString() + ","
                                                                                                                                             + Transaction.Transaction.TrxStatus.CLOSED.ToString()));
            try
            {
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<TransactionDTO>> task = transactionUseCases.GetTransactionDTOList(searchParameters, utilities, null, 0, 1000, false, false, false);
                    task.Wait();
                    transactions = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Failed to update display tags.") + " " + ex.Message, MessageType.Error);
            }


            if (transactions == null)
            {
                transactions = new List<TransactionDTO>();
            }
            received = transactions.Count;
            processing = transactions.Where(x => x.Status == Transaction.Transaction.TrxStatus.ORDERED.ToString() ||
                                                       x.Status == Transaction.Transaction.TrxStatus.PREPARED.ToString()).Count();
            //x.Status == Transaction.Transaction.TrxStatus.OPEN.ToString()||
            //x.Status==Transaction.Transaction.TrxStatus.INITIATED.ToString() ||
            //x.Status == Transaction.Transaction.TrxStatus.BOOKING.ToString()).Count();
            cancelled = transactions.Where(x => x.Status == Transaction.Transaction.TrxStatus.CANCELLED.ToString() ||
                                                       x.Status == Transaction.Transaction.TrxStatus.SYSTEMABANDONED.ToString()).Count();
            delivered = transactions.Where(x => x.Status == Transaction.Transaction.TrxStatus.CLOSED.ToString()).Count();
            SetDisplayTagsVM();

            List<TransactionDTO> newTransactionsList = transactions.Where(trx => trx.Status == Transaction.Transaction.TrxStatus.OPEN.ToString()
                                                                                      || trx.Status == Transaction.Transaction.TrxStatus.INITIATED.ToString()
                                                                                      || trx.Status == Transaction.Transaction.TrxStatus.BOOKING.ToString()
                                                                                      ).ToList();
            if (newTransactionsList == null)
            {
                newTransactionsList = new List<TransactionDTO>();
            }
            List<int> temp = new List<int>(listOfExistingNewTransactions);
            try
            {
                if (listOfExistingNewTransactions != null && listOfExistingNewTransactions.Any())
                {
                    foreach (int TrxId in temp)
                    {
                        if ((newTransactionsList.Any(x => x.TransactionId == TrxId) == false) && listOfExistingNewTransactions.Contains(TrxId))
                        {
                            listOfExistingNewTransactions.Remove(TrxId);
                        }
                    }
                    newTransactions = listOfExistingNewTransactions.Count;
                    DisplayTagsVM.DisplayTags[3][1].Text = newTransactions.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                SetFooterContent("Error while Updating display Tag" + ex.Message, MessageType.Error);
            }
            log.LogMethodExit();
        }
        private void GetDeliveryChannels()
        {
            log.LogMethodEntry();
            IDeliveryChannelUseCases deliveryChannelUseCases = DeliveryChannelUseCaseFactory.GetDeliveryChannelUseCases(ExecutionContext);
            List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>>();
            //searchParameters.Add(new KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>(DeliveryChannelDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<DeliveryChannelDTO> deliveryChannelDTOList = null;
            try
            {
                // await deliveryChannelUseCases.GetDeliveryChannel(searchParameters);
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
        }

        private List<ParafaitMessageQueueDTO> GetParafaitMessageQueueDTOList(List<TransactionDTO> transactionDTOList)
        {
            log.LogMethodEntry();
            List<string> transactionGuids = new List<string>();
            List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = null;
            if (transactionDTOList != null && transactionDTOList.Any())
            {
                transactionGuids = transactionDTOList.Select(x => x.Guid).ToList();
                IParafaitMessageQueueUseCases parafaitMessageQueueUseCases = ParafaitMessageQueueUseCaseFactory.GetParafaitMessageQueueUseCases(ExecutionContext);
                DateTime fromDateTime;
                fromDateTime = SiteDateTime.GetSiteDateTime(ExecutionContext.SiteId);
                fromDateTime = fromDateTime.AddHours(-1);
                List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.FROM_DATE, fromDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                try
                {
                    using (NoSynchronizationContextScope.Enter())
                    {
                        Task<List<ParafaitMessageQueueDTO>> task = parafaitMessageQueueUseCases.GetParafaitMessageQueueDTOList(transactionGuids, searchParameters);
                        task.Wait();
                        parafaitMessageQueueDTOList = task.Result;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
                }

            }
            if (parafaitMessageQueueDTOList == null)
            {
                parafaitMessageQueueDTOList = new List<ParafaitMessageQueueDTO>();
            }

            log.LogMethodExit(parafaitMessageQueueDTOList);
            return parafaitMessageQueueDTOList;
        }

        private void SetCustomDataGridVM()
        {
            log.LogMethodEntry();
            try
            {
                ObservableCollection<object> observableCollection = new ObservableCollection<object>() { ExecutionContext, searchedDeliveries.ToList() };
                LookupsContainerDTO lookupList = LookupsViewContainerList.GetLookupsContainerDTO(ExecutionContext.GetSiteId(), "ONLINE_FOOD_DELIVERY_TYPE");
                CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(searchedDeliveries);
                CustomDataGridVM.SelectOption = SelectOption.CheckBox;
                List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = GetParafaitMessageQueueDTOList(searchedDeliveries.ToList());
                CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                {
                    { "ChannelName",
                        new CustomDataGridColumnElement()
                        {
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Channel"),
                            SecondarySource = new ObservableCollection<object>(Channels),
                            SourcePropertyName = "TransctionOrderDispensingDTO.DeliveryChannelId",
                            ChildOrSecondarySourcePropertyName = "DeliveryChannelId"}
                    },
                    { "ExternalSystemReference",
                        new CustomDataGridColumnElement()
                        {
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Ref No")
                        }
                    },
                    { "TransctionOrderDispensingDTO.TransactionId",
                        new CustomDataGridColumnElement()
                        {
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Trxn Id")
                        }
                    },
                    { "TransctionOrderDispensingDTO.ScheduledDispensingTime",
                        new CustomDataGridColumnElement()
                        {
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Dispensing Time"),
                            DataGridColumnStringFormat = dateTimeFormat
                        }
                    },
                    { "TransctionOrderDispensingDTO",
                        new CustomDataGridColumnElement()
                        {
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Time Left"),
                            Converter = new CalculateRemainingTimeConverter(),
                            ConverterParameter = ExecutionContext
                        }
                    },
                    { "CustomerId",
                        new CustomDataGridColumnElement()
                        {
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Phone #"),
                            Type = DataEntryType.TextBlock,
                            Converter = new CustomerPhoneNumberConverter(),
                            ConverterParameter = observableCollection
                        }
                    },
                    { "LookupValue",
                        new CustomDataGridColumnElement()
                        {
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Type"),
                            SecondarySource = new ObservableCollection<object>(lookupList.LookupValuesContainerDTOList),
                            SourcePropertyName = "TransctionOrderDispensingDTO.OrderDispensingTypeId",
                            ChildOrSecondarySourcePropertyName = "LookupValueId"
                        }
                    },
                    { "Guid",
                        new CustomDataGridColumnElement()
                        {
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext,"Urban Piper Synch"),
                            Converter = new UrbanPiperStatusConverter(),
                            Type=DataEntryType.Button,
                            ActionButtonType = DataGridButtonType.Custom,
                            ConverterParameter = new ObservableCollection<Object>(){ parafaitMessageQueueDTOList }
                        }
                    }
                };
                if (CustomDataGridVM.CollectionToBeRendered != null && CustomDataGridVM.CollectionToBeRendered.Any())
                {
                    CustomDataGridVM.SelectedItems = new ObservableCollection<object>() { CustomDataGridVM.CollectionToBeRendered.First() };
                    rightSectionDisplayedTransaction = CustomDataGridVM.CollectionToBeRendered.First() as TransactionDTO;
                }
                else
                {
                    rightSectionDisplayedTransaction = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + "Custom DataGrid Error", MessageType.Error);
            }
            log.LogMethodExit();
        }
        private void SetDefaultValues()
        {
            log.LogMethodEntry();
            ActiveOrderChecked = true;
            TransactionId = string.Empty;
            ReferenceNo = string.Empty;
            Customer = string.Empty;
            SelectedChannel = null;
            SelectedDeliveryType = null;
            double businessStart = Convert.ToDouble(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "BUSINESS_DAY_START_TIME"));
            DateTime siteDateTime = SiteDateTime.GetSiteDateTime(ExecutionContext);
            FromDate = siteDateTime.Date.AddHours(businessStart).ToString(dateTimeFormat);
            ToDate = siteDateTime.Date.AddDays(1).AddHours(businessStart).ToString(dateTimeFormat);
            searchedDeliveries.Clear();
            //SetCustomDataGridVM();
            log.LogMethodExit();
        }
        private void OnPreviousNavigation(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(String.Empty, MessageType.None);

            if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null && selectedTransactionDTOList != null && selectedTransactionDTOList.Any())
            {
                TransactionDTO transactionDTO = selectedTransactionDTOList.FirstOrDefault(x => x.TransactionId == rightSectionDisplayedTransaction.TransactionId);
                int index = selectedTransactionDTOList.IndexOf(transactionDTO);
                if (index - 1 < CustomDataGridVM.UICollectionToBeRendered.Count)
                {
                    rightSectionDisplayedTransaction = selectedTransactionDTOList[index - 1];
                    OnRightSectionItemChanged();
                }
            }
            log.LogMethodExit();
        }
        private void OnNextNavigation(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(String.Empty, MessageType.None);
            if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null && selectedTransactionDTOList != null && selectedTransactionDTOList.Any())
            {
                TransactionDTO transactionDTO = selectedTransactionDTOList.FirstOrDefault(x => x.TransactionId == rightSectionDisplayedTransaction.TransactionId);
                int index = selectedTransactionDTOList.IndexOf(transactionDTO);
                if (index + 1 < selectedTransactionDTOList.Count)
                {
                    rightSectionDisplayedTransaction = selectedTransactionDTOList[index + 1];
                    OnRightSectionItemChanged();
                }
            }
            log.LogMethodExit();
        }
        private bool CanPreviousNavigationExecute()
        {
            log.LogMethodEntry();
            bool canExecute = true;
            if (CustomDataGridVM == null || CustomDataGridVM.UICollectionToBeRendered == null || CustomDataGridVM.SelectedItems == null ||
                 CustomDataGridVM.SelectedItems.Count <= 1)
            {
                canExecute = false;
            }
            if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null && selectedTransactionDTOList != null)
            {
                TransactionDTO transactionDTO = selectedTransactionDTOList.FirstOrDefault(x => x.TransactionId == rightSectionDisplayedTransaction.TransactionId);
                int index = selectedTransactionDTOList.IndexOf(transactionDTO);
                if (index - 1 < 0)
                {
                    canExecute = false;
                }
            }


            log.LogMethodExit(canExecute);
            return canExecute;
        }
        private bool CanNextNavigationExecute()
        {
            log.LogMethodEntry();
            bool canExecute = true;
            if (CustomDataGridVM == null || CustomDataGridVM.UICollectionToBeRendered == null || CustomDataGridVM.SelectedItems == null ||
                 CustomDataGridVM.SelectedItems.Count <= 1)
            {
                canExecute = false;
            }
            if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null && selectedTransactionDTOList != null)
            {
                TransactionDTO transactionDTO = selectedTransactionDTOList.FirstOrDefault(x => x.TransactionId == rightSectionDisplayedTransaction.TransactionId);
                int index = selectedTransactionDTOList.IndexOf(transactionDTO);
                if (index + 1 >= selectedTransactionDTOList.Count)
                {
                    canExecute = false;
                }
            }
            log.LogMethodExit(canExecute);
            return canExecute;
        }
        private void OnNavigationClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (deliveryOrderView != null)
            {
                deliveryOrderView.Close();
            }
            StopTimer();
            log.LogMethodExit();
        }
        private void OnDatePickerLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                selectedDatePicker = parameter as CustomTextBoxDatePicker;
                if (selectedDatePicker != null
                && selectedDatePicker.DatePickerView != null)
                {
                    selectedDatePicker.DatePickerView.Width = SystemParameters.PrimaryScreenWidth;
                    selectedDatePicker.DatePickerView.Height = SystemParameters.PrimaryScreenHeight;
                    selectedDatePicker.DatePickerView.Closed += OnDatePickerViewClosed;
                }
            }
            log.LogMethodExit();
        }

        private void OnDatePickerViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DatePickerView datePickerView = sender as DatePickerView;
            if (selectedDatePicker != null && !string.IsNullOrEmpty(selectedDatePicker.Name) && datePickerView != null
                && !string.IsNullOrEmpty(datePickerView.SelectedDate))
            {
                string dateText = Convert.ToDateTime(datePickerView.SelectedDate).ToString(dateTimeFormat);
                switch (selectedDatePicker.Name)
                {
                    case "tbxdpFrom":
                        FromDate = dateText;
                        break;
                    case "tbxdpTo":
                        ToDate = dateText;
                        break;
                }
            }
            log.LogMethodExit();
        }

        private void ClearRightSection()
        {
            SetGenericRightSectionContent(MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Id"), string.Empty, new ObservableCollection<RightSectionPropertyValues>(), CanPreviousNavigationExecute(), CanNextNavigationExecute());
        }
        private void InitializeCommands()
        {
            log.LogMethodEntry();
            navigationClickCommand = new DelegateCommand(OnNavigationClicked);
            loadedCommand = new DelegateCommand(OnLoaded);
            actionsCommand = new DelegateCommand(OnActionsClicked);
            datePickerLoadedCommand = new DelegateCommand(OnDatePickerLoaded);
            SelectedItemsChangedCommand = new DelegateCommand(SelectedItemsChanged);
            PreviousNavigationCommand = new DelegateCommand(OnPreviousNavigation);
            NextNavigationCommand = new DelegateCommand(OnNextNavigation);
            DisplayTagClickedCommand = new DelegateCommand(OnNewTransactionDisplayTagClicked);
            UrbanPiperStatusChanngedCommand = new DelegateCommand(OnUrbanPiperStatusClicked);
            //SelectedItemChanged = new DelegateCommand(OnSelectedItemChanged);
            log.LogMethodExit();
        }
        private void SelectedItemsChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            selectedTransactionDTOList = new List<TransactionDTO>();
            try
            {
                if (customDataGridVM.SelectedItems != null && customDataGridVM.SelectedItems.Any())
                {
                    foreach (object slectedItem in customDataGridVM.SelectedItems)
                    {
                        TransactionDTO selectedTransactionDTO = slectedItem as TransactionDTO;
                        selectedTransactionDTOList.Add(selectedTransactionDTO);
                    }
                    selectedTransactionDTOList = selectedTransactionDTOList.OrderByDescending(x => x.TransactionId).ToList();

                    TransactionDTO transactionDTO = customDataGridVM.SelectedItems.Last() as TransactionDTO;
                    if (transactionDTO != null)
                    {
                        ObservableCollection<RightSectionPropertyValues> propertyValues = new ObservableCollection<RightSectionPropertyValues>();
                        propertyValues.Add(new RightSectionPropertyValues()
                        {
                            Property = MessageViewContainerList.GetMessage(ExecutionContext, "Status"),
                            Value = transactionDTO.Status
                        });
                        propertyValues.Add(new RightSectionPropertyValues()
                        {
                            Property = MessageViewContainerList.GetMessage(ExecutionContext, "Aggregator Id"),
                            Value = transactionDTO.TransctionOrderDispensingDTO.ExternalSystemReference
                        });
                        propertyValues.Add(new RightSectionPropertyValues()
                        {
                            Property = MessageViewContainerList.GetMessage(ExecutionContext, "Customer Name"),
                            Value = transactionDTO.CustomerName
                        });
                        propertyValues.Add(new RightSectionPropertyValues()
                        {
                            Property = MessageViewContainerList.GetMessage(ExecutionContext, "Rider Name")
                        });
                        propertyValues.Add(new RightSectionPropertyValues()
                        {
                            Property = MessageViewContainerList.GetMessage(ExecutionContext, "Rider Contact")
                        });
                        propertyValues.Add(new RightSectionPropertyValues()
                        {
                            Property = MessageViewContainerList.GetMessage(ExecutionContext, "Rider Delivery Status")
                        });
                        propertyValues.Add(new RightSectionPropertyValues()
                        {
                            Property = MessageViewContainerList.GetMessage(ExecutionContext, "Remarks")
                        });
                        TransactionOrderDispensingDTO orderDispensingDTO = transactionDTO.TransctionOrderDispensingDTO as TransactionOrderDispensingDTO;
                        string subHeading = string.Empty;
                        if (orderDispensingDTO != null)
                        {
                            subHeading = orderDispensingDTO.TransactionId.ToString();
                            if (orderDispensingDTO.TransactionDeliveryDetailsDTOList != null &&
                            orderDispensingDTO.TransactionDeliveryDetailsDTOList.Count > 0)
                            {
                                string deliveryStatus = "-";
                                LookupsContainerDTO oosreasonlookup;
                                oosreasonlookup = LookupsViewContainerList.GetLookupsContainerDTO(ExecutionContext.GetSiteId(), "RIDER_DELIVERY_STATUS");

                                if (orderDispensingDTO.TransactionDeliveryDetailsDTOList.Any())
                                {
                                    TransactionDeliveryDetailsDTO activeTransactionDeliveryDetailsDTO = orderDispensingDTO.TransactionDeliveryDetailsDTOList.FirstOrDefault();

                                    if (oosreasonlookup != null)
                                    {
                                        LookupValuesContainerDTO lookup = oosreasonlookup.LookupValuesContainerDTOList.FirstOrDefault(l => l.LookupValueId == activeTransactionDeliveryDetailsDTO.RiderDeliveryStatus);
                                        if (lookup != null)
                                        {
                                            deliveryStatus = lookup.LookupValue;
                                        }
                                    }
                                    propertyValues[3].Value = activeTransactionDeliveryDetailsDTO.ExternalRiderName;
                                    propertyValues[4].Value = activeTransactionDeliveryDetailsDTO.RiderPhoneNumber;
                                    propertyValues[5].Value = deliveryStatus;
                                    propertyValues[6].Value = activeTransactionDeliveryDetailsDTO.Remarks;
                                }

                            }
                        }
                        rightSectionDisplayedTransaction = transactionDTO;
                        SetGenericRightSectionContent(MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Id"), subHeading, propertyValues, CanPreviousNavigationExecute(), CanNextNavigationExecute());
                    }
                }
                else
                {
                    ClearRightSection();
                    rightSectionDisplayedTransaction = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            //if (CustomDataGridVM.SelectedItems == null)
            //{
            //    SetGenericRightSectionContent(MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Id"), string.Empty, new ObservableCollection<RightSectionPropertyValues>(), false, false);
            //}
            log.LogMethodExit();
        }

        private void OnRightSectionItemChanged()
        {
            log.LogMethodEntry();
            try
            {
                //if (customDataGridVM.SelectedItem != null)
                //{
                TransactionDTO transactionDTO = rightSectionDisplayedTransaction;
                //    TransactionDTO transactionDTO = customDataGridVM.SelectedItem as TransactionDTO;
                if (transactionDTO != null)
                {
                    ObservableCollection<RightSectionPropertyValues> propertyValues = new ObservableCollection<RightSectionPropertyValues>();
                    propertyValues.Add(new RightSectionPropertyValues()
                    {
                        Property = MessageViewContainerList.GetMessage(ExecutionContext, "Status"),
                        Value = transactionDTO.Status
                    });
                    propertyValues.Add(new RightSectionPropertyValues()
                    {
                        Property = MessageViewContainerList.GetMessage(ExecutionContext, "Aggregator Id"),
                        Value = transactionDTO.TransctionOrderDispensingDTO.ExternalSystemReference
                    });
                    propertyValues.Add(new RightSectionPropertyValues()
                    {
                        Property = MessageViewContainerList.GetMessage(ExecutionContext, "Customer Name"),
                        Value = transactionDTO.CustomerName
                    });
                    propertyValues.Add(new RightSectionPropertyValues()
                    {
                        Property = MessageViewContainerList.GetMessage(ExecutionContext, "Rider Name")
                    });
                    propertyValues.Add(new RightSectionPropertyValues()
                    {
                        Property = MessageViewContainerList.GetMessage(ExecutionContext, "Rider Contact")
                    });
                    propertyValues.Add(new RightSectionPropertyValues()
                    {
                        Property = MessageViewContainerList.GetMessage(ExecutionContext, "Rider Delivery Status")
                    });
                    propertyValues.Add(new RightSectionPropertyValues()
                    {
                        Property = MessageViewContainerList.GetMessage(ExecutionContext, "Remarks")
                    });
                    TransactionOrderDispensingDTO orderDispensingDTO = transactionDTO.TransctionOrderDispensingDTO as TransactionOrderDispensingDTO;
                    string subHeading = string.Empty;
                    if (orderDispensingDTO != null)
                    {
                        subHeading = orderDispensingDTO.TransactionId.ToString();
                        if (orderDispensingDTO.TransactionDeliveryDetailsDTOList != null &&
                        orderDispensingDTO.TransactionDeliveryDetailsDTOList.Any())
                        {
                            string deliveryStatus = "-";
                            LookupsContainerDTO oosreasonlookup;
                            oosreasonlookup = LookupsViewContainerList.GetLookupsContainerDTO(ExecutionContext.GetSiteId(), "RIDER_DELIVERY_STATUS");

                            if (orderDispensingDTO.TransactionDeliveryDetailsDTOList.Exists(x => x.IsActive == true))
                            {
                                TransactionDeliveryDetailsDTO activeTransactionDeliveryDetailsDTO = orderDispensingDTO.TransactionDeliveryDetailsDTOList.Find(x => x.IsActive == true);

                                if (oosreasonlookup != null)
                                {
                                    LookupValuesContainerDTO lookup = oosreasonlookup.LookupValuesContainerDTOList.FirstOrDefault(l => l.LookupValueId == activeTransactionDeliveryDetailsDTO.RiderDeliveryStatus);
                                    if (lookup != null)
                                    {
                                        deliveryStatus = lookup.LookupValue;
                                    }
                                }
                                propertyValues[3].Value = activeTransactionDeliveryDetailsDTO.ExternalRiderName;
                                propertyValues[4].Value = activeTransactionDeliveryDetailsDTO.RiderPhoneNumber;
                                propertyValues[5].Value = deliveryStatus;
                                propertyValues[6].Value = activeTransactionDeliveryDetailsDTO.Remarks;
                            }
                            else
                            {
                                if (oosreasonlookup != null)
                                {
                                    LookupValuesContainerDTO lookup = oosreasonlookup.LookupValuesContainerDTOList.FirstOrDefault(l => l.LookupValue.ToLower() == "unassigned");
                                    if (lookup != null)
                                    {
                                        deliveryStatus = lookup.LookupValue;
                                    }
                                }
                                TransactionDeliveryDetailsDTO inactiveTransactionDeliveryDetailsDTO = orderDispensingDTO.TransactionDeliveryDetailsDTOList.OrderByDescending(x => x.LastUpdateDate).FirstOrDefault();
                                propertyValues[3].Value = inactiveTransactionDeliveryDetailsDTO.ExternalRiderName;
                                propertyValues[4].Value = inactiveTransactionDeliveryDetailsDTO.RiderPhoneNumber;
                                propertyValues[5].Value = deliveryStatus;
                                propertyValues[6].Value = inactiveTransactionDeliveryDetailsDTO.Remarks;
                            }
                        }
                    }
                    SetGenericRightSectionContent(MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Id"), subHeading, propertyValues, CanPreviousNavigationExecute(), CanNextNavigationExecute());
                }
                //}
                //else
                //{
                //    ClearRightSection();
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            //if (CustomDataGridVM.SelectedItems == null)
            //{
            //    SetGenericRightSectionContent(MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Id"), string.Empty, new ObservableCollection<RightSectionPropertyValues>(), false, false);
            //}
            log.LogMethodExit();
        }

        private void OnUrbanPiperStatusClicked(object parameter)
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);
            try
            {
                if (CustomDataGridVM != null && CustomDataGridVM.ButtonClickedModel != null && CustomDataGridVM.ButtonClickedModel.Item != null)
                {
                    TransactionDTO clickedTransactionDTO = CustomDataGridVM.ButtonClickedModel.Item as TransactionDTO;

                    if (clickedTransactionDTO != null)
                    {
                        IParafaitMessageQueueUseCases parafaitMessageQueueUseCases = ParafaitMessageQueueUseCaseFactory.GetParafaitMessageQueueUseCases(ExecutionContext);
                        DateTime fromDateTime;
                        List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = null;
                        fromDateTime = DateTime.Now.AddHours(-2);
                        List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.IS_ACTIVE, "1"));
                        searchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.ENTITY_GUID, clickedTransactionDTO.Guid));
                        searchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.FROM_DATE, fromDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        try
                        {
                            using (NoSynchronizationContextScope.Enter())
                            {
                                Task<List<ParafaitMessageQueueDTO>> task = parafaitMessageQueueUseCases.GetParafaitMessageQueue(searchParameters);
                                task.Wait();
                                parafaitMessageQueueDTOList = task.Result;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
                        }
                        if (parafaitMessageQueueDTOList != null && parafaitMessageQueueDTOList.Any())
                        {
                            parafaitMessageQueueDTOList = parafaitMessageQueueDTOList.OrderByDescending(x => x.MessageQueueId).ToList();
                            if (parafaitMessageQueueDTOList.Exists(x => !string.IsNullOrEmpty(x.Message)))
                            {
                                ParafaitMessageQueueDTO parafaitMessageQueueDTO = parafaitMessageQueueDTOList.First(x => !string.IsNullOrEmpty(x.Message));
                                if (parafaitMessageQueueDTO != null)
                                {
                                    string status = parafaitMessageQueueDTO.Status == MessageQueueStatus.Read ? "COMPLETE" : "PENDING";
                                    OpenGenericMessagePopupView("URBAN PIPER SYNC STATUS", status, parafaitMessageQueueDTO.Remarks, "OK", "OK", MessageButtonsType.OK); ;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
            }


        }
        private void OnLoad()
        {
            log.LogMethodEntry();
            PerformDBSearch();


            List<TransactionDTO> newtransactionsDTOList = searchedDeliveries.Where(trx => trx.Status == Transaction.Transaction.TrxStatus.OPEN.ToString()
                                                                                      || trx.Status == Transaction.Transaction.TrxStatus.INITIATED.ToString()
                                                                                      || trx.Status == Transaction.Transaction.TrxStatus.BOOKING.ToString()).ToList();

            foreach (TransactionDTO transactionDTO in newtransactionsDTOList)
            {
                listOfExistingNewTransactions.Add(transactionDTO.TransactionId);
            }

            newTransactions = newtransactionsDTOList.Count;
            DisplayTagsVM.DisplayTags[3][1].Text = newTransactions.ToString();
            searchedDeliveries = new ObservableCollection<TransactionDTO>(newtransactionsDTOList);
            SetCustomDataGridVM();
            log.LogMethodExit();
        }
        private void OnNewTransactionDisplayTagClicked(object obj)
        {
            log.LogMethodEntry();
            newTrxtimer.Stop();
            timeLeftUpdateTimer.Stop();
            IsLoadingVisible = true;
            ShowSearchArea = false;
            try
            {
                List<TransactionDTO> transactions = null;
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                String trxList = String.Join(",", listOfExistingNewTransactions);
                if (listOfExistingNewTransactions != null && listOfExistingNewTransactions.Count > 0)
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID_LIST, trxList));
                    try
                    {
                        using (NoSynchronizationContextScope.Enter())
                        {
                            Task<List<TransactionDTO>> task = transactionUseCases.GetTransactionDTOList(searchParameters, utilities, null, 0, 1000, true, false, false);
                            task.Wait();
                            transactions = task.Result;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
                    }
                }
                if (transactions == null)
                {
                    transactions = new List<TransactionDTO>();
                }
                transactions = transactions.FindAll(trx => trx.TransctionOrderDispensingDTO != null);
                searchedDeliveries = new ObservableCollection<TransactionDTO>(transactions);

                SetCustomDataGridVM();
                UpdateDisplayTags();
            }
            catch (Exception ex)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4032), MessageType.Error);
            }
            IsLoadingVisible = false;
            newTrxtimer.Start();
            timeLeftUpdateTimer.Start();
            log.LogMethodExit();
        }

        private void SetDeliveryTypes()
        {
            log.LogMethodEntry();
            List<LookupValuesContainerDTO> lookupValuesContainerDTOList = LookupsViewContainerList.GetLookupsContainerDTO(ExecutionContext.SiteId, "ONLINE_FOOD_DELIVERY_TYPE").LookupValuesContainerDTOList;
            if (lookupValuesContainerDTOList != null)
            {
                DeliveryTypes = new ObservableCollection<LookupValuesContainerDTO>(lookupValuesContainerDTOList);
            }
            log.LogMethodExit();
        }
        private void SetFetchNewTransactionInXSeconds()
        {
            log.LogMethodEntry();
            List<LookupValuesContainerDTO> lookupValuesContainerDTOList = LookupsViewContainerList.GetLookupsContainerDTO(ExecutionContext.SiteId, "DELIVERY_UI_SETUP").LookupValuesContainerDTOList;
            if (lookupValuesContainerDTOList != null)
            {
                string value = lookupValuesContainerDTOList.Where(x => x.LookupValue == "FETCH_NEW_TRX_IN_LAST_X_SECS").FirstOrDefault().Description;//check this query
                if (string.IsNullOrEmpty(value))
                {
                    fetchNewTransactionsInLastXSeconds = 30;
                }
                else
                {
                    fetchNewTransactionsInLastXSeconds = Convert.ToInt32(value);
                }
            }
            log.LogMethodExit();
        }
        private void PlayAlaramSounds()
        {
            log.LogMethodEntry();
            try
            {
                if (SoundPlayer.IsLoadCompleted == false)
                {
                    SoundPlayer.Load();
                }
                SoundPlayer.Play();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void NewTransactionsRefresh()
        {
            log.LogMethodEntry();
            Stopwatch sw = new Stopwatch();
            log.Debug("Timer starts");
            sw.Start();

            List<TransactionDTO> transactions = null;
            List<int> newTransactionIdList = new List<int>();
            try
            {
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();

                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.NEEDS_ORDER_DISPENSING, "1"));

                DateTime serverDateTime = SiteDateTime.GetSiteDateTime(ExecutionContext.SiteId);
                DateTime fromDateTime;
                DateTime toDateTime;
                double businessStart = Convert.ToDouble(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "BUSINESS_DAY_START_TIME"));
                fromDateTime = serverDateTime.Date.AddHours(businessStart);
                //fromDateTime = serverDateTime.AddMinutes(-fetchNewTransactionsInLastXSeconds);
                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, fromDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));


                toDateTime = serverDateTime;
                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, toDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));

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

                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, Transaction.Transaction.TrxStatus.OPEN.ToString() + ","
                                                                                                                                                 + Transaction.Transaction.TrxStatus.INITIATED.ToString() + ","
                                                                                                                                                  + Transaction.Transaction.TrxStatus.BOOKING.ToString()));// + ","
                                                                                                                                                                                                           //+ Transaction.Transaction.TrxStatus.ORDERED.ToString()));
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<TransactionDTO>> task = transactionUseCases.GetTransactionDTOList(searchParameters, utilities, null, 0, 1000, false, false, false);
                    task.Wait();
                    transactions = task.Result;
                }
                sw.Stop();
                int timeitTook = sw.Elapsed.Milliseconds;
                Console.WriteLine("Time It Took: " + timeitTook);
                log.Debug("Time It Took: " + timeitTook);
                if (transactions == null)
                {
                    transactions = new List<TransactionDTO>();
                }

                foreach (TransactionDTO transactionDTO in transactions)
                {
                    newTransactionIdList.Add(transactionDTO.TransactionId);
                    if (listOfExistingNewTransactions.Contains(transactionDTO.TransactionId) == false)
                    {
                        listOfExistingNewTransactions.Add(transactionDTO.TransactionId);
                        //SystemSounds.Exclamation.Play();
                    }
                }
                if (listOfExistingNewTransactions.Count > 0)
                {
                    PlayAlaramSounds();
                }
                //else
                //{
                //    SoundPlayer.Stop();
                //}

                newTransactions = listOfExistingNewTransactions.Count;
                DisplayTagsVM.DisplayTags[3][1].Text = newTransactions.ToString();
            }
            catch (Exception ex)
            {
                sw.Stop();
                Console.WriteLine("Failed Time It Took to" + sw.Elapsed.Milliseconds);
                log.Debug("Failed  TimeItTook :" + sw.Elapsed.Milliseconds);
                log.Error("Auto Refresh failed: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void AutoRefresh(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            newTrxtimer.Stop();
            NewTransactionsRefresh();
            newTrxtimer.Start();
            log.LogMethodExit();
        }
        private void TimeLeftRefresh(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //if (customDataGridVM.SelectedItems != null &&
            //   customDataGridVM.SelectedItems.Any())
            //{
            //    return;
            //}
            IsLoadingVisible = true;
            timeLeftUpdateTimer.Stop();

            //System.Threading.Thread.Sleep(1000);
            try
            {
                if (CustomDataGridVM != null && CustomDataGridVM.CollectionToBeRendered.Any())
                {
                    CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(searchedDeliveries);
                    CustomDataGridVM.SelectedItems = new ObservableCollection<object>() { CustomDataGridVM.CollectionToBeRendered.First() };
                    rightSectionDisplayedTransaction = CustomDataGridVM.CollectionToBeRendered.First() as TransactionDTO;
                }
            }
            catch (Exception ex)
            {
                SetFooterContent("Time Left Refresh Error" + ex.Message, MessageType.None);
                log.Error(ex.Message);
            }
            timeLeftUpdateTimer.Start();
            IsLoadingVisible = false;
            log.LogMethodExit();
        }

        private void SetDisplayTagsVM()
        {
            log.LogMethodEntry();
            if (DisplayTagsVM != null && DisplayTagsVM.DisplayTags != null && DisplayTagsVM.DisplayTags.Count > 0)
            {
                DisplayTagsVM.DisplayTags[0][1].Text = received.ToString();
                DisplayTagsVM.DisplayTags[1][1].Text = processing.ToString();
                DisplayTagsVM.DisplayTags[2][1].Text = delivered.ToString();
                // DisplayTagsVM.DisplayTags[3][1].Text = cancelled.ToString();
                DisplayTagsVM.DisplayTags[3][1].Text = newTransactions.ToString();
            }
            log.LogMethodExit();
        }

        public void StopTimer()
        {
            log.LogMethodEntry();
            if (newTrxtimer != null)
            {
                newTrxtimer.Stop();
            }
            log.LogMethodExit();
        }
        private void GetAcceptButtonVisibility()
        {
            log.LogMethodEntry();
            if (channels != null && channels.Any(x => x.AutoAcceptOrders == false))
            {
                AcceptButtonVisibility = Visibility.Visible;
            }
            else
            {
                AcceptButtonVisibility = Visibility.Collapsed;
            }
            log.LogMethodExit();
        }

        //private void AutoRefreshDummy(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry();
        //    newTransactions += 1;
        //    DisplayTagsVM.DisplayTags[3][1].Text = newTransactions.ToString();
        //    if (newTransactions > 0)
        //    {
        //        PlayAlaramSounds();
        //    }

        //    log.LogMethodExit();
        //}
        #endregion

        #region Constructor
        public DeliveryOrderVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);

            ExecutionContext = executionContext;

            //Uncomment for HQ testing testing
            //try
            //{
            //    ExecutionContext result;
            //    LoginRequest loginRequest = new LoginRequest();
            //    loginRequest.LoginId = "Semnox";
            //    loginRequest.Password = "semnoX!1";
            //    loginRequest.SiteId = 1020.ToString();
            //    loginRequest.MachineName = Environment.MachineName;
            //    IAuthenticationUseCases authenticationUseCases = AuthenticationUseCaseFactory.GetAuthenticationUseCases();
            //    using (NoSynchronizationContextScope.Enter())
            //    {
            //        Task<ExecutionContext> systemUserExecutionContextTask = authenticationUseCases.LoginSystemUser(loginRequest);
            //        systemUserExecutionContextTask.Wait();
            //        result = systemUserExecutionContextTask.Result;
            //    }
            //    ExecutionContext = result;
            //}
            //catch (Exception ex)
            //{
            //    SetFooterContent(ex.Message, MessageType.None);

            //}

            ModuleName = MessageViewContainerList.GetMessage(ExecutionContext, "Delivery Orders");
            dateTimeFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT");
            showSearchArea = true;
            enableActionButtons = true;
            SoundPlayer = new SoundPlayer("New Order Notify.wav");

            FooterVM = new FooterVM(executionContext)
            {
                MessageType = MessageType.None,
                Message = string.Empty,
                HideSideBarVisibility = Visibility.Collapsed
            };
            GenericRightSectionContentVM = new GenericRightSectionContentVM();
            Channels = new ObservableCollection<DeliveryChannelDTO>();
            GetDeliveryChannels();
            GetAcceptButtonVisibility();
            lastActivityTime = SiteDateTime.GetSiteDateTime(ExecutionContext.SiteId);
            utilities = GetUtility(ExecutionContext);
            deliveryOrdersRefreshFrequency = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "DELIVERY_ORDERS_REFRESH_FREQUENCY", 180);
            lookupValuesOfUrbanPiperCancellationRequestCodes = GetlookupValuesOfUrbanPiperCancellationRequestCodes();
            enableOrderShareAccrossPos = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ENABLE_ORDER_SHARE_ACROSS_POS");
            enableOrderShareAccrossPosUsers = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ENABLE_ORDER_SHARE_ACROSS_USERS");
            enableOrderShareAccrossPosCounters = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS");
            listOfExistingNewTransactions = new List<int>();
            searchedDeliveries = new ObservableCollection<TransactionDTO>();
            SetDeliveryTypes();
            SetFetchNewTransactionInXSeconds();
            SetDefaultValues();
            InitializeDisplayTagsVM();
            OnLoad();
            UpdateDisplayTags();
            InitializeCommands();


            timeLeftUpdateTimer = new DispatcherTimer();
            timeLeftUpdateTimer.Interval = TimeSpan.FromSeconds(deliveryOrdersRefreshFrequency);
            timeLeftUpdateTimer.Tick += TimeLeftRefresh;
            timeLeftUpdateTimer.Start();
            newTrxtimer = new DispatcherTimer();
            autoRefreshNewDeliveryCount = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "AUTO_REFRESH_NEW_DELIVERY_COUNT");
            frequencyForNewDelivery = ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "FREQUENCY_FOR_NEW_DELIVERY_REFRESH");
            if (autoRefreshNewDeliveryCount)
            {
                newTrxtimer.Interval = TimeSpan.FromSeconds(frequencyForNewDelivery);
                newTrxtimer.Tick += AutoRefresh;
                newTrxtimer.Start();
            }
            log.LogMethodExit();
        }
        #endregion
    }


}

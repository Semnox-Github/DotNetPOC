/********************************************************************************************
 * Project Name - Transaction UI
 * Description  - EditPaymentsVM
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     27-Sep-2021    Fiona                  Created 
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
using Semnox.Parafait.TableAttributeSetup;
using Semnox.Parafait.TableAttributeSetupUI;
using Semnox.Parafait.TableAttributeDetailsUtils;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.TransactionUI
{
    public class EditPaymentsVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ICommand navigationClickCommand;
        private ICommand loadedCommand;
        private ICommand actionsCommand;
        private string moduleName;
        private EditPaymentsView editPaymentsView;
        private double amountToSettle;
        private ObservableCollection<PaymentModesContainerDTO> paymentModes;
        private ObservableCollection<PaymentModesContainerDTO> compatiblePaymentModes;
        private PaymentModesContainerDTO selectedPaymentMode;
        private PaymentModesContainerDTO selectedCompatiblePaymentMode;
        private bool showSearchArea;
        private string dateTimeFormat;
        private ObservableCollection<TransactionPaymentsDTO> transactionPaymentsDTOCollection;
        private CustomDataGridVM transactionPaymentsCustomDataGridVM;
        private ICommand paymentModesSelectedCommand;
        private Visibility cCNameVisibility;
        private Visibility nameOnCCVisibility;
        private Visibility cardNumberVisibility;
        private bool paymentReferenceEnabled;
        private bool authorizarionEnabled;
        private string cCName;
        private bool cCNameEnabled;
        private string nameOnCC;
        private string cardNumber;
        private bool cardNumberEnabled;
        private string paymentReference;
        private string authorizarion;
        private bool nameonCCEnabled;
        private ICommand compatiblePaymentModesSelectedCommand;
        private List<TableAttributeDetailsDTO> tableAttributeDetailsDTOList;
        private ObservableCollection<TransactionPaymentSummaryDTO> transactionPaymentSummaryDTOCollection;
        private bool enableOrderShareAccrossPos;
        private bool enableOrderShareAccrossPosUsers;
        private bool enableOrderShareAccrossPosCounters;
        private string fromDate;
        private string toDate;
        private string attribute1;
        private Dictionary<int, TransactionPaymentsDTO> transactionPaymentDictionary;
        private List<AdvancedFiltersSummaryDTO> advancedFiltersSummaryDTOList;
        private Dictionary<Operator, string> operatorDictionary;
        private double unmatchedBalance;

      


        enum PaymentMode
        {
            CREDIT,
            CASH,
            OTHER
        }


        #endregion
        #region Properties
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
        public ObservableCollection<TransactionPaymentsDTO> TransactionPaymentsDTOCollection
        {
            get
            {
                log.LogMethodEntry();
                
                log.LogMethodExit();
                return transactionPaymentsDTOCollection;
            }
            set
            {
                if (!object.Equals(transactionPaymentsDTOCollection, value))
                {
                    transactionPaymentsDTOCollection = value;
                    OnPropertyChanged();
                }
            }

        }
        public double AmountToSettle
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return amountToSettle;
            }
            set
            {
                if (!object.Equals(amountToSettle, value))
                {
                    amountToSettle = value;
                    OnPropertyChanged();
                }
            }
        }
        public double UnmatchedBalance
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(unmatchedBalance);
                return unmatchedBalance;
            }
            set
            {
                if (!object.Equals(unmatchedBalance, value))
                {
                    unmatchedBalance = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<PaymentModesContainerDTO> PaymentModes
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
        public ObservableCollection<PaymentModesContainerDTO> CompatiblePaymentModes
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(compatiblePaymentModes);
                return compatiblePaymentModes;
            }
            set
            {
                if (!object.Equals(compatiblePaymentModes, value))
                {
                    compatiblePaymentModes = value;
                    OnPropertyChanged();
                }
            }
        }
        public PaymentModesContainerDTO SelectedPaymentMode
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
        public PaymentModesContainerDTO SelectedCompatiblePaymentMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedCompatiblePaymentMode);
                return selectedCompatiblePaymentMode;
            }
            set
            {
                if (!object.Equals(selectedCompatiblePaymentMode, value))
                {
                    selectedCompatiblePaymentMode = value;
                    OnPropertyChanged();
                }
            }
        }
        public CustomDataGridVM TransactionPaymentsCustomDataGridVM
        {
            get
            {
                log.LogMethodEntry();
                if (transactionPaymentsCustomDataGridVM == null)
                {
                    transactionPaymentsCustomDataGridVM = new CustomDataGridVM(ExecutionContext)
                    {
                        IsComboAndSearchVisible = false,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                    };
                }
                log.LogMethodExit(transactionPaymentsCustomDataGridVM);
                return transactionPaymentsCustomDataGridVM;
            }
            set
            {
                if (!object.Equals(transactionPaymentsCustomDataGridVM, value))
                {
                    transactionPaymentsCustomDataGridVM = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand PaymentModesSelectedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return paymentModesSelectedCommand;
            }
            set
            {
                if (!object.Equals(paymentModesSelectedCommand, value))
                {
                    paymentModesSelectedCommand = value;
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
        public string CCName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cCName);
                return cCName;
            }
            set
            {
                if (!object.Equals(cCName, value))
                {
                    cCName = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility CCNameVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return cCNameVisibility;
            }
            set
            {
                if (!object.Equals(cCNameVisibility, value))
                {
                    cCNameVisibility = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility NameOnCCVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(nameOnCCVisibility);
                return nameOnCCVisibility;
            }
            set
            {
                if (!object.Equals(nameOnCCVisibility, value))
                {
                    nameOnCCVisibility = value;
                    OnPropertyChanged();
                }
            }
        }
        public Visibility CardNumberVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return cardNumberVisibility;
            }
            set
            {
                if (!object.Equals(cardNumberVisibility, value))
                {
                    cardNumberVisibility = value;
                    OnPropertyChanged();
                }
            }
        }
        public string PaymentReference
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(paymentReference);
                return paymentReference;
            }
            set
            {
                if (!object.Equals(paymentReference, value))
                {
                    paymentReference = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool PaymentReferenceEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return !paymentReferenceEnabled;
            }
            set
            {
                if (!object.Equals(paymentReferenceEnabled, value))
                {
                    paymentReferenceEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool AuthorizarionEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(authorizarionEnabled);
                return !authorizarionEnabled;
            }
            set
            {
                if (!object.Equals(authorizarionEnabled, value))
                {
                    authorizarionEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Authorization
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(authorizarion);
                return authorizarion;
            }
            set
            {
                if (!object.Equals(authorizarion, value))
                {
                    authorizarion = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool CCNameEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cCNameEnabled);
                return !cCNameEnabled;
            }
            set
            {
                if (!object.Equals(cCNameEnabled, value))
                {
                    cCNameEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public string NameOnCC
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(nameOnCC);
                return nameOnCC;
            }
            set
            {
                if (!object.Equals(nameOnCC, value))
                {
                    nameOnCC = value;
                    OnPropertyChanged();
                }
            }
        }
        public string CardNumber
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardNumber);
                return cardNumber;
            }
            set
            {
                if (!object.Equals(cardNumber, value))
                {
                    cardNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool CardNumberEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardNumberEnabled);
                return !cardNumberEnabled;
            }
            set
            {
                if (!object.Equals(cardNumberEnabled, value))
                {
                    cardNumberEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool NameonCCEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(nameonCCEnabled);
                return !nameonCCEnabled;
            }
            set
            {
                if (!object.Equals(nameonCCEnabled, value))
                {
                    nameonCCEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand CompatiblePaymentModesSelectedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(compatiblePaymentModesSelectedCommand);
                return compatiblePaymentModesSelectedCommand;
            }
            set
            {
                if (!object.Equals(compatiblePaymentModesSelectedCommand, value))
                {
                    compatiblePaymentModesSelectedCommand = value;
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
        #endregion
        #region Methods
        private void InitializeCommands()
        {
            log.LogMethodEntry();
            NavigationClickCommand = new DelegateCommand(OnNavigationClicked);
            LoadedCommand = new DelegateCommand(OnLoaded);
            ActionsCommand = new DelegateCommand(OnActionsClicked);
            PaymentModesSelectedCommand = new DelegateCommand(OnPaymentModesSelectetionChanged);
            CompatiblePaymentModesSelectedCommand = new DelegateCommand(OnCompatiblePaymentModesSelectionChanged);
            log.LogMethodExit();
        }

        private void OnCompatiblePaymentModesSelectionChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(selectedCompatiblePaymentMode != null)
            {
                PaymentMode paymentMode = GetPaymentMode(selectedCompatiblePaymentMode);
                switch(paymentMode)
                {
                    case PaymentMode.CREDIT:
                        EnableDisableControls(true);
                        PaymentReferenceEnabled = false;
                        break;
                    case PaymentMode.OTHER:
                        EnableDisableControls(false);
                        PaymentReferenceEnabled = true;
                        break;
                    default:
                        EnableDisableControls(false);
                        PaymentReferenceEnabled = false;
                        break;

                }
            }
            log.LogMethodExit();
        }
        private List<TransactionPaymentSummaryDTO> GetSelectedRecords()
        {
            log.LogMethodEntry();
            List<TransactionPaymentSummaryDTO> selectedTransactionPaymentDTOList = new List<TransactionPaymentSummaryDTO>();
            foreach (object data in TransactionPaymentsCustomDataGridVM.SelectedItems)
            {
                TransactionPaymentSummaryDTO selectedDTO = data as TransactionPaymentSummaryDTO;
                if (selectedDTO != null)
                {
                    selectedTransactionPaymentDTOList.Add(selectedDTO);
                }
            }
            log.LogMethodExit(selectedTransactionPaymentDTOList);
            return selectedTransactionPaymentDTOList;
        }
        private PaymentMode GetPaymentMode(PaymentModesContainerDTO paymentModeDTO)
        {
            if(paymentModeDTO.IsCreditCard)
            {
                return PaymentMode.CREDIT;
            }
            if(paymentModeDTO.IsCash)
            {
                return PaymentMode.CASH;
            }
            return PaymentMode.OTHER;
        }
        private void OnPaymentModesSelectetionChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty, MessageType.None);
            GetCompatiblePaymentModes();
            log.LogMethodExit();
        }
        

        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                editPaymentsView = parameter as EditPaymentsView;
            }
            log.LogMethodExit();
        }
        private void OnNavigationClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            PerformClose();
            log.LogMethodExit();
        }
        private void OnActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty, MessageType.None);
            if (parameter != null)
            {
                Button button = parameter as Button;
                if (button != null && !string.IsNullOrEmpty(button.Name))
                {
                    switch (button.Name)
                    {
                        case "btnAdvancedFilters":
                            {
                                PerformAdvancedFilters();
                            }
                            break;
                        case "btnSeachVisible":
                            {
                                ShowSearchArea = !showSearchArea;
                            }
                            break;
                        case "bthGetTransactions":
                            {
                                RefreshGrid();
                            }
                            break;
                        case "btnConfirm":
                            {
                                UpdateTransactionPayments();
                            }
                            break;
                        case "btnAttribtes":
                            {
                                CallTableAttributesView();
                            }
                            break;
                        case "btnCancel":
                            {
                                PerformClose();
                            }
                            break;
                    }
                }
            }
         
            log.LogMethodExit();
        }
        private void PerformClose()
        {
            log.LogMethodEntry();
            if (editPaymentsView != null)
            {
                editPaymentsView.Close();
            }
            log.LogMethodExit();
        }
        private void RefreshGrid()
        {
            log.LogMethodEntry();
            LoadTransactionPayments();
            SetTransactionPaymentsCustomDataGridVM();
            log.LogMethodExit();
        }

        private void CallTableAttributesView()
        {
            log.LogMethodEntry();
            if(selectedCompatiblePaymentMode!=null && TransactionPaymentsCustomDataGridVM.SelectedItem!=null)
            {
                TransactionPaymentSummaryDTO selectedPaymentModesSummaryDTO = TransactionPaymentsCustomDataGridVM.SelectedItem as TransactionPaymentSummaryDTO;
                TransactionPaymentsDTO selectedtrxPaymentDTO = transactionPaymentDictionary[selectedPaymentModesSummaryDTO.PaymentId];
                TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(ExecutionContext, selectedtrxPaymentDTO, false, false, editPaymentsView);
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4124), MessageType.Info);
                //"Please select a Compatible Payment Mode to proceed"
            }
            log.LogMethodExit();
        }
        private void UpdateTransactionPayments()
        {
            log.LogMethodEntry();

            if (ValidatePaymentData())
            {
                try
                {
                    TransactionPaymentsDTO updatedTransactionPaymentsDTO = SetNewPaymentModeDetails();
                    if (updatedTransactionPaymentsDTO != null)
                    {
                        ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
                        List<KeyValuePair<TransactionPaymentsDTO, string>> result = null;
                        using (NoSynchronizationContextScope.Enter())
                        {

                            Task<List<KeyValuePair<TransactionPaymentsDTO, string>>> task = transactionUseCases.UpdateTransactionPaymentModeDetails(new List<TransactionPaymentsDTO>() { updatedTransactionPaymentsDTO });
                            task.Wait();
                            result = task.Result;
                            if (result != null && result.Any())
                            {
                                string successMessage = MessageViewContainerList.GetMessage(ExecutionContext, 4125);
                                //"Payment details have been updated"
                                ShowErrorMessages(result, successMessage);
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
                RefreshGrid();
            }
            log.LogMethodExit();
        }
        private void ShowErrorMessages(List<KeyValuePair<TransactionPaymentsDTO, string>> keyValuePairs, string successMsg)
        {
            log.LogMethodEntry(keyValuePairs, successMsg);
            List<KeyValuePair<TransactionPaymentsDTO, string>> errorRecordList = keyValuePairs.Where(key => key.Value != null).ToList();
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
                        stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Payment Id: ") + errorRecordList[i].Key.PaymentId + " ");
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
        private TransactionPaymentsDTO SetNewPaymentModeDetails()
        {
            log.LogMethodEntry();
            if (transactionPaymentsCustomDataGridVM.SelectedItem != null && selectedCompatiblePaymentMode!=null)
            {
                TransactionPaymentSummaryDTO transactionPaymentSummaryDTO = transactionPaymentsCustomDataGridVM.SelectedItem as TransactionPaymentSummaryDTO;
                TransactionPaymentsDTO selectedTransactionPaymentsDTO = transactionPaymentDictionary[transactionPaymentSummaryDTO.PaymentId];
                if (!string.IsNullOrEmpty(cCName) && cCNameEnabled)
                {
                    selectedTransactionPaymentsDTO.CreditCardName = CCName;
                }
                if (!string.IsNullOrEmpty(nameOnCC) && nameonCCEnabled)
                {
                    selectedTransactionPaymentsDTO.NameOnCreditCard = nameOnCC;
                }
                if (!string.IsNullOrEmpty(cardNumber) && cardNumberEnabled)
                {
                    selectedTransactionPaymentsDTO.CreditCardNumber = cardNumber;
                }
                if (!string.IsNullOrEmpty(paymentReference) && paymentReferenceEnabled)
                {
                    selectedTransactionPaymentsDTO.Reference = paymentReference;
                }
                if (!string.IsNullOrEmpty(authorizarion) && authorizarionEnabled)
                {
                    selectedTransactionPaymentsDTO.CreditCardAuthorization = authorizarion;
                }
                selectedTransactionPaymentsDTO.PaymentModeId = selectedCompatiblePaymentMode.PaymentModeId;
                log.LogMethodExit(selectedTransactionPaymentsDTO);
                return selectedTransactionPaymentsDTO;
            }
            else
            {
                return null;
            }
        }
        private bool ValidatePaymentData()
        {
            log.LogMethodEntry();
            bool result = true;
            if (selectedCompatiblePaymentMode == null)
            {
                result = false;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4124), MessageType.Info);
            }
           
            log.LogMethodExit();
            return result;
        }
        private void LoadTransactionPayments()
        {
            log.LogMethodEntry();
            transactionPaymentDictionary.Clear();
            transactionPaymentSummaryDTOCollection.Clear();
            IsLoadingVisible = true;
          
            List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
            if (selectedPaymentMode != null)
            {
                List<TransactionDTO> transactions=null;
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
                TransactionSearchCriteria searchCriteria = new TransactionSearchCriteria();
                searchCriteria.And(TransactionSearchCriteriaParameters.TRX_PAYMENT_MODE_ID, Operator.EQUAL_TO, selectedPaymentMode.PaymentModeId);
                if (enableOrderShareAccrossPos == false)
                {
                    searchCriteria.And(TransactionSearchCriteriaParameters.POS_NAME, Operator.EQUAL_TO, ExecutionContext.POSMachineName);
                }
                if (enableOrderShareAccrossPosUsers == false)
                {
                    searchCriteria.And(TransactionSearchCriteriaParameters.USER_ID, Operator.EQUAL_TO, ExecutionContext.UserPKId.ToString());
                }
                if (enableOrderShareAccrossPosCounters == false)
                {
                    POSMachineContainerDTO pOSMachineContainerDTO = POSMachineViewContainerList.GetPOSMachineContainerDTO(ExecutionContext);
                    int posTypeId = pOSMachineContainerDTO.POSTypeId;
                    searchCriteria.And(TransactionSearchCriteriaParameters.POS_TYPE_ID, Operator.EQUAL_TO, posTypeId.ToString());
                }
                DateTime fromDateTime;
                DateTime toDateTime;
                if (!string.IsNullOrEmpty(fromDate))
                {
                    fromDateTime = DateTime.Parse(fromDate);
                    searchCriteria.And(TransactionSearchCriteriaParameters.TRANSACTION_DATE, Operator.GREATER_THAN_OR_EQUAL_TO, fromDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                }
            
                if (!string.IsNullOrEmpty(toDate))
                {
                    toDateTime = DateTime.Parse(toDate);
                    searchCriteria.And(TransactionSearchCriteriaParameters.TRANSACTION_DATE, Operator.LESSER_THAN_OR_EQUAL_TO, toDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                }
                searchCriteria.And(TransactionSearchCriteriaParameters.STATUS, Operator.EQUAL_TO, Transaction.Transaction.TrxStatus.CLOSED.ToString());
                if (advancedFiltersSummaryDTOList!=null && advancedFiltersSummaryDTOList.Count > 0)
                {
                    foreach (AdvancedFiltersSummaryDTO advancedFiltersSummaryDTO in advancedFiltersSummaryDTOList)
                    {
                        Operator condition = operatorDictionary.FirstOrDefault(x => x.Value == advancedFiltersSummaryDTO.Condition.Trim()).Key; 
                        switch (advancedFiltersSummaryDTO.EnabledAttributeName.Trim())
                        {
                            case "Attribute1":
                                {
                                    searchCriteria.And(TransactionSearchCriteriaParameters.TRX_PAYMENT_ATTRIBUTE1, condition, advancedFiltersSummaryDTO.Value.ToString());
                                }
                                break;
                            case "Attribute2":
                                {
                                    searchCriteria.And(TransactionSearchCriteriaParameters.TRX_PAYMENT_ATTRIBUTE2, condition, advancedFiltersSummaryDTO.Value.ToString());
                                }
                                break;
                            case "Attribute3":
                                {
                                    searchCriteria.And(TransactionSearchCriteriaParameters.TRX_PAYMENT_ATTRIBUTE3, condition, advancedFiltersSummaryDTO.Value.ToString());
                                }
                                break;
                            case "Attribute4":
                                {
                                    searchCriteria.And(TransactionSearchCriteriaParameters.TRX_PAYMENT_ATTRIBUTE4, condition, advancedFiltersSummaryDTO.Value.ToString());
                                }
                                break;
                            case "Attribute5":
                                {
                                    searchCriteria.And(TransactionSearchCriteriaParameters.TRX_PAYMENT_ATTRIBUTE5, condition, advancedFiltersSummaryDTO.Value.ToString());
                                }
                                break;
                        }
                    }
                }
                try
                {
                    using (NoSynchronizationContextScope.Enter())
                    {
                        //Task<List<TransactionDTO>> task = transactionUseCases.GetTransactionDTOList(searchParameters, utilities, null, 0, 1000, true, false, false);
                        //task.Wait();
                        //transactions = task.Result;
                        Task<List<TransactionDTO>> task = transactionUseCases.GetTransactionDTOList(searchCriteria, true, false, false);
                        task.Wait();
                        transactions = task.Result;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
                }
                
                if (transactions==null)
                {
                    transactions = new List<TransactionDTO>();
                }
                List<TransactionPaymentsDTO> transactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
                double sum = 0;
                UnmatchedBalance = 0;
                try
                {
                    foreach (TransactionDTO transactionDTO in transactions.OrderBy(x => x.TransactionDate))
                    {
                        TransactionPaymentSummaryDTO transactionPaymentSummaryDTO = null;
                        if (transactionDTO.TrxPaymentsDTOList == null || transactionDTO.TrxPaymentsDTOList.Any() == false)
                        {
                            continue;
                        }
                        List<TransactionPaymentsDTO> trxPaymentsDTOList = transactionDTO.TrxPaymentsDTOList.Where(x => x.PaymentModeId == selectedPaymentMode.PaymentModeId).ToList();
                        double totalAmount = (transactionDTO.TrxPaymentsDTOList != null) ? transactionDTO.TrxPaymentsDTOList.Sum(x => x.Amount) : 0.0;
                        foreach (TransactionPaymentsDTO transactionPaymentsDTO in trxPaymentsDTOList)
                        {
                            if (transactionPaymentsDTO.ParentPaymentId > -1 || trxPaymentsDTOList.Exists(x=>x.ParentPaymentId == transactionPaymentsDTO.PaymentId))
                            {
                                continue;
                            }

                            if (AmountToSettle != 0)
                            {

                                if (transactionPaymentsDTO.Amount + sum <= amountToSettle)
                                {
                                    transactionPaymentSummaryDTO = new TransactionPaymentSummaryDTO(transactionDTO.TransactionId, transactionDTO.TransactionNumber, transactionDTO.TransactionDate, transactionDTO.CustomerName, transactionPaymentsDTO.paymentModeDTO.PaymentMode,
                                    transactionPaymentsDTO.Amount, transactionPaymentsDTO.TipAmount, transactionPaymentsDTO.Reference, true, transactionPaymentsDTO.CreditCardNumber, totalAmount, transactionPaymentsDTO.PaymentId, false);
                                }
                                else
                                {
                                    break;
                                }
                                sum = sum + transactionPaymentsDTO.Amount;
                                UnmatchedBalance = AmountToSettle - sum;
                            }
                            else
                            {
                                transactionPaymentSummaryDTO = new TransactionPaymentSummaryDTO(transactionDTO.TransactionId, transactionDTO.TransactionNumber, transactionDTO.TransactionDate, transactionDTO.CustomerName, transactionPaymentsDTO.paymentModeDTO.PaymentMode,
                                transactionPaymentsDTO.Amount, transactionPaymentsDTO.TipAmount, transactionPaymentsDTO.Reference, true, transactionPaymentsDTO.CreditCardNumber, totalAmount, transactionPaymentsDTO.PaymentId, false);
                            }
                            if (transactionPaymentSummaryDTO != null)
                            {
                                if (ValidatePaymentAttributes(transactionPaymentsDTO))
                                {
                                    transactionPaymentSummaryDTOCollection.Add(transactionPaymentSummaryDTO);
                                    transactionPaymentDictionary.Add(transactionPaymentsDTO.PaymentId, transactionPaymentsDTO);
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
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4592), MessageType.Info);
            }
            advancedFiltersSummaryDTOList.Clear();
            IsLoadingVisible = false;
            log.LogMethodExit();
        }

        private bool ValidatePaymentAttributes(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            bool result = true;
            if (advancedFiltersSummaryDTOList != null && advancedFiltersSummaryDTOList.Any())
            {
                PaymentModesContainerDTO paymentModesContainerDTO = PaymentModesViewContainerList.GetPaymentModesContainerDTO(ExecutionContext, transactionPaymentsDTO.PaymentModeId);
                List<TableAttributeDetailsDTO> tableAttributeDetailsDTOList = TableAttributesUIHelper.GetEnabledAttributes(ExecutionContext, EnabledAttributesDTO.TableWithEnabledAttributes.PaymentMode, paymentModesContainerDTO.Guid);

                foreach (AdvancedFiltersSummaryDTO advancedFiltersSummaryDTO in advancedFiltersSummaryDTOList)
                {
                    Operator condition = operatorDictionary.FirstOrDefault(x => x.Value == advancedFiltersSummaryDTO.Condition.Trim()).Key;
                    TableAttributeDetailsDTO tableAttributeDetailsDTO = tableAttributeDetailsDTOList.FirstOrDefault(x => x.EnabledAttributeName.ToLower() == advancedFiltersSummaryDTO.EnabledAttributeName.ToLower());
                    if (tableAttributeDetailsDTO != null)
                    {
                        string attribute = string.Empty;
                        switch (advancedFiltersSummaryDTO.EnabledAttributeName.Trim())
                        {
                            case "Attribute1":
                                {
                                    attribute = transactionPaymentsDTO.Attribute1;
                                }
                                break;
                            case "Attribute2":
                                {
                                    attribute = transactionPaymentsDTO.Attribute2;
                                }
                                break;
                            case "Attribute3":
                                {
                                    attribute = transactionPaymentsDTO.Attribute3;
                                }
                                break;
                            case "Attribute4":
                                {
                                    attribute = transactionPaymentsDTO.Attribute4;
                                }
                                break;
                            case "Attribute5":
                                {
                                    attribute = transactionPaymentsDTO.Attribute5;
                                }
                                break;
                        }
                        result = ValidateAttributeValue(condition, tableAttributeDetailsDTO.DataType, attribute, advancedFiltersSummaryDTO.Value);
                    }
                   
                }
            }
            return result;
        }


        private bool ValidateAttributeValue(Operator opvalue, TableAttributeSetupDTO.DataTypeEnum dataTypeEnum, string paymentAttribute, string compareToValue)
        {
            bool result = false;
            DateTime paymentAttributeDate = DateTime.MinValue;
            DateTime valueDate = DateTime.MinValue;
         
            int? dateCompareValue=null;

            decimal paymentAttrNumericValue=0;
            decimal compareToNumericValue=0;

            switch(dataTypeEnum)
            {
                case TableAttributeSetupDTO.DataTypeEnum.DATETIME:
                    {
                        if (DateTime.TryParse(paymentAttribute, out paymentAttributeDate)
                                && DateTime.TryParse(compareToValue, out valueDate))
                        {
                            dateCompareValue = paymentAttributeDate.CompareTo(valueDate);
                        }
                    }
                    break;
            }
           
            switch (opvalue)
            {
                case Operator.EQUAL_TO:
                    {
                        if (dataTypeEnum == TableAttributeSetupDTO.DataTypeEnum.DATETIME)
                        {
                            if (dateCompareValue != null && dateCompareValue == 0)
                            {
                                result = true;
                            }

                        }
                        else
                        {
                            result = paymentAttribute.Equals(compareToValue);
                        }
                    }
                    break;
                case Operator.NOT_EQUAL_TO:
                    {
                        if (dataTypeEnum == TableAttributeSetupDTO.DataTypeEnum.DATETIME)
                        {
                            if (dateCompareValue != null && dateCompareValue != 0)
                            {
                                result = true;
                            }

                        }
                        else
                        {
                            result = !paymentAttribute.Equals(compareToValue);
                        }
                       
                    }
                    break;
                case Operator.LESSER_THAN:
                    {
                        if (dataTypeEnum == TableAttributeSetupDTO.DataTypeEnum.DATETIME)
                        {
                            if (dateCompareValue!= null && dateCompareValue < 0)
                            {
                                result = true;
                            }
                            
                        }
                        else if(dataTypeEnum == TableAttributeSetupDTO.DataTypeEnum.NUMBER)
                        {

                            if (Decimal.TryParse(paymentAttribute, out paymentAttrNumericValue)
                                && Decimal.TryParse(compareToValue, out compareToNumericValue))
                            {
                                if (paymentAttrNumericValue < compareToNumericValue)
                                {
                                    result = true;
                                }
                                
                            }
                        }
                    }
                    break;
                case Operator.GREATER_THAN:
                    {
                        if (dataTypeEnum == TableAttributeSetupDTO.DataTypeEnum.DATETIME)
                        {
                            if (dateCompareValue != null && dateCompareValue > 0)
                            {
                                result = true;
                            }

                        }
                        else if (dataTypeEnum == TableAttributeSetupDTO.DataTypeEnum.NUMBER)
                        {

                            if (Decimal.TryParse(paymentAttribute, out paymentAttrNumericValue)
                                && Decimal.TryParse(compareToValue, out compareToNumericValue))
                            {
                                if (paymentAttrNumericValue > compareToNumericValue)
                                {
                                    result = true;
                                }

                            }
                        }
                    }
                    break;
                case Operator.LESSER_THAN_OR_EQUAL_TO:
                    {
                        if (dataTypeEnum == TableAttributeSetupDTO.DataTypeEnum.DATETIME)
                        {
                            if (dateCompareValue != null && dateCompareValue <= 0)
                            {
                                result = true;
                            }
                        }
                        else if (dataTypeEnum == TableAttributeSetupDTO.DataTypeEnum.NUMBER)
                        {

                            if (Decimal.TryParse(paymentAttribute, out paymentAttrNumericValue)
                                && Decimal.TryParse(compareToValue, out compareToNumericValue))
                            {
                                if (paymentAttrNumericValue <= compareToNumericValue)
                                {
                                    result = true;
                                }

                            }
                        }
                    }
                    break;
                case Operator.GREATER_THAN_OR_EQUAL_TO:
                    {
                        if (dataTypeEnum == TableAttributeSetupDTO.DataTypeEnum.DATETIME)
                        {
                            if (dateCompareValue != null && dateCompareValue >= 0)
                            {
                                result = true;
                            }

                        }
                        else if (dataTypeEnum == TableAttributeSetupDTO.DataTypeEnum.NUMBER)
                        {
                            if (Decimal.TryParse(paymentAttribute, out paymentAttrNumericValue)
                                && Decimal.TryParse(compareToValue, out compareToNumericValue))
                            {
                                if (paymentAttrNumericValue >= compareToNumericValue)
                                {
                                    result = true;
                                }

                            }
                        }
                    }
                    break;
                case Operator.LIKE:
                    {
                        if (Like(paymentAttribute, compareToValue))
                        {
                            result = true;
                        }
                    }
                    break;
                case Operator.NOT_LIKE:
                    {
                        if (!Like(paymentAttribute, compareToValue))
                        {
                            result = true;
                        }
                    }
                    break;

            }
            return result;
        }
        private bool Like(string toSearch, string toFind)
        {
            return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
        }

        private void OpenGenericMessagePopupView(string heading, string subHeading, string content,
            string okButtonText, string cancelButtonText, MessageButtonsType messageButtonsType)
        {
            log.LogMethodEntry(heading, subHeading, content, okButtonText, cancelButtonText, messageButtonsType);
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            messagePopupView.Loaded += OnWindowLoaded;
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
            if (genericMessagePopupVM.ButtonClickType == ButtonClickType.Cancel)
            {

            }
            log.LogMethodExit();
        }
        private void OnWindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Window window = sender as Window;
            if (editPaymentsView != null)
            {
                window.Owner = this.editPaymentsView;
                window.Width = this.editPaymentsView.Width;
                window.Height = this.editPaymentsView.Height;
                window.Top = this.editPaymentsView.Top;
                window.Left = this.editPaymentsView.Left;
            }
            log.LogMethodExit();
        }

        private void PerformAdvancedFilters()
        {
            log.LogMethodEntry();
            if (selectedPaymentMode==null)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4592), MessageType.Info);
                return;
            }
            AdvancedFilterView advancedFilterView = new AdvancedFilterView();
            AdvancedFilterVM advancedFilterVM = new AdvancedFilterVM(ExecutionContext, EnabledAttributesDTO.TableWithEnabledAttributes.PaymentMode, selectedPaymentMode.Guid);
            advancedFilterView.DataContext = advancedFilterVM;
            if (editPaymentsView != null)
            {
                advancedFilterView.Owner = editPaymentsView;
            }
            advancedFilterView.ShowDialog();
            if(advancedFilterVM.ButtonClick == AdvancedFilterVM.ButtonClickType.Ok)
            {
                //advancedFiltersSummaryDTOList.Clear();
                if (advancedFilterVM.AdvancedFiltersSummaryDTOCollection!=null && advancedFilterVM.AdvancedFiltersSummaryDTOCollection.Any())
                {
                    advancedFiltersSummaryDTOList = advancedFilterVM.AdvancedFiltersSummaryDTOCollection.ToList();
                }
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4591), MessageType.Info);
            }
            log.LogMethodExit();
        }
        private void GetCompatiblePaymentModes()
        {
            log.LogMethodEntry();
            CompatiblePaymentModes.Clear();
            try
            {
                if(selectedPaymentMode == null)
                {
                    return;
                }
                PaymentModesContainerDTO paymentModesContainerDTO = PaymentModesViewContainerList.GetPaymentModesContainerDTO(ExecutionContext, selectedPaymentMode.PaymentModeId);
                if (paymentModesContainerDTO != null)
                {
                    List<PaymentModesContainerDTO> compatibleaymentModesContainerDTOList = new List<PaymentModesContainerDTO>();
                    if (paymentModesContainerDTO.CompatiablePaymentModesContainerDTOList==null)
                    {
                        return;
                    }
                    foreach(CompatiablePaymentModesContainerDTO compatiablePaymentModesContainerDTO in paymentModesContainerDTO.CompatiablePaymentModesContainerDTOList)
                    {
                        PaymentModesContainerDTO paymentModesContainerDTO1 = PaymentModesViewContainerList.GetPaymentModesContainerDTO(ExecutionContext, compatiablePaymentModesContainerDTO.CompatiablePaymentModeId);
                        if(paymentModesContainerDTO1.PaymentModeId!= selectedPaymentMode.PaymentModeId && !compatibleaymentModesContainerDTOList.Exists(x=>x.PaymentModeId== paymentModesContainerDTO1.PaymentModeId))
                        {
                            compatibleaymentModesContainerDTOList.Add(paymentModesContainerDTO1);
                        }
                       
                        PaymentModesContainerDTO paymentModesContainerDTO2 = PaymentModesViewContainerList.GetPaymentModesContainerDTO(ExecutionContext, compatiablePaymentModesContainerDTO.PaymentModeId);
                        if (paymentModesContainerDTO2.PaymentModeId != selectedPaymentMode.PaymentModeId && !compatibleaymentModesContainerDTOList.Exists(x => x.PaymentModeId == paymentModesContainerDTO2.PaymentModeId))
                        {
                            compatibleaymentModesContainerDTOList.Add(paymentModesContainerDTO2);
                        }
                    }
                    SelectedCompatiblePaymentMode = null;
                  
                    CompatiblePaymentModes = new ObservableCollection<PaymentModesContainerDTO>(compatibleaymentModesContainerDTOList);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
            }
            log.LogMethodExit();
        }
        private void GetPaymentModes()
        {
            log.LogMethodEntry();
            try
            {

                List<PaymentModesContainerDTO> paymentModeContainerDTOList = PaymentModesViewContainerList.GetPaymentModesContainerDTOList(ExecutionContext);
                if (paymentModeContainerDTOList != null)
                {
                    PaymentModes = new ObservableCollection<PaymentModesContainerDTO>(paymentModeContainerDTOList);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
            }
            log.LogMethodExit();
        }
        private void SetTransactionPaymentsCustomDataGridVM()
        {
            log.LogMethodEntry();
            TransactionPaymentsCustomDataGridVM = new CustomDataGridVM(ExecutionContext)
            {
                IsComboAndSearchVisible = false,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            if(transactionPaymentSummaryDTOCollection==null)
            {
                transactionPaymentSummaryDTOCollection = new ObservableCollection<TransactionPaymentSummaryDTO>();
            }
            TransactionPaymentsCustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(transactionPaymentSummaryDTOCollection);
            TransactionPaymentsCustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
            {
                 { "TransactionId",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Id"),
                    }
                },
                { "TransactionDate",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Date"),
                    }
                },
                { "PaymentMode",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Payment Mode"),
                    }
                },
                { "PaymentAmount",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Amount"),
                    }
                },
                { "TipAmount",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Tip"),
                    }
                }
            };
            log.LogMethodExit();
        }
        private void SetInitialValues()
        {
            log.LogMethodEntry();
            ModuleName = MessageViewContainerList.GetMessage(ExecutionContext, "Edit Payments");
            CompatiblePaymentModes = new ObservableCollection<PaymentModesContainerDTO>();
            transactionPaymentsDTOCollection = new ObservableCollection<TransactionPaymentsDTO>();
            transactionPaymentDictionary = new Dictionary<int, TransactionPaymentsDTO>();
            PaymentModes = new ObservableCollection<PaymentModesContainerDTO>();
            GetPaymentModes();
            SetTransactionPaymentsCustomDataGridVM();

            enableOrderShareAccrossPos = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ENABLE_ORDER_SHARE_ACROSS_POS");
            enableOrderShareAccrossPosUsers = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ENABLE_ORDER_SHARE_ACROSS_USERS");
            enableOrderShareAccrossPosCounters = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS");
           
            AmountToSettle = 0;
            UnmatchedBalance = 0;
            ShowSearchArea = true;
            
            dateTimeFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT");
            double businessStart = Convert.ToDouble(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "BUSINESS_DAY_START_TIME"));
            FromDate = DateTime.Today.AddHours(businessStart).ToString(dateTimeFormat);
            ToDate = DateTime.Today.AddDays(1).AddHours(businessStart).ToString(dateTimeFormat);
            advancedFiltersSummaryDTOList = new List<AdvancedFiltersSummaryDTO>();
            InitalizeOperatorDictionary();
            log.LogMethodExit();
        }
        private void EnableDisableControls(bool val)
        {
            log.LogMethodEntry(val);

            //Clear the texbox
            Authorization = string.Empty;
            //txtCardExpiry.Text = string.Empty;
            CardNumber = string.Empty;
            CCName = string.Empty;
            NameOnCC = string.Empty;
            PaymentReference = string.Empty;

            //Update controls
            AuthorizarionEnabled = val;
            //txtCardExpiry.Enabled = val;
            CardNumberEnabled = val;
            CCNameEnabled = val;
            NameonCCEnabled = val;

            log.LogMethodExit(null);
        }
        private string GetMessage(string message = "", int messageNo = -1)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return messageNo > -1 ? MessageViewContainerList.GetMessage(ExecutionContext, messageNo) : MessageViewContainerList.GetMessage(ExecutionContext, message);
        }
        private void InitalizeOperatorDictionary()
        {
            log.LogMethodEntry();
            operatorDictionary = new Dictionary<Operator, string>();
            foreach (Operator condition in Enum.GetValues(typeof(Operator)))
            {
                IOperator @operator = OperatorFactory.GetOperator(condition);
                string displayName = GetMessage(@operator.DisplayName);
                operatorDictionary.Add(condition, displayName);
            }
            log.LogMethodExit();
        }
        #endregion
        #region Constructor
        public EditPaymentsVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.ExecutionContext = executionContext;
            SetInitialValues();
            InitializeCommands();

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

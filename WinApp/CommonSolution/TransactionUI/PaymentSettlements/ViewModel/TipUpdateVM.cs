/********************************************************************************************
 * Project Name - Tip Update UI
 * Description  - Tip Update VM
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     28-Sep-2021    Dakshak                  Created 
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
using Semnox.Parafait.Device;

namespace Semnox.Parafait.TransactionUI
{
    public class TipUpdateVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string moduleName;
        private string dateTimeFormat;
        private TipUpdateView tipUpdatetView;
        private ICommand navigationClickCommand;
        private ICommand loadedCommand;
        private ICommand actionsCommand;
        private CustomDataGridVM customDataGridVM;
        private ObservableCollection<TransactionPaymentSummaryDTO> transactionPaymentSummaryDTOCollection;
        private List<TransactionPaymentsDTO> transactionPaymentsDTOList;
        string limit;
        long tipLimit;
        private ButtonClickType buttonClickType;

        #endregion
        #region Properties
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
        public ButtonClickType ButtonClickType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonClickType);
                return buttonClickType;
            }
            set
            {
                log.LogMethodEntry(buttonClickType, value);
                SetProperty(ref buttonClickType, value);
                log.LogMethodExit(buttonClickType);
            }
        }
        #endregion

        #region methods
        private void OnNavigationClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (tipUpdatetView != null)
            {
                tipUpdatetView.Close();
            }
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                tipUpdatetView = parameter as TipUpdateView;
            }
            log.LogMethodExit();
        }
        private void OnActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                Button button = parameter as Button;
                if (button != null && !string.IsNullOrEmpty(button.Name))
                {
                    switch (button.Name)
                    {
                        case "btnComplete":
                            {
                                PerformComplete();
                            }
                            break;
                        case "btnCancel":
                            {
                                PerformCancel();
                            }
                            break;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void InitializeCommands()
        {
            log.LogMethodEntry();
            TransactionPaymentSummaryDTOCollection = new ObservableCollection<TransactionPaymentSummaryDTO>();
            NavigationClickCommand = new DelegateCommand(OnNavigationClicked);
            LoadedCommand = new DelegateCommand(OnLoaded);
            ActionsCommand = new DelegateCommand(OnActionsClicked);
            GeTransactionPaymentSummaryDTOList();
            log.LogMethodExit();
        }

        private void PerformComplete()
        {
            if (TransactionPaymentSummaryDTOCollection != null && TransactionPaymentSummaryDTOCollection.Any())
            {
                foreach (TransactionPaymentSummaryDTO transactionPaymentSummaryDTO in TransactionPaymentSummaryDTOCollection)
                {
                    if (tipLimit > 0 && ((transactionPaymentSummaryDTO.TotalAmount * tipLimit) / 100) < transactionPaymentSummaryDTO.TipAmount)
                    {
                        StringBuilder stringB = new StringBuilder("");
                        stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Total Transaction Amount: ") + transactionPaymentSummaryDTO.TotalAmount.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL") + " "));
                        stringB.Append(System.Environment.NewLine);
                        stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Tip Amount Limit: ") + ((transactionPaymentSummaryDTO.TotalAmount * tipLimit) / 100).ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL") + " "));
                        stringB.Append(System.Environment.NewLine);
                        stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Tip Amount Settled: ") + 0.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL") + " "));
                        stringB.Append(System.Environment.NewLine);
                        stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Balance Tip Amount Applicable: ") + (((transactionPaymentSummaryDTO.TotalAmount * tipLimit) / 100)).ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL") + " "));
                        stringB.Append(System.Environment.NewLine);
                        stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Tip amount is higher than the tip limit for this transaction. Please enter lower tip amount."));
                        OpenGenericMessagePopupView("Tip limit validation", null, stringB.ToString(), null, "OK", MessageButtonsType.OK);
                        return;
                    }
                }
            }
            if (tipUpdatetView != null)
            {
                buttonClickType = ButtonClickType.Ok;
                tipUpdatetView.Close();
            }
            //foreach (TransactionPaymentsDTO transactionPaymentsDTO in transactionPaymentsDTOList)
            //{
            //    transactionPaymentsDTO.TipAmount = TransactionPaymentSummaryDTOCollection.FirstOrDefault(x => x.PaymentId == transactionPaymentsDTO.PaymentId).TipAmount;
            //}
            //ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
            //using (NoSynchronizationContextScope.Enter())
            //{
            //    try
            //    {
            //        Task<List<KeyValuePair<TransactionPaymentsDTO, string>>> task = transactionUseCases.SettleTransactionPayments(transactionPaymentsDTOList);
            //        task.Wait();
            //        List<KeyValuePair<TransactionPaymentsDTO, string>> keyValuePairs = task.Result;
            //        if (keyValuePairs != null && keyValuePairs.Any())
            //        {
            //            string successMessage = MessageViewContainerList.GetMessage(ExecutionContext, 4027);
            //            //Order/Orders have been Accepted
            //            ShowErrorMessages(keyValuePairs, successMessage);
            //        }
            //    }
            //    catch (ValidationException vex)
            //    {
            //        log.Error(vex);
            //        SetFooterContent(vex.ToString(), MessageType.Error);
            //    }
            //    catch (UnauthorizedException uaex)
            //    {
            //        log.Error(uaex);
            //        SetFooterContent(uaex.ToString(), MessageType.Error); ;
            //    }
            //    catch (ParafaitApplicationException pax)
            //    {
            //        log.Error(pax);
            //        SetFooterContent(pax.ToString(), MessageType.Error);
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Error(ex);
            //        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again.") + " " + ex.Message, MessageType.Error);
            //    }
            //   }
        }

        private void ShowErrorMessages(List<KeyValuePair<TransactionPaymentsDTO, string>> keyValuePairs, string successMsg)
        {
            log.LogMethodEntry(keyValuePairs, successMsg);
            List<KeyValuePair<TransactionPaymentsDTO, string>> errorRecordList = keyValuePairs.Where(key => key.Value != null).ToList();
            if (errorRecordList != null && errorRecordList.Any())
            {
                StringBuilder stringB = new StringBuilder("");
                for (int i = 0; i < errorRecordList.Count; i++)
                {
                    stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Trxn Id: ") + errorRecordList[i].Key.TransactionId + " ");
                    stringB.Append(System.Environment.NewLine);
                    stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Payment Id: ") + errorRecordList[i].Key.PaymentId + " ");
                    stringB.Append(System.Environment.NewLine);
                    stringB.Append(MessageViewContainerList.GetMessage(ExecutionContext, "Message: " + errorRecordList[i].Value));
                }
                OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(ExecutionContext, "Message"), string.Empty,
                stringB.ToString(), MessageViewContainerList.GetMessage(ExecutionContext, "OK"), MessageViewContainerList.GetMessage(ExecutionContext, "OK"),
                MessageButtonsType.OK);
            }
            else
            {
                OpenGenericMessagePopupView(MessageViewContainerList.GetMessage(ExecutionContext, "Message"), string.Empty,
                successMsg, MessageViewContainerList.GetMessage(ExecutionContext, "OK"), MessageViewContainerList.GetMessage(ExecutionContext, "OK"),
                MessageButtonsType.OK);
                if (tipUpdatetView != null)
                {
                    tipUpdatetView.Close();
                }
            }
            log.LogMethodExit();
        }
        private void PerformCancel()
        {
            log.LogMethodEntry();
            if (tipUpdatetView != null)
            {
                tipUpdatetView.Close();
            }
            log.LogMethodExit();

        }


        private void SetInitialtValues()
        {
            log.LogMethodEntry();
            TransactionPaymentSummaryDTOCollection = new ObservableCollection<TransactionPaymentSummaryDTO>();
            dateTimeFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT");
            limit = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "MAX_TIP_AMOUNT_PERCENTAGE");
            tipLimit = Convert.ToInt64(string.IsNullOrEmpty(limit) ? "200" : limit);
            ObservableCollection<object> transactionPaymentSummaryDTOList = new ObservableCollection<object>();
            //GeTransactionPaymentSummaryDTOList();
            log.LogMethodExit();
        }
        private void SetTransactionPaymentsCustomDataGridVM()
        {
            log.LogMethodEntry();
            TransactionPaymentsCustomDataGridVM = new CustomDataGridVM(ExecutionContext)
            {
                IsComboAndSearchVisible = false,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            };
            TransactionPaymentsCustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(transactionPaymentSummaryDTOCollection);
            TransactionPaymentsCustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
            {

                { "TransactionId",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Id")
                    }
                },
                { "TransactionDate",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Date"),
                        DataGridColumnStringFormat = dateTimeFormat
                    }
                },
                { "PaymentMode",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Payment Mode")
                    }
                },
                { "CardNumber",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Card Number")
                    }
                },
                { "PaymentAmount",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Amount")
                    }
                },
                { "TipAmount",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Entered Tip"),
                        Type = DataEntryType.TextBox
                    }
                },
                { "TotalAmount",
                    new CustomDataGridColumnElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Total Amount")
                    }
                }
            };
            log.LogMethodExit();
        }
        private void GeTransactionPaymentSummaryDTOList()
        {
            transactionPaymentSummaryDTOCollection = new ObservableCollection<TransactionPaymentSummaryDTO>();

            //TransactionPaymentSummaryDTO transactionPaymentSummaryDTO = new TransactionPaymentSummaryDTO(1, DateTime.Now, "Cash", "11223322", 1000, 20, 1020);
            //    transactionPaymentSummaryDTOCollection.Add(new TransactionPaymentSummaryDTO(1, "11111", DateTime.Now, "Suresh", "Credit", 1000, 20, "pr1", true, "1100229091100", 1000));
            //    transactionPaymentSummaryDTOCollection.Add(new TransactionPaymentSummaryDTO(2, "22222", DateTime.Now, "Mahesh", "Credit", 1000, 20, "pr1", true, "1100229091100", 1200));
            //    transactionPaymentSummaryDTOCollection.Add(new TransactionPaymentSummaryDTO(3, "33333", DateTime.Now, "Mahesh", "Credit", 1000, 20, "pr1", true, "1100229091100", 1250));
            //    transactionPaymentSummaryDTOCollection.Add(new TransactionPaymentSummaryDTO(3, "33333", DateTime.Now, "Mahesh", "Credit", 1000, 20, "pr1", true, "1100229091100", 1222));
        }
        #endregion

        #region Constructor
        public TipUpdateVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.ExecutionContext = executionContext;
            ModuleName = MessageViewContainerList.GetMessage(ExecutionContext, "Tip Update");

            InitializeCommands();
            SetInitialtValues();
            SetTransactionPaymentsCustomDataGridVM();

            FooterVM = new FooterVM(executionContext)
            {
                MessageType = MessageType.None,
                Message = string.Empty,
                HideSideBarVisibility = Visibility.Collapsed
            };
            log.LogMethodExit();
        }


        public TipUpdateVM(ExecutionContext executionContext, List<TransactionPaymentSummaryDTO> TransactionPaymentSummaryDTOList)
        {
            log.LogMethodEntry(executionContext);
            this.ExecutionContext = executionContext;
            this.transactionPaymentsDTOList = transactionPaymentsDTOList;
            buttonClickType = ButtonClickType.Cancel;
            ModuleName = MessageViewContainerList.GetMessage(ExecutionContext, "Tip Update");
            InitializeCommands();
            SetInitialtValues();
            TransactionPaymentSummaryDTOCollection = new ObservableCollection<TransactionPaymentSummaryDTO>(TransactionPaymentSummaryDTOList);
            SetTransactionPaymentsCustomDataGridVM();
            FooterVM = new FooterVM(executionContext)
            {
                MessageType = MessageType.None,
                Message = string.Empty,
                HideSideBarVisibility = Visibility.Collapsed
            };
            log.LogMethodExit();
        }

        private void OpenGenericMessagePopupView(string heading, string subHeading, string content,
            string okButtonText, string cancelButtonText, MessageButtonsType messageButtonsType)
        {
            log.LogMethodEntry(heading, subHeading, content, okButtonText, cancelButtonText, messageButtonsType);
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            if (tipUpdatetView != null)
            {
                messagePopupView.Owner = this.tipUpdatetView;
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
            messagePopupView.Loaded += OnWindowLoaded;
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
            if (tipUpdatetView != null)
            {
                window.Owner = this.tipUpdatetView;
                window.Width = this.tipUpdatetView.Width;
                window.Height = this.tipUpdatetView.Height;
                window.Top = this.tipUpdatetView.Top;
                window.Left = this.tipUpdatetView.Left;
            }
            log.LogMethodExit();
        }

        #endregion
    }
}

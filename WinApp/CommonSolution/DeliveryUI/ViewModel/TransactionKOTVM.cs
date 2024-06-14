/********************************************************************************************
 * Project Name - Delivery UI
 * Description  - TransactionKOT VM
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     22-Jun-2021    Fiona                  Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.POS;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.KDS;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.DeliveryUI
{
    public class TransactionKOTVM : BaseWindowViewModel
    {
        #region members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomDataGridVM transactionKOTCustomDataGridVM;
        private string moduleName;
        private TransactionKOTView transactionKOTView;
        private bool amendTimeEnable;

        private ICommand navigationClickCommand;
        private ICommand loaded;
        private ICommand amendTimeCommand;
        private ICommand reprintKOTCommand;
        private ICommand selectionChangedCommand;

        private TransactionLineDTO selectedDTO;
        private ObservableCollection<TransactionLineDTO> transactionLineCollection;
        private TransactionDTO transactionDTO;
        private string headerRemarks = string.Empty;
        //private GenericDataEntryVM genericDataEntryVM;

        #endregion
        #region properties

        public CustomDataGridVM TransactionKOTCustomDataGridVM
        {
            get
            {
                return transactionKOTCustomDataGridVM;
            }
            set
            {
                if (!object.Equals(transactionKOTCustomDataGridVM, value))
                {
                    transactionKOTCustomDataGridVM = value;
                    OnPropertyChanged();
                }

            }
        }

        public ICommand Loaded
        {
            set
            {
                if (!object.Equals(loaded, value))
                {
                    loaded = value;
                    OnPropertyChanged();
                }
            }
            get
            {
                return loaded;
            }
        }
        public ICommand AmendTimeCommand
        {
            get
            {
                return amendTimeCommand;
            }
            set
            {
                if (!object.Equals(amendTimeCommand, value))
                {
                    amendTimeCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand ReprintKOTCommand
        {
            get
            {
                return reprintKOTCommand;
            }
            set
            {
                if (!object.Equals(reprintKOTCommand, value))
                {
                    reprintKOTCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand SelectionChangedCommand
        {
            get
            {
                return selectionChangedCommand;
            }
            set
            {
                if (!object.Equals(selectionChangedCommand, value))
                {
                    selectionChangedCommand = value;
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
        public bool AmendTimebtnEnableProperty
        {
            get
            {
                return amendTimeEnable;
            }
            set
            {
                if (!object.Equals(amendTimeEnable, value))
                {
                    amendTimeEnable = value;
                    OnPropertyChanged();
                }
            }
        }
        //public GenericDataEntryVM GenericDataEntryVM
        //{
        //    get
        //    {
        //        log.LogMethodEntry();
        //        log.LogMethodExit(genericDataEntryVM);
        //        return genericDataEntryVM;
        //    }
        //    set
        //    {
        //        log.LogMethodEntry(genericDataEntryVM, value);
        //        SetProperty(ref genericDataEntryVM, value);
        //        log.LogMethodExit(genericDataEntryVM);
        //    }
        //}
        #endregion
        #region methods

        private void OnNavigationClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (transactionKOTView != null)
            {
                PerformClose(transactionKOTView);
            }
            log.LogMethodExit();
        }
        private void PerformClose(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                Window window = parameter as Window;
                if (window != null)
                {
                    window.Close();
                }
            }
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry();
            if (parameter != null)
            {
                transactionKOTView = parameter as TransactionKOTView;
                if (transactionKOTCustomDataGridVM.CollectionToBeRendered == null || 
                    transactionKOTCustomDataGridVM.CollectionToBeRendered.Any() == false)
                {
                    SetGenericRightSectionContentVM();
                }
            }
            log.LogMethodExit();
        }
        private void SetTransactionKOTCustomDataGridVM()
        {
            log.LogMethodEntry();
            TransactionKOTCustomDataGridVM = new CustomDataGridVM(ExecutionContext)
            {
                IsComboAndSearchVisible = false,
                CollectionToBeRendered = new ObservableCollection<object>(transactionLineCollection),
                SelectOption = SelectOption.None
            };
            TransactionKOTCustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                {

                     {"ProductName",
                        new CustomDataGridColumnElement()
                        {
                            Type=DataEntryType.TextBlock,
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Products to print KOT")

                        }
                    },
                    {"Quantity",
                        new CustomDataGridColumnElement()
                        {
                            Type=DataEntryType.TextBlock,
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Quantity")

                        }
                    },
                    {"KDSOrderLineDTOList",
                        new CustomDataGridColumnElement()
                        {
                            Type=DataEntryType.TextBlock,
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Schedule Time"),
                            Converter = new CalculateMaximumScheduledTimeConverter(),
                            ConverterParameter = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT")
                        }
                    },
                    {"LineId",
                        new CustomDataGridColumnElement()
                        {
                            Type=DataEntryType.TextBlock,
                            Converter = new StatusConverter(),
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Status"),
                            ConverterParameter = transactionLineCollection.ToList()
                        }
                    },
                    {"KOTPrintCount",
                        new CustomDataGridColumnElement()
                        {
                            Type=DataEntryType.TextBlock,
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Print#"),
                        }
                    }
                };
            log.LogMethodExit();
        }
        private void SetDisplayTagsVM()
        {
            log.LogMethodEntry();
            string dateFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT");
            DisplayTagsVM = new DisplayTagsVM()
            {
                DisplayTags = new ObservableCollection<ObservableCollection<DisplayTag>>()
                    {
                        new ObservableCollection<DisplayTag>()
                                          {
                                              new DisplayTag()
                                              {
                                                  Text = MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Id"),
                                                  TextSize = TextSize.Small,
                                                  FontWeight = FontWeights.Bold
                                              },
                                              new DisplayTag()
                                              {
                                                  Text = transactionDTO.TransactionId.ToString(),
                                                  TextSize = TextSize.Small,
                                                  FontWeight = FontWeights.Bold
                                              }
                                          },
                        new ObservableCollection<DisplayTag>()
                                          {
                                              new DisplayTag()
                                              {
                                                  Text = MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Date"),
                                                  TextSize = TextSize.Small,
                                                  FontWeight = FontWeights.Bold
                                              },
                                              new DisplayTag()
                                              {
                                                  Text = transactionDTO.TransactionDate.ToString(dateFormat),
                                                  TextSize = TextSize.Small,
                                                  FontWeight = FontWeights.Bold
                                              }

                                          },

                    }

            };
            log.LogMethodExit();
        }
        private void OnAmendTimeClick(object obj)
        {
            log.LogMethodEntry();

            if (transactionLineCollection != null && transactionLineCollection.Any())
            {
                foreach (TransactionLineDTO transactionLineDTO in transactionLineCollection)
                {
                    if (transactionLineDTO.KDSOrderLineDTOList.Any(x => x.PreparedTime != null))
                    {
                        log.Error(MessageViewContainerList.GetMessage(ExecutionContext, 4021));
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4021), MessageType.Error);
                        return;
                    }
                    if (transactionLineDTO.KDSOrderLineDTOList.Any(x => x.DeliveredTime != null))
                    {
                        log.Error(MessageViewContainerList.GetMessage(ExecutionContext, 4021));
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4021), MessageType.Error);
                        return;
                    }
                    if (transactionLineDTO.KDSOrderLineDTOList.Any(x => x.PrepareStartTime != null))
                    {
                        log.Error(MessageViewContainerList.GetMessage(ExecutionContext, 4022));
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4022), MessageType.Error);
                        return;
                    }
                    if (transactionLineDTO.KDSOrderLineDTOList.Any(x => x.ScheduleTime == null))
                    {
                        log.Error(MessageViewContainerList.GetMessage(ExecutionContext, 4020));
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4020), MessageType.Error);
                        return;
                    }
                }
                List<string> amendMinutes = new List<string>();
                int lastMin = -360;
                while (lastMin < 360)
                {
                    amendMinutes.Add(lastMin.ToString());
                    lastMin += 5;
                }
                GenericDataEntryView dataEntryView = new GenericDataEntryView();
                string dateFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT");
                DateTime ScheduledTime = (DateTime)transactionDTO.TransctionOrderDispensingDTO.ScheduledDispensingTime;
                GenericDataEntryVM GenericDataEntryVM = new GenericDataEntryVM(ExecutionContext)
                {
                    Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Amend Schedule Time") + Environment.NewLine + ScheduledTime.ToString(dateFormat),
                    IsKeyboardVisible = false,
                    DataEntryCollections = new ObservableCollection<DataEntryElement>()
                    {
                        new DataEntryElement()
                        {
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Immediate?"),
                            Type= DataEntryType.CheckBox
                        },
                        new DataEntryElement()
                        {
                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Amend By Minutes"),
                            Type=DataEntryType.ComboBox,
                            Options=new ObservableCollection<object>(amendMinutes),
                            SelectedItem = amendMinutes[amendMinutes.Count/2],
                            ValidationType=ValidationType.NumberOnly
                        }
                    }
                };
                dataEntryView.Width = SystemParameters.PrimaryScreenWidth;
                dataEntryView.Height = SystemParameters.PrimaryScreenHeight;
                dataEntryView.DataContext = GenericDataEntryVM;
                dataEntryView.Closing += OnDataEntryClosing;
                dataEntryView.ShowDialog();
            }

            log.LogMethodExit();
        }

        private void OnDataEntryClosing(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry();
            GenericDataEntryView genericDataEntryView = sender as GenericDataEntryView;
            GenericDataEntryVM genericDataEntryVM = genericDataEntryView.DataContext as GenericDataEntryVM;
            if (genericDataEntryVM.DataEntryCollections == null)
            {
                return;
            }
            if (genericDataEntryVM.ButtonClickType == ButtonClickType.Ok)
            {

                if (genericDataEntryVM.DataEntryCollections[0].IsChecked == false &&
                                   (genericDataEntryVM.DataEntryCollections[1].SelectedItem == null
                                      || string.IsNullOrEmpty(genericDataEntryVM.DataEntryCollections[1].SelectedItem.ToString())
                                      || genericDataEntryVM.DataEntryCollections[1].SelectedItem.ToString() == "0"))
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Select an option"), MessageType.Info);
                    e.Cancel = true;
                }
                else
                {
                    double timeToAmend;
                    if (genericDataEntryVM.DataEntryCollections[0].IsChecked)
                    {
                        timeToAmend = 0;
                    }
                    else
                    {
                        timeToAmend = Convert.ToDouble(genericDataEntryVM.DataEntryCollections[1].SelectedItem.ToString());
                    }
                    foreach (TransactionLineDTO transactionLineDTO in transactionLineCollection)
                    {
                        if (transactionLineDTO.KDSOrderLineDTOList.Any(kot => kot.EntryType == KDSOrderLineDTO.KDSKOTEntryType.KOT) && transactionLineDTO.KDSOrderLineDTOList.Exists(kds => kds.DeliveredTime != null)) //kDSOrderLineDTO.EntryType == KDSOrderLineDTO.KDSKOTEntryType.KOT && kDSOrderLineDTO.DeliveredTime == null)
                        {
                            string message = MessageViewContainerList.GetMessage(ExecutionContext, 4021);
                            log.Error(message);
                            SetFooterContent(message, MessageType.Error);
                            return;
                        }
                        if (transactionLineDTO.KDSOrderLineDTOList.Any(kds => kds.EntryType == KDSOrderLineDTO.KDSKOTEntryType.KDS) && transactionLineDTO.KDSOrderLineDTOList.Exists(kot => kot.PreparedTime != null))
                        {
                            string message = MessageViewContainerList.GetMessage(ExecutionContext, 4021);
                            log.Error(message);
                            SetFooterContent(message, MessageType.Error);
                            return;
                        }
                        if (transactionLineDTO.KDSOrderLineDTOList.Any(x => x.OrderedTime != null || x.OrderedTime == DateTime.MinValue) || transactionLineDTO.KDSOrderLineDTOList.Any(x => x.PrepareStartTime != null || x.PrepareStartTime == DateTime.MinValue))
                        {
                            string message = MessageViewContainerList.GetMessage(ExecutionContext, 4022);
                            log.Error(message);
                            SetFooterContent(message, MessageType.Error);
                            return;
                        }
                        if (transactionLineDTO.KDSOrderLineDTOList.FirstOrDefault().ScheduleTime != null)
                        {
                            LookupValuesList lookupValuesList = new LookupValuesList(ExecutionContext);
                            DateTime serverDateTime = lookupValuesList.GetServerDateTime();
                            if (transactionLineDTO.KDSOrderLineDTOList.FirstOrDefault().ScheduleTime.Value.AddMinutes(timeToAmend) < serverDateTime)
                            {
                                string message = MessageViewContainerList.GetMessage(ExecutionContext, 4023);
                                log.Error(message);
                                SetFooterContent(message, MessageType.Error);
                                return;
                            }
                        }
                        else
                        {
                            string message = MessageViewContainerList.GetMessage(ExecutionContext, 4020);
                            log.Error(message);
                            SetFooterContent(message, MessageType.Error);
                            return;
                        }

                    }

                    ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
                    List<TransactionLineDTO> result = null;
                    try
                    {
                        using (NoSynchronizationContextScope.Enter())
                        {
                            Task<List<TransactionLineDTO>> task = transactionUseCases.AmendKOTScheduleTime(transactionDTO.TransactionId, timeToAmend);
                            task.Wait();
                            result = task.Result;
                        }
                        if (result != null)
                        {
                            List<TransactionLineDTO> transactionLineDTOList = new List<TransactionLineDTO>();
                            foreach (TransactionLineDTO trxLine in result)
                            {
                                if (trxLine.KDSOrderLineDTOList != null && trxLine.KDSOrderLineDTOList.Any())
                                {
                                    transactionLineDTOList.Add(trxLine);
                                }
                            }
                            transactionLineCollection = new ObservableCollection<TransactionLineDTO>(transactionLineDTOList);
                            SetTransactionKOTCustomDataGridVM();

                        }
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4055), MessageType.Info);
                        //Time successfully amended
                    }
                    catch (Exception)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong please try again...."), MessageType.Error);
                    }
                }
            }
            else if (genericDataEntryVM.ButtonClickType == ButtonClickType.Cancel)
            {
                SetFooterContent(string.Empty, MessageType.None);
            }

            log.LogMethodEntry();
        }

        private void OnReprintKOTClick(object obj)
        {
            log.LogMethodEntry();
            try
            {
                //if (TransactionKOTCustomDataGridVM.SelectedItems!=null && TransactionKOTCustomDataGridVM.SelectedItems.Any())
                if (transactionDTO != null && transactionDTO.TransactionId > -1
                    && transactionLineCollection != null && transactionLineCollection.Any())
                {
                    //List<TransactionLineDTO> selectedTransactionLineDTOList = (List<TransactionLineDTO>)(object)TransactionKOTCustomDataGridVM.SelectedItems.ToList();
                    //if (selectedTransactionLineDTOList != null && selectedTransactionLineDTOList.Any())
                    //{

                    //}
                    Utilities utilities = DeliveryOrderVM.GetUtility(ExecutionContext);//POSUtils.ParafaitMessageBox
                    TransactionUtils transactionUtils = new TransactionUtils(utilities);
                    Semnox.Parafait.Transaction.Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionDTO.TransactionId, utilities);
                    Type type = Type.GetType("Parafait_POS.KOTPrintProducts,Parafait POS");
                    System.Windows.Forms.Form kotPrintForm = null;
                    if (type != null)
                    {
                        ConstructorInfo constructorN = type.GetConstructor(new Type[] { typeof(Transaction.Transaction) });
                        kotPrintForm = (System.Windows.Forms.Form)constructorN.Invoke(new object[] { transaction });
                    }
                    else
                    {
                        throw new Exception(MessageViewContainerList.GetMessage(ExecutionContext, 1479, "POS KOTPrintProducts"));
                    }
                    if (kotPrintForm != null)
                    {
                        kotPrintForm.ShowDialog();
                        kotPrintForm.Dispose();
                        kotPrintForm = null;
                        ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
                        try
                        {
                            List<TransactionDTO> transactions = null;
                            List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, transactionDTO.TransactionId.ToString()));
                            using (NoSynchronizationContextScope.Enter())
                            {
                                Task<List<TransactionDTO>> task = transactionUseCases.GetTransactionDTOList(searchParameters, utilities, null, 0, 1000, true, false, false);
                                task.Wait();
                                transactions = task.Result;
                            }
                            if (transactions != null)
                            {
                                TransactionDTO transactionDTOLocal = transactions[0];
                                List<TransactionLineDTO> transactionLineDTOList = new List<TransactionLineDTO>();
                                if (transactionDTOLocal.TransactionLinesDTOList != null)
                                {
                                    foreach (TransactionLineDTO trxLine in transactionDTOLocal.TransactionLinesDTOList)
                                    {
                                        if (trxLine.KDSOrderLineDTOList != null && trxLine.KDSOrderLineDTOList.Any())
                                        {
                                            transactionLineDTOList.Add(trxLine);
                                        }
                                    }
                                }
                                transactionLineCollection = new ObservableCollection<TransactionLineDTO>(transactionLineDTOList);
                                SetTransactionKOTCustomDataGridVM();
                            }
                            //SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4055), MessageType.Info);
                            //Time successfully amended
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong please try again...."), MessageType.Error);
                        }
                    }
                }
                else
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2460), MessageType.Info);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong please try again...."), MessageType.Error);
            }
            //TransactionKOTCustomDataGridVM.SelectOption = SelectOption.None;
            log.LogMethodExit();
        }
        private void SetGenericRightSectionContentVM()
        {
            log.LogMethodEntry();
            GenericRightSectionContentVM.Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Info");
            GenericRightSectionContentVM.SubHeading = string.Empty;
            GenericRightSectionContentVM.PropertyCollections =  GetRightSectionCommonProperties();
            log.LogMethodExit();
        }

        private ObservableCollection<RightSectionPropertyValues> GetRightSectionCommonProperties()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return new ObservableCollection<RightSectionPropertyValues>
            {
                new RightSectionPropertyValues()
                {
                    Property=MessageViewContainerList.GetMessage(ExecutionContext, "Transaction Remarks"),
                    Value = headerRemarks
                },
                new RightSectionPropertyValues()
                {
                    Property=MessageViewContainerList.GetMessage(ExecutionContext, "Customer Reference No."),
                    Value = transactionDTO==null || transactionDTO.TransctionOrderDispensingDTO == null ? string.Empty : transactionDTO.TransctionOrderDispensingDTO.DeliveryChannelCustomerReferenceNo
                }
             };
        }

        private void OnSelectionChanged(object obj)
        {
            log.LogMethodEntry();
            if (TransactionKOTCustomDataGridVM.SelectedItem != null)
            {
                selectedDTO = (TransactionLineDTO)TransactionKOTCustomDataGridVM.SelectedItem;
                GenericRightSectionContentVM.Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Info");
                GenericRightSectionContentVM.SubHeading = selectedDTO.ProductName;
                GenericRightSectionContentVM.PropertyCollections = GetRightSectionCommonProperties();
                GenericRightSectionContentVM.PropertyCollections.Add(
                    new RightSectionPropertyValues()
                    {
                        Property = MessageViewContainerList.GetMessage(ExecutionContext, "Line Remarks"),
                        Value = selectedDTO.Remarks
                    });
            }
            else
            {
                SetGenericRightSectionContentVM();
            }
            log.LogMethodExit();
        }
        private void OnPreviousNavigation(object parameter)
        {
            log.LogMethodEntry(parameter);

            SetFooterContent(String.Empty, MessageType.None);
            if (TransactionKOTCustomDataGridVM != null && TransactionKOTCustomDataGridVM.UICollectionToBeRendered != null &&
             TransactionKOTCustomDataGridVM.UICollectionToBeRendered.IndexOf(TransactionKOTCustomDataGridVM.SelectedItem) > 0)
            {
                TransactionKOTCustomDataGridVM.SelectedItem = TransactionKOTCustomDataGridVM.UICollectionToBeRendered[TransactionKOTCustomDataGridVM.UICollectionToBeRendered.IndexOf(TransactionKOTCustomDataGridVM.SelectedItem) - 1];
            }
            log.LogMethodExit();

        }
        private void OnNextNavigation(object parameter)
        {
            log.LogMethodEntry(parameter);

            SetFooterContent(String.Empty, MessageType.None);
            if (TransactionKOTCustomDataGridVM != null && TransactionKOTCustomDataGridVM.UICollectionToBeRendered != null &&
               TransactionKOTCustomDataGridVM.UICollectionToBeRendered.IndexOf(TransactionKOTCustomDataGridVM.SelectedItem) < TransactionKOTCustomDataGridVM.UICollectionToBeRendered.Count - 1)
            {
                TransactionKOTCustomDataGridVM.SelectedItem = TransactionKOTCustomDataGridVM.UICollectionToBeRendered[TransactionKOTCustomDataGridVM.UICollectionToBeRendered.IndexOf(TransactionKOTCustomDataGridVM.SelectedItem) + 1];
            }
            log.LogMethodExit();

        }

        #endregion
        #region constructor
        public TransactionKOTVM(ExecutionContext executionContext, TransactionDTO transactionDTO)
        {
            log.LogMethodEntry(executionContext);
            ExecutionContext = executionContext;
            FooterVM = new FooterVM(ExecutionContext)
            {
                HideSideBarVisibility = System.Windows.Visibility.Collapsed
            };
            SetFooterContent(string.Empty, MessageType.None);
            ModuleName = MessageViewContainerList.GetMessage(ExecutionContext, "Delivery Orders");
            NavigationClickCommand = new DelegateCommand(OnNavigationClick);
            Loaded = new DelegateCommand(OnLoaded);
            headerRemarks = transactionDTO.Remarks;
            AmendTimeCommand =new DelegateCommand(OnAmendTimeClick);
            ReprintKOTCommand= new DelegateCommand(OnReprintKOTClick);
            SelectionChangedCommand = new DelegateCommand(OnSelectionChanged);
            PreviousNavigationCommand = new DelegateCommand(OnPreviousNavigation);
            NextNavigationCommand = new DelegateCommand(OnNextNavigation);
            this.transactionDTO = transactionDTO;
            AmendTimebtnEnableProperty = false;
            
            List<TransactionLineDTO> transactionLineDTOList=new List<TransactionLineDTO>();
            foreach(TransactionLineDTO trxLine in transactionDTO.TransactionLinesDTOList)
            {
                if(trxLine.KDSOrderLineDTOList != null && trxLine.KDSOrderLineDTOList.Any())
                {
                   
                    transactionLineDTOList.Add(trxLine);
                }
            }
            if(transactionLineDTOList.Any())
            {
                if (transactionLineDTOList.Exists( tl => tl.KDSOrderLineDTOList != null 
                                                       && tl.KDSOrderLineDTOList.Exists(kds=> kds.OrderedTime.HasValue && kds.OrderedTime != DateTime.MinValue)))
                {
                    AmendTimebtnEnableProperty = false;
                }
                else
                {
                    AmendTimebtnEnableProperty = true;
                }
            }

            transactionLineCollection = new ObservableCollection<TransactionLineDTO>(transactionLineDTOList);

            GenericRightSectionContentVM = new GenericRightSectionContentVM()
            {
                Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Info"),
                IsNextNavigationEnabled = true,
                IsPreviousNavigationEnabled = true
            };
            SetDisplayTagsVM();
            SetTransactionKOTCustomDataGridVM();

            log.LogMethodExit();
        }
        #endregion
    }
}

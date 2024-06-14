/********************************************************************************************
 * Project Name - Transaction UI
 * Description  - RiderAssignment VM
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     11-Jun-2021    Fiona                  Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq; 
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities; 
using Semnox.Parafait.CommonUI; 
using Semnox.Parafait.GenericUtilities; 
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI
{
    public class RiderAssignmentVM : ViewModelBase
    {
        #region members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private FooterVM footerVM;
        private string moduleName;

        private ICommand navigationClickCommand;
        private ICommand comboBoxSelectionChangedCommand;
        private ICommand moreClickedCommand;
        private ICommand selectionChangedCommand;
        private ICommand assignNewRiderCommand;
        private ICommand saveCommand;
        private ICommand loaded;

        private RiderAssignmentView riderAssignmentView;
        private DisplayTagsVM displayTagsVM;
        private ComboGroupVM comboGroupVM;
        private CustomDataGridVM customDataGridVM;
        private CustomDataGridVM transactionOrderDispensingCustomDataGridVM;
       
        private TransactionOrderDispensingDTO transactionOrderDispensingDTO;
        private bool assignNewRiderButtonIsEnabled;
        private GenericDataEntryVM genericDataEntryVM;
        private string externalSystemReference;
       
        private TransactionDeliveryDetailsDTO selectedTransactionDeliveryDetailsDTO;
        private Dictionary<string, string> riderdetails;
        private ObservableCollection<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsCollection;
        private bool manualRiderAssignmentAllowed;
        //private ICommand isSelectedCommand;


        #endregion
        #region properties
        public ICommand Loaded
        {
            set
            {
                log.LogMethodEntry(loaded, value);
                SetProperty(ref loaded, value);
                log.LogMethodExit(loaded);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loaded);
                return loaded;
            }
        }
        public FooterVM FooterVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(footerVM);
                return footerVM;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref footerVM, value);
                log.LogMethodExit();
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
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref moduleName, value);
                }
                log.LogMethodExit();
            }
        }
        public ICommand NavigationClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return navigationClickCommand;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref navigationClickCommand, value);
                log.LogMethodExit();
            }
        }
        public ICommand AssignNewRiderCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return assignNewRiderCommand;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref assignNewRiderCommand, value);
                log.LogMethodExit();
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return saveCommand;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref saveCommand, value);
                log.LogMethodExit();
            }
        }      
        //public ICommand IsSelectedCommand
        //{
        //    get
        //    {
        //        logger.LogMethodEntry();
        //        logger.LogMethodExit();
        //        return isSelectedCommand;
        //    }
        //    set
        //    {
        //        logger.LogMethodEntry();
        //        SetProperty(ref isSelectedCommand, value);
        //        logger.LogMethodExit();
        //    }
        //}
        public bool AssignNewRiderButtonIsEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return assignNewRiderButtonIsEnabled;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref assignNewRiderButtonIsEnabled, value);
                log.LogMethodExit();
            }
        }
        public DisplayTagsVM DisplayTagsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTagsVM);
                return displayTagsVM;
            }
            set
            {
                log.LogMethodEntry(displayTagsVM, value);
                SetProperty(ref displayTagsVM, value);
                log.LogMethodExit(displayTagsVM);
            }
        }
        public GenericDataEntryVM GenericDataEntryVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericDataEntryVM);
                return genericDataEntryVM;
            }
            set
            {
                log.LogMethodEntry(genericDataEntryVM, value);
                SetProperty(ref genericDataEntryVM, value);
                log.LogMethodExit(genericDataEntryVM);
            }
        }
        public ComboGroupVM ComboGroupVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(comboGroupVM);
                return comboGroupVM;
            }
            set
            {
                log.LogMethodEntry(comboGroupVM, value);
                SetProperty(ref comboGroupVM, value);
                log.LogMethodExit(comboGroupVM);
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
        public CustomDataGridVM TransactionOrderDispensingCustomDataGridVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(transactionOrderDispensingCustomDataGridVM);
                return transactionOrderDispensingCustomDataGridVM;
            }
            set
            {
                log.LogMethodEntry(transactionOrderDispensingCustomDataGridVM, value);
                SetProperty(ref transactionOrderDispensingCustomDataGridVM, value);
                log.LogMethodExit(transactionOrderDispensingCustomDataGridVM);
            }
        }
        public ICommand MoreClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(moreClickedCommand);
                return moreClickedCommand;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref moreClickedCommand, value);
                log.LogMethodExit();
            }
        }
        public ICommand SelectionChangedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectionChangedCommand);
                return selectionChangedCommand;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref selectionChangedCommand, value);
                log.LogMethodExit();
            }
        }
        public ICommand ComboBoxSelectionChangedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(comboBoxSelectionChangedCommand);
                return comboBoxSelectionChangedCommand;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref comboBoxSelectionChangedCommand, value);
                log.LogMethodExit();
            }
        }


        #endregion
        #region methods
        private void OnNavigationClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                riderAssignmentView = parameter as RiderAssignmentView;
                PerformClose(riderAssignmentView);
            }
            log.LogMethodExit();
        }
        private void SetFooterContent(string message, MessageType messageType)
        {
            log.LogMethodEntry(message, messageType);
            if (footerVM != null)
            {
                this.footerVM.Message = message;
                this.footerVM.MessageType = messageType;
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
       

        private  void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(parameter != null)
            {
                riderAssignmentView = parameter as RiderAssignmentView;
            }
            SetFooterContent(string.Empty, MessageType.None);
            log.LogMethodExit();
        }
        private async void OnAssignNewRiderClick(object obj)
        {
            log.LogMethodEntry();
            List<string> riderNameList = new List<string>();
            riderdetails = new Dictionary<string, string>();
            List<LookupValuesContainerDTO> lookupValuesContainerDTOListforRiderRole = LookupsViewContainerList.GetLookupsContainerDTO(ExecutionContext.SiteId, "RIDER_ASSIGNMENT_ROLES").LookupValuesContainerDTOList;
          

            List<string> roleNames =lookupValuesContainerDTOListforRiderRole.Where(value => value.LookupValue == "RIDER_ASSIGNMENT_ROLE_NAME").Select(x=>x.Description).ToList();
            List<UserContainerDTO> userContainerDTOList = new List<UserContainerDTO>();
            foreach (string roleName in roleNames)
            {
                int riderRoldId = UserRoleViewContainerList.GetUserRoleContainerDTOList(ExecutionContext).Find(x => x.Role == roleName).RoleId;
                userContainerDTOList.AddRange(UserViewContainerList.GetUserContainerDTOList(ExecutionContext).Where(x => x.RoleId == riderRoldId).ToList());
            }
          
         
            int i = 1;
            foreach (UserContainerDTO userContainerDTO in userContainerDTOList)
            {
                riderNameList.Add(userContainerDTO.UserName);
                riderdetails.Add(userContainerDTO.UserName, userContainerDTO.PhoneNumber);
            }

            
            GenericDataEntryView dataEntryView = new GenericDataEntryView();
            List<LookupValuesContainerDTO> lookupValuesContainerDTOList = LookupsViewContainerList.GetLookupsContainerDTO(ExecutionContext.SiteId, "RIDER_DELIVERY_STATUS").LookupValuesContainerDTOList;
            List<string> validRiderDeliveryStatusValues = new List<string>();
            Dictionary<string, int> validRiderDeliveryStatus = new Dictionary<string, int>();

            foreach (LookupValuesContainerDTO lookupValuesContainerDTO in lookupValuesContainerDTOList)
            {
                validRiderDeliveryStatus.Add(lookupValuesContainerDTO.LookupValue, lookupValuesContainerDTO.LookupValueId);
                validRiderDeliveryStatusValues.Add(lookupValuesContainerDTO.LookupValue);
            }

            GenericDataEntryVM = new GenericDataEntryVM(ExecutionContext)
            {
                Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Add New Rider"),
                IsKeyboardVisible = false,
                DataEntryCollections = new ObservableCollection<DataEntryElement>()
                {
                    new DataEntryElement()
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Rider Name"),
                        Type=DataEntryType.ComboBox,
                        Options=new ObservableCollection<object>(riderNameList),
                        SelectedItem=riderNameList[0],
                        IsEditable=false
                        
                    },
                     new DataEntryElement()
                    {
                        Heading= MessageViewContainerList.GetMessage(ExecutionContext, "Rider Contact"),
                        Type=DataEntryType.TextBlock,
                        Text=riderdetails[riderNameList[0]],
                        IsEditable=false
                    },
                    new DataEntryElement()
                    {
                         Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Rider Status"),
                         Type = DataEntryType.ComboBox,
                         Options = new ObservableCollection<object>(validRiderDeliveryStatusValues),
                         SelectedItem=validRiderDeliveryStatusValues[0],
                         IsEditable=false
                    }
                }
                 
            };
            GenericDataEntryVM.DataEntryCollections[0].PropertyChanged += OnRiderNameComboBoxPropertyChanged;
            dataEntryView.Width = SystemParameters.PrimaryScreenWidth;
            dataEntryView.Height = SystemParameters.PrimaryScreenHeight;
            dataEntryView.DataContext = GenericDataEntryVM;
            if (riderAssignmentView != null)
            {
                dataEntryView.Owner = riderAssignmentView;
            }
            dataEntryView.ShowDialog();
           
            if (GenericDataEntryVM.ButtonClickType == ButtonClickType.Ok)
            {
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
                if(GenericDataEntryVM.DataEntryCollections[0].SelectedItem==null)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4056), MessageType.Info);
                    //"Rider not selected"
                    return;
                }
                if(GenericDataEntryVM.DataEntryCollections[2].SelectedItem == null)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4057), MessageType.Info);
                    //Rider Status not selected
                    return;
                }

                string riderName = (string)GenericDataEntryVM.DataEntryCollections[0].SelectedItem;
                string riderStatus = GenericDataEntryVM.DataEntryCollections[2].SelectedItem.ToString();
                int riderId = userContainerDTOList.Where(x => x.UserName == riderName).FirstOrDefault().UserId;
                if(transactionOrderDispensingCustomDataGridVM.CollectionToBeRendered.Any()
                    && transactionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Any(x=>x.IsActive==true))
                {
                    GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                    GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                    {
                        Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "DE-ACTIVATE EXISTING RIDER", null),
                        Content = MessageViewContainerList.GetMessage(ExecutionContext, "Do you Want the exsisting Rider to be deactivated?"),
                        OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "YES", null),
                        CancelButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "NO", null),
                        MessageButtonsType = MessageButtonsType.OkCancel
                    };
                    messagePopupView.DataContext = messagePopupVM;
                    if (riderAssignmentView != null)
                    {
                        messagePopupView.Owner = riderAssignmentView;
                    }
                    messagePopupView.ShowDialog();
                    if (messagePopupVM.ButtonClickType == ButtonClickType.Cancel)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4058), MessageType.Info);
                        //Rider Assignment cancelled. Existing rider is not deactivated
                        return;
                    }
                }
                TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO = new TransactionDeliveryDetailsDTO(-1, transactionOrderDispensingDTO.TransactionOrderDispensingId, riderId, GenericDataEntryVM.DataEntryCollections[0].SelectedItem.ToString(), GenericDataEntryVM.DataEntryCollections[1].Text, validRiderDeliveryStatus[riderStatus], string.Empty,string.Empty, true);
                
                try
                {
                    TransactionOrderDispensingDTO resultDTO = await transactionUseCases.AssignRider(transactionOrderDispensingDTO.TransactionId, transactionDeliveryDetailsDTO);
                    if (resultDTO != null)
                    {
                        transactionOrderDispensingDTO = resultDTO;
                        RefresshRiderAssignmentView();
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4059), MessageType.Info);
                        //New Rider has been Assigned
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
            log.LogMethodExit();
        }
        private void OnRiderNameComboBoxPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DataEntryElement entryElement = sender as DataEntryElement;
            if (entryElement != null && !string.IsNullOrEmpty(e.PropertyName) && e.PropertyName == "SelectedItem" && riderdetails!=null)
            {
                GenericDataEntryVM.DataEntryCollections[1].Text = riderdetails[(GenericDataEntryVM.DataEntryCollections[0].SelectedItem).ToString()];
            }
            log.LogMethodExit();
        }
        private async void OnSaveClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            if((transactionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Where(x=>x.IsChanged==true)).Any())
            {
                TransactionOrderDispensingDTO result = null;
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
                try
                {
                    result = await transactionUseCases.SaveRiderAssignmentRemarks(transactionOrderDispensingDTO.TransactionId, transactionOrderDispensingDTO.TransactionDeliveryDetailsDTOList);
                    if(manualRiderAssignmentAllowed)
                    {
                        result = await transactionUseCases.SaveRiderDeliveryStatus(transactionOrderDispensingDTO.TransactionId, transactionOrderDispensingDTO.TransactionDeliveryDetailsDTOList);
                    }
                    if (result != null)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4062), MessageType.Info);
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

                
                if(result!=null)
                {
                    transactionOrderDispensingDTO = result;
                    RefresshRiderAssignmentView();
                }
            }
            log.LogMethodExit();
        }
        private async void OnMoreClicked(object obj)
        {
            log.LogMethodEntry();
            if (TransactionOrderDispensingCustomDataGridVM != null && TransactionOrderDispensingCustomDataGridVM.ButtonClickedModel != null && TransactionOrderDispensingCustomDataGridVM.ButtonClickedModel.Item != null)
            {
                //List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                //searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, "1"));
                //Utilities utilities = new Utilities();
                //transactionUseCases.GetTransactionDTOList(searchParameters, utilities, null, 0, 0, true, true, true);
                if (TransactionOrderDispensingCustomDataGridVM.ButtonClickedModel.ColumnIndex == 5)
                {
                    await UnassignRider();
                }
                else if(TransactionOrderDispensingCustomDataGridVM.ButtonClickedModel.ColumnIndex == 6)
                {
                    await ReAssignRider();
                }
                log.LogMethodExit();
            }
        }

        private async Task UnassignRider()
        {
            log.LogMethodEntry();
            ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
            if (selectedTransactionDeliveryDetailsDTO.IsActive)
            {
                try
                {
                    TransactionOrderDispensingDTO resultDTO = await transactionUseCases.UnAssignRider(transactionOrderDispensingDTO.TransactionId, selectedTransactionDeliveryDetailsDTO);
                    if (resultDTO != null)
                    {
                        transactionOrderDispensingDTO = resultDTO;
                        RefresshRiderAssignmentView();
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4063), MessageType.Info);
                        //Rider has been Unassigned
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
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4064), MessageType.Info);
                //Rider is already Unassigned
            }
            log.LogMethodExit();
        }
        private async Task ReAssignRider()
        {
            log.LogMethodEntry();
            ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
            if (selectedTransactionDeliveryDetailsDTO.IsActive == false)
            {
                if (TransactionOrderDispensingCustomDataGridVM.CollectionToBeRendered != null &&
                    transactionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Any(x => x.IsActive == true))
                {
                    GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                    GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext)
                    {
                        Heading = MessageViewContainerList.GetMessage(this.ExecutionContext, "DE-ACTIVATE EXISTING RIDER", null),
                        Content = MessageViewContainerList.GetMessage(ExecutionContext, 4060),//Do you Want the exsisting Rider to be deactivated?
                        OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "Yes", null),
                        CancelButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "No", null),
                        MessageButtonsType = MessageButtonsType.OkCancel
                    };
                    messagePopupView.DataContext = messagePopupVM;
                    if (riderAssignmentView != null)
                    {
                        messagePopupView.Owner = riderAssignmentView;
                    }
                    messagePopupView.ShowDialog();
                    if (messagePopupVM.ButtonClickType == ButtonClickType.Cancel)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4058), MessageType.Info);
                        return;
                    }
                }
                try
                {
                    TransactionOrderDispensingDTO resultDTO = await transactionUseCases.AssignRider(transactionOrderDispensingDTO.TransactionId, selectedTransactionDeliveryDetailsDTO);
                    if (resultDTO != null)
                    {
                        transactionOrderDispensingDTO = resultDTO;
                        RefresshRiderAssignmentView();
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4065), MessageType.Info);
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
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4066), MessageType.Info);
                //Rider is already Assigned
            }
            log.LogMethodExit();
        }
        private void OnSelectionChanged(object parameter)
        {
            log.LogMethodEntry();
            if(TransactionOrderDispensingCustomDataGridVM.SelectedItem != null)
            {
                selectedTransactionDeliveryDetailsDTO = (TransactionDeliveryDetailsDTO)TransactionOrderDispensingCustomDataGridVM.SelectedItem;
            }
            //SetFooterContent(string.Empty, MessageType.None);
            //ComboBoxSelectionChanged(null);
            log.LogMethodExit();
        }
           
        private void RefresshRiderAssignmentView()
        {
            log.LogMethodEntry();
            transactionDeliveryDetailsCollection = new ObservableCollection<TransactionDeliveryDetailsDTO>(transactionOrderDispensingDTO.TransactionDeliveryDetailsDTOList);
            transactionOrderDispensingCustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(transactionOrderDispensingDTO.TransactionDeliveryDetailsDTOList);
            log.LogMethodExit();
        }

        private void SetDisplayTagsVM()
        {
            log.LogMethodEntry();
            IDeliveryChannelUseCases deliveryChannelUseCases = DeliveryChannelUseCaseFactory.GetDeliveryChannelUseCases(ExecutionContext);
            List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>(DeliveryChannelDTO.SearchByParameters.DELIVERY_CHANNEL_ID, transactionOrderDispensingDTO.DeliveryChannelId.ToString()));
            searchParameters.Add(new KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>(DeliveryChannelDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<DeliveryChannelDTO> deliveryChannelDTOList = null;
            using (NoSynchronizationContextScope.Enter())
            {
                Task<List<DeliveryChannelDTO>> task =  deliveryChannelUseCases.GetDeliveryChannel(searchParameters);
                task.Wait();
                deliveryChannelDTOList = task.Result;
            }
             
            DeliveryChannelDTO deliveryChannelDTO;
            if (deliveryChannelDTOList != null)
            {
                deliveryChannelDTO = deliveryChannelDTOList[0];
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong.. Try Again later"), MessageType.Warning);
                PerformClose(riderAssignmentView);
                return;
            }


            DisplayTagsVM = new DisplayTagsVM()
            {
                DisplayTags = new ObservableCollection<ObservableCollection<DisplayTag>>()
                {
                    new ObservableCollection<DisplayTag>()
                                      {
                                          new DisplayTag()
                                          {
                                              Text = MessageViewContainerList.GetMessage(ExecutionContext,  "Transaction Id"),
                                          },
                                          new DisplayTag()
                                          {
                                              Text = transactionOrderDispensingDTO.TransactionId.ToString(),
                                              TextSize = TextSize.Medium,
                                              FontWeight = FontWeights.Bold
                                          }
                                      },
                    new ObservableCollection<DisplayTag>()
                                      {
                                          new DisplayTag()
                                          {
                                              Text = MessageViewContainerList.GetMessage(ExecutionContext, "Reference No"),
                                          },
                                          new DisplayTag()
                                          {
                                              Text = externalSystemReference,
                                              TextSize = TextSize.Medium,
                                              FontWeight = FontWeights.Bold
                                          }
                                      },
                    new ObservableCollection<DisplayTag>()
                                      {
                                          new DisplayTag()
                                          {
                                              Text = MessageViewContainerList.GetMessage(ExecutionContext, "Channel"),
                                          },
                                          new DisplayTag()
                                          {
                                              Text =  deliveryChannelDTO.ChannelName,
                                              TextSize = TextSize.Medium,
                                              FontWeight = FontWeights.Bold
                                          }
                                      }
                }
            };

            AssignNewRiderButtonIsEnabled = deliveryChannelDTO.ManualRiderAssignmentAllowed;
            manualRiderAssignmentAllowed = deliveryChannelDTO.ManualRiderAssignmentAllowed;
            SetCustomDataGridVM();
            log.LogMethodExit();
        }
        private void SetCustomDataGridVM()
        {
            log.LogMethodEntry();
            List<LookupValuesContainerDTO> lookupValuesContainerDTOListforRiderRole = LookupsViewContainerList.GetLookupsContainerDTO(ExecutionContext.SiteId, "RIDER_ASSIGNMENT_ROLES").LookupValuesContainerDTOList;
            //List<string> riderRoleNames= lookupValuesContainerDTOListforRiderRole.Where(value => value.LookupValue == "RIDER_ASSIGNMENT_ROLE_NAME").Select(x=>x.Description).ToList();
            List<LookupValuesContainerDTO> lookupValuesContainerDTOList1 = lookupValuesContainerDTOListforRiderRole.Where(value => value.LookupValue == "RIDER_ASSIGNMENT_ROLE_NAME").ToList();
            ObservableCollection<LookupValuesContainerDTO> riderRoleNames = new ObservableCollection<LookupValuesContainerDTO>(lookupValuesContainerDTOList1);



            List<LookupValuesContainerDTO> lookupValuesContainerDTOList = LookupsViewContainerList.GetLookupsContainerDTO(ExecutionContext.SiteId, "RIDER_DELIVERY_STATUS").LookupValuesContainerDTOList;
            List<string> validRiderDeliveryStatusValues = new List<string>();
            List<string> validRiderDeliveryStatusIds = new List<string>();
            foreach (LookupValuesContainerDTO lookupValuesContainerDTO in lookupValuesContainerDTOList)
            {
                validRiderDeliveryStatusValues.Add(lookupValuesContainerDTO.LookupValue);
                validRiderDeliveryStatusIds.Add(lookupValuesContainerDTO.LookupValueId.ToString());
            }
            TransactionOrderDispensingCustomDataGridVM = new CustomDataGridVM(ExecutionContext)
            {
                IsComboAndSearchVisible = false,
                CollectionToBeRendered = new ObservableCollection<object>(transactionOrderDispensingDTO.TransactionDeliveryDetailsDTOList),
                DataGridRowReadOnlyProperty = "IsActive",
                HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                {
                   {"RiderId",
                        new CustomDataGridColumnElement()
                                        {
                                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Rider Name"),
                                            Type=DataEntryType.TextBlock,
                                            IsReadOnly=true,
                                            ConverterParameter=ExecutionContext,
                                            Converter=new RiderNameConverter()

                                        }
                    },
                   {"RiderPhoneNumber",
                        new CustomDataGridColumnElement()
                                        {
                                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Rider Contact"),
                                            Type=DataEntryType.TextBlock,
                                            IsReadOnly=true
                                        }
                    },
                   {"ExternalRiderName",
                        new CustomDataGridColumnElement()
                                        {
                                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "External Rider Name"),
                                            Type=DataEntryType.TextBlock,
                                            IsReadOnly=true,

                                        }
                    },
                   {"RiderDeliveryStatus",
                        new CustomDataGridColumnElement()
                                        {
                                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Rider Delivery Status"),
                                            Type=DataEntryType.ComboBox,
                                            DisplayMemberPath = "LookupValue",
                                            Options=new ObservableCollection<object>(lookupValuesContainerDTOList),
                                            SecondarySource=new ObservableCollection<object>(lookupValuesContainerDTOList),
                                            SourcePropertyName="RiderDeliveryStatus",
                                            ChildOrSecondarySourcePropertyName="LookupValueId",
                                            IsReadOnly=!(manualRiderAssignmentAllowed),
                                            Converter=new RiderDeliveryStatusConverter(),
                                            ConverterParameter=lookupValuesContainerDTOList
                                             //!(deliveryChannelDTO.ManualRiderAssignmentAllowed)
                                        }
                    },
                   {"Remarks",
                        new CustomDataGridColumnElement()
                                        {
                                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Remarks").PadRight(18),
                                            Type=DataEntryType.TextBox,
                                            //DataGridColumnFixedSize=1,
                                            //DataGridColumnLengthUnitType=DataGridLengthUnitType.Star,
                                        }
                    },
                   {"Un-assign Rider",
                        new CustomDataGridColumnElement()
                                        {
                                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "UN-ASSIGN") + Environment.NewLine+MessageViewContainerList.GetMessage(ExecutionContext, "RIDER"),
                                            Type=DataEntryType.Button,
                                            ActionButtonType = DataGridButtonType.Content,
                                            //DataGridColumnFixedSize=1,
                                            //DataGridColumnLengthUnitType=DataGridLengthUnitType.Star,
                                            IsEnable=manualRiderAssignmentAllowed,
                                            
                                        }
                   },
                   {"Re-assign Rider",
                        new CustomDataGridColumnElement()
                                        {
                                            Heading = MessageViewContainerList.GetMessage(ExecutionContext, "RE-ASSIGN") + Environment.NewLine+MessageViewContainerList.GetMessage(ExecutionContext, "RIDER"),
                                            Type=DataEntryType.Button,
                                            ActionButtonType = DataGridButtonType.Content,
                                            //DataGridColumnFixedSize=1,
                                            //DataGridColumnLengthUnitType=DataGridLengthUnitType.Star,
                                            IsEnable=manualRiderAssignmentAllowed
                                        }
                    }

                }             
            };
            log.LogMethodExit();
        }
        #endregion
        #region Constructor
        public RiderAssignmentVM(ExecutionContext executioncontext,TransactionOrderDispensingDTO transctionOrderDispensingDTO, string externalSystemReference=null)
        {
            log.LogMethodEntry(executioncontext, transctionOrderDispensingDTO, externalSystemReference);
            this.ExecutionContext = executioncontext;
            FooterVM = new FooterVM(ExecutionContext)
            {
                HideSideBarVisibility = System.Windows.Visibility.Collapsed
            };
            ModuleName = MessageViewContainerList.GetMessage(ExecutionContext, "Delivery Orders");
            NavigationClickCommand = new DelegateCommand(OnNavigationClick);
            AssignNewRiderCommand = new DelegateCommand(OnAssignNewRiderClick);
            SaveCommand = new DelegateCommand(OnSaveClick);
            Loaded = new DelegateCommand(OnLoaded);
            MoreClickedCommand=new DelegateCommand(OnMoreClicked);
            SelectionChangedCommand=new DelegateCommand(OnSelectionChanged);
            this.transactionOrderDispensingDTO = transctionOrderDispensingDTO;
            this.externalSystemReference = externalSystemReference;
            if(transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList==null)
            {
                transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList = new List<TransactionDeliveryDetailsDTO>();
            }
            transactionDeliveryDetailsCollection = new ObservableCollection<TransactionDeliveryDetailsDTO>(transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList);
           
            SetDisplayTagsVM();
            log.LogMethodExit();
        }

        
    }
    #endregion

}


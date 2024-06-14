/********************************************************************************************
 * Project Name - Delivery UI
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
using Semnox.Parafait.DeliveryIntegration;
using Semnox.Parafait.GenericUtilities; 
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.DeliveryUI
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
        private List<string> riderNameList;
        private List<UserContainerDTO> riderUserContainerDTOList;
        private string assignedStatus;
        private List<string> validRiderDeliveryStatusValues;
        private Dictionary<string, int> validRiderDeliveryStatusDictionary;
        //private ICommand isSelectedCommand;


        #endregion
        #region properties
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
        public FooterVM FooterVM
        {
            get
            {
                return footerVM;
            }
            set
            {
                if (!object.Equals(footerVM, value))
                {
                    footerVM = value;
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
        public ICommand AssignNewRiderCommand
        {
            get
            {
                return assignNewRiderCommand;
            }
            set
            {
                if (!object.Equals(assignNewRiderCommand, value))
                {
                    assignNewRiderCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                return saveCommand;
            }
            set
            {
                if (!object.Equals(saveCommand, value))
                {
                    saveCommand = value;
                    OnPropertyChanged();
                }
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
                return assignNewRiderButtonIsEnabled;
            }
            set
            {
                if (!object.Equals(assignNewRiderButtonIsEnabled, value))
                {
                    assignNewRiderButtonIsEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public DisplayTagsVM DisplayTagsVM
        {
            get
            {
                return displayTagsVM;
            }
            set
            {
                if (!object.Equals(displayTagsVM, value))
                {
                    displayTagsVM = value;
                    OnPropertyChanged();
                }
            }
        }
        public GenericDataEntryVM GenericDataEntryVM
        {
            get
            {
                return genericDataEntryVM;
            }
            set
            {
                if (!object.Equals(genericDataEntryVM, value))
                {
                    genericDataEntryVM = value;
                    OnPropertyChanged();
                }
            }
        }
        public ComboGroupVM ComboGroupVM
        {
            get
            {
                return comboGroupVM;
            }
            set
            {
                if (!object.Equals(comboGroupVM, value))
                {
                    comboGroupVM = value;
                    OnPropertyChanged();
                }
            }
        }
        public CustomDataGridVM CustomDataGridVM
        {
            get
            {
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
        public CustomDataGridVM TransactionOrderDispensingCustomDataGridVM
        {
            get
            {
                return transactionOrderDispensingCustomDataGridVM;
            }
            set
            {
                if (!object.Equals(transactionOrderDispensingCustomDataGridVM, value))
                {
                    transactionOrderDispensingCustomDataGridVM = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand MoreClickedCommand
        {
            get
            {
                return moreClickedCommand;
            }
            set
            {
                if (!object.Equals(moreClickedCommand, value))
                {
                    moreClickedCommand = value;
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
        public ICommand ComboBoxSelectionChangedCommand
        {
            get
            {
                return comboBoxSelectionChangedCommand;
            }
            set
            {
                if (!object.Equals(comboBoxSelectionChangedCommand, value))
                {
                    comboBoxSelectionChangedCommand = value;
                    OnPropertyChanged();
                }
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
        private void GetRiderInformation()
        {
            log.LogMethodEntry();
            riderNameList = new List<string>();
            riderdetails = new Dictionary<string, string>();
            List<LookupValuesContainerDTO> lookupValuesContainerDTOListforRiderRole = LookupsViewContainerList.GetLookupsContainerDTO(ExecutionContext.SiteId, "RIDER_ASSIGNMENT_ROLES").LookupValuesContainerDTOList;
            string rolenames = string.Empty;
            List<string> roleNamesList = new List<string>();
            riderUserContainerDTOList = new List<UserContainerDTO>();

            if (lookupValuesContainerDTOListforRiderRole != null && lookupValuesContainerDTOListforRiderRole.Exists(value => value.LookupValue == "RIDER_ASSIGNMENT_ROLE_NAME"))
            {
                rolenames = lookupValuesContainerDTOListforRiderRole.FirstOrDefault(value => value.LookupValue == "RIDER_ASSIGNMENT_ROLE_NAME").Description;
            }
            if (!string.IsNullOrWhiteSpace(rolenames))
            {
                roleNamesList = rolenames.Split('|').ToList();
            }

            List<UserRoleContainerDTO> allUsersRoles = UserRoleViewContainerList.GetUserRoleContainerDTOList(ExecutionContext);
            foreach (string roleName in roleNamesList)
            {
                if (allUsersRoles.Exists(x => x.Role.ToLower() == roleName.ToLower()))
                {
                    int riderRoldId = allUsersRoles.Find(x => x.Role.ToLower() == roleName.ToLower()).RoleId;
                    riderUserContainerDTOList.AddRange(UserViewContainerList.GetUserContainerDTOList(ExecutionContext).Where(x => x.RoleId == riderRoldId).ToList());
                }
            }

            foreach (UserContainerDTO userContainerDTO in riderUserContainerDTOList)
            {
                riderNameList.Add(userContainerDTO.UserName);
                riderdetails.Add(userContainerDTO.UserName, userContainerDTO.PhoneNumber);
            }


            List<LookupValuesContainerDTO> lookupValuesContainerDTOListForRiderStatus = LookupsViewContainerList.GetLookupsContainerDTO(ExecutionContext.SiteId, "RIDER_DELIVERY_STATUS").LookupValuesContainerDTOList;

            validRiderDeliveryStatusValues = new List<string>();
            validRiderDeliveryStatusDictionary = new Dictionary<string, int>();

            assignedStatus = string.Empty;
            foreach (LookupValuesContainerDTO lookupValuesContainerDTO in lookupValuesContainerDTOListForRiderStatus)
            {
                validRiderDeliveryStatusDictionary.Add(lookupValuesContainerDTO.LookupValue, lookupValuesContainerDTO.LookupValueId);
                validRiderDeliveryStatusValues.Add(lookupValuesContainerDTO.LookupValue);
                if (lookupValuesContainerDTO.LookupValue.ToUpper() == "ASSIGNED")
                {
                    assignedStatus = lookupValuesContainerDTO.LookupValue;
                }

            }
            log.LogMethodExit();
        }
        private async void OnAssignNewRiderClick(object obj)
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);

            GenericDataEntryView dataEntryView = new GenericDataEntryView();

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
                        SelectedItem=riderNameList.Any()== false? null :  riderNameList[0],
                        IsEditable=false

                    },
                     new DataEntryElement()
                    {
                        Heading= MessageViewContainerList.GetMessage(ExecutionContext, "Rider Contact"),
                        Type=DataEntryType.TextBlock,
                        Text = riderNameList.Any()== false? string.Empty : riderdetails[riderNameList[0]],
                        IsEditable = false,
                        IsMandatory = true
                    },
                    new DataEntryElement()
                    {
                         Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Rider Status"),
                         Type = DataEntryType.ComboBox,
                         Options = new ObservableCollection<object>(validRiderDeliveryStatusValues),
                         SelectedItem = string.IsNullOrWhiteSpace(assignedStatus)? validRiderDeliveryStatusValues[0] : assignedStatus,
                         IsEditable = false
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
                if (GenericDataEntryVM.DataEntryCollections[0].SelectedItem == null)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4056), MessageType.Info);
                    //"Rider not selected"
                    return;
                }
                if (string.IsNullOrEmpty(GenericDataEntryVM.DataEntryCollections[1].Text))
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4581), MessageType.Info);
                    //Cannot Proceed. Rider Contact not entered
                    return;
                }
                if (GenericDataEntryVM.DataEntryCollections[2].SelectedItem == null)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 4057), MessageType.Info);
                    //Rider Status not selected
                    return;
                }

                string riderName = (string)GenericDataEntryVM.DataEntryCollections[0].SelectedItem;
                string riderStatus = GenericDataEntryVM.DataEntryCollections[2].SelectedItem.ToString();
                int riderId = riderUserContainerDTOList.Where(x => x.UserName == riderName).FirstOrDefault().UserId;
                if (transactionOrderDispensingCustomDataGridVM.CollectionToBeRendered.Any()
                    && transactionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Any(x => x.IsActive == true))
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
                TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO = new TransactionDeliveryDetailsDTO(-1, transactionOrderDispensingDTO.TransactionOrderDispensingId, riderId, GenericDataEntryVM.DataEntryCollections[0].SelectedItem.ToString(), GenericDataEntryVM.DataEntryCollections[1].Text, validRiderDeliveryStatusDictionary[riderStatus], string.Empty, string.Empty, true);

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
            SetFooterContent(string.Empty, MessageType.None);
            if ((transactionOrderDispensingDTO.TransactionDeliveryDetailsDTOList.Where(x => x.IsChanged == true)).Any())
            {
                TransactionOrderDispensingDTO result = null;
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(ExecutionContext);
                try
                {
                    result = await transactionUseCases.SaveRiderAssignmentRemarks(transactionOrderDispensingDTO.TransactionId, transactionOrderDispensingDTO.TransactionDeliveryDetailsDTOList);
                    if (manualRiderAssignmentAllowed)
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
            SetFooterContent(string.Empty, MessageType.None);
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
                    List<LookupValuesContainerDTO> lookupValuesContainerDTOList = LookupsContainerList.GetLookupsContainerDTO(ExecutionContext.SiteId, "RIDER_DELIVERY_STATUS").LookupValuesContainerDTOList;
                    if (lookupValuesContainerDTOList.Exists(x => x.LookupValue.ToLower() == "reassigned"))
                    {
                        selectedTransactionDeliveryDetailsDTO.RiderDeliveryStatus = lookupValuesContainerDTOList.FirstOrDefault(x => x.LookupValue.ToLower() == "reassigned").LookupValueId;
                    }
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
                                            DataGridColumnFixedSize=250,
                                            DataGridColumnLengthUnitType=DataGridLengthUnitType.Pixel,
                                            MaxLength = 2000
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
            MoreClickedCommand = new DelegateCommand(OnMoreClicked);
            SelectionChangedCommand = new DelegateCommand(OnSelectionChanged);
            this.transactionOrderDispensingDTO = transctionOrderDispensingDTO;
            this.externalSystemReference = externalSystemReference;
            if (transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList == null)
            {
                transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList = new List<TransactionDeliveryDetailsDTO>();
            }
            transactionDeliveryDetailsCollection = new ObservableCollection<TransactionDeliveryDetailsDTO>(transctionOrderDispensingDTO.TransactionDeliveryDetailsDTOList);

            SetDisplayTagsVM();
            GetRiderInformation();
            log.LogMethodExit();
        }


    }
    #endregion

}


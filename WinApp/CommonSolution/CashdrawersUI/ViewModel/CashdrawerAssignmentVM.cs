/********************************************************************************************
 * Project Name - CashdrawerUI                                                                        
 * Description  -CashdrawerAssignmentVM
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0     11-Aug-2021      Girish Kundar     Created 
 *2.140.0     14-Sep-2021   Girish Kundar        Modified : Added GetCurrentClockedInUsers() method to get currently logged in user
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.CashdrawersUI;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer.Cashdrawers;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;
namespace CashdrawersUI.ViewModel
{
    public class CashdrawerAssignmentVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ObservableCollection<POSMachineContainerDTO> posMachineContainerDTOList;
        private List<POSMachineContainerDTO> posmachineDTOList;
        private POSMachineContainerDTO selectedPOSMachineContainerDTO;
        private CashdrawerDTO selectedCashdrawerDTO;
        private int currentPosMachineId;
        private int selectedCashDrawerId;
        private int managerId;
        private string cashdrawerOneLable;
        private string cashdrawerTwoLable;
        private string cashdrawerThreeLable;
        private string message;
        private string loginId;
        private string buttonCancel;
        private string buttonAssign;
        private string buttonUnAssign;
        private AuthenticateManagerDelegate authenticateManagerDelegate;
        private CashdrawerAssignmentView cashdrawerAssignmentView;
        private DisplayTagsVM displayTagsVM;
        private ICommand cancelCommand;
        private ICommand assignCommand;
        private ICommand unAssignCommand;
        private ICommand navigationClickCommand;
        private ICommand onLoadCommand;
        private ICommand selectionChangedCmd;
        private ICommand toggleCheckedCommand;
        private ICommand toggleUnCheckedCommand;
        private string labelStatus;
        private string moduleName;
        private ObservableCollection<DisplayTag> displayTags1;
        private ObservableCollection<DisplayTag> displayTags2;
        private GenericToggleButtonsVM genericToggleButtonsVM;
        #endregion Members
        #region Properties

        public List<POSMachineContainerDTO> POSMachineContainerDTOList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(posmachineDTOList);
                return posmachineDTOList;
            }
            set
            {
                log.LogMethodEntry(posmachineDTOList, value);
                SetProperty(ref posmachineDTOList, value);
                log.LogMethodExit(posmachineDTOList);
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
            set
            {
                log.LogMethodEntry(genericToggleButtonsVM, value);
                SetProperty(ref genericToggleButtonsVM, value);
                log.LogMethodExit(genericToggleButtonsVM);
            }
        }
        public ObservableCollection<DisplayTag> DisplayTags1
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags1);
                return displayTags1;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags1, value);
                log.LogMethodExit(displayTags1);
            }
        }
        public ObservableCollection<DisplayTag> DisplayTags2
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTags2);
                return displayTags1;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref displayTags2, value);
                log.LogMethodExit(displayTags2);
            }
        }
        /// <summary>
        /// DisplayTagsVM
        /// </summary>
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
                log.LogMethodEntry(value);
                SetProperty(ref displayTagsVM, value);
                log.LogMethodExit(displayTagsVM);
            }
        }

        /// <summary>
        /// POSMachineSource
        /// </summary>
        public ObservableCollection<POSMachineContainerDTO> POSMachineSource
        {
            get
            {
                return posMachineContainerDTOList;
            }
            set
            {
                posMachineContainerDTOList = value;
            }
        }

        /// <summary>
        /// labelStatus
        /// </summary>
        public string LabelStatus
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(labelStatus);
                return labelStatus;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref labelStatus, value);
            }
        }

        /// <summary>
        /// selectedCashDrawerId
        /// </summary>
        public int SelectedCashDrawerId
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedCashDrawerId);
                return selectedCashDrawerId;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedCashDrawerId, value);
            }
        }

        /// <summary>
        /// CashdrawerOneLable
        /// </summary>
        public string CashdrawerOneLable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cashdrawerOneLable);
                return cashdrawerOneLable;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref cashdrawerOneLable, value);
            }
        }
        /// <summary>
        /// CashdrawerTwoLable
        /// </summary>
        public string CashdrawerTwoLable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cashdrawerTwoLable);
                return cashdrawerTwoLable;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref cashdrawerTwoLable, value);
            }
        }
        /// <summary>
        /// cashdrawerThreeLable
        /// </summary>
        public string CashdrawerThreeLable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cashdrawerThreeLable);
                return cashdrawerOneLable;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref cashdrawerThreeLable, value);
            }
        }
        /// <summary>
        /// ModuleName
        /// </summary>
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
            }
        }
        /// <summary>
        /// ButtonCancel
        /// </summary>
        public string ButtonCancel
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonCancel);
                return buttonCancel;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref buttonCancel, value);
                }
            }
        }

        public string ButtonAssign
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonAssign);
                return buttonAssign;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref buttonAssign, value);
                }
            }
        }

        /// <summary>
        /// POSMachineContainerDTO
        /// </summary>
        public POSMachineContainerDTO SelectedPOSMachineContainerDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedPOSMachineContainerDTO);
                return selectedPOSMachineContainerDTO;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref selectedPOSMachineContainerDTO, value);
                }
            }
        }
        /// <summary>
        /// CashdrawerDTO
        /// </summary>
        public CashdrawerDTO SelectedPOSCashdrawerDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedCashdrawerDTO);
                return selectedCashdrawerDTO;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref selectedCashdrawerDTO, value);
                }
            }
        }
        /// <summary>
        /// SuccessMessage
        /// </summary>
        public string SuccessMessage
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(message);
                return message;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref message, value);
            }
        }

        #endregion Properties

        #region Commands
        public ICommand NavigationClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(navigationClickCommand);
                return navigationClickCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                navigationClickCommand = value;
            }
        }

        /// <summary>   
        /// SearchCommand
        /// </summary>
        public ICommand OnLoadCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(onLoadCommand);
                return onLoadCommand;
            }
            set
            {
                log.LogMethodEntry(onLoadCommand, value);
                SetProperty(ref onLoadCommand, value);
                log.LogMethodExit(onLoadCommand);
            }
        }
        /// <summary>   
        /// ToggleCheckedCommand
        /// </summary>
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
        /// <summary>   
        /// toggleUnCheckedCommand
        /// </summary>
        public ICommand ToggleUncheckedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(toggleUnCheckedCommand);
                return toggleUnCheckedCommand;
            }
            set
            {
                log.LogMethodEntry(toggleUnCheckedCommand, value);
                SetProperty(ref toggleUnCheckedCommand, value);
                log.LogMethodExit(toggleUnCheckedCommand);
            }
        }

        /// <summary>
        /// CancelCommand
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(cancelCommand);
                return cancelCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                cancelCommand = value;
            }
        }

        /// <summary>
        /// AssignCommand
        /// </summary>
        public ICommand AssignCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(assignCommand);
                return assignCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                assignCommand = value;
            }
        }

        /// <summary>
        /// unAssignCommand
        /// </summary>
        public ICommand UnAssignCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(unAssignCommand);
                return assignCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                unAssignCommand = value;
            }
        }

        /// <summary>
        /// SelectionChanged
        /// </summary>
        public ICommand SelectionChanged
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(selectionChangedCmd);
                return selectionChangedCmd;
            }
            set
            {
                log.LogMethodEntry(value);
                selectionChangedCmd = value;
            }
        }
        #endregion Properties
        #region Constructor

        /// <summary>
        /// CashdrawerAssignmentVM
        /// </summary>
        /// <param name="executionContext"></param>
        public CashdrawerAssignmentVM(ExecutionContext executionContext, int managerId,
                                      int currentPosMachineId, string loginId)
        {
            log.LogMethodEntry(executionContext, currentPosMachineId, loginId);
            try
            {
                this.ExecutionContext = executionContext;
                this.managerId = managerId;
                this.loginId = loginId;
                LoadLables();
                FooterVM = new FooterVM(this.ExecutionContext)
                {
                    Message = "",
                    MessageType = MessageType.None,
                    HideSideBarVisibility = Visibility.Collapsed
                };
                using (NoSynchronizationContextScope.Enter())
                {
                    cancelCommand = new DelegateCommand(CancelButtonClick);
                    assignCommand = new DelegateCommand(AssignButtonClick);

                    selectionChangedCmd = new DelegateCommand(ComboxSelectionChanged);
                    navigationClickCommand = new DelegateCommand(NavigationClick);
                    onLoadCommand = new DelegateCommand(OnLoaded);
                    toggleCheckedCommand = new DelegateCommand(OnToggleChecked);
                    GenericToggleButtonsVM = new GenericToggleButtonsVM();
                    LoadPOSMachines(currentPosMachineId);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, ex.Message);
                SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
            }
            log.LogMethodExit();
        }

        #endregion Constructor


        private void UnAssignCashDrawer(int cashdrawerId)
        {
            try
            {
                log.LogMethodEntry(cashdrawerId);
                SetFooterContent(string.Empty, MessageType.None);
                String ErrorMessage = String.Empty;
                if (managerId > -1)
                {
                    string actionType = ShiftDTO.ShiftActionType.ROpen.ToString() + "," + ShiftDTO.ShiftActionType.Open.ToString();
                    List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParams = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                    searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.POS_MACHINE, SelectedPOSMachineContainerDTO.POSName));
                    searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_ACTION_IN, actionType));
                    searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.CASHDRAWER_ID, cashdrawerId.ToString()));
                    IShiftUseCases shiftUseCases = UserUseCaseFactory.GetShiftUseCases(ExecutionContext);
                    List<ShiftDTO> taskResult = null;
                    using (NoSynchronizationContextScope.Enter())
                    {
                        Task<List<ShiftDTO>> task = GetShifts(searchParams);
                        task.Wait();
                        taskResult = task.Result;
                    }
                    if (taskResult != null && taskResult.Any())
                    {
                        var shiftDTO = taskResult.FirstOrDefault();
                        using (NoSynchronizationContextScope.Enter())
                        {
                            shiftUseCases = UserUseCaseFactory.GetShiftUseCases(ExecutionContext);
                            CashdrawerActivityDTO cashdrawerActivityDTO = new CashdrawerActivityDTO(selectedCashdrawerDTO.CashdrawerId, managerId.ToString());
                            Task<ShiftDTO> task = shiftUseCases.UnAssignCashdrawer(shiftDTO.ShiftKey, cashdrawerActivityDTO);
                            task.Wait();
                            log.LogVariableState("Updated ShiftDTO", task.Result);
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 48), MessageType.Info);
                        };
                    }
                }
                else
                {
                    ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "Unable to find the Open/Ropen shift for the user " + loginId); // new message 8Y
                    SetFooterContent(ErrorMessage, MessageType.Error);
                    return;
                }
                RefreshCashdrawerButtons(SelectedPOSMachineContainerDTO);
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "Un Assignment failed ");
                SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
        }

        /// <summary>
        /// This method should load the cashdrawers of selected posmachine
        /// </summary>
        /// <param name="param"></param>
        private void ComboxSelectionChanged(object param)
        {
            try
            {
                IsLoadingVisible = true;
                log.LogMethodEntry(param);
                OnToggleChecked(param);
                IsLoadingVisible = true;
                String ErrorMessage = String.Empty;
                POSMachineContainerDTOCollection pOSMachineContainerDTOCollection = POSMachineViewContainerList.GetPOSMachineContainerDTOCollection(ExecutionContext.GetSiteId(), "");
                var pOSMachineContainerDTO = pOSMachineContainerDTOCollection.POSMachineContainerDTOList.Where(x => x.POSMachineId == SelectedPOSMachineContainerDTO.POSMachineId).FirstOrDefault();
                ShowCashdrawerButtons(pOSMachineContainerDTO.POSCashdrawerContainerDTOList);
                IsLoadingVisible = false;
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, ex.Message);
                SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
        }

        internal void OnToggleChecked(object parameter)
        {
            SetFooterContent("", MessageType.None);
            cashdrawerAssignmentView = parameter as CashdrawerAssignmentView;
            if (GenericToggleButtonsVM != null && GenericToggleButtonsVM.SelectedToggleButtonItem != null)
            {
                selectedCashDrawerId = Convert.ToInt32(GenericToggleButtonsVM.SelectedToggleButtonItem.Key.ToString());
                CashdrawerBL cashdrawerBL = new CashdrawerBL(ExecutionContext, selectedCashDrawerId);
                selectedCashdrawerDTO = cashdrawerBL.CashdrawerDTO;
                if (IsAssigned(selectedCashDrawerId))
                {
                    ButtonAssign = MessageContainerList.GetMessage(ExecutionContext, "UNASSIGN");
                }
                else
                {
                    ButtonAssign = MessageContainerList.GetMessage(ExecutionContext, "ASSIGN");
                }
            }
            else
            {
                ButtonAssign = MessageContainerList.GetMessage(ExecutionContext, "ASSIGN");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method should display the cashdrawer as buttons
        /// </summary>
        /// <param name="pOSCashdrawerContainerDTOList"></param>
        private void ShowCashdrawerButtons(List<POSCashdrawerContainerDTO> pOSCashdrawerContainerDTOList)
        {
            log.LogMethodEntry(pOSCashdrawerContainerDTOList);
            IsLoadingVisible = true;
            List<CashdrawerContainerDTO> cashdrawerDTOList = new List<CashdrawerContainerDTO>();
            cashdrawerDTOList = CashdrawerViewContainerList.GetCashdrawerContainerDTOList(ExecutionContext);
            log.LogVariableState("cashdrawerDTOList", cashdrawerDTOList);

            if (pOSCashdrawerContainerDTOList != null && pOSCashdrawerContainerDTOList.Any())
            {
                // check this CDR is already assinged if not then change the name as 
                string actionType = ShiftDTO.ShiftActionType.ROpen.ToString() + "," + ShiftDTO.ShiftActionType.Open.ToString();
                List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParams = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_ACTION_IN, actionType));
                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.POS_MACHINE, SelectedPOSMachineContainerDTO.POSName));
                IShiftUseCases shiftUseCases = UserUseCaseFactory.GetShiftUseCases(ExecutionContext);
                List<ShiftDTO> taskResult = null;
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<ShiftDTO>> task = GetShifts(searchParams);
                    task.Wait();
                    taskResult = task.Result;
                }
                if (taskResult != null && taskResult.Any())
                {
                    foreach (POSCashdrawerContainerDTO posCashdrawerContainerDTO in pOSCashdrawerContainerDTOList)
                    {
                        if (taskResult.Exists(shift => shift.CashdrawerId == posCashdrawerContainerDTO.CashdrawerId) == false)
                        {
                            // Cashdrawer is not assigned. Show assign text
                            ButtonAssign = MessageContainerList.GetMessage(ExecutionContext, "ASSIGN");
                            break;
                        }
                        else
                        {
                            ButtonAssign = MessageContainerList.GetMessage(ExecutionContext, "UNASSIGN");
                        }
                    }

                }
                // Logic to show CDR 
                GenericToggleButtonsVM.ToggleButtonItems = new ObservableCollection<CustomToggleButtonItem>();
                foreach (POSCashdrawerContainerDTO pOSCashdrawerContainerDTO in pOSCashdrawerContainerDTOList)
                {
                    string cashdrawerName = cashdrawerDTOList.Where(drawer => drawer.CashdrawerId == pOSCashdrawerContainerDTO.CashdrawerId).FirstOrDefault().CashdrawerName;
                    string cashdrawerAssigneeName = MessageContainerList.GetMessage(ExecutionContext, "Available");
                    if (taskResult != null && taskResult.Exists(shift => shift.CashdrawerId == pOSCashdrawerContainerDTO.CashdrawerId))
                    {
                        cashdrawerAssigneeName = taskResult.Where(shift => shift.CashdrawerId == pOSCashdrawerContainerDTO.CashdrawerId).FirstOrDefault().ShiftLoginId;
                    }
                    DisplayTags1 = new ObservableCollection<DisplayTag>()
                                                    {
                                            new DisplayTag(){ Text = MessageContainerList.GetMessage(ExecutionContext,cashdrawerName), TextSize = TextSize.Small,FontWeight = FontWeights.Bold},
                                            new DisplayTag(){ Text = cashdrawerAssigneeName, FontWeight = FontWeights.Bold, TextSize = TextSize.Medium},
                                                        };
                    CustomToggleButtonItem customToggleButtonItem = new CustomToggleButtonItem();
                    customToggleButtonItem.DisplayTags = DisplayTags1;
                    customToggleButtonItem.Key = pOSCashdrawerContainerDTO.CashdrawerId.ToString();
                    customToggleButtonItem.IsEnabled = true;
                    GenericToggleButtonsVM.ToggleButtonItems.Add(customToggleButtonItem);
                }
                GenericToggleButtonsVM.IsVerticalOrientation = true;
                GenericToggleButtonsVM.Columns = 3;
                foreach (CustomToggleButtonItem ToggleButtonItems in GenericToggleButtonsVM.ToggleButtonItems)
                {
                    ToggleButtonItems.IsChecked = false; // To make the button unselected while loading
                }
            }
            else
            {
                // Clear all the buttons
                GenericToggleButtonsVM.ToggleButtonItems = null;
            }
            IsLoadingVisible = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method should refresh display the cashdrawer as buttons
        /// </summary>
        /// <param name="pOSCashdrawerContainerDTO"></param>
        private void RefreshCashdrawerButtons(POSMachineContainerDTO pOSMachineContainerDTO)
        {
            log.LogMethodEntry(pOSMachineContainerDTO);
            IsLoadingVisible = true;
            if (pOSMachineContainerDTO != null)
            {
                ShowCashdrawerButtons(pOSMachineContainerDTO.POSCashdrawerContainerDTOList);
                GenericToggleButtonsVM.SelectedToggleButtonItem = null;
            }
            IsLoadingVisible = false;
            log.LogMethodExit();
        }


        #region Methods

        /// <summary>
        /// SetFooterContent
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        internal void SetFooterContent(string message, MessageType messageType)
        {
            log.LogMethodEntry(message);
            if (FooterVM != null)
            {
                FooterVM.Message = message;
                FooterVM.MessageType = messageType;
            }
            log.LogMethodExit();
        }

        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            cashdrawerAssignmentView = parameter as CashdrawerAssignmentView;
            log.LogMethodExit();
        }

        /// <summary>
        /// LoadPOSMachines
        /// </summary>
        /// <returns></returns>
        private void LoadPOSMachines(int currentPosMachineId)
        {
            log.LogMethodEntry(currentPosMachineId);
            // GenericToggleButtonsVM.ToggleButtonItems[0].IsChecked = true
            posmachineDTOList = new List<POSMachineContainerDTO>();
            POSMachineContainerDTOCollection pOSMachineContainerDTOCollection = POSMachineViewContainerList.GetPOSMachineContainerDTOCollection(ExecutionContext.GetSiteId(), "");
            foreach (POSMachineContainerDTO posMachineContainerDTO in pOSMachineContainerDTOCollection.POSMachineContainerDTOList)
            {
                ParafaitDefaultContainerDTOCollection parafaitDefaultContainerCollection = ParafaitDefaultContainerList.GetParafaitDefaultContainerDTOCollection(ExecutionContext.GetSiteId(), ExecutionContext.GetUserPKId(), posMachineContainerDTO.POSMachineId);
                log.LogVariableState("ParafaitDefaultContainerDTOCollection", parafaitDefaultContainerCollection);
                ParafaitDefaultContainerDTO parafaitDefaultContainerDTO = parafaitDefaultContainerCollection.ParafaitDefaultContainerDTOList.Where(x => x.DefaultValueName == "CASHDRAWER_INTERFACE_MODE").FirstOrDefault();
                ParafaitDefaultContainerDTO parafaitDefaultContainerDTOBlindClose = parafaitDefaultContainerCollection.ParafaitDefaultContainerDTOList.Where(x => x.DefaultValueName == "ENABLE_BLIND_CLOSE_SHIFT").FirstOrDefault();
                if (parafaitDefaultContainerDTO != null 
                     && (parafaitDefaultContainerDTO.DefaultValue == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.MULTIPLE)
                          || parafaitDefaultContainerDTOBlindClose.DefaultValue == "Y"))
                {
                    posmachineDTOList.Add(posMachineContainerDTO);
                }
            }
            if(posmachineDTOList.Any() == false)
            {
                string message = MessageContainerList.GetMessage(ExecutionContext, "Cash drawer assignment requires multiple cash drawer to be assigned to the POS");
                SetFooterContent(message, MessageType.Info);
                return;
            }
            if (managerId > -1)
            {
                // Get all the posmachines where interface mode  is set as Multimode
                POSMachineSource = new ObservableCollection<POSMachineContainerDTO>(posmachineDTOList.OrderBy(x => x.POSMachineId));
            }
            else
            {
                // Get current posmachines where interface mode  is set as Multimode
                var pOSMachineContainerDTOs = posmachineDTOList.Where(x => x.POSMachineId == currentPosMachineId).ToList();
                POSMachineSource = new ObservableCollection<POSMachineContainerDTO>(pOSMachineContainerDTOs);
            }
            log.LogVariableState("POSMachineSource", POSMachineSource);
            SelectedPOSMachineContainerDTO = POSMachineSource.Where(x => x.POSMachineId == currentPosMachineId).FirstOrDefault();
            ShowCashdrawerButtons(selectedPOSMachineContainerDTO.POSCashdrawerContainerDTOList);
            log.LogVariableState("selectedPOSMachineContainerDTO", selectedPOSMachineContainerDTO);
            log.LogMethodExit();
        }

        private void LoadLables()
        {
            log.LogMethodEntry();
            buttonCancel = MessageContainerList.GetMessage(ExecutionContext, "CANCEL");
            buttonAssign = MessageContainerList.GetMessage(ExecutionContext, "ASSIGN");
            buttonUnAssign = MessageContainerList.GetMessage(ExecutionContext, "UNASSIGN");
            moduleName = MessageContainerList.GetMessage(ExecutionContext, "ASSIGN CASHDRAWER");
            log.LogMethodExit();
        }

        /// <summary>
        /// NavigationClick
        /// </summary>
        /// <param name="param"></param>
        private void NavigationClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            cashdrawerAssignmentView = param as CashdrawerAssignmentView;
            try
            {
                if (cashdrawerAssignmentView != null)
                {
                    cashdrawerAssignmentView.Close();
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.Message;
            }
            log.LogMethodExit();
        }

        private void CancelButtonClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            cashdrawerAssignmentView = param as CashdrawerAssignmentView;
            try
            {
                if (cashdrawerAssignmentView != null)
                {
                    cashdrawerAssignmentView.Close();
                }
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Check - the cashdrawer id is mapped to any of the shift for the selected POS machine with Open or ROpen state
        /// </summary>
        /// <param name="cashdrawerId"></param>
        /// <returns></returns>
        private bool IsAssigned(int cashdrawerId)
        {
            log.LogMethodEntry(cashdrawerId);
            string actionType = ShiftDTO.ShiftActionType.ROpen.ToString() + "," + ShiftDTO.ShiftActionType.Open.ToString();
            List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParams = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
            searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_ACTION_IN, actionType));
            searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.CASHDRAWER_ID, cashdrawerId.ToString()));
            searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.POS_MACHINE, SelectedPOSMachineContainerDTO.POSName));
            IShiftUseCases shiftUseCases = UserUseCaseFactory.GetShiftUseCases(ExecutionContext);
            List<ShiftDTO> taskResult = null;
            using (NoSynchronizationContextScope.Enter())
            {
                Task<List<ShiftDTO>> task = GetShifts(searchParams);
                task.Wait();
                taskResult = task.Result;
                if (taskResult != null && taskResult.Any())
                {
                    log.LogVariableState("ShiftDTOList", taskResult);
                    log.LogMethodExit(true);
                    return true;
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        private void AssignButtonClick(object param)
        {
            try
            {
                log.LogMethodEntry(param);
                SetFooterContent(string.Empty, MessageType.None);
                cashdrawerAssignmentView = param as CashdrawerAssignmentView;
                String ErrorMessage = String.Empty;
                if (selectedCashdrawerDTO == null || GenericToggleButtonsVM.SelectedToggleButtonItem == null)
                {
                    ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "Please select cashdrawer");
                    SetFooterContent(ErrorMessage, MessageType.Warning);
                    selectedCashdrawerDTO = null;
                    return;
                }
                int cashdrawerId = selectedCashdrawerDTO.CashdrawerId;
                if (IsAssigned(cashdrawerId))
                {
                    log.Debug("IsAssigned = true");
                    UnAssignCashDrawer(cashdrawerId);
                    ButtonAssign = MessageContainerList.GetMessage(ExecutionContext, "ASSIGN");
                }
                else
                {

                    UsersDTO usersDTO = null;
                    CashdrawerBL cashdrawerBL = new CashdrawerBL(ExecutionContext, cashdrawerId);
                    if (managerId > -1)
                    {
                        // Get all clocked in Users
                        UsersList userListBL = new UsersList(ExecutionContext);
                        List<UsersDTO> usersDTOList = userListBL.GetCurrentClockedInUsers(); // Use use case here
                        if (usersDTOList == null || usersDTOList.Any() == false)
                        {
                            ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "No clocked in users");
                            SetFooterContent(ErrorMessage, MessageType.Error);
                            return;
                        }
                        POSUserAssignmentView pOSUserAssignmentView = new POSUserAssignmentView();
                        POSUserAssignmentVM pOSUserAssignmentVM = new POSUserAssignmentVM(ExecutionContext, usersDTOList, cashdrawerBL.CashdrawerDTO);
                        pOSUserAssignmentView.DataContext = pOSUserAssignmentVM;
                        pOSUserAssignmentView.Owner = cashdrawerAssignmentView;
                        pOSUserAssignmentView.ShowDialog();
                        usersDTO = pOSUserAssignmentVM.SelectedUserDTO;
                    }
                    if (usersDTO != null)
                    {
                        log.Debug("Selected pOSUserAssignmentVM.SelectedUserContainerDTO  : " + usersDTO.UserName);
                        string actionType = ShiftDTO.ShiftActionType.ROpen.ToString() + "," + ShiftDTO.ShiftActionType.Open.ToString();
                        List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParams = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                        searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_LOGIN_ID, usersDTO.LoginId));
                        searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_ACTION_IN, actionType));
                        searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.POS_MACHINE, SelectedPOSMachineContainerDTO.POSName));
                        List<ShiftDTO> taskResult = null;
                        using (NoSynchronizationContextScope.Enter())
                        {
                            Task<List<ShiftDTO>> task = GetShifts(searchParams);
                            task.Wait();
                            taskResult = task.Result;
                        }
                        if (taskResult != null && taskResult.Any())
                        {
                            // get shift dto 
                            var userShift = taskResult.Where(shift => shift.ShiftUserName.Trim() == usersDTO.UserName.Trim()).FirstOrDefault();
                            if (userShift != null)
                            {
                                if (userShift.CashdrawerId > -1)
                                {
                                    ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "Cashdrawer is already assigned to the user" + usersDTO.UserName); // new message 9Y Cashdrawer is already assigned to the user &1.
                                    SetFooterContent(ErrorMessage, MessageType.Error);
                                    return;
                                }
                                using (NoSynchronizationContextScope.Enter())
                                {
                                    IShiftUseCases shiftUseCases = UserUseCaseFactory.GetShiftUseCases(ExecutionContext);
                                    CashdrawerActivityDTO cashdrawerActivityDTO = new CashdrawerActivityDTO(cashdrawerId, managerId.ToString());
                                    Task<ShiftDTO> task = shiftUseCases.AssignCashdrawer(userShift.ShiftKey, cashdrawerActivityDTO);
                                    task.Wait();
                                    log.LogVariableState("Updated ShiftDTO", task.Result);
                                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 48), MessageType.Info);
                                }
                            }
                        }
                        else
                        {
                            ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "Unable to find the Open/Ropen shift for the user " + usersDTO.LoginId); // new message 8Y
                            SetFooterContent(ErrorMessage, MessageType.Info);
                            return;
                        }
                    }
                    // Refresh UI
                    RefreshCashdrawerButtons(SelectedPOSMachineContainerDTO);
                }
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                string ErrorMessage = MessageContainerList.GetMessage(ExecutionContext, "Assignment failed ");
                SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                IsLoadingVisible = false;
                this.SuccessMessage = ex.Message;
                SetFooterContent(SuccessMessage, string.IsNullOrEmpty(SuccessMessage) ? MessageType.None : MessageType.Error);
            }
        }

        private async Task<List<ShiftDTO>> GetShifts(List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParams)
        {
            log.LogMethodEntry(searchParams);
            IShiftUseCases shiftUseCases = UserUseCaseFactory.GetShiftUseCases(ExecutionContext);
            List<ShiftDTO> shiftListDTO = await shiftUseCases.GetShift(searchParams, false);
            log.LogMethodExit(shiftListDTO);
            return shiftListDTO;
        }

        private void ShowMessagePopup(string heading, string subHeading, string content)
        {
            log.LogMethodEntry(heading, subHeading, content);
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            messagePopupView.Owner = cashdrawerAssignmentView;
            GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext)
            {
                Heading = heading,
                SubHeading = subHeading,
                Content = content,
                CancelButtonText = MessageContainerList.GetMessage(ExecutionContext, "OK"),
                TimerMilliSeconds = 5000,
                PopupType = PopupType.Timer,
            };
            messagePopupView.DataContext = messagePopupVM;
            messagePopupView.ShowDialog();
            log.LogMethodExit();
        }

        private void CloseAddWindow(string message)
        {
            SuccessMessage = message;
            if (cashdrawerAssignmentView != null)
            {
                cashdrawerAssignmentView.Close();
            }
        }
        #endregion Methods
    }

}

/********************************************************************************************************
 * Project Name - POS Redesign
 * Description  - Game Management View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************************
 *2.100.0     25-Sep-2020   Amitha Joy              Created for POS UI Redesign 
 *2.110.0     04-Jan-2021   Mathew Ninan            Changed lookup for OOS reasons and sorted by name 
 *2.120.0     25-Mar-2021   Prajwal                 Container redesign 
 *2.130.0     06-Aug-2021   Abhishek                Adding OOS reason and remarks in screen 
 ********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

using Semnox.Core.Utilities;
using Semnox.Parafait.DigitalSignage;
using Semnox.Parafait.Game;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;
using System.Threading.Tasks;

namespace Semnox.Parafait.GamesUI
{
    public enum StatusCode
    {
        Success,
        Failure
    }

    public class GMMainVM : BaseWindowViewModel
    {
        #region Members
        private int addedMachineId = -1;
        private bool canToggle = false;
        private ObjectType selectedType;
        private int selectedId;
        private List<DisplayParameters> searchParams;
        private LookupsContainerDTO gameMachineStatus;
        private LookupsContainerDTO gameMachineAttributes;

        private Visibility textBlockVisibility;
        private Visibility contentAreaVisibility;
        private Visibility rightSectionContentVisibility;

        private ICommand rightSectionEditCommand;
        private ICommand rightSectionClickedCommand;
        private ICommand buttonGroupCommand;

        private GMMainView mainView;
        private CustomDataGridVM customDataGridVM;
        private GenericRightSectionActionVM genericRightSectionActionVM;

        private ObservableCollection<MachineDTO> machinesList;
        private ObservableCollection<HubDTO> hubsList;
        private ObservableCollection<GameContainerDTO> gamesList;
        private ObservableCollection<ThemeContainerDTO> themesList;

        private bool editMachineAccess;
        private bool editLimitedAccess;
        private bool editConfigAccess;
        private bool addMachineAccess;
        private bool setOosAccess;
        private bool showMachineNameWithGameName;

        private string hubRestartFailureMessage;
        private string setInserviceSuccessMessage;
        private string setInserviceFailureMessage;
        private string setOosSuccessMessage;
        private string setOosFailureMessage;
        private string oldMode;
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
                    customDataGridVM = new CustomDataGridVM(this.ExecutionContext);
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
        public Visibility RightSectionContentVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rightSectionContentVisibility);
                return rightSectionContentVisibility;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref rightSectionContentVisibility, value);
                log.LogMethodExit(rightSectionContentVisibility);
            }
        }

        public Visibility TextBlockVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(textBlockVisibility);
                return textBlockVisibility;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref textBlockVisibility, value);
                log.LogMethodExit(textBlockVisibility);
            }
        }

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

        public ICommand ButtonGroupCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonGroupCommand);
                return buttonGroupCommand;
            }
            private set
            {
                log.LogMethodEntry(value);
                buttonGroupCommand = value;
            }
        }

        public ICommand RightSectionEditCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rightSectionEditCommand);
                return rightSectionEditCommand;
            }
            private set
            {
                log.LogMethodEntry(value);
                rightSectionEditCommand = value;
            }
        }

        public ICommand RightSectionLastButtonCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rightSectionClickedCommand);
                return rightSectionClickedCommand;
            }
            private set
            {
                log.LogMethodEntry(value);
                rightSectionClickedCommand = value;
            }
        }

        public Visibility ContentAreaVisibility
        {
            get
            {
                CheckContentAreaVisibility();
                log.LogMethodExit(contentAreaVisibility);
                return contentAreaVisibility;
            }
            private set
            {
                log.LogMethodEntry(value);
                SetProperty(ref contentAreaVisibility, value);
            }
        }

        public GenericRightSectionActionVM GenericRightSectionActionVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericRightSectionActionVM);
                return genericRightSectionActionVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref genericRightSectionActionVM, value);
            }
        }

        public ObservableCollection<HubDTO> Hubs
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(hubsList);
                return hubsList;
            }
            set
            {
                log.LogMethodEntry(value);
                hubsList = value;
                SetProperty(ref hubsList, value);
            }
        }

        public ObservableCollection<MachineDTO> Machines
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(machinesList);
                return machinesList;
            }
            set
            {
                log.LogMethodEntry(value);
                machinesList = value;
                SetProperty(ref machinesList, value);
            }
        }

        #endregion

        #region Methods        
        private void OnItemSelected(int id)
        {
            log.LogMethodEntry(id);
            this.selectedId = id;
            this.ShowSelectedItem();
            log.LogMethodExit();
        }

        private string GetNumberFormattedString(int count)
        {
            log.LogMethodEntry(count);
            string numberFormattedString = "-";
            if (count >= 0)
            {
                numberFormattedString = count.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT"));
            }
            log.LogMethodExit(numberFormattedString);
            return numberFormattedString;
        }

        private void SetDisplayTagsVM()
        {
            log.LogMethodEntry();
            if (DisplayTagsVM == null)
            {
                DisplayTagsVM = new DisplayTagsVM();
            }
            if (this.selectedType == ObjectType.Machine && this.machinesList != null
                && this.machinesList.Count > 0)
            {
                List<MachineDTO> activeMachineDTOs = this.machinesList.Where(h => h.IsActive.ToLower() == "Y".ToLower()).ToList();

                if (activeMachineDTOs != null && activeMachineDTOs.Count > 0)
                {
                    DisplayTagsVM.DisplayTags = new ObservableCollection<ObservableCollection<DisplayTag>>()
                                    {
                                      new ObservableCollection<DisplayTag>()
                                      {
                                          new DisplayTag()
                                          {
                                              Text = MessageViewContainerList.GetMessage(ExecutionContext,  "Active Machines")
                                          },
                                          new DisplayTag()
                                          {
                                              Text = GetNumberFormattedString(activeMachineDTOs.Count),
                                              TextSize = TextSize.Medium,
                                              FontWeight = FontWeights.Bold
                                          }
                                      },
                                      new ObservableCollection<DisplayTag>()
                                      {
                                          new DisplayTag()
                                          {
                                              Text = MessageViewContainerList.GetMessage(ExecutionContext, "In Service")
                                          },
                                          new DisplayTag()
                                          {
                                              Text = GetNumberFormattedString(activeMachineDTOs.Where( h => h.GameMachineAttributes.FindAll(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE).Select(x => x.AttributeValue=="0").FirstOrDefault()).Count()),
                                              TextSize = TextSize.Medium,
                                              FontWeight = FontWeights.Bold
                                          }
                                      },
                                      new ObservableCollection<DisplayTag>()
                                      {
                                          new DisplayTag()
                                          {
                                              Text = MessageViewContainerList.GetMessage(ExecutionContext, "Out of Service")
                                          },
                                          new DisplayTag()
                                          {
                                              Text = GetNumberFormattedString(activeMachineDTOs.Where( h => h.GameMachineAttributes.FindAll(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE).Select(x => x.AttributeValue!="0").FirstOrDefault()).Count()),
                                              TextSize = TextSize.Medium,
                                              FontWeight = FontWeights.Bold
                                          }
                                      }
                                    };
                }
            }
            else if (this.selectedType == ObjectType.Hub && this.hubsList != null && this.hubsList.Count > 0)
            {
                DisplayTagsVM.DisplayTags = new ObservableCollection<ObservableCollection<DisplayTag>>()
                                            {
                                                new ObservableCollection<DisplayTag>()
                                                {
                                                    new DisplayTag()
                                                    {
                                                      Text = MessageViewContainerList.GetMessage(ExecutionContext,  "Active Hubs")
                                                    },
                                                    new DisplayTag()
                                                    {
                                                        Text = GetNumberFormattedString(this.hubsList.Where(h => h.IsActive == true).Count()),
                                                        TextSize = TextSize.Medium,
                                                        FontWeight = FontWeights.Bold
                                                    }
                                                }
                                            };
            }
            log.LogMethodExit();
        }

        private async void OnRightSectionEditClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            try
            {
                mainView = parameter as GMMainView;
                if (this.selectedType == ObjectType.Machine && this.CustomDataGridVM != null &&
                    this.CustomDataGridVM.SelectedItem != null && this.machinesList != null)
                {
                    log.LogVariableState("Edit machine selected for machine id:", selectedId);
                    SetFooterContent(string.Empty, MessageType.None);
                    MachineDTO editmachine = new MachineDTO();
                    editmachine = this.machinesList.Where(x => x.MachineId == selectedId).FirstOrDefault();
                    if (editmachine != null)
                    {
                        // open edit machine page
                        GMEditMachineView gMEditMachineView = new GMEditMachineView();
                        int editIndex = this.machinesList.IndexOf(editmachine);
                        int editPreviousHubId = editmachine.MasterId;
                        string editPreviousStatus = editmachine.IsActive;
                        GMEditMachineVM gMEditMachineVM = new GMEditMachineVM(editmachine, machinesList, hubsList, gamesList, themesList,
                            editMachineAccess, editLimitedAccess, editConfigAccess, showMachineNameWithGameName, gameMachineStatus, gameMachineAttributes,
                            ExecutionContext, gMEditMachineView.GMEditMachineGridContainer);
                        gMEditMachineView.DataContext = gMEditMachineVM;
                        if (mainView!=null)
                        {
                            gMEditMachineView.Owner = mainView;
                        }
                        gMEditMachineView.ShowDialog();
                        string message = gMEditMachineVM.SuccessMessage;
                        log.LogVariableState("Edit machine status :", message);
                        gMEditMachineView = null;
                        MachineDTO machineDTO = gMEditMachineVM.EditMachineDetails;
                        this.Machines.RemoveAt(editIndex);
                        List<MachineDTO> tempMachineList = await GetMachineList(ExecutionContext, machineDTO.MachineId.ToString(),string.Empty, null);
                        if (tempMachineList != null && tempMachineList.Any())
                        {
                            machineDTO = tempMachineList.FirstOrDefault();
                        }
                        this.Machines.Insert(editIndex, machineDTO);
                        if (editPreviousHubId != machineDTO.MasterId)
                        {
                            int hubindex = this.hubsList.IndexOf(hubsList.Where(x => x.MasterId == machineDTO.MasterId).FirstOrDefault());
                            this.Hubs.RemoveAt(hubindex);
                            List<HubDTO> modifiedHubList = await GetHubList(ExecutionContext, machineDTO.MasterId.ToString(), machineDTO.HubName);
                            HubDTO modifiedHub = modifiedHubList.FirstOrDefault();
                            this.Hubs.Insert(hubindex, modifiedHub);
                            if (editPreviousHubId != -1)
                            {
                                int oldhubindex = this.hubsList.IndexOf(hubsList.Where(x => x.MasterId == editPreviousHubId).FirstOrDefault());
                                this.Hubs.RemoveAt(oldhubindex);
                                List<HubDTO> oldhubList = await GetHubList(ExecutionContext, editPreviousHubId.ToString(), null);
                                HubDTO oldhub = oldhubList.FirstOrDefault();
                                this.Hubs.Insert(oldhubindex, oldhub);
                            }
                        }
                        if (editPreviousStatus != machineDTO.IsActive)
                        {
                            List<MachineDTO> childMachinesList = await GetMachineList(ExecutionContext, null, null, machineDTO.MachineId.ToString());
                            if (childMachinesList != null)
                            {
                                foreach (MachineDTO cm in childMachinesList)
                                {
                                    MachineDTO listItem = this.machinesList.Where(x => x.MachineId == cm.MachineId).FirstOrDefault();
                                    int childMachineIndex = this.machinesList.IndexOf(listItem);
                                    this.Machines.RemoveAt(childMachineIndex);
                                    this.Machines.Insert(childMachineIndex, cm);
                                    if (this.RightSectionContentVisibility != Visibility.Visible)
                                    {
                                        this.RightSectionContentVisibility = Visibility.Visible;
                                        this.OnLeftPaneMenuSelected(LeftPaneVM);
                                    }
                                    else
                                    {
                                        int index = CustomDataGridVM.CollectionToBeRendered.IndexOf(CustomDataGridVM.CollectionToBeRendered.Where(m => (m as MachineDTO).MachineId == cm.MachineId).FirstOrDefault());
                                        if (index >= 0)
                                        {
                                            CustomDataGridVM.CollectionToBeRendered[index] = cm;
                                        }
                                        MachineDTO displayedMachine = (MachineDTO)CustomDataGridVM.UICollectionToBeRendered.Where(m => (m as MachineDTO).MachineId == cm.MachineId).FirstOrDefault();
                                        if (CheckComboFilterForMachine(cm))
                                        {
                                            if (displayedMachine != null)
                                            {
                                                CustomDataGridVM.UICollectionToBeRendered[CustomDataGridVM.UICollectionToBeRendered.IndexOf(displayedMachine)] = cm;
                                            }
                                            else
                                            {
                                                CustomDataGridVM.UICollectionToBeRendered.Add(cm);
                                            }
                                        }
                                        else if (displayedMachine != null)
                                        {
                                            CustomDataGridVM.UICollectionToBeRendered.Remove(displayedMachine);
                                        }
                                    }
                                }
                            }
                        }
                        UpdateMachineStatus(gMEditMachineVM.SuccessMessage, mainView);
                        SetFooterContent(message, string.IsNullOrEmpty(message) ? MessageType.None : MessageType.Info);
                        SetDisplayTagsVM();
                    }
                }
                log.LogMethodExit();
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                SetFooterContent(vex.Message.ToString(), MessageType.Error);
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                throw;
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                SetFooterContent(pax.Message.ToString(), MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
            }
        }
        private bool CheckComboFilterForHub(HubDTO machineDTO)
        {
            log.LogMethodEntry(machineDTO);
            if (CustomDataGridVM == null)
            {
                OnLeftPaneMenuSelected(LeftPaneVM);
                log.LogMethodExit("false");
                return false;
            }
            else
            {
                string allContent = MessageViewContainerList.GetMessage(ExecutionContext, "All", null);
                string firstSelectedItem = CustomDataGridVM.ComboGroupVM.ComboList[0].SelectedItem;
                string secondSelectedItem = CustomDataGridVM.ComboGroupVM.ComboList[1].SelectedItem;
                string activeText = machineDTO.IsActive ? "Active" : "Inactive";
                if (CustomDataGridVM.ComboGroupVM.ComboList.All(x => x.SelectedItem == allContent)
                    || (machineDTO.MasterName == firstSelectedItem && allContent == secondSelectedItem)
                    || (allContent == firstSelectedItem && activeText == secondSelectedItem)
                    || (machineDTO.MasterName == firstSelectedItem && activeText == secondSelectedItem))
                {
                    log.LogMethodExit("true");
                    return true;
                }
                else
                {
                    log.LogMethodExit("false");
                    return false;
                }
            }
        }

        private bool CheckComboFilterForMachine(MachineDTO machineDTO)
        {
            log.LogMethodEntry(machineDTO);
            if (CustomDataGridVM == null)
            {
                OnLeftPaneMenuSelected(LeftPaneVM);
                log.LogMethodExit("false");
                return false;
            }
            else
            {
                string allContent = MessageViewContainerList.GetMessage(ExecutionContext, "All", null);
                string firstSelectedItem = CustomDataGridVM.ComboGroupVM.ComboList[0].SelectedItem;
                string secondSelectedItem = CustomDataGridVM.ComboGroupVM.ComboList[1].SelectedItem;
                string inServiceContent = machineDTO.GameMachineAttributes.FindAll(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE).Select(x => x.AttributeValue == "0" ?
                MessageViewContainerList.GetMessage(ExecutionContext, "In Service", null) :
                MessageViewContainerList.GetMessage(ExecutionContext, "Out of Service", null)).FirstOrDefault();
                string activeContent = gameMachineStatus.LookupValuesContainerDTOList.Where(x => x.LookupValue == machineDTO.IsActive).Select(x => x.Description).FirstOrDefault();
                if (CustomDataGridVM.ComboGroupVM.ComboList.All(x => x.SelectedItem == allContent)
                    || (activeContent == firstSelectedItem && allContent == secondSelectedItem)
                    || (allContent == firstSelectedItem && inServiceContent == secondSelectedItem)
                    || (activeContent == firstSelectedItem && inServiceContent == secondSelectedItem))
                {
                    log.LogMethodExit("true");
                    return true;
                }
                else
                {
                    log.LogMethodExit("false");
                    return false;
                }
            }
        }

        private void SelectMachine(MachineDTO machineDTO, GMMainView mainView)
        {
            log.LogMethodEntry(machineDTO, mainView);
            if (!CheckComboFilterForMachine(machineDTO))
            {
                if (CustomDataGridVM.SelectedItem != null)
                {
                    this.addedMachineId = this.selectedId;
                }
                else if (CustomDataGridVM.UICollectionToBeRendered.Count > 0)
                {
                    this.OnItemSelected((CustomDataGridVM.UICollectionToBeRendered[0] as MachineDTO).MachineId);
                }
            }
            else
            {
                OnItemSelected(machineDTO.MachineId);
            }
            log.LogMethodExit();
        }

        private async void OnRightSectionLastClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            try
            {
                mainView = parameter as GMMainView;
                if (this.selectedType == ObjectType.Hub)
                {
                    log.Info("Restart AP selected for hub id" + selectedId);
                    // restart AP
                    List<HubDTO> hubupdate = this.hubsList.Where(x => x.MasterId == selectedId).ToList();
                    if (hubupdate != null)
                    {
                        HubDTO hubDTO = this.hubsList.Where(x => x.MasterId == selectedId).FirstOrDefault();
                        if (hubDTO != null)
                        {
                            hubDTO.RestartAP = true;
                            log.LogVariableState("Hub Post", hubupdate);
                            IHubUseCases iHubUseCases = GameUseCaseFactory.GetHubUseCases(ExecutionContext);
                            string result = await iHubUseCases.SaveHubs(hubupdate);
                            log.LogVariableState("Hub Post Result", result);
                            if (result == "Success")
                            {
                                CustomDataGridVM.CollectionToBeRendered[CustomDataGridVM.CollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem)] = hubDTO;
                                CustomDataGridVM.UICollectionToBeRendered[CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem)] = hubDTO;
                                if (CustomDataGridVM.SelectedItem == null)
                                {
                                    CustomDataGridVM.SelectedItem = hubDTO;
                                }
                                SetFooterContent(MessageViewContainerList.GetMessage(this.ExecutionContext, 2791, hubupdate.FirstOrDefault().MasterName), MessageType.Info);
                            }
                            else
                            {
                                SetFooterContent(hubRestartFailureMessage, MessageType.Error);
                            }
                        }
                        else
                        {
                            SetFooterContent(hubRestartFailureMessage, MessageType.Error);
                        }
                    }
                    else
                    {
                        SetFooterContent(hubRestartFailureMessage, MessageType.Error);
                    }
                }
                if (this.selectedType == ObjectType.Machine)
                {
                    log.Info("Add new machine selected");
                    SetFooterContent(string.Empty,MessageType.None);
                    // open add machine page
                    GMAddMachineView gMAddMachineView = new GMAddMachineView();
                    if (parameter != null)
                    {
                            gMAddMachineView.Owner = mainView;
                    }
                    GMAddMachineVM gMAddMachineVM = new GMAddMachineVM(machinesList, hubsList, gamesList, themesList, showMachineNameWithGameName, ExecutionContext);
                    gMAddMachineView.DataContext = gMAddMachineVM;
                    gMAddMachineView.ShowDialog();
                    string message = gMAddMachineVM.SuccessMessage;
                    if (!string.IsNullOrEmpty(message) && gMAddMachineVM.StatusCode == StatusCode.Success)
                    {
                        MachineDTO machineDTO = gMAddMachineVM.AddMachineDetails;
                        if (machineDTO != null)
                        {
                            List<MachineDTO> tempmachineList = await GetMachineList(ExecutionContext, machineDTO.MachineId.ToString(), string.Empty ,null);
                            if (tempmachineList != null && tempmachineList.Any())
                            {
                                machineDTO = tempmachineList.FirstOrDefault();
                            }
                            this.Machines.Add(machineDTO);
                            int hubindex = this.hubsList.IndexOf(hubsList.Where(x => x.MasterId == machineDTO.MasterId).FirstOrDefault());
                            this.Hubs.RemoveAt(hubindex);
                            List<HubDTO> modifiedHubList = await GetHubList(ExecutionContext, machineDTO.MasterId.ToString(), machineDTO.HubName);
                            HubDTO modifiedHub = modifiedHubList.FirstOrDefault();
                            this.Hubs.Insert(hubindex, modifiedHub);
                            if (this.Machines.Count == 1)
                            {
                                this.OnLeftPaneMenuSelected(LeftPaneVM);
                            }
                            else
                            {
                                List<object> machineSortedList = CustomDataGridVM.CollectionToBeRendered.ToList();
                                machineSortedList.Add(machineDTO);
                                machineSortedList = machineSortedList.OrderBy(m => (m as MachineDTO).MachineName).ThenBy(m => (m as MachineDTO).GameName).ToList();
                                int machineIndex = machineSortedList.IndexOf(machineDTO);
                                CustomDataGridVM.CollectionToBeRendered.Insert(machineIndex, machineDTO);
                                if (CheckComboFilterForMachine(machineDTO))
                                {
                                    addedMachineId = machineDTO.MachineId;
                                    List<object> sortedList = CustomDataGridVM.UICollectionToBeRendered.ToList();
                                    sortedList.Add(machineDTO);
                                    sortedList = sortedList.OrderBy(m => (m as MachineDTO).MachineName).ThenBy(m => (m as MachineDTO).GameName).ToList();
                                    int index = sortedList.IndexOf(machineDTO);
                                    CustomDataGridVM.UICollectionToBeRendered.Insert(index, machineDTO);
                                    CustomDataGridVM.SelectedItem = machineDTO;
                                }
                                else
                                {
                                    addedMachineId = machineDTO.MachineId;
                                }
                            }
                            gMAddMachineView = null;
                            SetFooterContent(message, string.IsNullOrEmpty(message) ? MessageType.None : MessageType.Info);
                            SetDisplayTagsVM();
                        }
                    }
                    else
                    {
                        SetFooterContent(message, string.IsNullOrEmpty(message) ? MessageType.None : MessageType.Error);
                    }
                }
                log.LogMethodExit();
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                SetFooterContent(vex.Message.ToString(), MessageType.Error);
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                throw;
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                SetFooterContent(pax.Message.ToString(), MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
            }
        }

        private async void OnButtonGroupClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            canToggle = false;
            try
            {
                if (parameter != null && GenericRightSectionActionVM != null && GenericRightSectionActionVM.ButtonGroupVM != null)
                {
                    mainView = parameter as GMMainView;
                    if (GenericRightSectionActionVM.ButtonGroupVM.IsFirstButtonEnabled)
                    {
                        log.Info("Set as in service selected for machine id " + this.selectedId);
                        // set as in service
                        MachineDTO selectedmachine = customDataGridVM.SelectedItem as MachineDTO;
                        int selectedmachineindex = this.machinesList.IndexOf(selectedmachine);
                        if (selectedmachine != null && selectedmachine.GameMachineAttributes != null)
                        {
                            string reason = "";
                            List<MachineAttributeDTO> machineattributeList = selectedmachine.GameMachineAttributes.ToList();
                            if (machineattributeList != null)
                            {
                                if (machineattributeList.Any(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE))
                                {
                                    machineattributeList.Find((x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)).AttributeValue = "0";
                                    machineattributeList.Find((x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)).LastUpdatedBy = ExecutionContext.GetUserId();
                                    List<MachineAttributeLogDTO> mloglist = new List<MachineAttributeLogDTO>();
                                    mloglist.Add(new MachineAttributeLogDTO(-1, selectedmachine.MachineId, ExecutionContext.GetMachineId(), ExecutionContext.POSMachineName, selectedmachine.MachineNameGameNameHubName + " set to in service from POS",
                                        reason, "", false, DateTime.Now, MachineAttributeLogDTO.UpdateTypes.IN_TO_SERVICE.ToString(), null, ExecutionContext.GetSiteId(), false, -1, ExecutionContext.GetUserPKId().ToString(), DateTime.Now, ExecutionContext.GetUserPKId().ToString(), DateTime.Now
                                        ));
                                    machineattributeList.Find((x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)).MachineAttributeLogDTOList = mloglist;
                                }
                                else
                                {
                                    machineattributeList.Clear();
                                    MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE, "0", "Y", "N", MachineAttributeDTO.AttributeContext.MACHINE, "", false, ExecutionContext.GetSiteId(), ExecutionContext.GetUserId(), DateTime.Now, -1, ExecutionContext.GetUserId(), DateTime.Now);
                                    updatedAttribute.IsChanged = true;
                                    List<MachineAttributeLogDTO> mloglist = new List<MachineAttributeLogDTO>();
                                    mloglist.Add(new MachineAttributeLogDTO(-1, selectedmachine.MachineId, ExecutionContext.GetMachineId(), ExecutionContext.POSMachineName, selectedmachine.MachineNameGameNameHubName + " set to in service from POS",
                                        reason, "", false, DateTime.Now, MachineAttributeLogDTO.UpdateTypes.IN_TO_SERVICE.ToString(), null, ExecutionContext.GetSiteId(), false, -1, ExecutionContext.GetUserPKId().ToString(), DateTime.Now, ExecutionContext.GetUserPKId().ToString(), DateTime.Now
                                        ));
                                    updatedAttribute.MachineAttributeLogDTOList = mloglist;
                                    machineattributeList.Add(updatedAttribute);
                                }
                                log.LogVariableState("Machine Attribute", machineattributeList);
                                string result = await PostMachineAttributeAsync(ExecutionContext, machineattributeList.FindAll(x => x.IsChanged == true), selectedId.ToString());
                                if (result == "Success")
                                {
                                    this.Machines.RemoveAt(selectedmachineindex);
                                    List<MachineDTO> tempMachineList = await GetMachineList(ExecutionContext, selectedmachine.MachineId.ToString(), string.Empty, null);
                                    if (tempMachineList != null && tempMachineList.Any())
                                    {
                                        MachineDTO machineDTO = tempMachineList.FirstOrDefault();
                                        this.Machines.Insert(selectedmachineindex, machineDTO);
                                    }
                                    UpdateMachineStatus(setInserviceSuccessMessage, mainView);
                                }
                                else
                                {
                                    SetFooterContent(setInserviceFailureMessage, MessageType.Error);
                                }
                            }
                        }

                    }
                    else if (GenericRightSectionActionVM.ButtonGroupVM.SecondButtonContent == MessageViewContainerList.GetMessage(ExecutionContext, "OUT OF SERVICE")
                         && GenericRightSectionActionVM.ButtonGroupVM.IsSecondButtonEnabled)
                    {
                        OnOutOfService(mainView);
                    }
                    SetDisplayTagsVM();
                }
                log.LogMethodExit();
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                SetFooterContent(vex.Message.ToString(), MessageType.Error);
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                throw;
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                SetFooterContent(pax.Message.ToString(), MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
            }
        }

        private void UpdateMachineStatus(string message, GMMainView mainView)
        {
            log.LogMethodEntry(message, mainView);
            if (machinesList != null && CustomDataGridVM != null && CustomDataGridVM.SelectedItem != null)
            {
                MachineDTO selectedmachine = this.machinesList.Where(x => x.MachineId == selectedId).FirstOrDefault();
                if (selectedmachine != null)
                {
                    CustomDataGridVM.CollectionToBeRendered[CustomDataGridVM.CollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem)] = selectedmachine;
                    CustomDataGridVM.UICollectionToBeRendered[CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem)] = selectedmachine;
                    if (CustomDataGridVM.SelectedItem == null)
                    {
                        if (CheckComboFilterForMachine(selectedmachine))
                        {
                            CustomDataGridVM.SelectedItem = selectedmachine;
                        }
                        else
                        {
                            CustomDataGridVM.UICollectionToBeRendered.Remove(selectedmachine);
                            if (CustomDataGridVM.UICollectionToBeRendered.Count > 0)
                            {
                                CustomDataGridVM.SelectedItem = CustomDataGridVM.UICollectionToBeRendered[0];
                            }
                            else
                            {
                                TextBlockVisibility = Visibility.Visible;
                            }
                        }
                    }
                    SetFooterContent(message, string.IsNullOrEmpty(message) ? MessageType.None : MessageType.Info);
                }
            }
        }
        private void OnOutOfService(GMMainView gMMainView)
        {
            log.LogMethodEntry(gMMainView);
            log.Info("Set as out of service selcted for machine id " + this.selectedId);
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            GenericMessagePopupVM genericMessagePopupVM = new GenericMessagePopupVM(ExecutionContext)
            {
                OkButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "OK", null),
                CancelButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "CANCEL", null),
                MessageButtonsType = MessageButtonsType.OkCancel,
                SubHeading = GenericRightSectionContentVM.Heading,
                Heading = MessageViewContainerList.GetMessage(ExecutionContext, "OUT OF SERVICE", null),
                Content = MessageViewContainerList.GetMessage(ExecutionContext, 567)
            };
            messagePopupView.DataContext = genericMessagePopupVM;
            messagePopupView.Owner = gMMainView;
            messagePopupView.ShowDialog();
            OpenDataEntryView(genericMessagePopupVM, gMMainView);
            log.LogMethodExit();
        }

        private async void OpenDataEntryView(GenericMessagePopupVM genericMessagePopupVM,GMMainView gmMainView)
        {
            log.LogMethodEntry(genericMessagePopupVM);
            if (genericMessagePopupVM != null && genericMessagePopupVM.ButtonClickType == ButtonClickType.Ok)
            {
                LookupsContainerDTO oosreasonlookup;
                ObservableCollection<object> oosreasons = new ObservableCollection<object>();
                oosreasonlookup = GetLookupValues(ExecutionContext, "MACHINE_OOS_REASONS");
                if (oosreasonlookup != null && oosreasonlookup.LookupValuesContainerDTOList != null)
                {
                    GenericDataEntryView dataEntryView = new GenericDataEntryView();
                    oosreasons = new ObservableCollection<object>(oosreasonlookup.LookupValuesContainerDTOList.OrderBy(x => x.LookupValue).ToList());
                    GenericDataEntryVM dataEntryVM = new GenericDataEntryVM(ExecutionContext)
                    {
                        Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Out of Service Reason"),
                        ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 949),
                        DataEntryCollections = new ObservableCollection<DataEntryElement>()
                                {
                                    new DataEntryElement()
                                      {
                                           Type = DataEntryType.ComboBox,
                                           DefaultValue = MessageViewContainerList.GetMessage(ExecutionContext, "Select"),
                                           IsMandatory = true,
                                           IsEditable=true,
                                           IsEnabled = true,
                                           IsReadOnly=false,
                                           Options = oosreasons,
                                           Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Out of Service Reason"),
                                           DisplayMemberPath = "LookupValue"
                                      },
                                    new DataEntryElement()
                                      {
                                           Type = DataEntryType.TextBox,
                                           DefaultValue = MessageViewContainerList.GetMessage(ExecutionContext, "Enter"),
                                           IsMandatory = true,
                                           Heading =  MessageViewContainerList.GetMessage(ExecutionContext, "Remarks"),
                                           ValidationType = CommonUI.ValidationType.None
                                      },
                                }
                    };
                    dataEntryView.DataContext = dataEntryVM;
                    if (gmMainView!=null)
                    {
                        dataEntryView.Owner = gmMainView;
                    }
                    dataEntryView.ShowDialog();
                    await OnDataEntryViewClosedAsync(dataEntryVM);
                }
            }
            else
            {
                canToggle = false;
            }
            log.LogMethodExit();
        }

        private async Task OnDataEntryViewClosedAsync(GenericDataEntryVM dataEntryVM)
        {
            log.LogMethodEntry(dataEntryVM);
            try
            {
                if (dataEntryVM != null)
            {
                if (dataEntryVM.ButtonClickType == ButtonClickType.Ok && machinesList != null)
                {
                    // set out of service
                    MachineDTO selectedmachine = this.machinesList.Where(x => x.MachineId == this.selectedId).FirstOrDefault();
                    int selectedmachineindex = this.machinesList.IndexOf(selectedmachine);
                    if (selectedmachine != null && dataEntryVM.DataEntryCollections != null
                          && selectedmachine.GameMachineAttributes != null)
                    {
                        string reason = (dataEntryVM.DataEntryCollections.Where(x => x.Type == DataEntryType.ComboBox && x.Heading == MessageViewContainerList.GetMessage(ExecutionContext, "Out of Service Reason")).FirstOrDefault().SelectedItem as LookupValuesContainerDTO).LookupValue;
                        string remarks = dataEntryVM.DataEntryCollections.Where(x => x.Type == DataEntryType.TextBox && x.Heading == MessageViewContainerList.GetMessage(ExecutionContext, "Remarks")).FirstOrDefault().Text;
                        List<MachineAttributeDTO> machineattributeList = selectedmachine.GameMachineAttributes.ToList();
                        if (machineattributeList.Any(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE))
                        {
                            machineattributeList.Find((x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)).AttributeValue = "1";
                            //  machineattributeList.Find((x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)).LastUpdatedBy = executionContext.GetUserId();
                            List<MachineAttributeLogDTO> mloglist = new List<MachineAttributeLogDTO>();
                            mloglist.Add(new MachineAttributeLogDTO(-1, selectedmachine.MachineId, ExecutionContext.GetMachineId(), ExecutionContext.POSMachineName, selectedmachine.MachineNameGameNameHubName + " set to out of service from POS",
                                reason, remarks, true, DateTime.Now, MachineAttributeLogDTO.UpdateTypes.OUT_OF_SERVICE.ToString(), null, ExecutionContext.GetSiteId(), false, -1, ExecutionContext.GetUserPKId().ToString(), DateTime.Now, ExecutionContext.GetUserPKId().ToString(), DateTime.Now
                                ));
                            machineattributeList.Find((x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)).MachineAttributeLogDTOList = mloglist;
                        }
                        else
                        {
                            machineattributeList.Clear();
                            MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE, "1", "Y", "N", MachineAttributeDTO.AttributeContext.MACHINE, "", false, ExecutionContext.GetSiteId(), ExecutionContext.GetUserId(), DateTime.Now, -1, ExecutionContext.GetUserId(), DateTime.Now);
                            updatedAttribute.IsChanged = true;
                            List<MachineAttributeLogDTO> mloglist = new List<MachineAttributeLogDTO>();
                            mloglist.Add(new MachineAttributeLogDTO(-1, selectedmachine.MachineId, ExecutionContext.GetMachineId(), ExecutionContext.POSMachineName, selectedmachine.MachineNameGameNameHubName + " set to out of service from POS",
                            reason, remarks, true, DateTime.Now, MachineAttributeLogDTO.UpdateTypes.OUT_OF_SERVICE.ToString(), null, ExecutionContext.GetSiteId(), false, -1, ExecutionContext.GetUserPKId().ToString(), DateTime.Now, ExecutionContext.GetUserPKId().ToString(), DateTime.Now
                            ));
                            updatedAttribute.MachineAttributeLogDTOList = mloglist;
                            machineattributeList.Add(updatedAttribute);
                        }
                        log.LogVariableState("Machine Attribute", machineattributeList);
                        string result = await PostMachineAttributeAsync(ExecutionContext, machineattributeList.FindAll(x => x.IsChanged == true), selectedId.ToString());
                        if (result == "Success")
                        {
                            this.Machines.RemoveAt(selectedmachineindex);
                            List<MachineDTO> listMachineDTO = await GetMachineList(ExecutionContext, selectedmachine.MachineId.ToString(), string.Empty, null);
                            if (listMachineDTO != null && listMachineDTO.Any())
                            {
                               this.Machines.Insert(selectedmachineindex, listMachineDTO.FirstOrDefault());
                            }
                            SetFooterContent(setOosSuccessMessage, string.IsNullOrEmpty(setOosSuccessMessage) ? MessageType.None : MessageType.Info);
                            UpdateMachineStatus(setOosSuccessMessage, mainView);
                        }
                        else
                        {
                            SetFooterContent(setOosFailureMessage, MessageType.Error);
                            canToggle = false;
                        }
                    }
                    else
                    {
                        canToggle = false;
                    }
                }
                }
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                SetFooterContent(vex.Message.ToString(), MessageType.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(ex.Message.ToString(), MessageType.Error);
            }
            log.LogMethodExit();
        }

        private void OnSearchClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithFooter(() =>
            {
                if (parameter != null)
                {
                    mainView = parameter as GMMainView;
                    if (mainView != null)
                    {
                        ContextSearchVM.Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Search", null);
                        ContextSearchVM.SearchText = string.Empty;
                        ContextSearchVM.SearchIndexes = new ObservableCollection<int>() { 0 };
                        List<HubDTO> filteredHubs = null;
                        List<MachineDTO> filteredMachines = null;
                        if (CustomDataGridVM.UICollectionToBeRendered != null)
                        {
                            switch (selectedType)
                            {
                                case ObjectType.Hub:
                                    {
                                        filteredHubs = CustomDataGridVM.UICollectionToBeRendered.Cast<HubDTO>().ToList();
                                        ContextSearchVM.SearchParams = new ObservableCollection<DisplayParameters>(filteredHubs.Select(h => new DisplayParameters()
                                        {
                                            Id = h.MasterId,
                                            ParameterNames = new ObservableCollection<string>() { h.MasterName }
                                        }).ToList());
                                    }
                                    break;
                                case ObjectType.Machine:
                                    {
                                        ContextSearchVM.SearchIndexes = new ObservableCollection<int>() { 0, 1, 2, 3 };
                                        filteredMachines = CustomDataGridVM.UICollectionToBeRendered.Cast<MachineDTO>().ToList();
                                        ContextSearchVM.SearchParams = new ObservableCollection<DisplayParameters>(filteredMachines.Select(m => new DisplayParameters()
                                        {
                                            Id = m.MachineId,
                                            ParameterNames = new ObservableCollection<string> { m.MachineName, m.GameName,
                                        gameMachineStatus.LookupValuesContainerDTOList.Where(x => x.LookupValue == m.IsActive).Select(x => x.Description).FirstOrDefault(),
                                        m.MasterId == -1 ? null : this.Hubs.Where(x => x.MasterId == m.MasterId).FirstOrDefault().MasterName }
                                        }).ToList());
                                    }
                                    break;
                            }
                        }
                        ContextSearchView contextSearchView = new ContextSearchView();
                        contextSearchView.Owner = mainView;
                        contextSearchView.DataContext = ContextSearchVM;
                        contextSearchView.ShowDialog();
                        SetFooterContent(String.Empty, MessageType.None);
                        if (contextSearchView != null && contextSearchView.SelectedId != -1)
                        {
                            if (filteredHubs != null)
                            {
                                CustomDataGridVM.SelectedItem = filteredHubs.FirstOrDefault(x => x.MasterId == contextSearchView.SelectedId);
                            }
                            else if (filteredMachines != null)
                            {
                                CustomDataGridVM.SelectedItem = filteredMachines.FirstOrDefault(x => x.MachineId == contextSearchView.SelectedId);
                            }
                        }
                    }
                }
                log.LogMethodExit();
            });
        }
        private void IsAddedMachineID()
        {
            log.LogMethodEntry();
            if (addedMachineId == -1)
            {
                SetFooterContent(String.Empty, MessageType.None);
            }
            else if (CustomDataGridVM.SelectedItem != null)
            {
                addedMachineId = -1;
            }
            log.LogMethodExit();
        }

        private void OnIsSelected(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithFooter(() =>
            {
                IsAddedMachineID();
                if (CustomDataGridVM != null)
                {
                    if (CustomDataGridVM.SelectedItem != null)
                    {
                        if (CustomDataGridVM.SelectedItem is MachineDTO)
                        {
                            OnItemSelected((CustomDataGridVM.SelectedItem as MachineDTO).MachineId);
                        }
                        else if (CustomDataGridVM.SelectedItem is HubDTO)
                        {
                            OnItemSelected((CustomDataGridVM.SelectedItem as HubDTO).MasterId);
                        }
                    }
                    else if (this.RightSectionContentVisibility == Visibility.Visible)
                    {
                        this.TextBlockVisibility = Visibility.Visible;
                    }
                }
                log.LogMethodExit();
            });
        }

        private void OnPreviousNavigation(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithFooter(() =>
            {
                SetFooterContent(String.Empty, MessageType.None);
                if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null &&
                 CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) > 0)
                {
                    CustomDataGridVM.SelectedItem = CustomDataGridVM.UICollectionToBeRendered[CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) - 1];
                }
                log.LogMethodExit();
            });
        }

        private void OnNextNavigation(object parameter)
        {
            log.LogMethodEntry(parameter);
            ExecuteActionWithFooter(() =>
            {
                SetFooterContent(String.Empty, MessageType.None);
                if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null &&
                   CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) < CustomDataGridVM.UICollectionToBeRendered.Count - 1)
                {
                    CustomDataGridVM.SelectedItem = CustomDataGridVM.UICollectionToBeRendered[CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) + 1];
                }
                log.LogMethodExit();
            });
        }

        private bool CanPreviousNavigationExecute()
        {
            log.LogMethodEntry();
            bool canExecute = true;
            if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null &&
                 CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) == 0)
            {
                canExecute = false;
            }
            log.LogMethodExit(canExecute);
            return canExecute;
        }

        private bool CanNextNavigationExecute()
        {
            log.LogMethodEntry();
            bool canExecute = true;
            if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null &&
               CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) == CustomDataGridVM.UICollectionToBeRendered.Count - 1)
            {
                canExecute = false;
            }
            log.LogMethodExit(canExecute);
            return canExecute;
        }

        private void SetCustomDataGridVM()
        {
            log.LogMethodEntry();
            switch (selectedType)
            {
                case ObjectType.Hub:
                    {
                        this.CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(this.Hubs);
                        this.CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                        {
                            { "MasterName", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Access Point") } },
                            { "MachineCount", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Machine Count") } },
                            { "IsActive", new CustomDataGridColumnElement(){ Converter = new HubStatusConverter(), Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Status") } }
                        };
                    }
                    break;
                case ObjectType.Machine:
                    {
                        CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(this.Machines.OrderBy(m => m.MachineName).ThenBy(m => m.GameName));
                        CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                        {
                            { "MachineName", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Machine Name") } },
                            { "GameName", new CustomDataGridColumnElement(){ Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Game Name") } },
                            { "Description", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Status"),
                            SourcePropertyName = "IsActive", ChildOrSecondarySourcePropertyName = "LookupValue", SecondarySource = new ObservableCollection<object>(gameMachineStatus.LookupValuesContainerDTOList) } },
                            { "GameMachineAttributes", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Connection"),
                            Converter = new MachineStatusConverter(), ConverterParameter = ExecutionContext} },
                            { "MasterName", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Access Point"),
                            SourcePropertyName = "MasterId", ChildOrSecondarySourcePropertyName = "MasterId", SecondarySource = new ObservableCollection<object>(this.Hubs)} },
                        };
                    }
                    break;
            }
            if (CustomDataGridVM.UICollectionToBeRendered.Count <= 0)
            {
                TextBlockVisibility = Visibility.Visible;
            }
            log.LogMethodExit();
        }

        private async void OnLeftPaneMenuSelected(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(String.Empty, MessageType.None);
            if (LeftPaneVM != null)
            {
                this.DisplayTagsVM = null;
                CustomDataGridVM.Clear();
                if (LeftPaneVM.SelectedMenuItem == MessageViewContainerList.GetMessage(ExecutionContext, "Previous View"))
                {
                    OldMode = "Y";
                    log.LogMethodEntry("old mode selected");
                    log.LogVariableState("old mode selected", OldMode);
                    if (parameter != null)
                    {
                        mainView = parameter as GMMainView;

                        if (mainView != null)
                        {
                            log.Info("Game Management screen is closed");
                            mainView.Close();
                        }
                    }

                }
                else if (LeftPaneVM.SelectedMenuItem == MessageViewContainerList.GetMessage(ExecutionContext, "Hubs"))
                {
                    RightSectionContentVisibility = Visibility.Visible;
                    this.selectedType = ObjectType.Hub;
                    if (Hubs != null && Hubs.Count > 0)
                    {
                        SetDisplayTagsVM();
                        ObservableCollection<string> hubscomboList = new ObservableCollection<string>(Hubs.Select(h => h.MasterName));
                        hubscomboList.Add(MessageViewContainerList.GetMessage(ExecutionContext, "All"));
                        ObservableCollection<string> statuscomboList = new ObservableCollection<string>();
                        statuscomboList.Add("Active");
                        statuscomboList.Add("Inactive");
                        statuscomboList.Add(MessageViewContainerList.GetMessage(ExecutionContext, "All"));
                        CustomDataGridVM.ComboGroupVM = new ComboGroupVM()
                        {
                            ComboList = new ObservableCollection<ComboBoxField>()
                            {
                                new ComboBoxField()
                                {
                                    Header = MessageViewContainerList.GetMessage(ExecutionContext, "Access Point"),
                                    Items = hubscomboList,
                                    PropertyName = "MasterName",
                                    SelectedItem = hubscomboList.Last(),
                                },
                                new ComboBoxField()
                                {
                                    Header = MessageViewContainerList.GetMessage(ExecutionContext, "Status"),
                                    Items = statuscomboList,
                                    PropertyName = "IsActive",
                                    SelectedItem = MessageViewContainerList.GetMessage(ExecutionContext, "Active")
                                },
                            },
                        };
                        SetCustomDataGridVM();
                    }
                    else
                    {
                        TextBlockVisibility = Visibility.Visible;
                    }
                }
                else if (LeftPaneVM.SelectedMenuItem == MessageViewContainerList.GetMessage(ExecutionContext, "Machines"))
                {
                    this.selectedType = ObjectType.Machine;
                    if (Machines != null && Machines.Count > 0)
                    {
                        RightSectionContentVisibility = Visibility.Visible;
                        SetDisplayTagsVM();
                        ObservableCollection<string> statuslist = new ObservableCollection<string>(gameMachineStatus.LookupValuesContainerDTOList.Select(l => l.Description));
                        statuslist.Add(MessageViewContainerList.GetMessage(ExecutionContext, "All"));
                        ObservableCollection<string> connectionlist = new ObservableCollection<string>()
                        {
                            MessageViewContainerList.GetMessage(ExecutionContext, "In Service"),
                            MessageViewContainerList.GetMessage(ExecutionContext, "Out of Service"),
                            MessageViewContainerList.GetMessage(ExecutionContext, "All" )
                        };
                        CustomDataGridVM.ComboGroupVM = new ComboGroupVM()
                        {
                            ComboList = new ObservableCollection<ComboBoxField>()
                                 {
                                      new ComboBoxField()
                                      {
                                          Header = MessageViewContainerList.GetMessage(ExecutionContext, "Status"),
                                          PropertyName = "Description",
                                          Items = statuslist,
                                          SelectedItem =MessageViewContainerList.GetMessage(ExecutionContext, "Active")
                                      },
                                     new ComboBoxField() {
                                          Header = MessageViewContainerList.GetMessage(ExecutionContext, "Connection"),
                                          Items = connectionlist,
                                          PropertyName = "GameMachineAttributes",
                                          SelectedItem =connectionlist.Last()
                                      },
                                 },
                        };
                        SetCustomDataGridVM();
                    }
                    else if (addMachineAccess)
                    {
                        if (this.hubsList != null && this.gamesList != null)
                        {
                            GenericRightSectionActionVM = new GenericRightSectionActionVM()
                            {
                                ActionFirstGroupVisibility = Visibility.Collapsed,
                                IsLastButtonEnabled = true,
                                LastButtonContent = MessageViewContainerList.GetMessage(ExecutionContext, "ADD NEW MACHINE")
                            };
                            RightSectionContentVisibility = Visibility.Hidden;
                            TextBlockVisibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        TextBlockVisibility = Visibility.Visible;
                    }
                }
                CheckContentAreaVisibility();
            }
            log.LogMethodExit();
        }


        private void OnLeftPaneNavigationClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(String.Empty, MessageType.None);
            if (parameter != null)
            {
                mainView = parameter as GMMainView;
                if (mainView != null)
                {
                    log.Info("Game Management screen is closed");
                    mainView.Close();
                }
            }
            log.LogMethodExit();
        }

        private void SetGenericRightSection(string heading, string subHeading, ObservableCollection<RightSectionPropertyValues> rightSectionPropertValues,
            string lastButtonContent, Visibility actionFirstGroupVisibility)
        {
            log.LogMethodEntry(heading, subHeading, rightSectionPropertValues,
            lastButtonContent, actionFirstGroupVisibility);
            SetGenericRightSectionContent(heading, subHeading, rightSectionPropertValues, CanPreviousNavigationExecute(),
                CanNextNavigationExecute());
            GenericRightSectionActionVM.LastButtonContent = lastButtonContent;
            GenericRightSectionActionVM.ActionFirstGroupVisibility = actionFirstGroupVisibility;
            GenericRightSectionActionVM.IsEditEnabled = false;
            GenericRightSectionActionVM.IsLastButtonEnabled = false;
            TextBlockVisibility = Visibility.Collapsed;
            log.LogMethodExit();
        }

        private string GetPropertyValueWithFormat(PropertyInfo prop, object propValue)
        {
            log.LogMethodEntry(prop, propValue);
            string value = "-";
            if (propValue != null)
            {
                if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    if (((DateTime)propValue).ToString() != DateTime.MinValue.ToString())
                    {
                        value = ((DateTime)propValue).ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT"));
                    }
                }
                else if (prop.PropertyType == typeof(Int32))
                {
                    value = GetNumberFormattedString((Int32)propValue);
                }
                else if (prop.PropertyType == typeof(Double))
                {
                    value = GetNumberFormattedString(Convert.ToInt32(propValue));
                }
                else if (propValue.ToString() != "-1")
                {
                    value = propValue.ToString();
                }
            }
            log.LogMethodExit(value);
            return value;
        }

        private void ShowSelectedItem()
        {
            log.LogMethodEntry();
            switch (this.selectedType)
            {
                case ObjectType.Hub:
                    {
                        HubDTO hubDTO = this.Hubs.FirstOrDefault(g => g.MasterId == this.selectedId);
                        if (hubDTO != null)
                        {
                            ObservableCollection<RightSectionPropertyValues> rightSectionPropertValues = new ObservableCollection<RightSectionPropertyValues>();

                            Type myType = hubDTO.GetType();
                            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                            foreach (PropertyInfo prop in props.Where(x => x.Name != "SiteId" && x.Name != "SynchStatus" && x.Name != "Guid" && x.Name != "MasterEntityId" && x.Name != "IsChanged"
                            && x.Name != "HubNameWithMachineCount" && x.Name != "EBYTEDTO"
                            ))
                            {
                                rightSectionPropertValues.Add(new RightSectionPropertyValues()
                                {
                                    Property = MessageViewContainerList.GetMessage(ExecutionContext, prop.Name.ToString(), null),
                                    Value = GetPropertyValueWithFormat(prop, prop.GetValue(hubDTO, null))
                                });
                            }
                            if (GenericRightSectionContentVM != null)
                            {
                                SetGenericRightSection(hubDTO.MasterName, hubDTO.IsActive ? "Active" : "Inactive", rightSectionPropertValues
                                    , MessageViewContainerList.GetMessage(ExecutionContext, "RESTART"), Visibility.Collapsed);
                                if (hubDTO.IsActive)
                                {
                                    GenericRightSectionActionVM.IsLastButtonEnabled = true;
                                }
                            }
                        }
                    }
                    break;
                case ObjectType.Machine:
                    {
                        MachineDTO machineDTO = this.Machines.FirstOrDefault(g => g.MachineId == this.selectedId);
                        if (machineDTO != null)
                        {
                            bool isInService = machineDTO.GameMachineAttributes.FindAll(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE).Select(x => x.AttributeValue == "0" ? true : false).FirstOrDefault();
                            ObservableCollection<RightSectionPropertyValues> rightSectionPropertValues = GetMachineRightSectionValues(machineDTO);
                            if (GenericRightSectionContentVM != null)
                            {
                                SetGenericRightSection(machineDTO.MachineName, machineDTO.GameName, rightSectionPropertValues
                                    , MessageViewContainerList.GetMessage(ExecutionContext, "ADD NEW MACHINE"), Visibility.Visible);
                                GenericRightSectionActionVM.EditButtonContent = MessageViewContainerList.GetMessage(ExecutionContext, "EDIT");
                                GenericRightSectionActionVM.ButtonGroupVM = new ButtonGroupVM()
                                {
                                    FirstButtonContent = MessageViewContainerList.GetMessage(ExecutionContext, "IN SERVICE"),
                                    SecondButtonContent = MessageViewContainerList.GetMessage(ExecutionContext, "OUT OF SERVICE")
                                };
                                GenericRightSectionActionVM.ButtonGroupVM.IsFirstButtonEnabled = false;
                                GenericRightSectionActionVM.ButtonGroupVM.IsSecondButtonEnabled = false;
                                if (addMachineAccess)
                                {
                                    GenericRightSectionActionVM.IsLastButtonEnabled = true;
                                }
                                if (editMachineAccess || editLimitedAccess || editConfigAccess)
                                {
                                    GenericRightSectionActionVM.IsEditEnabled = true;
                                }
                                if (setOosAccess)
                                {
                                    GenericRightSectionActionVM.ButtonGroupVM.IsFirstButtonEnabled = !isInService;
                                    GenericRightSectionActionVM.ButtonGroupVM.IsSecondButtonEnabled = isInService;
                                }
                            }
                        }
                    }
                    break;
            }
            log.LogMethodExit();
        }

        private void SetRightSectionValues(MachineDTO machineDTO, PropertyInfo prop, ref ObservableCollection<RightSectionPropertyValues> rightSectionPropertValues
            , bool isMachine, bool isActive, bool isTheme, bool isRefMachineId)
        {
            log.LogMethodEntry(machineDTO, prop, rightSectionPropertValues
            ,isMachine, isActive, isTheme, isRefMachineId);
            object propValue = prop.GetValue(machineDTO, null);
            string value = "-";
            string propname = string.Empty;
            if (isMachine)
            {
                propname = MessageViewContainerList.GetMessage(ExecutionContext, prop.Name.ToString(), null);
                if (propValue != null && propValue.ToString() != "-1")
                {
                    value = propValue.ToString();
                }
            }
            else if (isActive)
            {
                propname = MessageViewContainerList.GetMessage(ExecutionContext, "MachineStatus", null);
                if (propValue != null && propValue.ToString() != "-1")
                {
                    value = gameMachineStatus.LookupValuesContainerDTOList.Where(x => x.LookupValue == propValue.ToString()).FirstOrDefault().Description;
                }
            }
            else if (isTheme)
            {
                propname = MessageViewContainerList.GetMessage(ExecutionContext, "ThemeNumber", null);
                if (propValue != null && propValue.ToString() != "-1")
                {
                    ThemeContainerDTO themeContainerDTO = themesList.FirstOrDefault(x => x.Id.ToString() == propValue.ToString());
                    if(themeContainerDTO != null)
                    {
                        value = themeContainerDTO.ThemeNameWithThemeNumber;
                    }
                    else
                    {
                        log.Debug("Theme is inactive");
                        value = string.Empty;
                    }
                }
            }
            else if (isRefMachineId)
            {
                propname = MessageViewContainerList.GetMessage(ExecutionContext, "ReferenceMachineName", null);
                if (propValue != null && propValue.ToString() != "-1")
                {
                    value = machinesList.Where(x => x.MachineId.ToString() == propValue.ToString()).FirstOrDefault().MachineName;
                }
            }
            else
            {
                propname = MessageViewContainerList.GetMessage(ExecutionContext, prop.Name.ToString(), null);
                value = GetPropertyValueWithFormat(prop, propValue);
            }
            if (!string.IsNullOrEmpty(propname))
            {
                rightSectionPropertValues.Add(new RightSectionPropertyValues()
                {
                    Property = propname,
                    Value = value
                });
            }
            log.LogMethodExit();
        }

        private ObservableCollection<RightSectionPropertyValues> GetMachineRightSectionValues(MachineDTO machineDTO)
        {
            log.LogMethodEntry(machineDTO);
            ObservableCollection<RightSectionPropertyValues> rightSectionPropertValues = new ObservableCollection<RightSectionPropertyValues>();
            Type myType = machineDTO.GetType();
            bool isOutOfService = machineDTO.GameMachineAttributes.FirstOrDefault(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE).AttributeValue == "1" ? true : false;
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            List<string> machineProps = new List<string> { "MachineId", "MachineName", "GameName", "HubName", "SerialNumber", "MachineTag", "MacAddress" };

            foreach (string mprop in machineProps)
            {
                foreach (PropertyInfo prop in props.Where(x => x.Name == mprop))
                {
                    SetRightSectionValues(machineDTO, prop, ref rightSectionPropertValues, true, false, false, false);
                    if (mprop == "MachineName" && isOutOfService)
                    {
                        MachineAttributeLogDTO machineAttributeLogDTO = new MachineAttributeLogDTO();
                        try
                        {
                            using (NoSynchronizationContextScope.Enter())
                            {
                                Task<MachineAttributeLogDTO> t = GetOOSReason(ExecutionContext, machineDTO.MachineId);
                                t.Wait();
                                machineAttributeLogDTO = t.Result;
                            }
                        }
                        catch(Exception ex)
                        {
                            log.Debug("Error in fetching OOS reason for machine id:"+ machineDTO.MachineId);
                        }
                        if (machineAttributeLogDTO != null)
                        {
                            rightSectionPropertValues.Add(new RightSectionPropertyValues()
                            {
                                Property = MessageViewContainerList.GetMessage(ExecutionContext, "Out of Service Reason", null),
                                Value = machineAttributeLogDTO.UserReason
                            });
                            rightSectionPropertValues.Add(new RightSectionPropertyValues()
                            {
                                Property = MessageViewContainerList.GetMessage(ExecutionContext, "Out of Service Remarks", null),
                                Value = machineAttributeLogDTO.UserRemarks
                            });
                        }
                    }
                }
            }
            foreach (PropertyInfo prop in props.Where(x => x.Name == "IsActive"))
            {
                SetRightSectionValues(machineDTO, prop, ref rightSectionPropertValues, false, true, false, false);
            }
            foreach (PropertyInfo prop in props.Where(x => x.Name == "ThemeId"))
            {
                SetRightSectionValues(machineDTO, prop, ref rightSectionPropertValues, false, false, true, false);
            }
            foreach (PropertyInfo prop in props.Where(x => x.Name == "ReferenceMachineId"))
            {
                SetRightSectionValues(machineDTO, prop, ref rightSectionPropertValues, false, false, false, true);
            }
            foreach (PropertyInfo prop in props.Where(x => x.Name != "SiteId" && x.Name != "SynchStatus" && x.Name != "Guid" && x.Name != "MasterEntityId" && x.Name != "IsChanged"
            && x.Name != "CustomDataSetId" && x.Name != "IsChangedRecursive" && x.Name != "MachineTransferLogDTOList" && x.Name != "MachineInputDevicesDTOList"
            && x.Name != "MachineCommunicationLogDTO" && x.Name != "PreviousMachineId" && x.Name != "NextMachineId" && x.Name != "GameMachineAttributes"
            && x.Name != "GameId" && x.Name != "MasterId" && x.Name != "MachineNameGameName" && x.Name != "MachineNameGameNameHubName"
            && x.Name != "MachineNameHubName" && x.Name != "MachineId" && x.Name != "MachineName" && x.Name != "GameName"
            && x.Name != "HubName" && x.Name != "SerialNumber" && x.Name != "MachineTag" && x.Name != "MacAddress" && x.Name != "ReferenceMachineId"
            && x.Name != "ThemeId" && x.Name != "ThemeNumber" && x.Name != "IsActive"))
            {
                SetRightSectionValues(machineDTO, prop, ref rightSectionPropertValues, false, false, false, false);
            }
            log.LogMethodExit(rightSectionPropertValues);
            return rightSectionPropertValues;
        }

        private void CheckContentAreaVisibility()
        {
            log.LogMethodEntry();
            if (this.CustomDataGridVM == null || (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null &&
                CustomDataGridVM.UICollectionToBeRendered.Count == 0))
            {
                ContentAreaVisibility = Visibility.Collapsed;
            }
            else
            {
                ContentAreaVisibility = Visibility.Visible;
            }
            log.LogMethodExit();
        }

        private bool CheckRoleAccess(ExecutionContext executionContext, string formname)
        {
            log.LogMethodEntry(executionContext, formname);
            List<UserContainerDTO> users = UserViewContainerList.GetUserContainerDTOList(executionContext);
            int loggedinroleid = users.Where(x => x.LoginId == executionContext.GetUserId()).Select(x => x.RoleId).FirstOrDefault();
            //            List<UserRoleContainerDTO> userroles = UserRoleViewContainerList.GetUserRoleContainerDTOList(executionContext);
            //            UserRoleContainerDTO userroledto = userroles.FindAll(x => x.RoleId == loggedinroleid).FirstOrDefault();
            //            bool accessallowed = userroledto.ManagementFormAccessContainerDTOList.FindAll(x => x.FormName == formname).Select(x => x.AccessAllowed).FirstOrDefault();
            bool accessallowed = UserRoleViewContainerList.CheckAccess(executionContext.GetSiteId(), loggedinroleid, formname);
            log.LogMethodExit(accessallowed);
            return accessallowed;
        }

        private LookupsContainerDTO GetLookupValues(ExecutionContext executionContext, string lookupname)
        {
            log.LogMethodEntry(executionContext);
            LookupsContainerDTO lookup = LookupsViewContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), lookupname);
            log.LogMethodExit(lookup);
            return lookup;
        }
        private async Task<MachineAttributeLogDTO> GetOOSReason(ExecutionContext executionContext, int machineId)
        {
            MachineAttributeLogDTO result = new MachineAttributeLogDTO();
            List<MachineAttributeLogDTO> machineAttributeLogDTOList;
            log.LogMethodEntry(executionContext, machineId);
            IMachineUseCases iMachineUseCases = GameUseCaseFactory.GetMachineUseCases(executionContext);
            List<KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>> searchparams = new List<KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>>();
            if (machineId >= 0)
            {
                searchparams.Add(new KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>(MachineAttributeLogDTO.SearchByParameters.MACHINE_ID, machineId.ToString()));
                searchparams.Add(new KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>(MachineAttributeLogDTO.SearchByParameters.UPDATE_TYPE, MachineAttributeLogDTO.UpdateTypes.OUT_OF_SERVICE.ToString()));
            }
            machineAttributeLogDTOList = await iMachineUseCases.GetMachineAttributeLogs(searchparams);
            if (machineAttributeLogDTOList != null && machineAttributeLogDTOList.Any())
            {
                result = machineAttributeLogDTOList.OrderByDescending(x => x.Timestamp).FirstOrDefault();
            }
            log.LogMethodExit(result);
            return result;
        }
        private async Task<List<MachineDTO>> GetMachineList(ExecutionContext executionContext, string machineid, string machinename, string referencemachineid)
        {
            List<MachineDTO> machinesList;
            log.LogMethodEntry(executionContext, machineid, machinename, referencemachineid);
            IMachineUseCases iMachineUseCases = GameUseCaseFactory.GetMachineUseCases(executionContext);
            List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchparams = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
            if (machinename != null && machinename != String.Empty)
                searchparams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACHINE_NAME, machinename));
            if (referencemachineid != null && referencemachineid != String.Empty)
                searchparams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.REFERENCE_MACHINE_ID, referencemachineid));
            if (machineid != null && machineid != String.Empty && machineid != "-1")
                searchparams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACHINE_ID, machineid));
            machinesList = await iMachineUseCases.GetMachines(searchparams, true);
            log.LogMethodExit(machinesList);
            return machinesList;
        }
        private List<GameContainerDTO> GetGameList(ExecutionContext executionContext)
        {
            List<GameContainerDTO> gamesList;
            log.LogMethodEntry(executionContext);
            gamesList = GameViewContainerList.GetGameContainerDTOList(executionContext);
            log.LogMethodExit(gamesList);
            return gamesList;
        }
        private async Task<List<HubDTO>> GetHubList(ExecutionContext executionContext, string hubId, string hubName)
        {
            List<HubDTO> hubsList;
            log.LogMethodEntry(executionContext, hubId, hubName);
            IHubUseCases iHubUseCases = GameUseCaseFactory.GetHubUseCases(executionContext);
            List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchparams = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>();
            if (hubId != null && hubId != String.Empty)
                searchparams.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.HUB_ID, hubId));
            if (hubName != null && hubName != String.Empty)
                searchparams.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.HUB_NAME, hubName));
            hubsList = await iHubUseCases.GetHubs(searchparams, true, true);
            log.LogMethodExit(hubsList);
            return hubsList;
        }
        private List<ThemeContainerDTO> GetReaderThemeList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            LookupsContainerDTO readertype = LookupsViewContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), "THEME_TYPE");
            List<ThemeContainerDTO> themes = ThemeViewContainerList.GetThemeContainerDTOList(executionContext);
            List<ThemeContainerDTO> readerthemes = new List<ThemeContainerDTO>();
            foreach (LookupValuesContainerDTO t in readertype.LookupValuesContainerDTOList)
            {
                if (t.LookupValue == "Audio" || t.LookupValue == "Display" || t.LookupValue == "Visualization")
                {
                    foreach (ThemeContainerDTO r in themes.FindAll(x => x.TypeId == t.LookupValueId).ToList())
                    {
                        readerthemes.Add(r);
                    }
                }
            }
            log.LogMethodExit(readerthemes);
            return readerthemes;
        }
        private async Task<string> PostMachineAttributeAsync(ExecutionContext executionContext, List<MachineAttributeDTO> machineattributesList, string moduleId)
        {
            log.LogMethodEntry(executionContext, machineattributesList, moduleId);
            const string moduleName = "MACHINES";
            IReaderConfigurationUseCases iReaderConfigurationUseCases = GameUseCaseFactory.GetReaderConfigurationUseCases(executionContext);
            string result = await iReaderConfigurationUseCases.SaveMachineAttributes(machineattributesList, moduleName, moduleId);
            log.LogMethodExit(result);
            return result;
        }
        #endregion

        #region Constructors
        public GMMainVM(ExecutionContext executionContext)
        {
            log.Info("Game Management screen is opened");
            log.LogMethodEntry(executionContext);
            this.ExecutionContext = executionContext;
            OldMode = "N";
            ExecuteAction(() =>
            {
                LeftPaneMenuSelectedCommand = new DelegateCommand(OnLeftPaneMenuSelected);
                LeftPaneNavigationClickedCommand = new DelegateCommand(OnLeftPaneNavigationClicked);
                NextNavigationCommand = new DelegateCommand(OnNextNavigation);
                SearchCommand = new DelegateCommand(OnSearchClicked);
                IsSelectedCommand = new DelegateCommand(OnIsSelected);
                PreviousNavigationCommand = new DelegateCommand(OnPreviousNavigation);

                rightSectionClickedCommand = new DelegateCommand(OnRightSectionLastClicked);
                rightSectionEditCommand = new DelegateCommand(OnRightSectionEditClicked);
                buttonGroupCommand = new DelegateCommand(OnButtonGroupClicked);

                hubRestartFailureMessage = MessageViewContainerList.GetMessage(this.ExecutionContext, 2792, null);
                setOosSuccessMessage = MessageViewContainerList.GetMessage(this.ExecutionContext, 622, null);
                setOosFailureMessage = MessageViewContainerList.GetMessage(this.ExecutionContext, 2794, null);
                setInserviceSuccessMessage = MessageViewContainerList.GetMessage(this.ExecutionContext, 2598);
                setInserviceFailureMessage = MessageViewContainerList.GetMessage(this.ExecutionContext, 2793, null);
                showMachineNameWithGameName = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "SHOW_GAME_NAME_IN_GAME_MANAGEMENT") == "Y" ? true : false;
            });
            ExecuteAction(() =>
            {
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<HubDTO>> t = GetHubList(executionContext, null, null);
                    t.Wait();
                    List<HubDTO> hubs = t.Result;
                    if (hubs != null)
                    {
                        hubsList = new ObservableCollection<HubDTO>(hubs.OrderBy(x => x.HubNameWithMachineCount));
                    }
                }
            });
            ExecuteAction(() =>
            {
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<MachineDTO>> t = GetMachineList(executionContext, null, null, null);
                    t.Wait();
                    List<MachineDTO> machines = t.Result;
                    if (machines != null)
                    {
                        machinesList = new ObservableCollection<MachineDTO>(machines.OrderBy(x => showMachineNameWithGameName ? x.MachineNameGameName : x.MachineName));
                    }
                }
            });
            ExecuteAction(() =>
            {
                List<GameContainerDTO> games = GetGameList(executionContext);
                if (games != null)
                {
                    gamesList = new ObservableCollection<GameContainerDTO>(games.OrderBy(x => x.GameName));
                }
            });
            ExecuteAction(() =>
            {
                List<ThemeContainerDTO> themes = GetReaderThemeList(executionContext);
                if (themes != null)
                {
                    themesList = new ObservableCollection<ThemeContainerDTO>(themes.OrderBy(x => x.ThemeNameWithThemeNumber));
                }
                gameMachineStatus = GetLookupValues(executionContext, "GAME_MACHINE_STATUS");
                gameMachineAttributes = GetLookupValues(executionContext, "GAME_PROFILE_ATTRIBUTES");

                editMachineAccess = CheckRoleAccess(this.ExecutionContext, "Edit Machine");
                editLimitedAccess = CheckRoleAccess(this.ExecutionContext, "Edit Machine Limited");
                editConfigAccess = CheckRoleAccess(this.ExecutionContext, "Edit Machine Config");
                addMachineAccess = CheckRoleAccess(this.ExecutionContext, "Add Machine");
                setOosAccess = CheckRoleAccess(this.ExecutionContext, "Set Out Of Service");
                FooterVM = new FooterVM(this.ExecutionContext)
                {
                    Message = "",
                    MessageType = MessageType.None,
                    HideSideBarVisibility = Visibility.Visible
                };
            }
            );
            ExecuteAction(() =>
            {
                LeftPaneVM = new LeftPaneVM(this.ExecutionContext)
                {
                    ModuleName = MessageViewContainerList.GetMessage(executionContext, "Game Management", null).ToUpper(),
                    SearchVisibility = Visibility.Collapsed,
                    MenuItems = new ObservableCollection<string>()
                {
                    MessageViewContainerList.GetMessage(executionContext, "Machines"), MessageViewContainerList.GetMessage(executionContext, "Hubs")
                },
                    SelectedMenuItem = MessageViewContainerList.GetMessage(executionContext, "Machines")
                };
                if (SystemOptionViewContainerList.GetSystemOption<bool>(ExecutionContext, "Show Previous View", "Game Management"))
                {
                    LeftPaneVM.MenuItems.Add(MessageViewContainerList.GetMessage(this.ExecutionContext, "Previous View", null));
                }
                GenericRightSectionActionVM = new GenericRightSectionActionVM();
                GenericRightSectionContentVM = new GenericRightSectionContentVM();
                DisplayTagsVM = new DisplayTagsVM();
                textBlockVisibility = Visibility.Visible;
                rightSectionContentVisibility = Visibility.Visible;
            }
            );
            CheckContentAreaVisibility();
            log.LogMethodExit();
        }
        #endregion
    }
}

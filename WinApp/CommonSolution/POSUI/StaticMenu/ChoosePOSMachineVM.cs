/********************************************************************************************
 * Project Name - POSUI
 * Description  - View model class of choose pos machine
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.130.0     8-June-2021      Lakshminarayana      Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.POS;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    public class ChoosePOSMachineVM : ViewModelBase
    {
        #region Members        
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string name;
        private ChoosePOSMachineView choosePOSMachineView;
        private CustomDataGridVM customDataGridVM;
        private POSMachineContainerDTO selectedPOSMachineContainerDTO;
        private ICommand actionsCommand;
        private ICommand loadedCommand;
        #endregion

        #region Properties        
        public string Name
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(name);
                return name;
            }
            set
            {
                log.LogMethodEntry(name, value);
                SetProperty(ref name, value);
                log.LogMethodExit(name);
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
                log.LogMethodEntry(actionsCommand, value);
                SetProperty(ref actionsCommand, value);
                log.LogMethodExit(actionsCommand);
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

        public POSMachineContainerDTO SelectedPOSMachineContainerDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedPOSMachineContainerDTO);
                return selectedPOSMachineContainerDTO;
            }
        }
        #endregion

        #region Methods
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(parameter != null)
            {
                choosePOSMachineView = parameter as ChoosePOSMachineView;
            }
            PerformSearch();
            log.LogMethodExit();
        }
        private void OnActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(parameter != null)
            {
                Button button = parameter as Button;
                if(button != null && !string.IsNullOrEmpty(button.Name))
                {
                    switch(button.Name)
                    {                       
                        case "btnSearch":
                            {
                                PerformSearch();
                            }
                            break;
                        case "btnCancel":
                            {
                                PerformClose();
                            }
                            break;
                        case "btnConfirm":
                            {
                                PerformConfirm();
                            }
                            break;
                    }
                }
            }
            log.LogMethodExit();
        }        
        private void PerformConfirm()
        {
            log.LogMethodEntry();
            if(customDataGridVM.SelectedItem == null)
            {
                log.LogMethodExit(null, "No Items selected");
                return;
            }
            try
            {
                selectedPOSMachineContainerDTO = customDataGridVM.SelectedItem as POSMachineContainerDTO;
            }
            catch (Exception ex)
            {
                log.Error("Error while finding the selected product", ex);
            }
            PerformClose();
            log.LogMethodExit();
        }
        private async void PerformSearch()
        {
            log.LogMethodEntry();
            try
            {
                List<POSMachineContainerDTO> posMachineContainerDTOList = POSMachineViewContainerList.GetPOSMachineContainerDTOList(ExecutionContext);
                ObservableCollection<POSMachineContainerDTO> posMachineDTOObservableCollection = new ObservableCollection<POSMachineContainerDTO>();
                foreach (var posMachineContainerDTO in posMachineContainerDTOList)
                {
                    if(string.IsNullOrWhiteSpace(name) == false && posMachineContainerDTO.POSName.Contains(name) == false)
                    {
                        continue;
                    }
                    posMachineDTOObservableCollection.Add(posMachineContainerDTO);
                }
                customDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(posMachineDTOObservableCollection.Cast<object>().ToList());
            }
            catch (Exception ex)
            {
                log.Error("Error occured while retrieving the product menu panel list", ex);
            }
            
            log.LogMethodExit();
        }
        private void PerformClose()
        {
            log.LogMethodEntry();
            if(choosePOSMachineView != null)
            {
                choosePOSMachineView.Close();
            }
            log.LogMethodExit();
        }
        private void InitalizeCommands()
        {
            log.LogMethodEntry();
            loadedCommand = new DelegateCommand(OnLoaded);
            actionsCommand = new DelegateCommand(OnActionsClicked);
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public ChoosePOSMachineVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.ExecutionContext = executionContext;
            customDataGridVM = new CustomDataGridVM(ExecutionContext) { IsComboAndSearchVisible = false };
            customDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
            {
                { "POSMachineId", new CustomDataGridColumnElement(){ Heading ="Id"} },
                { "POSName", new CustomDataGridColumnElement() { Heading ="POS Name"} },
                { "ComputerName", new CustomDataGridColumnElement() { Heading ="Computer Name"} },
            };

            InitalizeCommands();
            log.LogMethodExit();
        }
        #endregion
    }
}

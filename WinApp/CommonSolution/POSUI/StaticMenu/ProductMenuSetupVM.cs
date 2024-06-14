/********************************************************************************************
 * Project Name - POSUI
 * Description  - View model class of product menu setup
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
using Semnox.Parafait.Product;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    public class ProductMenuSetupVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum LeftPaneSelectedType
        {
            ProductMenu,
            Panels,
        }
        private ProductMenuSetupView productMenuSetupView;
        private bool isposMachineTab;
        private bool showAll;
        private LeftPaneSelectedType selectedLeftPaneSelectedType;
        private CustomDataGridVM customDataGridVM;
        private GenericToggleButtonsVM genericToggleButtonsVM;
        private bool isNextNavigationEnabled;
        private bool isPreviousNavigationEnabled;
        private DisplayTag panelCountDisplayTag;
        private DisplayTag posMachineCountDisplayTag;
        private DeviceClass barcodeReader;

        private ICommand loadedCommand;
        private ICommand toggleCheckedCommand;
        private ICommand onSelectionChangedCommand;
        private ICommand deletePanelClickedCommand;
        private ICommand newPanelClickedCommand;
        private ICommand editPanelClickedCommand;
        private ICommand newProductMenuClickCommand;
        private ICommand editProductMenuClickCommand;
        private ICommand deleteProductMenuClickCommand;
        private ICommand addProductMenuPanelMappingClickCommand;
        private ICommand addPOSMachineMapClickCommand;
        private ICommand deletePanelMappingClickedCommand;
        private ICommand deletePOSMachineMapClickedCommand;
        #endregion

        #region Properties
        public bool IsPosMachineTab
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isposMachineTab);
                return isposMachineTab;
            }
            set
            {
                log.LogMethodEntry(isposMachineTab, value);
                SetProperty(ref isposMachineTab, value);
                log.LogMethodExit(isposMachineTab);
            }
        }

        public bool ShowAll
        {
            get
            {
                return showAll;
            }
            set
            {
                SetProperty(ref showAll, value);
                if (selectedLeftPaneSelectedType == LeftPaneSelectedType.ProductMenu)
                {
                    SetProductMenuDTOList();
                }
                else if (selectedLeftPaneSelectedType == LeftPaneSelectedType.Panels)
                {
                    SetProductMenuPanelDTOList();
                }
            }
        }
        public GenericToggleButtonsVM GenericToggleButtonsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericToggleButtonsVM);
                if (genericToggleButtonsVM == null)
                {
                    genericToggleButtonsVM = GetGenericToggleButtonsVM();
                }
                return genericToggleButtonsVM;
            }
            set
            {
                log.LogMethodEntry(genericToggleButtonsVM, value);
                SetProperty(ref genericToggleButtonsVM, value);
                log.LogMethodExit(genericToggleButtonsVM);
            }
        }

        public CustomDataGridVM CustomDataGridVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(customDataGridVM);
                if (customDataGridVM == null)
                {
                    customDataGridVM = new CustomDataGridVM(ExecutionContext) { IsComboAndSearchVisible = false };
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
        public LeftPaneSelectedType SelectedLeftPaneSelectedType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedLeftPaneSelectedType);
                return selectedLeftPaneSelectedType;
            }
            set
            {
                log.LogMethodEntry(selectedLeftPaneSelectedType, value);
                SetProperty(ref selectedLeftPaneSelectedType, value);
                log.LogMethodExit(selectedLeftPaneSelectedType);
            }
        }
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

        public ICommand DeletePanelClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(deletePanelClickedCommand);
                return deletePanelClickedCommand;
            }
            set
            {
                log.LogMethodEntry(deletePanelClickedCommand, value);
                SetProperty(ref deletePanelClickedCommand, value);
                log.LogMethodExit(deletePanelClickedCommand);
            }
        }

        public ICommand DeletePanelMappingClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(deletePanelMappingClickedCommand);
                return deletePanelMappingClickedCommand;
            }
            set
            {
                log.LogMethodEntry(deletePanelMappingClickedCommand, value);
                SetProperty(ref deletePanelMappingClickedCommand, value);
                log.LogMethodExit(deletePanelMappingClickedCommand);
            }
        }

        public ICommand DeletePOSMachineMapClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(deletePOSMachineMapClickedCommand);
                return deletePOSMachineMapClickedCommand;
            }
            set
            {
                log.LogMethodEntry(deletePOSMachineMapClickedCommand, value);
                SetProperty(ref deletePOSMachineMapClickedCommand, value);
                log.LogMethodExit(deletePOSMachineMapClickedCommand);
            }
        }

        public ICommand NewPanelClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(newPanelClickedCommand);
                return newPanelClickedCommand;
            }
            set
            {
                log.LogMethodEntry(newPanelClickedCommand, value);
                SetProperty(ref newPanelClickedCommand, value);
                log.LogMethodExit(newPanelClickedCommand);
            }
        }

        public ICommand EditPanelClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(editPanelClickedCommand);
                return editPanelClickedCommand;
            }
            set
            {
                log.LogMethodEntry(editPanelClickedCommand, value);
                SetProperty(ref editPanelClickedCommand, value);
                log.LogMethodExit(editPanelClickedCommand);
            }
        }

        public ICommand OnSelectionChangedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(onSelectionChangedCommand);
                return onSelectionChangedCommand;
            }
            set
            {
                log.LogMethodEntry(onSelectionChangedCommand, value);
                SetProperty(ref onSelectionChangedCommand, value);
                log.LogMethodExit();
            }
        }

        public bool IsNextNavigationEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isNextNavigationEnabled);
                return isNextNavigationEnabled;
            }
            set
            {
                log.LogMethodEntry(isNextNavigationEnabled, value);
                SetProperty(ref isNextNavigationEnabled, value);
                log.LogMethodExit(isNextNavigationEnabled);
            }
        }

        public bool IsPreviousNavigationEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isPreviousNavigationEnabled);
                return isPreviousNavigationEnabled;
            }
            set
            {
                log.LogMethodEntry(isPreviousNavigationEnabled, value);
                SetProperty(ref isPreviousNavigationEnabled, value);
                log.LogMethodExit(isPreviousNavigationEnabled);
            }
        }

        public ICommand NewProductMenuClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(newProductMenuClickCommand);
                return newProductMenuClickCommand;
            }
            set
            {
                log.LogMethodEntry(newProductMenuClickCommand, value);
                SetProperty(ref newProductMenuClickCommand, value);
                log.LogMethodExit(newProductMenuClickCommand);
            }
        }


        public ICommand EditProductMenuClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(editProductMenuClickCommand);
                return editProductMenuClickCommand;
            }
            set
            {
                log.LogMethodEntry(editProductMenuClickCommand, value);
                SetProperty(ref editProductMenuClickCommand, value);
                log.LogMethodExit(editProductMenuClickCommand);
            }
        }

        public ICommand DeleteProductMenuClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(deleteProductMenuClickCommand);
                return deleteProductMenuClickCommand;
            }
            set
            {
                log.LogMethodEntry(deleteProductMenuClickCommand, value);
                SetProperty(ref deleteProductMenuClickCommand, value);
                log.LogMethodExit(deleteProductMenuClickCommand);
            }
        }

        public ICommand AddProductMenuPanelMappingClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addProductMenuPanelMappingClickCommand);
                return addProductMenuPanelMappingClickCommand;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref addProductMenuPanelMappingClickCommand, value);
                log.LogMethodExit();
            }
        }

        public ICommand AddPOSMachineMapClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addPOSMachineMapClickCommand);
                return addPOSMachineMapClickCommand;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref addPOSMachineMapClickCommand, value);
                log.LogMethodExit();
            }
        }
        #endregion

        #region Methods

        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                productMenuSetupView = parameter as ProductMenuSetupView;
            }
            log.LogMethodExit();
        }

        private void ClearErrorMessage()
        {
            log.LogMethodEntry();
            if(FooterVM != null)
            {
                FooterVM.Message = string.Empty;
                FooterVM.MessageType = MessageType.None;
            }
            log.LogMethodExit();
        }

        private GenericToggleButtonsVM GetGenericToggleButtonsVM()
        {
            log.LogMethodEntry();

            panelCountDisplayTag = new DisplayTag() { FontWeight = FontWeights.Bold, TextSize = TextSize.Medium };
            posMachineCountDisplayTag = new DisplayTag() { FontWeight = FontWeights.Bold, TextSize = TextSize.Medium };

            SetPanelAndPosMachineCount();

            var result = new GenericToggleButtonsVM()
            {
                ToggleButtonItems = new ObservableCollection<CustomToggleButtonItem>()
                     {
                         new CustomToggleButtonItem() { DisplayTags = new ObservableCollection<DisplayTag>() {
                         new DisplayTag() { Text = MessageViewContainerList.GetMessage(ExecutionContext, "Panels"), TextSize = TextSize.Small, FontWeight = FontWeights.Bold, },
                          panelCountDisplayTag}, Key = "Sub Panels" },
                         new CustomToggleButtonItem() { DisplayTags = new ObservableCollection<DisplayTag>() {
                         new DisplayTag() { Text = MessageViewContainerList.GetMessage(ExecutionContext, "POS Machines"), TextSize = TextSize.Small, FontWeight = FontWeights.Bold, },
                         posMachineCountDisplayTag }, Key = "POS Machines" }
                     }
            };
            log.LogMethodExit(genericToggleButtonsVM);
            return result;
        }

        private void SetPanelAndPosMachineCount()
        {
            ProductMenuViewModel productMenuViewModel = CustomDataGridVM.SelectedItem as ProductMenuViewModel;
            string panelCount = string.Empty;
            string posMachineCount = string.Empty;
            if (productMenuViewModel != null)
            {
                panelCount = productMenuViewModel.PanelCount.ToString();
                posMachineCount = productMenuViewModel.POSMachineCount.ToString();
            }
            panelCountDisplayTag.Text = panelCount;
            posMachineCountDisplayTag.Text = posMachineCount;
        }

        private void OnLeftPaneMenuSelected(object parameter)
        {
            log.LogMethodEntry(parameter);
            CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>();
            if (LeftPaneVM.SelectedMenuItem.ToLower() == MessageViewContainerList.GetMessage(ExecutionContext, "Product Menu").ToLower())
            {
                SelectedLeftPaneSelectedType = LeftPaneSelectedType.ProductMenu;
                CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                {
                    { "MenuId", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Menu Id") } },
                    { "Name", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Name") } },
                    { "Description", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Description") } },
                    { "Type", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Type") } },
                    { "IsActive", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Active?"), Type = DataEntryType.CheckBox, IsReadOnly = true} }
                };
                SetProductMenuDTOList();
            }
            else if (LeftPaneVM.SelectedMenuItem.ToLower() == MessageViewContainerList.GetMessage(ExecutionContext, "Panels").ToLower())
            {
                SelectedLeftPaneSelectedType = LeftPaneSelectedType.Panels;
                CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                {
                    { "PanelId", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Panel Id") } },
                    { "Name", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Name") } },
                    { "IsActive", new CustomDataGridColumnElement() { Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Active?"), Type = DataEntryType.CheckBox, IsReadOnly = true} }
                };
                SetProductMenuPanelDTOList();
            }
            log.LogMethodExit();
        }

        private async void SetProductMenuDTOList(int menuId = -1)
        {
            log.LogMethodEntry(menuId);
            IsLoadingVisible = true;
            ClearErrorMessage();
            
            List<ProductMenuViewModel> productMenuViewModelList = await GetProductMenuViewModelList();
            if(selectedLeftPaneSelectedType != LeftPaneSelectedType.ProductMenu)
            {
                log.LogMethodExit(null, "Different panel is selected");
                return;
            }
            CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(productMenuViewModelList);
            if (productMenuViewModelList.Any())
            {
                int index = 0;
                for (int i = 0; i < productMenuViewModelList.Count; i++)
                {
                    if (productMenuViewModelList[i].ProductMenuDTO.MenuId == menuId)
                    {
                        index = i;
                        break;
                    }
                }
                CustomDataGridVM.SelectedItem = CustomDataGridVM.UICollectionToBeRendered[index];
            }
            IsLoadingVisible = false;
            log.LogMethodExit();
        }

        private async void SetProductMenuPanelDTOList(int panelId = -1)
        {
            log.LogMethodEntry(panelId);
            ClearErrorMessage();
            List<ProductMenuPanelViewModel> productMenuPanelViewModelList = await GetProductMenuPanelViewModelList();
            if (selectedLeftPaneSelectedType != LeftPaneSelectedType.Panels)
            {
                log.LogMethodExit(null, "Different panel is selected");
                return;
            }
            CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(productMenuPanelViewModelList);
            if (productMenuPanelViewModelList.Any())
            {
                int index = 0;
                for (int i = 0; i < productMenuPanelViewModelList.Count; i++)
                {
                    if (productMenuPanelViewModelList[i].PanelId == panelId)
                    {
                        index = i;
                        break;
                    }
                }
                CustomDataGridVM.SelectedItem = CustomDataGridVM.UICollectionToBeRendered[index];
            }
            log.LogMethodExit();
        }

        private async Task<List<ProductMenuViewModel>> GetProductMenuViewModelList()
        {
            log.LogMethodEntry();
            List<ProductMenuViewModel> result = new List<ProductMenuViewModel>();
            bool? isActive = null;
            if (showAll == false)
            {
                isActive = true;
            }
            try
            {
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(ExecutionContext);
                List<ProductMenuDTO> productMenuDTOList = await productMenuUseCases.GetProductMenuDTOList(isActive: isActive.HasValue && isActive.Value? "1": null, loadActiveChildRecords: !showAll, loadChildRecords: true, siteId: ExecutionContext.IsCorporate ? ExecutionContext.SiteId : -1);
                List<ProductMenuPanelDTO> productMenuPanelDTOList = await productMenuUseCases.GetProductMenuPanelDTOList(isActive: isActive.HasValue && isActive.Value ? "1" : null, loadActiveChildRecords: !showAll, loadChildRecords: false, siteId: ExecutionContext.IsCorporate? ExecutionContext.SiteId : -1);
                Dictionary<int, ProductMenuPanelDTO> panelIdProductMenuPanelDTODictionary = new Dictionary<int, ProductMenuPanelDTO>();
                if (productMenuPanelDTOList != null)
                {
                    foreach (var productMenuPanelDTO in productMenuPanelDTOList)
                    {
                        if (panelIdProductMenuPanelDTODictionary.ContainsKey(productMenuPanelDTO.PanelId))
                        {
                            continue;
                        }
                        panelIdProductMenuPanelDTODictionary.Add(productMenuPanelDTO.PanelId, productMenuPanelDTO);
                    }
                }

                IPOSMachineUseCases posMachineUseCases = POSUseCaseFactory.GetPOSMachineUseCases(ExecutionContext);
                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
                if (ExecutionContext.IsCorporate)
                {
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, ExecutionContext.SiteId.ToString()));
                }
                List<POSMachineDTO> posMachineDTOList = await posMachineUseCases.GetPOSMachines(searchParameters, true, true);
                if (posMachineDTOList == null)
                {
                    posMachineDTOList = new List<POSMachineDTO>();
                }
                Dictionary<int, POSMachineDTO> posMachineIdPOSMachineDTODictionary = new Dictionary<int, POSMachineDTO>();
                Dictionary<int, List<ProductMenuPOSMachineMapDTO>> productMenuIdProductMenuPOSMachineMapDTOListDictionary = new Dictionary<int, List<ProductMenuPOSMachineMapDTO>>();
                foreach (var posMachineDTO in posMachineDTOList)
                {
                    if(posMachineIdPOSMachineDTODictionary.ContainsKey(posMachineDTO.POSMachineId))
                    {
                        continue;
                    }
                    posMachineIdPOSMachineDTODictionary.Add(posMachineDTO.POSMachineId, posMachineDTO);
                    if(posMachineDTO.ProductMenuPOSMachineMapDTOList != null)
                    {
                        foreach (var productMenuPOSMachineMapDTO in posMachineDTO.ProductMenuPOSMachineMapDTOList)
                        {
                            if(productMenuIdProductMenuPOSMachineMapDTOListDictionary.ContainsKey(productMenuPOSMachineMapDTO.MenuId) == false)
                            {
                                productMenuIdProductMenuPOSMachineMapDTOListDictionary.Add(productMenuPOSMachineMapDTO.MenuId, new List<ProductMenuPOSMachineMapDTO>());
                            }
                            productMenuIdProductMenuPOSMachineMapDTOListDictionary[productMenuPOSMachineMapDTO.MenuId].Add(productMenuPOSMachineMapDTO);
                        }
                    }
                }
                if (productMenuDTOList != null)
                {
                    foreach (var productMenuDTO in productMenuDTOList)
                    {
                        if (productMenuIdProductMenuPOSMachineMapDTOListDictionary.ContainsKey(productMenuDTO.MenuId) == false)
                        {
                            productMenuIdProductMenuPOSMachineMapDTOListDictionary.Add(productMenuDTO.MenuId, new List<ProductMenuPOSMachineMapDTO>());
                        }
                        ProductMenuViewModel productMenuViewModel = new ProductMenuViewModel(ExecutionContext, productMenuDTO, panelIdProductMenuPanelDTODictionary, productMenuIdProductMenuPOSMachineMapDTOListDictionary[productMenuDTO.MenuId],  posMachineIdPOSMachineDTODictionary);
                        result.Add(productMenuViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }
            log.LogMethodExit(result);
            return result;
        }

        private async Task<List<ProductMenuPanelViewModel>> GetProductMenuPanelViewModelList()
        {
            log.LogMethodEntry();
            List<ProductMenuPanelViewModel> result = new List<ProductMenuPanelViewModel>();
            bool? isActive = null;
            if (showAll == false)
            {
                isActive = true;
            }
            try
            {
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(ExecutionContext);
                List<ProductMenuPanelDTO> productMenuPanelDTOList = await productMenuUseCases.GetProductMenuPanelDTOList(isActive: isActive.HasValue && isActive.Value ? "1" : null, loadActiveChildRecords: !showAll, loadChildRecords: true, siteId: ExecutionContext.IsCorporate ? ExecutionContext.SiteId : -1);
                if (productMenuPanelDTOList != null)
                {
                    foreach (var productMenuPanelDTO in productMenuPanelDTOList)
                    {
                        ProductMenuPanelViewModel productMenuPanelViewModel = new ProductMenuPanelViewModel(ExecutionContext, productMenuPanelDTO);
                        result.Add(productMenuPanelViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }
            log.LogMethodExit(result);
            return result;
        }

        private async void DeletePanelClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            ProductMenuPanelViewModel productMenuPanelViewModel = customDataGridVM.SelectedItem as ProductMenuPanelViewModel;
            if (productMenuPanelViewModel == null)
            {
                return;
            }
            productMenuPanelViewModel.ProductMenuPanelDTO.IsActive = false;
            try
            {
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(ExecutionContext);
                List<ProductMenuPanelDTO> productMenuPanelDTOList = await productMenuUseCases.SaveProductMenuPanelDTOList(new List<ProductMenuPanelDTO>() { productMenuPanelViewModel.ProductMenuPanelDTO });
                SetProductMenuPanelDTOList();
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }
            log.LogMethodExit();
        }

        private async void DeletePanelMappingClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            ProductMenuViewModel productMenuViewModel = customDataGridVM.SelectedItem as ProductMenuViewModel;
            if (productMenuViewModel == null)
            {
                log.LogMethodExit("productMenuViewModel == null");
                return;
            }
            ProductMenuDTO productMenuDTO = productMenuViewModel.ProductMenuDTO;
            if (productMenuDTO == null ||
                productMenuDTO.ProductMenuPanelMappingDTOList == null)
            {
                log.LogMethodExit("productMenuDTO == null || productMenuDTO.ProductMenuPanelMappingDTOList == null");
                return;
            }
            ProductMenuPanelMappingViewModel productMenuPanelMappingViewModel = parameter as ProductMenuPanelMappingViewModel;
            if (productMenuPanelMappingViewModel == null ||
                productMenuPanelMappingViewModel.ProductMenuPanelMappingDTO == null)
            {
                log.LogMethodExit("productMenuPanelMappingViewModel == null || productMenuPanelMappingViewModel.ProductMenuPanelMappingDTO == null");
                return;
            }
            ProductMenuPanelMappingDTO productMenuPanelMappingDTO = productMenuDTO.ProductMenuPanelMappingDTOList.FirstOrDefault(x => x.Id == productMenuPanelMappingViewModel.ProductMenuPanelMappingDTO.Id);
            productMenuPanelMappingDTO.IsActive = false;
            try
            {
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(ExecutionContext);
                List<ProductMenuDTO> productMenuDTOList = await productMenuUseCases.SaveProductMenuDTOList(new List<ProductMenuDTO>() { productMenuDTO });
                SetProductMenuDTOList(productMenuDTO.MenuId);
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }
            log.LogMethodExit();
        }

        private async void DeletePOSMachineMapClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            ProductMenuPOSMachineMapViewModel productMenuPOSMachineMapViewModel = parameter as ProductMenuPOSMachineMapViewModel;
            if (productMenuPOSMachineMapViewModel == null)
            {
                log.LogMethodExit("productMenuPOSMachineMapViewModel == null");
                return;
            }
            ProductMenuPOSMachineMapDTO productMenuPOSMachineMapDTO = productMenuPOSMachineMapViewModel.ProductMenuPOSMachineMapDTO;
            if (productMenuPOSMachineMapDTO == null || 
                productMenuPOSMachineMapDTO.MenuId <= -1 || 
                productMenuPOSMachineMapDTO.POSMachineId <= -1)
            {
                log.LogMethodExit("productMenuPOSMachineMapDTO == null");
                return;
            }

            try
            {
                IPOSMachineUseCases pOSMachineUseCases = POSUseCaseFactory.GetPOSMachineUseCases(ExecutionContext);
                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_MACHINE_ID, productMenuPOSMachineMapDTO.POSMachineId.ToString()));

                List<POSMachineDTO> pOSMachineDTOList = await pOSMachineUseCases.GetPOSMachines(searchParameters, true, true);
                if (pOSMachineDTOList == null || pOSMachineDTOList.Any() == false)
                {
                    log.LogMethodExit("pOSMachineDTOList == null || pOSMachineDTOList.Any() == false");
                    return;
                }
                POSMachineDTO pOSMachineDTO = pOSMachineDTOList[0];
                if (pOSMachineDTO.ProductMenuPOSMachineMapDTOList == null)
                {
                    pOSMachineDTO.ProductMenuPOSMachineMapDTOList = new List<ProductMenuPOSMachineMapDTO>();
                }
                if (pOSMachineDTO.ProductMenuPOSMachineMapDTOList.Any(x => x.MenuId == productMenuPOSMachineMapDTO.MenuId && x.IsActive) == false)
                {
                    log.LogMethodExit("Menu link doesn't exists");
                    return;
                }
                ProductMenuPOSMachineMapDTO toBeDeletedProductMenuPOSMachineMapDTO = pOSMachineDTO.ProductMenuPOSMachineMapDTOList.FirstOrDefault(x => x.MenuId == productMenuPOSMachineMapDTO.MenuId && x.IsActive);
                toBeDeletedProductMenuPOSMachineMapDTO.IsActive = false;
                string result = await pOSMachineUseCases.SavePOSMachines(new List<POSMachineDTO>() { pOSMachineDTO });

                SetProductMenuDTOList(productMenuPOSMachineMapDTO.MenuId);
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }

            log.LogMethodExit();
        }

        private void NewPanelClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            try
            {
                ProductMenuPanelDTO productMenuPanelDTO = new ProductMenuPanelDTO(-1, 5, 0, string.Empty, 2, 2, 2, 2, 4, string.Empty, true);
                ProductMenuPanelSetupView productMenuPanelSetupView = new ProductMenuPanelSetupView();
                ProductMenuPanelSetupViewModel productMenuPanelSetupViewModel = new ProductMenuPanelSetupViewModel(ExecutionContext, productMenuPanelDTO, barcodeReader);
                productMenuPanelSetupView.DataContext = productMenuPanelSetupViewModel;
                productMenuPanelSetupView.ShowDialog();
                SetProductMenuPanelDTOList(productMenuPanelSetupViewModel.ProductMenuPanelDTO.PanelId);
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }
            log.LogMethodExit();
        }

        private void EditPanelClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            ProductMenuPanelViewModel productMenuPanelViewModel = customDataGridVM.SelectedItem as ProductMenuPanelViewModel;
            if (productMenuPanelViewModel == null)
            {
                return;
            }
            try
            {
                ProductMenuPanelSetupView productMenuPanelSetupView = new ProductMenuPanelSetupView();
                ProductMenuPanelSetupViewModel productMenuPanelSetupViewModel = new ProductMenuPanelSetupViewModel(ExecutionContext, productMenuPanelViewModel.ProductMenuPanelDTO, barcodeReader);
                productMenuPanelSetupView.DataContext = productMenuPanelSetupViewModel;
                productMenuPanelSetupView.ShowDialog();
                SetProductMenuPanelDTOList(productMenuPanelViewModel.ProductMenuPanelDTO.PanelId);
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }
            log.LogMethodExit();
        }

        private void OnToggleChecked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            if (genericToggleButtonsVM != null && genericToggleButtonsVM.SelectedToggleButtonItem != null)
            {
                switch (genericToggleButtonsVM.SelectedToggleButtonItem.Key)
                {
                    case "Sub Panels":
                        {
                            IsPosMachineTab = true;
                        }
                        break;
                    case "POS Machines":
                        {
                            IsPosMachineTab = false;
                        }
                        break;
                }
            }
            log.LogMethodExit();
        }
        private void OnLeftpaneNavigationClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            if (parameter != null)
            {
                Window window = parameter as Window;
                if (parameter != null)
                {
                    window.Close();
                }
            }
            log.LogMethodExit();
        }

        private void OnSelectionChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            IsNextNavigationEnabled = CanNextNavigationExecute();
            IsPreviousNavigationEnabled = CanPreviousNavigationExecute();
            if (selectedLeftPaneSelectedType == LeftPaneSelectedType.ProductMenu)
            {
                //GenericToggleButtonsVM = GetGenericToggleButtonsVM();
                SetPanelAndPosMachineCount();
            }
            else if (selectedLeftPaneSelectedType == LeftPaneSelectedType.Panels)
            {
                ProductMenuPanelViewModel productMenuPanelViewModel = customDataGridVM.SelectedItem as ProductMenuPanelViewModel;
                this.GenericRightSectionContentVM = new GenericRightSectionContentVM();
                if (productMenuPanelViewModel != null)
                {
                    GenericRightSectionContentVM.Heading = productMenuPanelViewModel.Name;
                    GenericRightSectionContentVM.PropertyCollections = new ObservableCollection<RightSectionPropertyValues>();
                    GenericRightSectionContentVM.PropertyCollections.Add(new RightSectionPropertyValues() { Property = "Name", Value = productMenuPanelViewModel.Name });
                    GenericRightSectionContentVM.PropertyCollections.Add(new RightSectionPropertyValues() { Property = "Column Count", Value = productMenuPanelViewModel.ColumnCount.ToString() });
                    GenericRightSectionContentVM.PropertyCollections.Add(new RightSectionPropertyValues() { Property = "Row Count", Value = productMenuPanelViewModel.RowCount.ToString() });
                    GenericRightSectionContentVM.PropertyCollections.Add(new RightSectionPropertyValues() { Property = "ImageURL", Value = productMenuPanelViewModel.ImageURL });
                    GenericRightSectionContentVM.PropertyCollections.Add(new RightSectionPropertyValues() { Property = "Display Order", Value = productMenuPanelViewModel.DisplayOrder.ToString() });
                    GenericRightSectionContentVM.PropertyCollections.Add(new RightSectionPropertyValues() { Property = "Cell Margin Left", Value = productMenuPanelViewModel.CellMarginLeft.ToString() });
                    GenericRightSectionContentVM.PropertyCollections.Add(new RightSectionPropertyValues() { Property = "Cell Margin Right", Value = productMenuPanelViewModel.CellMarginRight.ToString() });
                    GenericRightSectionContentVM.PropertyCollections.Add(new RightSectionPropertyValues() { Property = "Cell Margin Top", Value = productMenuPanelViewModel.CellMarginTop.ToString() });
                    GenericRightSectionContentVM.PropertyCollections.Add(new RightSectionPropertyValues() { Property = "Cell Margin Bottom", Value = productMenuPanelViewModel.CellMarginBottom.ToString() });
                    GenericRightSectionContentVM.PropertyCollections.Add(new RightSectionPropertyValues() { Property = "IsActive?", Value = productMenuPanelViewModel.IsActiveString });
                }
                GenericRightSectionContentVM.IsNextNavigationEnabled = IsNextNavigationEnabled;
                GenericRightSectionContentVM.IsPreviousNavigationEnabled = IsPreviousNavigationEnabled;
            }
            log.LogMethodExit();
        }
        private void InitializeCommands()
        {
            log.LogMethodEntry();
            loadedCommand = new DelegateCommand(OnLoaded);
            LeftPaneMenuSelectedCommand = new DelegateCommand(OnLeftPaneMenuSelected);
            LeftPaneNavigationClickedCommand = new DelegateCommand(OnLeftpaneNavigationClicked);
            toggleCheckedCommand = new DelegateCommand(OnToggleChecked);
            onSelectionChangedCommand = new DelegateCommand(OnSelectionChanged);
            deletePanelClickedCommand = new DelegateCommand(DeletePanelClicked);
            newPanelClickedCommand = new DelegateCommand(NewPanelClicked);
            editPanelClickedCommand = new DelegateCommand(EditPanelClicked);
            deletePanelMappingClickedCommand = new DelegateCommand(DeletePanelMappingClicked);
            PreviousNavigationCommand = new DelegateCommand(PreviousNavigationClicked);
            NextNavigationCommand = new DelegateCommand(NextNavigationClicked);
            newProductMenuClickCommand = new DelegateCommand(NewProductMenuClick);
            editProductMenuClickCommand = new DelegateCommand(EditProductMenuClick);
            deleteProductMenuClickCommand = new DelegateCommand(DeleteProductMenuClick);
            addProductMenuPanelMappingClickCommand = new DelegateCommand(AddProductMenuPanelMappingClick);
            AddPOSMachineMapClickCommand = new DelegateCommand(AddPOSMachineMapClick);
            deletePOSMachineMapClickedCommand = new DelegateCommand(DeletePOSMachineMapClicked);
            log.LogMethodExit();
        }

        private void PreviousNavigationClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            ExecuteActionWithFooter(() =>
            {
                SetFooterContent(String.Empty, MessageType.None);
                if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null &&
                 CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) > 0)
                {
                    CustomDataGridVM.SelectedItem = CustomDataGridVM.UICollectionToBeRendered[CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) - 1];
                }
            });
            log.LogMethodExit();
        }

        private void NextNavigationClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            ExecuteActionWithFooter(() =>
            {
                SetFooterContent(String.Empty, MessageType.None);
                if (CustomDataGridVM != null && CustomDataGridVM.UICollectionToBeRendered != null &&
                   CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) < CustomDataGridVM.UICollectionToBeRendered.Count - 1)
                {
                    CustomDataGridVM.SelectedItem = CustomDataGridVM.UICollectionToBeRendered[CustomDataGridVM.UICollectionToBeRendered.IndexOf(CustomDataGridVM.SelectedItem) + 1];
                }
            });
            log.LogMethodExit();
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

        private void NewProductMenuClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            try
            {
                ProductMenuEditView productMenuEditView = new ProductMenuEditView();
                ProductMenuDTO productMenuDTO = new ProductMenuDTO();
                productMenuDTO.Type = "O";
                ProductMenuEditViewModel productMenuEditViewModel = new ProductMenuEditViewModel(ExecutionContext, productMenuDTO);
                productMenuEditView.DataContext = productMenuEditViewModel;
                productMenuEditView.Owner = productMenuSetupView;
                productMenuEditView.ShowDialog();
                productMenuDTO = productMenuEditViewModel.ProductMenuDTO;
                SetProductMenuDTOList(productMenuDTO.MenuId);
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }

            log.LogMethodExit();
        }

        private void EditProductMenuClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            try
            {
                ProductMenuViewModel productMenuViewModel = CustomDataGridVM.SelectedItem as ProductMenuViewModel;
                if (productMenuViewModel == null || productMenuViewModel.ProductMenuDTO == null)
                {
                    log.LogMethodExit("productMenuViewModel == null || productMenuViewModel.ProductMenuDTO == null");
                    return;
                }
                ProductMenuEditView productMenuEditView = new ProductMenuEditView();
                ProductMenuEditViewModel productMenuEditViewModel = new ProductMenuEditViewModel(ExecutionContext, productMenuViewModel.ProductMenuDTO);
                productMenuEditView.DataContext = productMenuEditViewModel;
                productMenuEditView.Owner = productMenuSetupView;
                productMenuEditView.ShowDialog();
                SetProductMenuDTOList(productMenuEditViewModel.ProductMenuDTO.MenuId);
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }

            log.LogMethodExit();
        }

        private async void DeleteProductMenuClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            ProductMenuViewModel productMenuViewModel = customDataGridVM.SelectedItem as ProductMenuViewModel;
            if (productMenuViewModel == null)
            {
                return;
            }
            productMenuViewModel.ProductMenuDTO.IsActive = false;
            try
            {
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(ExecutionContext);
                List<ProductMenuDTO> productMenuDTOList = await productMenuUseCases.SaveProductMenuDTOList(new List<ProductMenuDTO>() { productMenuViewModel.ProductMenuDTO });
                SetProductMenuDTOList();
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }
            log.LogMethodExit();
        }

        private async void AddProductMenuPanelMappingClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            ProductMenuViewModel productMenuViewModel = customDataGridVM.SelectedItem as ProductMenuViewModel;
            if (productMenuViewModel == null)
            {
                log.LogMethodExit("productMenuViewModel == null");
                return;
            }
            ProductMenuDTO productMenuDTO = productMenuViewModel.ProductMenuDTO;
            if (productMenuDTO == null)
            {
                log.LogMethodExit("productMenuDTO == null");
                return;
            }
            ChoosePanelView choosePanelView = new ChoosePanelView();
            ChoosePanelVM choosePanelVM = new ChoosePanelVM(ExecutionContext, barcodeReader);
            choosePanelView.DataContext = choosePanelVM;
            choosePanelView.Owner = productMenuSetupView;
            choosePanelView.ShowDialog();
            if (choosePanelVM.SelectedProductMenuPanelDTO == null)
            {
                log.LogMethodExit("choosePanelVM.SelectedProductMenuPanelDTO == null");
                return;
            }
            ProductMenuPanelMappingDTO productMenuPanelMappingDTO = new ProductMenuPanelMappingDTO(-1, productMenuDTO.MenuId, choosePanelVM.SelectedProductMenuPanelDTO.PanelId, true);
            if (productMenuDTO.ProductMenuPanelMappingDTOList == null)
            {
                productMenuDTO.ProductMenuPanelMappingDTOList = new List<ProductMenuPanelMappingDTO>();
            }
            if(productMenuDTO.ProductMenuPanelMappingDTOList.Any(x => x.PanelId == choosePanelVM.SelectedProductMenuPanelDTO.PanelId && x.IsActive))
            {
                log.LogMethodExit("Panel is already linked");
                return;
            }
            productMenuDTO.ProductMenuPanelMappingDTOList.Add(productMenuPanelMappingDTO);
            try
            {
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(ExecutionContext);
                List<ProductMenuDTO> productMenuDTOList = await productMenuUseCases.SaveProductMenuDTOList(new List<ProductMenuDTO>() { productMenuDTO });
                SetProductMenuDTOList(productMenuDTO.MenuId);
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }
            log.LogMethodExit();
        }

        private async void AddPOSMachineMapClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            ClearErrorMessage();
            ProductMenuViewModel productMenuViewModel = customDataGridVM.SelectedItem as ProductMenuViewModel;
            if (productMenuViewModel == null)
            {
                log.LogMethodExit("productMenuViewModel == null");
                return;
            }
            ProductMenuDTO productMenuDTO = productMenuViewModel.ProductMenuDTO;
            if (productMenuDTO == null)
            {
                log.LogMethodExit("productMenuDTO == null");
                return;
            }
            ChoosePOSMachineView choosePOSMachineView = new ChoosePOSMachineView();
            ChoosePOSMachineVM choosePOSMachineVM = new ChoosePOSMachineVM(ExecutionContext);
            choosePOSMachineView.DataContext = choosePOSMachineVM;
            choosePOSMachineView.Owner = productMenuSetupView;
            choosePOSMachineView.ShowDialog();
            if (choosePOSMachineVM.SelectedPOSMachineContainerDTO == null)
            {
                log.LogMethodExit("choosePOSMachineVM.SelectedPOSMachineDTO == null");
                return;
            }
            try
            {
                IPOSMachineUseCases pOSMachineUseCases = POSUseCaseFactory.GetPOSMachineUseCases(ExecutionContext);
                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_MACHINE_ID, choosePOSMachineVM.SelectedPOSMachineContainerDTO.POSMachineId.ToString()));

                List<POSMachineDTO> pOSMachineDTOList = await pOSMachineUseCases.GetPOSMachines(searchParameters, true, true);
                if (pOSMachineDTOList == null || pOSMachineDTOList.Any() == false)
                {
                    log.LogMethodExit("pOSMachineDTOList == null || pOSMachineDTOList.Any() == false");
                    return;
                }
                POSMachineDTO pOSMachineDTO = pOSMachineDTOList[0];
                if (pOSMachineDTO.ProductMenuPOSMachineMapDTOList == null)
                {
                    pOSMachineDTO.ProductMenuPOSMachineMapDTOList = new List<ProductMenuPOSMachineMapDTO>();
                }
                if (pOSMachineDTO.ProductMenuPOSMachineMapDTOList.Any(x => x.MenuId == productMenuDTO.MenuId && x.IsActive))
                {
                    log.LogMethodExit("Menu is already linked");
                    return;
                }
                ProductMenuPOSMachineMapDTO productMenuPOSMachineMapDTO = new ProductMenuPOSMachineMapDTO(-1, productMenuDTO.MenuId, pOSMachineDTO.POSMachineId, true);
                pOSMachineDTO.ProductMenuPOSMachineMapDTOList.Add(productMenuPOSMachineMapDTO);
                pOSMachineUseCases = POSUseCaseFactory.GetPOSMachineUseCases(ExecutionContext);
                string result = await pOSMachineUseCases.SavePOSMachines(new List<POSMachineDTO>() { pOSMachineDTO });

                SetProductMenuDTOList(productMenuDTO.MenuId);
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public ProductMenuSetupVM(ExecutionContext exectuionContext, DeviceClass barcodeReader)
        {
            log.LogMethodEntry(exectuionContext);

            this.ExecutionContext = exectuionContext;
            this.barcodeReader = barcodeReader;

            this.LeftPaneVM = new LeftPaneVM(ExecutionContext)
            {
                SearchVisibility = Visibility.Collapsed,
                ModuleName = MessageViewContainerList.GetMessage(ExecutionContext, "Setup Product Menu"),
                MenuItems = { MessageViewContainerList.GetMessage(ExecutionContext, "Product Menu") , MessageViewContainerList.GetMessage(ExecutionContext, "Panels") },
                SelectedMenuItem = MessageViewContainerList.GetMessage(ExecutionContext, "Product Menu")
            };
            this.FooterVM = new FooterVM(ExecutionContext)
            {
                Message = string.Empty,
                MessageType = MessageType.None
            };

            SetProductMenuDTOList();
            InitializeCommands();

            log.LogMethodExit();
        }
        #endregion
    }

    public class ProductMenuViewModel : ViewModelBase
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProductMenuDTO productMenuDTO;
        private List<ProductMenuPanelMappingViewModel> productMenuPanelMappingViewModelList;
        private List<ProductMenuPOSMachineMapViewModel> productMenuPOSMachineMapViewModelList;

        #endregion

        #region Properties
        public int MenuId
        {
            get
            {
                return productMenuDTO.MenuId;
            }
            set
            {
                productMenuDTO.MenuId = value;
                OnPropertyChanged("MenuId");
            }
        }
        public string Name
        {
            get
            {
                return productMenuDTO.Name;
            }
            set
            {
                productMenuDTO.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Description
        {
            get
            {
                return productMenuDTO.Description;
            }
            set
            {
                productMenuDTO.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public ProductMenuDTO ProductMenuDTO
        {
            get
            {
                return productMenuDTO;
            }
            set
            {
                productMenuDTO = value;
                OnPropertyChanged("ProductMenuDTO");
            }
        }

        public string Type
        {
            get
            {
                string result = string.Empty;
                if (productMenuDTO.Type == "O")
                {
                    result = MessageViewContainerList.GetMessage(ExecutionContext, "Order Sale");
                }
                else if (productMenuDTO.Type == "B")
                {
                    result = MessageViewContainerList.GetMessage(ExecutionContext, "Reservation");
                }
                else if (productMenuDTO.Type == "R")
                {
                    result = MessageViewContainerList.GetMessage(ExecutionContext, "Redemption");
                }
                return result;
            }
        }

        public bool IsActive
        {
            get
            {
                return productMenuDTO.IsActive;
            }
            set
            {
                productMenuDTO.IsActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        public List<ProductMenuPanelMappingViewModel> ProductMenuPanelMappingViewModelList
        {
            get
            {
                return productMenuPanelMappingViewModelList;
            }
        }


        public List<ProductMenuPOSMachineMapViewModel> ProductMenuPOSMachineMapViewModelList
        {
            get
            {
                return productMenuPOSMachineMapViewModelList;
            }
        }

        public int PanelCount
        {
            get
            {
                int result = 0;
                if (productMenuPanelMappingViewModelList != null &&
                    productMenuPanelMappingViewModelList.Count > 0)
                {
                    result = productMenuPanelMappingViewModelList.Count;
                }
                return result;
            }
        }

        public int POSMachineCount
        {
            get
            {
                int result = 0;
                if (productMenuPOSMachineMapViewModelList != null &&
                    productMenuPOSMachineMapViewModelList.Count > 0)
                {
                    result = productMenuPOSMachineMapViewModelList.Count;
                }
                return result;
            }
        }

        #endregion

        #region Methods
        #endregion

        #region Constructor
        public ProductMenuViewModel(ExecutionContext executionContext, 
                                    ProductMenuDTO productMenuDTO, 
                                    Dictionary<int, ProductMenuPanelDTO> panelIdProductMenuPanelDTODictionary,
                                    List<ProductMenuPOSMachineMapDTO> productMenuPOSMachineMapDTOList,
                                    Dictionary<int, POSMachineDTO> posMachineIdPOSMachineDTODictionary)
        {
            log.LogMethodEntry(executionContext, productMenuDTO);
            ExecutionContext = executionContext;
            this.productMenuDTO = productMenuDTO;

            productMenuPanelMappingViewModelList = new List<ProductMenuPanelMappingViewModel>();
            if (productMenuDTO.ProductMenuPanelMappingDTOList != null)
            {
                foreach (var productMenuPanelMappingDTO in productMenuDTO.ProductMenuPanelMappingDTOList)
                {
                    if (productMenuPanelMappingDTO.IsActive == false)
                    {
                        continue;
                    }
                    string panelName = string.Empty;
                    if (panelIdProductMenuPanelDTODictionary.ContainsKey(productMenuPanelMappingDTO.PanelId))
                    {
                        panelName = panelIdProductMenuPanelDTODictionary[productMenuPanelMappingDTO.PanelId].Name;
                    }
                    productMenuPanelMappingViewModelList.Add(new ProductMenuPanelMappingViewModel(ExecutionContext, productMenuPanelMappingDTO, panelName));
                }
            }
            productMenuPOSMachineMapViewModelList = new List<ProductMenuPOSMachineMapViewModel>();
            if(productMenuPOSMachineMapDTOList != null)
            {
                foreach (var productMenuPOSMachineMapDTO in productMenuPOSMachineMapDTOList)
                {
                    if(posMachineIdPOSMachineDTODictionary.ContainsKey(productMenuPOSMachineMapDTO.POSMachineId) == false)
                    {
                        continue;
                    }
                    ProductMenuPOSMachineMapViewModel productMenuPOSMachineMapViewModel = new ProductMenuPOSMachineMapViewModel(ExecutionContext, productMenuPOSMachineMapDTO, posMachineIdPOSMachineDTODictionary[productMenuPOSMachineMapDTO.POSMachineId].POSName);
                    productMenuPOSMachineMapViewModelList.Add(productMenuPOSMachineMapViewModel);
                }
                
            }
            log.LogMethodExit();
        }
        #endregion
    }

    public class ProductMenuPanelMappingViewModel : ViewModelBase
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProductMenuPanelMappingDTO productMenuPanelMappingDTO;
        private string panelName;

        public ProductMenuPanelMappingViewModel(ExecutionContext executionContext, ProductMenuPanelMappingDTO productMenuPanelMappingDTO, string panelName)
        {
            log.LogMethodEntry(executionContext, productMenuPanelMappingDTO, panelName);
            ExecutionContext = executionContext;
            this.productMenuPanelMappingDTO = productMenuPanelMappingDTO;
            this.panelName = panelName;
            log.LogMethodExit();
        }

        public int PanelId
        {
            get
            {
                return productMenuPanelMappingDTO.PanelId;
            }
        }

        public string PanelName
        {
            get
            {
                return panelName;
            }
        }

        public ProductMenuPanelMappingDTO ProductMenuPanelMappingDTO
        {
            get
            {
                return productMenuPanelMappingDTO;
            }
        }
    }

    public class ProductMenuPOSMachineMapViewModel : ViewModelBase
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProductMenuPOSMachineMapDTO productMenuPOSMachineMapDTO;
        private string posName;

        public ProductMenuPOSMachineMapViewModel(ExecutionContext executionContext, ProductMenuPOSMachineMapDTO productMenuPOSMachineMapDTO, string posName)
        {
            log.LogMethodEntry(executionContext, productMenuPOSMachineMapDTO, posName);
            ExecutionContext = executionContext;
            this.productMenuPOSMachineMapDTO = productMenuPOSMachineMapDTO;
            this.posName = posName;
            log.LogMethodExit();
        }

        public int POSMachineId
        {
            get
            {
                return productMenuPOSMachineMapDTO.POSMachineId;
            }
        }

        public string POSName
        {
            get
            {
                return posName;
            }
        }

        public ProductMenuPOSMachineMapDTO ProductMenuPOSMachineMapDTO
        {
            get
            {
                return productMenuPOSMachineMapDTO;
            }
        }
    }
    public class ProductMenuPanelViewModel : ViewModelBase
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProductMenuPanelDTO productMenuPanelDTO;
        #endregion

        #region Properties

        public int PanelId
        {
            get
            {
                return productMenuPanelDTO.PanelId;
            }
            set
            {
                productMenuPanelDTO.PanelId = value;
                OnPropertyChanged("PanelId");
            }
        }
        public string Name
        {
            get
            {
                return productMenuPanelDTO.Name;
            }
            set
            {
                productMenuPanelDTO.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public int RowCount
        {
            get
            {
                return productMenuPanelDTO.RowCount;
            }
            set
            {
                productMenuPanelDTO.RowCount = value;
                OnPropertyChanged("RowCount");
            }
        }
        public int ColumnCount
        {
            get
            {
                return productMenuPanelDTO.ColumnCount;
            }
            set
            {
                productMenuPanelDTO.ColumnCount = value;
                OnPropertyChanged("ColumnCount");
            }
        }
        public string ImageURL
        {
            get
            {
                return productMenuPanelDTO.ImageURL;
            }
        }

        public int DisplayOrder
        {
            get
            {
                return productMenuPanelDTO.DisplayOrder;
            }
            set
            {
                productMenuPanelDTO.DisplayOrder = value;
                OnPropertyChanged("DisplayOrder");
            }
        }

        public int CellMarginLeft
        {
            get
            {
                return productMenuPanelDTO.CellMarginLeft;
            }
            set
            {
                productMenuPanelDTO.CellMarginLeft = value;
                OnPropertyChanged("CellMarginLeft");
            }
        }

        public int CellMarginRight
        {
            get
            {
                return productMenuPanelDTO.CellMarginRight;
            }
            set
            {
                productMenuPanelDTO.CellMarginRight = value;
                OnPropertyChanged("CellMarginRight");
            }
        }

        public int CellMarginTop
        {
            get
            {
                return productMenuPanelDTO.CellMarginTop;
            }
            set
            {
                productMenuPanelDTO.CellMarginTop = value;
                OnPropertyChanged("CellMarginTop");
            }
        }

        public int CellMarginBottom
        {
            get
            {
                return productMenuPanelDTO.CellMarginBottom;
            }
            set
            {
                productMenuPanelDTO.CellMarginBottom = value;
                OnPropertyChanged("CellMarginBottom");
            }
        }

        public string IsActiveString
        {
            get
            {
                return productMenuPanelDTO.IsActive ? MessageViewContainerList.GetMessage(ExecutionContext, "True") : MessageViewContainerList.GetMessage(ExecutionContext, "False");
            }
        }

        public bool IsActive
        {
            get
            {
                return productMenuPanelDTO.IsActive;
            }
            set
            {
                productMenuPanelDTO.IsActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        public ProductMenuPanelDTO ProductMenuPanelDTO
        {
            get
            {
                return productMenuPanelDTO;
            }
            set
            {
                productMenuPanelDTO = value;
                OnPropertyChanged("ProductMenuPanelDTO");
            }
        }
        #endregion

        #region Methods
        #endregion

        #region Constructor
        public ProductMenuPanelViewModel(ExecutionContext executionContext, ProductMenuPanelDTO productMenuPanelDTO)
        {
            log.LogMethodEntry(executionContext, productMenuPanelDTO);
            ExecutionContext = executionContext;
            this.productMenuPanelDTO = productMenuPanelDTO;
            log.LogMethodExit();
        }
        #endregion
    }

    public class ProductMenuPOSMachineViewModel : ViewModelBase
    {
        #region Members
        private string name;
        private string description;
        private string type;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetProperty(ref name, value);
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                SetProperty(ref description, value);
            }
        }
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                SetProperty(ref type, value);
            }
        }
        #endregion

        #region Methods
        #endregion

        #region Constructor
        #endregion
    }
}

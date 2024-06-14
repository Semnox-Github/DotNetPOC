/********************************************************************************************
 * Project Name - POSUI
 * Description  - View model class of choose panel
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
    public class ChoosePanelVM : ViewModelBase
    {
        #region Members        
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string name;
        private DeviceClass barcodeReader;
        private ChoosePanelView choosePanelView;
        private GenericDisplayItemsVM genericDisplayItemsVM;
        private GenericToggleButtonsVM genericToggleButtonsVM;
        private ProductMenuPanelDTO selectedProductMenuPanelDTO;
        private Dictionary<int, ProductMenuPanelDTO> panelIdProductMenuPanelDTODictionary;
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
        public GenericDisplayItemsVM GenericDisplayItemsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericDisplayItemsVM);
                return genericDisplayItemsVM;
            }
            set
            {
                log.LogMethodEntry(genericDisplayItemsVM, value);
                SetProperty(ref genericDisplayItemsVM, value);
                log.LogMethodExit(genericDisplayItemsVM);
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

        public ProductMenuPanelDTO SelectedProductMenuPanelDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedProductMenuPanelDTO);
                return selectedProductMenuPanelDTO;
            }
        }
        #endregion

        #region Methods
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(parameter != null)
            {
                choosePanelView = parameter as ChoosePanelView;
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
                        case "btnViewPanel":
                            {
                                PerformViewPanel();
                            }
                            break;
                        case "btnNewPanel":
                            {
                                PerformNewPanel();
                            }
                            break;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void PerformNewPanel()
        {
            log.LogMethodEntry();
            try
            {
                ProductMenuPanelDTO productMenuPanelDTO = new ProductMenuPanelDTO(-1, 6, 0, string.Empty, 2, 2, 2, 2, 4, string.Empty, true);
                ProductMenuPanelSetupView productMenuPanelSetupView = new ProductMenuPanelSetupView();
                ProductMenuPanelSetupViewModel productMenuPanelSetupViewModel = new ProductMenuPanelSetupViewModel(ExecutionContext, productMenuPanelDTO, barcodeReader);
                productMenuPanelSetupView.DataContext = productMenuPanelSetupViewModel;
                productMenuPanelSetupView.ShowDialog();
                PerformSearch();
            }
            catch (Exception ex)
            {
                log.Error("Error while finding the selected product", ex);
            }
            log.LogMethodExit();
        }

        private void PerformViewPanel()
        {
            log.LogMethodEntry();
            ProductMenuPanelDTO productMenuPanelDTO = GetSelectedProductMenuPanelDTO();
            if (productMenuPanelDTO == null)
            {
                log.LogMethodExit(null, "No Items selected");
                return;
            }
            try
            {
                selectedProductMenuPanelDTO = productMenuPanelDTO;
                ProductMenuPanelSetupView productMenuPanelSetupView = new ProductMenuPanelSetupView();
                ProductMenuPanelSetupViewModel productMenuPanelSetupViewModel = new ProductMenuPanelSetupViewModel(ExecutionContext, selectedProductMenuPanelDTO, barcodeReader);
                productMenuPanelSetupView.DataContext = productMenuPanelSetupViewModel;
                productMenuPanelSetupView.ShowDialog();
                PerformSearch();
            }
            catch (Exception ex)
            {
                log.Error("Error while finding the selected product", ex);
            }
            log.LogMethodExit();
        }

        private void PerformConfirm()
        {
            log.LogMethodEntry();
            ProductMenuPanelDTO productMenuPanelDTO = GetSelectedProductMenuPanelDTO();
            if (productMenuPanelDTO == null)
            {
                log.LogMethodExit(null, "No Items selected");
                return;
            }
            try
            {
                selectedProductMenuPanelDTO = productMenuPanelDTO;
            }
            catch (Exception ex)
            {
                log.Error("Error while finding the selected product", ex);
            }
            PerformClose();
            log.LogMethodExit();
        }

        private ProductMenuPanelDTO GetSelectedProductMenuPanelDTO()
        {
            log.LogMethodEntry();
            ProductMenuPanelDTO result = null;
            if (genericToggleButtonsVM.SelectedToggleButtonItem == null)
            {
                log.LogMethodExit(result, "genericToggleButtonsVM.SelectedToggleButtonItem == null");
                return result;
            }
            try
            {
                string idString = genericToggleButtonsVM.SelectedToggleButtonItem.Key;
                int panelId = Convert.ToInt32(idString);
                if(panelIdProductMenuPanelDTODictionary.ContainsKey(panelId))
                {
                    result = panelIdProductMenuPanelDTODictionary[panelId];
                }
            }
            catch (Exception)
            {
                result = null;
            }
            
            log.LogMethodExit(result);
            return result;
        }

        private async void PerformSearch()
        {
            log.LogMethodEntry();
            try
            {
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(ExecutionContext);
                List<ProductMenuPanelDTO> productMenuPanelDTOList = await productMenuUseCases.GetProductMenuPanelDTOList(isActive: "1", name: name, siteId: ExecutionContext.IsCorporate ? ExecutionContext.SiteId : -1);
                if (productMenuPanelDTOList == null)
                {
                    productMenuPanelDTOList = new List<ProductMenuPanelDTO>();
                }
                panelIdProductMenuPanelDTODictionary = new Dictionary<int, ProductMenuPanelDTO>();
                foreach (var productMenuPanelDTO in productMenuPanelDTOList)
                {
                    if(panelIdProductMenuPanelDTODictionary.ContainsKey(productMenuPanelDTO.PanelId))
                    {
                        continue;
                    }
                    panelIdProductMenuPanelDTODictionary.Add(productMenuPanelDTO.PanelId, productMenuPanelDTO);
                }
                genericDisplayItemsVM.DisplayItemModels = new System.Collections.ObjectModel.ObservableCollection<object>(productMenuPanelDTOList.Cast<object>().ToList());

                genericToggleButtonsVM.ToggleButtonItems = new ObservableCollection<CustomToggleButtonItem>(
                   productMenuPanelDTOList.Select(p => new CustomToggleButtonItem()
                   {
                       Key = p.PanelId.ToString(),
                       DisplayTags = new ObservableCollection<DisplayTag>() {
               new DisplayTag() { Text = p.Name} }
                   }));
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
            if(choosePanelView != null)
            {
                choosePanelView.Close();
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
        public  ChoosePanelVM(ExecutionContext executionContext, DeviceClass barcodeReader)
        {
            log.LogMethodEntry(executionContext);
            this.ExecutionContext = executionContext;
            this.barcodeReader = barcodeReader;
            genericDisplayItemsVM = new GenericDisplayItemsVM(executionContext)
            {
                PropertyAndValueCollection = new Dictionary<string, string>() { { "Name", "" } },
                ButtonType = ButtonType.None,
            };
            genericToggleButtonsVM = new GenericToggleButtonsVM() { IsVerticalOrientation = true, Columns = 5 };

            InitalizeCommands();
            log.LogMethodExit();
        }
        #endregion
    }
}

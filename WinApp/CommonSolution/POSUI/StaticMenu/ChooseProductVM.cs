/********************************************************************************************
 * Project Name - POSUI
 * Description  - View model class of choose products
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.130.0     8-June-2021      Lakshminarayana      Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Product;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    class ChooseProductVM : ViewModelBase
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int selectedProductTypeId = -1;
        private DeviceClass barcodeReader;
        private int selectedCategoryId = -1;
        private List<KeyValuePair<int, string>> productTypeList;
        private List<KeyValuePair<int, string>> categoryList;
        private string name;

        private ProductsContainerDTO selectedProductsContainerDTO;

        private ChooseProductView chooseProductView;
        private GenericDisplayItemsVM genericDisplayItemsVM;
        private GenericToggleButtonsVM genericToggleButtonsVM;

        private ICommand actionsCommand;
        private ICommand loadedCommand;

        #endregion

        #region Properties
        public List<KeyValuePair<int, string>> ProductTypeList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(productTypeList);
                return productTypeList;
            }
            set
            {
                log.LogMethodEntry(productTypeList, value);
                SetProperty(ref productTypeList, value);
                log.LogMethodExit(productTypeList);
            }
        }
        public List<KeyValuePair<int, string>> CategoryList
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(categoryList);
                return categoryList;
            }
            set
            {
                log.LogMethodEntry(categoryList, value);
                SetProperty(ref categoryList, value);
                log.LogMethodExit();
            }
        }
        public int SelectedProductTypeId
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedProductTypeId);
                return selectedProductTypeId;
            }
            set
            {
                log.LogMethodEntry(selectedProductTypeId, value);
                SetProperty(ref selectedProductTypeId, value);
                log.LogMethodExit(selectedProductTypeId);
            }
        }
        public int SelectedCategoryId
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedCategoryId);
                return selectedCategoryId;
            }
            set
            {
                log.LogMethodEntry(selectedCategoryId, value);
                SetProperty(ref selectedCategoryId, value);
                log.LogMethodExit(selectedCategoryId);
            }
        }
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

        public ProductsContainerDTO SelectedProductsContainerDTO
        {
            get
            {
                return selectedProductsContainerDTO;
            }
        }
        #endregion

        #region Methods
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                chooseProductView = parameter as ChooseProductView;
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
                        case "btnClear":
                            {
                                PerformClear();
                            }
                            break;
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
        private void PerformClear()
        {
            log.LogMethodEntry();
            SelectedCategoryId = -1;
            SelectedProductTypeId = -1;
            Name = string.Empty;
            selectedProductsContainerDTO = null;
            log.LogMethodExit();
        }
        private void PerformConfirm()
        {
            log.LogMethodEntry();
            try
            {
                string idString = genericToggleButtonsVM.SelectedToggleButtonItem.Key;
                selectedProductsContainerDTO = ProductViewContainerList.GetProductsContainerDTO(ExecutionContext, ManualProductType.SELLABLE.ToString(), Convert.ToInt32(idString));
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
                List<ProductsContainerDTO> productsContainerDTOList = await GetProductsContainerDTOList(Name, SelectedProductTypeId, SelectedCategoryId);
                genericDisplayItemsVM.DisplayItemModels = new System.Collections.ObjectModel.ObservableCollection<object>(productsContainerDTOList.Cast<object>().ToList());

                genericToggleButtonsVM.ToggleButtonItems = new ObservableCollection<CustomToggleButtonItem>(
                   productsContainerDTOList.Select(p => new CustomToggleButtonItem()
                   {
                       Key = p.ProductId.ToString(),
                       DisplayTags = new ObservableCollection<DisplayTag>() {
               new DisplayTag() { Text = p.ProductName} }
                   }));
            }
            catch (Exception ex)
            {

                GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                messagePopupView.Owner = chooseProductView;
                messagePopupView.DataContext = new GenericMessagePopupVM(ExecutionContext)
                {
                    Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Error"),
                    //SubHeading = MessageViewContainerList.GetMessage(ExecutionContext, "Required fields"),
                    Content = ex.Message,
                    CancelButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "OK"),
                    TimerMilliSeconds = 5000,
                    PopupType = PopupType.Timer,
                };

                messagePopupView.ShowDialog();
            }

            log.LogMethodExit();
        }

        private async Task<List<ProductsContainerDTO>> GetProductsContainerDTOList(string nameOrDescription, int productTypeId, int categoryId)
        {
            return await Task<List<ProductsContainerDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry();
                List<ProductsContainerDTO> productsContainerDTOList = ProductViewContainerList.GetActiveProductsContainerDTOList(ExecutionContext, ManualProductType.SELLABLE.ToString());
                IEnumerable<ProductsContainerDTO> query = productsContainerDTOList;
                if (productTypeId != -1)
                {
                    query = query.Where(x => x.ProductTypeId == productTypeId);
                }
                if (categoryId != -1)
                {
                    CategoryContainerDTO categoryContainerDTO = CategoryViewContainerList.GetCategoryContainerDTO(ExecutionContext, categoryId);
                    query = query.Where(x => x.CategoryId == categoryId || (categoryContainerDTO.ChildCategoryIdList != null && categoryContainerDTO.ChildCategoryIdList.Contains(x.CategoryId)));
                }
                if (string.IsNullOrWhiteSpace(nameOrDescription) == false)
                {
                    query = query.Where(x => ContainsSearchText(x, nameOrDescription));
                }

                List<ProductsContainerDTO> result = query.ToList();
                log.LogMethodExit(result);
                return result;
            });
        }

        private bool ContainsSearchText(ProductsContainerDTO productsContainerDTO, string nameOrDescription)
        {
            log.LogMethodEntry(productsContainerDTO, nameOrDescription);
            bool result = false;
            if (string.IsNullOrWhiteSpace(productsContainerDTO.ProductName) == false &&
                productsContainerDTO.ProductName.IndexOf(nameOrDescription, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                result = true;
                log.LogMethodExit(result);
            }
            if (string.IsNullOrWhiteSpace(productsContainerDTO.Description) == false &&
                productsContainerDTO.Description.IndexOf(nameOrDescription, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                result = true;
                log.LogMethodExit(result);
            }
            if (productsContainerDTO.InventoryItemContainerDTO != null &&
                productsContainerDTO.InventoryItemContainerDTO.ProductBarcodeContainerDTOList != null &&
                productsContainerDTO.InventoryItemContainerDTO.ProductBarcodeContainerDTOList.Any(x => x.BarCode == nameOrDescription))
            {
                result = true;
                log.LogMethodExit(result);
            }
            log.LogMethodExit(result);
            return result;
        }

        private void PerformClose()
        {
            log.LogMethodEntry();
            if(barcodeReader != null)
            {
                barcodeReader.UnRegister();
            }
            if (chooseProductView != null)
            {
                chooseProductView.Close();
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

        private void SetCategoryList()
        {
            log.LogMethodEntry();
            List<CategoryContainerDTO> categoryContainerDTOList = CategoryViewContainerList.GetCategoryContainerDTOList(ExecutionContext);
            List<KeyValuePair<int, string>> categoryList = new List<KeyValuePair<int, string>>();
            categoryList.Add(new KeyValuePair<int, string>(-1, MessageViewContainerList.GetMessage(ExecutionContext, "SELECT")));
            foreach (var categoryContainerDTO in categoryContainerDTOList)
            {
                categoryList.Add(new KeyValuePair<int, string>(categoryContainerDTO.CategoryId, categoryContainerDTO.Name));
            }
            CategoryList = categoryList;
            log.LogMethodExit();
        }

        private void SetProductTypeList()
        {
            log.LogMethodEntry();
            List<ProductTypeContainerDTO> productTypeContainerDTOList = ProductTypeViewContainerList.GetProductTypeContainerDTOList(ExecutionContext);
            List<KeyValuePair<int, string>> productTypeList = new List<KeyValuePair<int, string>>();
            productTypeList.Add(new KeyValuePair<int, string>(-1, MessageViewContainerList.GetMessage(ExecutionContext, "SELECT")));
            foreach (var productTypeContainerDTO in productTypeContainerDTOList)
            {
                productTypeList.Add(new KeyValuePair<int, string>(productTypeContainerDTO.ProductTypeId, productTypeContainerDTO.ProductType));
            }
            ProductTypeList = productTypeList;
            log.LogMethodExit();
        }

        #endregion

        #region Constructor
        public ChooseProductVM(ExecutionContext executionContext, DeviceClass barcodeReader)
        {
            log.LogMethodEntry(executionContext);
            this.ExecutionContext = executionContext;
            this.barcodeReader = barcodeReader;
            if(barcodeReader != null)
            {
                barcodeReader.Register(new EventHandler(BarcodeScanCompleteEventHandle));
            }
            
            genericDisplayItemsVM = new GenericDisplayItemsVM(executionContext)
            {
                PropertyAndValueCollection = new Dictionary<string, string>() { { "Name", "" } },
                ButtonType = ButtonType.None,
            };
            genericToggleButtonsVM = new GenericToggleButtonsVM() { IsVerticalOrientation = true, Columns = 5 };


            PerformClear();

            SetProductTypeList();
            SetCategoryList();

            InitalizeCommands();
            PerformSearch();
            log.LogMethodExit();
        }

        private void BarcodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                log.Debug("Scanned barcode: " + checkScannedEvent.Message);
                Name = checkScannedEvent.Message;
                PerformSearch();
            }
            log.LogMethodExit();
        }


        #endregion
    }
}

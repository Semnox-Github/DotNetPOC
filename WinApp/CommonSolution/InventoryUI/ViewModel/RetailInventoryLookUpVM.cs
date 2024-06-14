/********************************************************************************************
 * Project Name - Retail Inventory Lookup UI
 * Description  - RetailInventoryLookup VM
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *            08-May-2023   Prashanth               Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Product;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Semnox.Parafait.InventoryUI
{
    public class RetailInventoryLookUpVM : BaseWindowViewModel
    {
        #region members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DeviceClass barcodeReader;
        private string dateTimeFormat;
        private string numberFormat;
        private string textToBeSearched;
        private ICommand backNavigationCommand;
        private ICommand searchInventoryCommand;
        private ICommand loadedCommand;
        private CustomDataGridVM customDataGridVM;
        private RetailInventoryLookUpView retailInventoryLookUpView;
        #endregion

        #region properties
        public string TextToBeSearched
        {
            get
            {
                return textToBeSearched;
            }
            set
            {
                if (!object.Equals(textToBeSearched, value))
                {
                    textToBeSearched = value;
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
        public ICommand BackNavigationCommand
        {
            get
            {
                return backNavigationCommand;
            }
            set
            {
                if (!object.Equals(backNavigationCommand, value))
                {
                    backNavigationCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand LoadedCommand
        {
            get
            {
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
        public ICommand SearchInventoryCommand
        {
            get
            {
                return searchInventoryCommand;
            }
            set
            {
                if (!object.Equals(searchInventoryCommand, value))
                {
                    searchInventoryCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region methods
        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetFooterContent(string.Empty, MessageType.None);
            string code = "";
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                code = ProcessScannedBarCode(
                                            checkScannedEvent.Message,
                                            ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "LEFT_TRIM_BARCODE", 0),
                                            ParafaitDefaultViewContainerList.GetParafaitDefault<int>(ExecutionContext, "RIGHT_TRIM_BARCODE", 0));
                log.Debug("code: " + code);
                TextToBeSearched = code;
            }
            log.LogMethodExit();
        }
        private string ProcessScannedBarCode(string Code, int leftTrim, int rightTrim)
        {
            log.LogMethodEntry(Code, leftTrim, rightTrim);
            try
            {
                Code = System.Text.RegularExpressions.Regex.Replace(Code, @"\W+", "");
                log.LogMethodExit((Code.Substring(leftTrim, Code.Length - rightTrim - leftTrim)));
                return (Code.Substring(leftTrim, Code.Length - rightTrim - leftTrim));
            }
            catch (Exception ex)
            {
                log.Error("Error occured in Processing Scanned Bar Code", ex);
                log.LogMethodExit(Code);
                return Code;
            }
        }
        private void InitializeCommands()
        {
            log.LogMethodEntry();
            BackNavigationCommand = new DelegateCommand(OnBackNavigationClicked, ButtonEnable);
            SearchInventoryCommand = new DelegateCommand(OnSearchClicked, ButtonEnable);
            LoadedCommand = new DelegateCommand(OnLoaded);
            log.LogMethodExit();
        }
        
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry();
            if (parameter != null)
            {
                retailInventoryLookUpView = parameter as RetailInventoryLookUpView;                
            }
            log.LogMethodExit();
        }
        private void OnBackNavigationClicked(object parameter)
        {
            log.LogMethodEntry();
            PerformClose(retailInventoryLookUpView);
            log.LogMethodExit();
        }
        private void PerformClose(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                UnRegisterDevices();
                retailInventoryLookUpView.Close();
            }
            log.LogMethodExit();
        }
        internal void UnRegisterDevices()
        {
            log.LogMethodEntry();
            ExecuteActionWithFooter(() =>
            {
                if (barcodeReader != null)
                {
                    barcodeReader.UnRegister();
                }
            });
            log.LogMethodExit();
        }
        private async void OnSearchClicked(object parameter)
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);
            if (string.IsNullOrWhiteSpace(TextToBeSearched))
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1671), MessageType.Warning);
                return;
            }                       
            string productIdList = string.Empty;
            List<int> productIds = new List<int>();
            List<ProductsContainerDTO> products = ProductViewContainerList.GetActiveProductsContainerDTOList(ExecutionContext, ManualProductType.INVENTORY.ToString()).Where(x => x.InventoryItemContainerDTO != null).ToList();
            if (products != null && products.Count > 0)
            {
                productIds = products.Where(x => !string.IsNullOrWhiteSpace(x.InventoryProductCode) && x.InventoryProductCode.ToLower().Contains(TextToBeSearched.ToLower()) ||
                x.InventoryItemContainerDTO.ProductBarcodeContainerDTOList != null && x.InventoryItemContainerDTO.ProductBarcodeContainerDTOList.Any(bc => !string.IsNullOrWhiteSpace(bc.BarCode) && bc.BarCode.ToLower().Equals(TextToBeSearched.ToLower())) ||
                !string.IsNullOrWhiteSpace(x.ProductName) && x.ProductName.ToLower().Contains(TextToBeSearched.ToLower()) ||
                !string.IsNullOrWhiteSpace(x.Description) && x.Description.ToLower().Contains(TextToBeSearched.ToLower()) ||
                !string.IsNullOrWhiteSpace(x.SearchDescription) && x.SearchDescription.ToLower().Contains(TextToBeSearched.ToLower()) ||
                !string.IsNullOrWhiteSpace(x.HsnSacCode) && x.HsnSacCode.ToLower().Contains(TextToBeSearched.ToLower())).Select(y => y.InventoryItemContainerDTO.ProductId).ToList();
            }
            if (productIds.Count == 0)
            {
                CustomDataGridVM.CollectionToBeRendered = new System.Collections.ObjectModel.ObservableCollection<object>(new List<InventoryDTO>());
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1710), MessageType.Warning);
                return;
            }
            else if (productIds.Count > 10)
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5145), MessageType.Warning);
                return;
            }
            
            
            List<InventoryDTO> inventoryDTOs = new List<InventoryDTO>();
            try
            {
                inventoryDTOs = await GetInventory(productIds);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(ex.Message, MessageType.Error);
                return;
            }
            
            ShowInventoryDetails(inventoryDTOs);
            log.LogMethodExit();
        }

        private async Task<List<InventoryDTO>> GetInventory(List<int> productIds)
        {
            log.LogMethodEntry(productIds);
            string productIdList = string.Join(",", productIds);
            List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
            inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID_LIST, productIdList));
            List<InventoryDTO> inventoryDTOs = new List<InventoryDTO>();
            IInventoryStockUseCases inventoryStockUseCases = InventoryUseCaseFactory.GetInventoryStockUseCases(ExecutionContext);
            inventoryDTOs = await inventoryStockUseCases.GetInventoryDTOList(inventorySearchParams);
            log.LogMethodExit(inventoryDTOs);
            return inventoryDTOs;
        }
        private void ShowInventoryDetails(List<InventoryDTO> inventoryDTOs)
        {
            if (inventoryDTOs != null && inventoryDTOs.Any())
            {
                List<ProductsContainerDTO> products = ProductViewContainerList.GetActiveProductsContainerDTOList(ExecutionContext, ManualProductType.INVENTORY.ToString()).Where(x => x.InventoryItemContainerDTO != null).ToList();
                List<InventoryLotDTO> inventoryLotDTOs = new List<InventoryLotDTO>(inventoryDTOs.Where(i => i.InventoryLotDTO != null).Select(x => x.InventoryLotDTO));
                CustomDataGridVM.CollectionToBeRendered = new System.Collections.ObjectModel.ObservableCollection<object>(inventoryDTOs);
                CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
                {
                    {"Code", new CustomDataGridColumnElement(){Heading=MessageViewContainerList.GetMessage(ExecutionContext, "Product Code")}},
                    {"ProductId", new CustomDataGridColumnElement()
                    {
                        Heading =MessageViewContainerList.GetMessage(ExecutionContext, "Product Name"),
                        Converter = new ProductIdNameConverter(),
                        ConverterParameter = new List<object> { products }
                    }},
                    {"LocationName", new CustomDataGridColumnElement(){Heading=MessageViewContainerList.GetMessage(ExecutionContext,"Location")} },
                    {"Quantity", new CustomDataGridColumnElement(){Heading=MessageViewContainerList.GetMessage(ExecutionContext,"Quantity on Hand"), DataGridColumnHorizontalAlignment = HorizontalAlignment.Right, DataGridColumnStringFormat = numberFormat} },
                    {"UOM", new CustomDataGridColumnElement(){Heading=MessageViewContainerList.GetMessage(ExecutionContext,"Uom")} },
                    {"LotNumber", new CustomDataGridColumnElement(){Heading=MessageViewContainerList.GetMessage(ExecutionContext,"Lot")} },
                    {"LotId",new CustomDataGridColumnElement(){Heading=MessageViewContainerList.GetMessage(ExecutionContext,"Expiry Date"),
                    Converter = new LotIdToExpiryDateConverter(),
                    ConverterParameter = new List<object> { inventoryLotDTOs, dateTimeFormat} } },
                };               
            }
            else
            {
                CustomDataGridVM.CollectionToBeRendered = new System.Collections.ObjectModel.ObservableCollection<object>(new List<InventoryDTO>());
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5146), MessageType.Info);
            }
        }

        
        private void SetUserControlVM()
        {
            log.LogMethodEntry();
            CustomDataGridVM = new CustomDataGridVM(ExecutionContext)
            {
                IsComboAndSearchVisible = false,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            CustomDataGridVM.HeaderCollection = new Dictionary<string, CustomDataGridColumnElement>()
            {
                {"Code", new CustomDataGridColumnElement(){Heading=MessageViewContainerList.GetMessage(ExecutionContext, "Product Code")}},
                {"ProductName", new CustomDataGridColumnElement(){Heading=MessageViewContainerList.GetMessage(ExecutionContext, "Product Name")} },
                {"LocationName", new CustomDataGridColumnElement(){Heading=MessageViewContainerList.GetMessage(ExecutionContext,"Location")} },
                {"Quantity", new CustomDataGridColumnElement(){Heading=MessageViewContainerList.GetMessage(ExecutionContext,"Quantity on Hand"), DataGridColumnHorizontalAlignment=HorizontalAlignment.Right, DataGridColumnStringFormat = numberFormat} },
                {"UOM", new CustomDataGridColumnElement(){Heading=MessageViewContainerList.GetMessage(ExecutionContext,"Uom")} },
                {"LotNumber", new CustomDataGridColumnElement(){Heading=MessageViewContainerList.GetMessage(ExecutionContext,"Lot")} },
                {"ExpiryDate",new CustomDataGridColumnElement(){Heading=MessageViewContainerList.GetMessage(ExecutionContext,"Expiry Date"), DataGridColumnStringFormat=dateTimeFormat} },
            };
            FooterVM = new FooterVM(ExecutionContext)
            {
                Message = string.Empty,
                MessageType = MessageType.None,
                HideSideBarVisibility = Visibility.Collapsed
            };
            log.LogMethodExit();
        }
        
        #endregion

        #region constructors
        public RetailInventoryLookUpVM(ExecutionContext executionContext, DeviceClass barcodeReader)
        {
            log.LogMethodEntry(executionContext, barcodeReader);
            try
            {
                this.ExecutionContext = executionContext;
                this.barcodeReader = barcodeReader;
                dateTimeFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT");
                numberFormat = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT");
                if (barcodeReader != null)
                {
                    barcodeReader.Register(BarCodeScanCompleteEventHandle);
                }
                InitializeCommands();
                SetUserControlVM();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, ex.Message);
                SetFooterContent(ErrorMessage, string.IsNullOrEmpty(ErrorMessage) ? MessageType.None : MessageType.Warning);
            }            
            log.LogMethodExit();
        }
        #endregion
    }
}

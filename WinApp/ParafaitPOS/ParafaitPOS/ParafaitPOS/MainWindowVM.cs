
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.GamesUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Semnox.Parafait.RedemptionUI;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.TransactionUI;
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.POSUI.StaticMenu;
using Semnox.Parafait.POS;
using Semnox.Parafait.Authentication;
using Semnox.Parafait.Product;
using System.Windows.Media;

namespace ParafaitPOS
{
    public class MainWindowVM : ViewModelBase
    {
        #region Members
        Window mainWindow;
        GenericContentVM genericContentVM = null;
        private ICommand _leftPaneMenuSelectedCommand;
        private ObjectType _selectedType;
        private int _selectedId;
        private string text1;
        private List<string> _comboOptions;
        private FooterVM _footerVM;
        private LeftPaneVM _leftPaneVM;
        private ICommand leftPaneNavigationClickedCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Properties
        public ICommand LeftPaneMenuSelectedCommand
        {
            get
            {
                if (_leftPaneMenuSelectedCommand == null)
                    _leftPaneMenuSelectedCommand = new DelegateCommand(OnLeftPaneMenuSelected);
                return _leftPaneMenuSelectedCommand;
            }
            set
            {
                _leftPaneMenuSelectedCommand = value;
            }
        }

        public ObjectType SelectedObjectType
        {
            get
            {
                return _selectedType;
            }
            set
            {
                _selectedType = value;
                OnPropertyChanged("SelectedType");
            }
        }

        public int SelectedId
        {
            get
            {
                return _selectedId;
            }
            set
            {
                _selectedId = value;
                OnPropertyChanged("SelectedId");
            }
        }


        public LeftPaneVM LeftPaneVM
        {
            get
            {
                return _leftPaneVM;
            }
            set
            {
                SetProperty(ref _leftPaneVM, value);
            }
        }

        public FooterVM FooterVM
        {
            get
            {
                return _footerVM;
            }
            set
            {
                SetProperty(ref _footerVM, value);
            }
        }

        public string Text1
        {
            get { return text1; }
            set
            {
                SetProperty(ref text1, value);
            }
        }

        public List<string> ComboOptions
        {
            get
            {
                return _comboOptions;
            }
            set
            {
                _comboOptions = value;
            }
        }

        public ICommand LeftPaneNavigationClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(leftPaneNavigationClickedCommand);
                return leftPaneNavigationClickedCommand;
            }
            private set
            {
                log.LogMethodEntry(value);
                leftPaneNavigationClickedCommand = value;
            }
        }
        #endregion

        #region Constructors
        public MainWindowVM(ExecutionContext executionContext, Window window)
        {

            leftPaneNavigationClickedCommand = new DelegateCommand(OnLeftPaneNavigationClicked);
            _footerVM = new FooterVM(executionContext)
            {

                Message = "",

                MessageType = MessageType.None,

                HideSideBarVisibility = System.Windows.Visibility.Visible
            };

            _leftPaneVM = new LeftPaneVM(executionContext)
            {
                ModuleName = "Main Window",

                MenuItems = new System.Collections.ObjectModel.ObservableCollection<string>()
                {
                    "Setup Product Menu",
                    "Order Sale",
                    "Game Management",
                    "Login",
                    "Manager approval",
                    "Redemption",
                    "TicketMode",
                    "ExchangeToken",
                    "ExchangeCredits"
                }
                
            };

        }
        #endregion

        #region Methods
        private async void OnLeftPaneMenuSelected(object parameter)
        {
            if (parameter != null)
            {
                MainWindow mainWindow = parameter as MainWindow;

                if (mainWindow != null)
                {
                    if (LeftPaneVM.SelectedMenuItem.ToLower() == "game management")
                    {
                        GMMainView gMMainView = new GMMainView();

                        gMMainView.DataContext = new GMMainVM(App.machineUserContext);

                        gMMainView.ShowDialog();
                    }
                    if(LeftPaneVM.SelectedMenuItem.ToLower() == "Setup Product Menu".ToLower())
                    {
                        ExecutionContext executionContext = SystemUserExecutionContextBuilder.GetSystemUserExecutionContext();
                        ExecutionContext = executionContext;
                        IntPtr handle = (new WindowInteropHelper(mainWindow)).Handle;
                        POSDeviceCollection pOSDeviceCollection = new POSDeviceCollection(executionContext, handle);
                        ProductMenuSetupView productStaticMenuView = new ProductMenuSetupView();
                        DeviceClass barcodeReader = pOSDeviceCollection.GetPrimaryDevice(DeviceType.BarcodeReader);
                        if (barcodeReader != null)
                        {
                            barcodeReader.Register(new EventHandler(BarcodeScanCompleteEventHandle));
                        }
                        ProductMenuSetupVM productStaticMenuVM = new ProductMenuSetupVM(executionContext, pOSDeviceCollection.GetPrimaryDevice(DeviceType.BarcodeReader));
                        productStaticMenuView.DataContext = productStaticMenuVM;
                        productStaticMenuView.ShowDialog();

                    }
                    if (LeftPaneVM.SelectedMenuItem.ToLower() == "Order Sale".ToLower())
                    {
                        
                        ExecutionContext executionContext = SystemUserExecutionContextBuilder.GetSystemUserExecutionContext();
                        ExecutionContext = executionContext;
                        ProductMenuViewContainerList.Rebuild(ExecutionContext, ProductMenuType.ORDER_SALE);
                        SalesView salesView = new SalesView();
                        SalesVM salesVM = new SalesVM(executionContext);
                        salesView.DataContext = salesVM;
                        salesView.ShowDialog();
                    }
                    if (LeftPaneVM.SelectedMenuItem.ToLower() == "login")
                    {
                        AuthenticateUserView authenticateUserView = new AuthenticateUserView(false);
                        IntPtr handle = (new WindowInteropHelper(mainWindow)).Handle;
                        POSDeviceCollection posDeviceCollection = new POSDeviceCollection(App.machineUserContext, handle);
                        ExecutionContext userexecutionContext = new ExecutionContext(null, App.machineUserContext.SiteId, App.machineUserContext.MachineId, -1, App.machineUserContext.IsCorporate, App.machineUserContext.LanguageId); 
                        AuthenticateUserVM authenticateUserVM = new AuthenticateUserVM(userexecutionContext, "", "PARAFAIT POS", loginStyle.FullScreen, true, posDeviceCollection.GetPrimaryDevice(DeviceType.CardReader));
                        authenticateUserView.DataContext = authenticateUserVM;
                        authenticateUserView.ShowDialog();
                    }
                    if (LeftPaneVM.SelectedMenuItem.ToLower() == "manager approval")
                    {                        
                        if (UserViewContainerList.IsSelfApprovalAllowed(App.machineUserContext.SiteId, App.machineUserContext.UserPKId))
                        {
                            MessageBox.Show("is manager");
                        }
                        else
                        {
                            MessageBox.Show("not manager");
                            IntPtr handle = (new WindowInteropHelper(mainWindow)).Handle;
                            POSDeviceCollection posDeviceCollection = new POSDeviceCollection(App.machineUserContext, handle);
                            AuthenticateManagerVM authenticatemanagerVM = new AuthenticateManagerVM(App.machineUserContext, posDeviceCollection.GetPrimaryDevice(DeviceType.CardReader));
                            AuthenticateManagerView authenticateManagerView = new AuthenticateManagerView();                            
                            authenticateManagerView.DataContext = authenticatemanagerVM;
                            authenticateManagerView.ShowDialog();
                        }

                    }
                    if (LeftPaneVM.SelectedMenuItem.ToLower() == "redemption")
                    {
                        RedemptionMainVM redemptionMainVM = new RedemptionMainVM(App.machineUserContext);
                        RedemptionView redemptionView = new RedemptionView();
                        redemptionView.DataContext = redemptionMainVM;
                        redemptionView.ShowDialog();
                    }
                    if (LeftPaneVM.SelectedMenuItem.ToLower() == "ticketmode")
                    {
                        IntPtr handle = (new WindowInteropHelper(mainWindow)).Handle;
                        POSDeviceCollection posDeviceCollection = new POSDeviceCollection(App.machineUserContext, handle);
                        TaskChangeTicketModeVM taskChangeTicketModeVM = new TaskChangeTicketModeVM(App.machineUserContext, posDeviceCollection.GetPrimaryDevice(DeviceType.CardReader));
                        TaskChangeTicketModeView taskChangeTicketModeView = new TaskChangeTicketModeView();
                        taskChangeTicketModeView.DataContext = taskChangeTicketModeVM;
                        taskChangeTicketModeView.ShowDialog();
                    }
                    if (LeftPaneVM.SelectedMenuItem.ToLower() == "exchangetoken")
                    {
                        IntPtr handle = (new WindowInteropHelper(mainWindow)).Handle;
                        POSDeviceCollection posDeviceCollection = new POSDeviceCollection(App.machineUserContext, handle);
                        TaskExchangeCreditTokenVM taskExchangeCreditTokenVM = new TaskExchangeCreditTokenVM(App.machineUserContext, TaskType.EXCHANGETOKENFORCREDIT, posDeviceCollection.GetPrimaryDevice(DeviceType.CardReader));
                        TaskExchangeCreditTokenView taskExchangeCreditTokenView = new TaskExchangeCreditTokenView();
                        taskExchangeCreditTokenView.DataContext = taskExchangeCreditTokenVM;
                        taskExchangeCreditTokenView.ShowDialog();
                    }
                    if (LeftPaneVM.SelectedMenuItem.ToLower() == "exchangecredits")
                    {
                        IntPtr handle = (new WindowInteropHelper(mainWindow)).Handle;
                        POSDeviceCollection posDeviceCollection = new POSDeviceCollection(App.machineUserContext, handle);
                        TaskExchangeCreditTokenVM taskExchangeCreditTokenVM = new TaskExchangeCreditTokenVM(App.machineUserContext, TaskType.EXCHANGECREDITFORTOKEN, posDeviceCollection.GetPrimaryDevice(DeviceType.CardReader));
                        TaskExchangeCreditTokenView taskExchangeCreditTokenView = new TaskExchangeCreditTokenView();
                        taskExchangeCreditTokenView.DataContext = taskExchangeCreditTokenVM;
                        taskExchangeCreditTokenView.ShowDialog();
                    }
                }
            }
        }

        private void BarcodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                log.Debug("Scanned barcode: " + checkScannedEvent.Message);
            }
            log.LogMethodExit();
        }

        private List<ProductMenuPanelContainerDTO> GetProductMenuPanelContainerDTOList(List<ProductMenuPanelDTO> productMenuPanelDTOList)
        {
            List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList = new List<ProductMenuPanelContainerDTO>();
            Dictionary<string, ProductMenuPanelDTO> panelGuidProductMenuPanelDTODictionary = new Dictionary<string, ProductMenuPanelDTO>();
            foreach (var productMenuPanelDTO in productMenuPanelDTOList)
            {
                panelGuidProductMenuPanelDTODictionary.Add(productMenuPanelDTO.Guid, productMenuPanelDTO);
            }
            foreach (var productMenuPanelDTO in productMenuPanelDTOList)
            {
                ProductMenuPanelContainerDTO productMenuPanelContainerDTO = GetProductMenuPanelContainerDTO(productMenuPanelDTO, panelGuidProductMenuPanelDTODictionary);
                productMenuPanelContainerDTOList.Add(productMenuPanelContainerDTO);
            }
            return productMenuPanelContainerDTOList;
        }

        private ProductMenuPanelContainerDTO GetProductMenuPanelContainerDTO(ProductMenuPanelDTO productMenuPanelDTO, Dictionary<string, ProductMenuPanelDTO> panelGuidProductMenuPanelDTODictionary)
        {
            ProductMenuPanelContainerDTO productMenuPanelContainerDTO = new ProductMenuPanelContainerDTO(productMenuPanelDTO.PanelId, true,
                 productMenuPanelDTO.DisplayOrder, productMenuPanelDTO.Name,
                 productMenuPanelDTO.CellMarginLeft, productMenuPanelDTO.CellMarginRight,
                 productMenuPanelDTO.CellMarginTop, productMenuPanelDTO.CellMarginBottom,
                 productMenuPanelDTO.RowCount, productMenuPanelDTO.ColumnCount,
                 productMenuPanelDTO.ImageURL, productMenuPanelDTO.Guid);
            productMenuPanelContainerDTO.ProductMenuPanelContentContainerDTOList = new List<ProductMenuPanelContentContainerDTO>();
            if(productMenuPanelDTO.ProductMenuPanelContentDTOList != null)
            {
                foreach (var productMenuPanelContentDTO in productMenuPanelDTO.ProductMenuPanelContentDTOList)
                {
                    int productId = -1;
                    int childPanelId = -1;
                    string name = string.Empty;
                    if (productMenuPanelContentDTO.ObjectType == "PRODUCTS")
                    {
                        ProductsContainerDTO productsContainerDTO = ProductViewContainerList.GetProductsContainerDTOOrDefault(ExecutionContext, ManualProductType.SELLABLE.ToString(), productMenuPanelContentDTO.ObjectGuid);
                        if (productsContainerDTO == null)
                        {
                            productsContainerDTO = ProductViewContainerList.GetProductsContainerDTOOrDefault(ExecutionContext, ManualProductType.REDEEMABLE.ToString(), productMenuPanelContentDTO.ObjectGuid);
                        }
                        if (productsContainerDTO != null)
                        {
                            name = productsContainerDTO.ProductName;
                            productId = productsContainerDTO.ProductId;
                        }
                    }
                    else if (productMenuPanelContentDTO.ObjectType == "PRODUCT_MENU_PANEL")
                    {
                        if(panelGuidProductMenuPanelDTODictionary.ContainsKey(productMenuPanelContentDTO.ObjectGuid))
                        {
                            ProductMenuPanelDTO childProductMenuPanelDTO = panelGuidProductMenuPanelDTODictionary[productMenuPanelContentDTO.ObjectGuid];

                            name = childProductMenuPanelDTO.Name;
                            childPanelId = childProductMenuPanelDTO.PanelId;
                        }
                    }
                    ProductMenuPanelContentContainerDTO productMenuPanelContentContainerDTO =
                        new ProductMenuPanelContentContainerDTO(productMenuPanelContentDTO.Id,
                                                                 productMenuPanelContentDTO.PanelId,
                                                                 productId,
                                                                 childPanelId,
                                                                 name,
                                                                 (productMenuPanelContentDTO.RowIndex * productMenuPanelDTO.ColumnCount) + productMenuPanelContentDTO.ColumnIndex,
                                                                 productMenuPanelContentDTO.ImageURL,
                                                                 productMenuPanelContentDTO.BackColor,
                                                                 productMenuPanelContentDTO.TextColor,
                                                                 productMenuPanelContentDTO.Font,
                                                                 productMenuPanelContentDTO.ColumnIndex,
                                                                 productMenuPanelContentDTO.RowIndex,
                                                                 productMenuPanelContentDTO.ButtonType, false);
                    productMenuPanelContainerDTO.ProductMenuPanelContentContainerDTOList.Add(productMenuPanelContentContainerDTO);
                }
            }
            return productMenuPanelContainerDTO;
        }

        private void OnLeftPaneNavigationClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
             if (parameter != null)
            {
                MainWindow mainwindow = parameter as MainWindow;

                if (mainwindow != null)
                {
                    log.Info("MainWindow is closed");
                    mainwindow.Close();
                }
            }
            log.LogMethodExit();
        }

        internal void ShowSelectedItem(MainWindow mainWindow)
        {
            mainWindow.ZeroSelectedTextBlock.Visibility = System.Windows.Visibility.Collapsed;

            for (int i = 0; i < mainWindow.ActionGrid.Children.Count; i++)
            {
                if (mainWindow.ActionGrid.Children[i] is GenericRightSectionContentUserControl)
                {
                    mainWindow.ActionGrid.Children.RemoveAt(i);
                    i--;
                }
            }

            GenericRightSectionContentUserControl rightSectionUserControl = new GenericRightSectionContentUserControl();

  
            genericContentVM.SelectedDisplayItem = genericContentVM.DisplayParams.FirstOrDefault(g => g.Id == this.SelectedId);
        }
        #endregion

    }
}

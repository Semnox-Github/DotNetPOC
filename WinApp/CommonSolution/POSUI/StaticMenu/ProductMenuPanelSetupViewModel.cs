/********************************************************************************************
 * Project Name - POSUI
 * Description  - View model class of product menu panel setup
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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    public class ProductMenuPanelSetupViewModel : BaseWindowViewModel
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ObservableCollection<ProductMenuPanelContentSetupViewModel> menuPanelContentSetupViewModelList = new ObservableCollection<ProductMenuPanelContentSetupViewModel>();
        private CellGridViewModel cellGridViewModel;
        private ProductMenuPanelDTO productMenuPanelDTO;
        private PanelMargin panelMargin;
        private string moduleName;
        private DeviceClass barcodeReader;
        private ICommand navigationClickCommand;
        private ICommand chooseProductClickCommand;
        private ICommand choosePanelClickCommand;
        private ICommand clearClickCommand;
        private ICommand editPanelClickCommand;
        private ICommand selectPanelContentCommand;
        private ICommand saveClickCommand;
        private ICommand loadedCommand;
        private FileExplorerVM panelFileExplorerVM;
        private FileExplorerVM panelContentFileExplorerVM;
        private ProductMenuPanelSetupView productMenuPanelSetupView;

        private Brush defaultBackgroundColor;
        private FontFamily defaultFont;
        private Brush defaultTextColor;
        private double defaultFontSize;

        public ProductMenuPanelSetupViewModel(ExecutionContext executionContext, ProductMenuPanelDTO productMenuPanelDTO, DeviceClass barcodeReader)
        {
            log.LogMethodEntry(executionContext, productMenuPanelDTO);
            ExecutionContext = executionContext;
            this.barcodeReader = barcodeReader;
            panelFileExplorerVM = new FileExplorerVM(executionContext, "IMAGE_DIRECTORY", productMenuPanelDTO.ImageURL);
            panelContentFileExplorerVM = new FileExplorerVM(executionContext, "IMAGE_DIRECTORY", string.Empty);
            CreateDefaultFontAndColors();
            FooterVM = new FooterVM(ExecutionContext)
            {
                Message = string.Empty,
                MessageType = MessageType.None,
                HideSideBarVisibility = Visibility.Collapsed
            };
            this.productMenuPanelDTO = productMenuPanelDTO;
            panelMargin = new PanelMargin(productMenuPanelDTO.CellMarginLeft, productMenuPanelDTO.CellMarginRight, productMenuPanelDTO.CellMarginTop, productMenuPanelDTO.CellMarginBottom);
            DisplayProductMenuPanel();
            moduleName = "Edit Panel";
            navigationClickCommand = new DelegateCommand(NavigationClick);
            chooseProductClickCommand = new DelegateCommand(ChooseProductClick);
            saveClickCommand = new DelegateCommand(SaveClick);
            choosePanelClickCommand = new DelegateCommand(ChoosePanelClick);
            clearClickCommand = new DelegateCommand(ClearClick);
            editPanelClickCommand = new DelegateCommand(EditPanelClick);
            loadedCommand = new DelegateCommand(OnLoaded);
            selectPanelContentCommand = new DelegateCommand(SelectPanelContentClick);
            log.LogMethodExit();
        }

        private void CreateDefaultFontAndColors()
        {
            defaultBackgroundColor = (Brush)Application.Current.Resources["CustomToggleButton.UnChecked.Background"];
            defaultFont = (FontFamily)Application.Current.Resources["FontFamily"];
            defaultTextColor = (Brush)Application.Current.Resources["CustomToggleButton.UnChecked.Foreground"];
            defaultFontSize = (Double)Application.Current.Resources["FontSize.Small"];

            string defaultBackgroundColorString = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_BACKGROUND_COLOR");
            string defaultFontString = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_TEXT_FONT");
            string defaultTextColorString = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_TEXT_COLOR");
            defaultFontSize = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_TEXT_FONT_SIZE", defaultFontSize);

            if (string.IsNullOrWhiteSpace(defaultBackgroundColorString) == false &&
               IsValidColorString(defaultBackgroundColorString))
            {
                defaultBackgroundColor = GetBrush(defaultBackgroundColorString);
            }

            if (string.IsNullOrWhiteSpace(defaultTextColorString) == false &&
               IsValidColorString(defaultTextColorString))
            {
                defaultTextColor = GetBrush(defaultTextColorString);
            }
            if (string.IsNullOrWhiteSpace(defaultFontString) == false)
            {
                try
                {
                    defaultFont = new FontFamily(defaultFontString);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while parsing the font family", ex);
                    defaultFont = (FontFamily)Application.Current.Resources["FontFamily"];
                }
            }
        }

        private bool IsValidColorString(string colorString)
        {
            log.LogMethodEntry(colorString);
            bool valid;
            string trimmedColorString = colorString.Replace("#", string.Empty).Trim();
            try
            {
                GetBrush(trimmedColorString);
                valid = true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while validating color string", ex);
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private static Brush GetBrush(string colorString)
        {
            return new BrushConverter().ConvertFromString(colorString) as SolidColorBrush;
        }

        private void DisplayProductMenuPanel()
        {
            log.LogMethodEntry();
            ProductMenuPanelContentSetupViewModelList = new ObservableCollection<ProductMenuPanelContentSetupViewModel>();
            cellGridViewModel = new CellGridViewModel(productMenuPanelDTO.RowCount, productMenuPanelDTO.ColumnCount);
            foreach (var productMenuPanelContentDTO in productMenuPanelDTO.ProductMenuPanelContentDTOList)
            {
                if (productMenuPanelContentDTO.IsActive == false)
                {
                    continue;
                }
                ProductMenuPanelContentSetupViewModel menuPanelContentSetupViewModel = new ProductMenuPanelContentSetupViewModel(ExecutionContext, productMenuPanelContentDTO, panelMargin, defaultBackgroundColor, defaultFont, defaultTextColor, defaultFontSize);
                Add(menuPanelContentSetupViewModel);
            }
            CreateEmptyButtons();
            OnPropertyChanged("SelectedProductMenuPanelContentSetupViewModel");
            OnPropertyChanged("SelectedMenuPanelContentSetupViewModelButtonType");
            log.LogMethodExit();
        }

        private void CreateEmptyButtons()
        {
            log.LogMethodEntry();
            foreach (var cellViewModel in cellGridViewModel.CellViewModelList)
            {
                if (cellViewModel.IsOccupied)
                {
                    continue;
                }
                ProductMenuPanelContentButtonType buttonType = ProductMenuPanelContentButtonType.NORMAL;

                while (cellGridViewModel.GetEmptyCellViewModelList(cellViewModel.RowIndex, cellViewModel.ColumnIndex, buttonType.VerticalCellCount, buttonType.HorizontalCellCount).Any() == false)
                {
                    buttonType = buttonType.GetSmallerButtonType();
                }
                ProductMenuPanelContentSetupViewModel menuPanelContentSetupViewModel = new ProductMenuPanelContentSetupViewModel(ExecutionContext, buttonType, panelMargin, defaultBackgroundColor, defaultFont, defaultTextColor, defaultFontSize);
                menuPanelContentSetupViewModel.RowIndex = cellViewModel.RowIndex;
                menuPanelContentSetupViewModel.ColumnIndex = cellViewModel.ColumnIndex;
                Add(menuPanelContentSetupViewModel);
            }
            log.LogMethodExit();
        }

        public ObservableCollection<ProductMenuPanelContentSetupViewModel> ProductMenuPanelContentSetupViewModelList
        {
            get
            {
                return menuPanelContentSetupViewModelList;
            }
            private set
            {
                menuPanelContentSetupViewModelList = value;
                OnPropertyChanged("ProductMenuPanelContentSetupViewModelList");
            }
        }

        public List<KeyValuePair<string, string>> ProductMenuPanelTypeList
        {
            get
            {
                return new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("S", MessageViewContainerList.GetMessage(ExecutionContext, "Static")),
                    new KeyValuePair<string, string>("D", MessageViewContainerList.GetMessage(ExecutionContext, "Dynamic")),
                };
            }
        }

        public List<KeyValuePair<ProductMenuPanelContentButtonType, string>> ProductMenuPanelContentButtonTypeList
        {
            get
            {
                return new List<KeyValuePair<ProductMenuPanelContentButtonType, string>>()
                {
                    new KeyValuePair<ProductMenuPanelContentButtonType, string>(ProductMenuPanelContentButtonType.SMALL, "Half"),
                    new KeyValuePair<ProductMenuPanelContentButtonType, string>(ProductMenuPanelContentButtonType.NORMAL, "Normal"),
                    new KeyValuePair<ProductMenuPanelContentButtonType, string>(ProductMenuPanelContentButtonType.LARGE, "Large")
                };
            }
        }

        private void ClearEmptyButtons()
        {
            log.LogMethodEntry();
            List<ProductMenuPanelContentSetupViewModel> itemsToBeRemoved = menuPanelContentSetupViewModelList.Where(x => x.IsContentAssigned == false).ToList();
            foreach (var menuPanelContentSetupViewModel in itemsToBeRemoved)
            {
                Remove(menuPanelContentSetupViewModel);
            }
            log.LogMethodExit();
        }

        public double PanelWidth
        {
            get
            {
                return productMenuPanelDTO.ColumnCount * ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_WIDTH", 144) + (productMenuPanelDTO.ColumnCount - 1) * (panelMargin.Left + panelMargin.Right) + 20;
            }
        }

        public double PanelHeight
        {
            get
            {
                return productMenuPanelDTO.RowCount * ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_HEIGHT", 96) + (productMenuPanelDTO.RowCount - 1) * (panelMargin.Top + panelMargin.Bottom) + 20;
            }
        }


        public FileExplorerVM PanelFileExplorerVM
        {
            get
            {
                return panelFileExplorerVM;
            }
        }

        public FileExplorerVM PanelContentFileExplorerVM
        {
            get
            {
                return panelContentFileExplorerVM;
            }
        }

        public ProductMenuPanelContentButtonType SelectedMenuPanelContentSetupViewModelButtonType
        {
            get
            {
                ProductMenuPanelContentButtonType result = SelectedProductMenuPanelContentSetupViewModel == null ? null : SelectedProductMenuPanelContentSetupViewModel.ButtonType;
                return result;
            }
            set
            {
                ProductMenuPanelContentSetupViewModel menuPanelContentSetupViewModel = SelectedProductMenuPanelContentSetupViewModel;
                if (menuPanelContentSetupViewModel != null)
                {
                    List<CellViewModel> cellViewModelList = cellGridViewModel.GetCellViewModelListOfRange(menuPanelContentSetupViewModel.RowIndex, menuPanelContentSetupViewModel.ColumnIndex, value.VerticalCellCount, value.HorizontalCellCount);
                    if (cellViewModelList.Any() == false ||
                        cellViewModelList.Where(x => x.IsOccupied).Any(x => x.ProductMenuPanelContentSetupViewModel.IsContentAssigned && x.ProductMenuPanelContentSetupViewModel != menuPanelContentSetupViewModel))
                    {
                        OnPropertyChanged("SelectedMenuPanelContentSetupViewModelButtonType");
                        return;
                    }
                    List<ProductMenuPanelContentSetupViewModel> menuPanelContentSetupViewModelToBeRemoved = cellGridViewModel.GetProductMenuPanelContentSetupViewModelListInRange(menuPanelContentSetupViewModel.RowIndex, menuPanelContentSetupViewModel.ColumnIndex, value.VerticalCellCount, value.HorizontalCellCount);
                    if (menuPanelContentSetupViewModelToBeRemoved.Any(x => x != menuPanelContentSetupViewModel && x.IsContentAssigned))
                    {
                        OnPropertyChanged("SelectedMenuPanelContentSetupViewModelButtonType");
                        OnPropertyChanged("SelectedMenuPanelContentSetupViewModelButtonType");
                        return;
                    }

                    foreach (var panel in menuPanelContentSetupViewModelToBeRemoved)
                    {
                        Remove(panel);
                    }
                    menuPanelContentSetupViewModel.ButtonType = value;
                    Add(menuPanelContentSetupViewModel);
                    CreateEmptyButtons();
                    OnPropertyChanged("ProductMenuPanelContentSetupViewModelList");
                }
            }
        }

        private void Remove(ProductMenuPanelContentSetupViewModel panel)
        {
            productMenuPanelDTO.ProductMenuPanelContentDTOList.Remove(panel.ProductMenuPanelContentDTO);
            cellGridViewModel.RemoveProductMenuPanelContentSetupViewModel(panel);
            menuPanelContentSetupViewModelList.Remove(panel);
        }

        private void Add(ProductMenuPanelContentSetupViewModel menuPanelContentSetupViewModel)
        {
            cellGridViewModel.AddProductMenuPanelContentSetupViewModel(menuPanelContentSetupViewModel);
            menuPanelContentSetupViewModelList.Add(menuPanelContentSetupViewModel);
        }


        public ProductMenuPanelContentSetupViewModel SelectedProductMenuPanelContentSetupViewModel
        {
            get
            {
                return ProductMenuPanelContentSetupViewModelList.FirstOrDefault(x => x.Selected);
            }
        }

        public int Rows
        {
            get
            {
                return productMenuPanelDTO.RowCount;
            }
            set
            {
                if (value == productMenuPanelDTO.RowCount)
                {
                    return;
                }
                if (value > productMenuPanelDTO.RowCount)
                {
                    cellGridViewModel.RowCount = value;
                }
                else
                {
                    List<ProductMenuPanelContentSetupViewModel> menuPanelContentSetupViewModelToBeRemoved = cellGridViewModel.GetProductMenuPanelContentSetupViewModelListInRange(value, 0, productMenuPanelDTO.RowCount - value, productMenuPanelDTO.ColumnCount);
                    if (menuPanelContentSetupViewModelToBeRemoved.Any(x => x.IsContentAssigned))
                    {
                        OnPropertyChanged("Rows");
                        return;
                    }
                    foreach (var menuPanelContentSetupViewModel in menuPanelContentSetupViewModelToBeRemoved)
                    {
                        Remove(menuPanelContentSetupViewModel);
                    }
                    cellGridViewModel.RowCount = value;
                }
                productMenuPanelDTO.RowCount = value;
                ClearEmptyButtons();
                CreateEmptyButtons();
                OnPropertyChanged("Rows");
                OnPropertyChanged("PanelHeight");
            }
        }

        public string ImageURL
        {
            get
            {
                return productMenuPanelDTO.ImageURL;
            }
            set
            {
                if (productMenuPanelDTO.ImageURL == value)
                {
                    return;
                }
                productMenuPanelDTO.ImageURL = value;
                OnPropertyChanged("ImageURL");
            }
        }

        public ProductMenuPanelDTO ProductMenuPanelDTO
        {
            get
            {
                return productMenuPanelDTO;
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
                if (productMenuPanelDTO.Name == value)
                {
                    return;
                }
                productMenuPanelDTO.Name = value;
                OnPropertyChanged("Name");
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
                if (productMenuPanelDTO.IsActive == value)
                {
                    return;
                }
                productMenuPanelDTO.IsActive = value;
                OnPropertyChanged("IsActive");
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
                if (productMenuPanelDTO.DisplayOrder == value)
                {
                    return;
                }
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
                if (productMenuPanelDTO.CellMarginLeft == value)
                {
                    return;
                }
                productMenuPanelDTO.CellMarginLeft = value;
                OnPropertyChanged("CellMarginLeft");
                PanelMargin = new PanelMargin(productMenuPanelDTO.CellMarginLeft, panelMargin.Right, panelMargin.Top, panelMargin.Bottom);
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
                if (productMenuPanelDTO.CellMarginRight == value)
                {
                    return;
                }
                productMenuPanelDTO.CellMarginRight = value;
                OnPropertyChanged("CellMarginRight");
                PanelMargin = new PanelMargin(panelMargin.Left, productMenuPanelDTO.CellMarginRight, panelMargin.Top, panelMargin.Bottom);
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
                if (productMenuPanelDTO.CellMarginTop == value)
                {
                    return;
                }
                productMenuPanelDTO.CellMarginTop = value;
                OnPropertyChanged("CellMarginTop");
                PanelMargin = new PanelMargin(panelMargin.Left, panelMargin.Right, productMenuPanelDTO.CellMarginTop, panelMargin.Bottom);
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
                if (productMenuPanelDTO.CellMarginBottom == value)
                {
                    return;
                }
                productMenuPanelDTO.CellMarginBottom = value;
                OnPropertyChanged("CellMarginBottom");
                PanelMargin = new PanelMargin(panelMargin.Left, panelMargin.Right, panelMargin.Top, productMenuPanelDTO.CellMarginBottom);
            }
        }

        public PanelMargin PanelMargin
        {
            get
            {
                return panelMargin;
            }
            set
            {
                panelMargin = value;
                if (menuPanelContentSetupViewModelList != null)
                {
                    foreach (var menuPanelContentSetupViewModel in menuPanelContentSetupViewModelList)
                    {
                        menuPanelContentSetupViewModel.PanelMargin = panelMargin;
                    }
                }
                OnPropertyChanged("PanelMargin");
                OnPropertyChanged("PanelHeight");
                OnPropertyChanged("PanelWidth");
            }
        }

        public int Columns
        {
            get
            {
                return productMenuPanelDTO.ColumnCount;
            }
            set
            {
                if (value == productMenuPanelDTO.ColumnCount)
                {
                    return;
                }
                if (value > productMenuPanelDTO.ColumnCount)
                {
                    cellGridViewModel.ColumnCount = value;
                }
                else
                {
                    List<ProductMenuPanelContentSetupViewModel> menuPanelContentSetupViewModelToBeRemoved = cellGridViewModel.GetProductMenuPanelContentSetupViewModelListInRange(0, value, productMenuPanelDTO.RowCount, productMenuPanelDTO.ColumnCount - value);
                    if (menuPanelContentSetupViewModelToBeRemoved.Any(x => x.IsContentAssigned))
                    {
                        OnPropertyChanged("Rows");
                        return;
                    }
                    foreach (var menuPanelContentSetupViewModel in menuPanelContentSetupViewModelToBeRemoved)
                    {
                        Remove(menuPanelContentSetupViewModel);
                    }

                    cellGridViewModel.ColumnCount = value;
                }
                productMenuPanelDTO.ColumnCount = value;
                ClearEmptyButtons();
                CreateEmptyButtons();
                OnPropertyChanged("Columns");
                OnPropertyChanged("PanelWidth");
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
        /// navigationClickCommand
        /// </summary>
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

        private void NavigationClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            ProductMenuPanelSetupView productMenuPanelSetupView = param as ProductMenuPanelSetupView;
            try
            {
                if (productMenuPanelSetupView != null)
                {
                    productMenuPanelSetupView.Close();
                }
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                //this.SuccessMessage = ex.ToString();
            };
        }


        /// <summary>
        /// choosePanelClickCommand
        /// </summary>
        public ICommand ChoosePanelClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(choosePanelClickCommand);
                return choosePanelClickCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                choosePanelClickCommand = value;
            }
        }
        
        private void ChoosePanelClick(object param)
        {
            log.LogMethodEntry(param);
            ChoosePanelView choosePanelView = new ChoosePanelView();
            ChoosePanelVM choosePanelVM = new ChoosePanelVM(ExecutionContext, barcodeReader);
            choosePanelView.DataContext = choosePanelVM;
            choosePanelView.Owner = productMenuPanelSetupView;
            choosePanelView.ShowDialog();
            if (SelectedProductMenuPanelContentSetupViewModel != null &&
                choosePanelVM.SelectedProductMenuPanelDTO != null)
            {
                SelectedProductMenuPanelContentSetupViewModel.ObjectType = "PRODUCT_MENU_PANEL";
                SelectedProductMenuPanelContentSetupViewModel.ObjectGuid = choosePanelVM.SelectedProductMenuPanelDTO.Guid;
                SelectedProductMenuPanelContentSetupViewModel.ProductMenuPanelContentDTO.IsActive = true;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// editPanelClickCommand
        /// </summary>
        public ICommand EditPanelClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(editPanelClickCommand);
                return editPanelClickCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                editPanelClickCommand = value;
            }
        }

        private async void EditPanelClick(object param)
        {
            log.LogMethodEntry(param);
            try
            {
                if (SelectedProductMenuPanelContentSetupViewModel == null ||
                SelectedProductMenuPanelContentSetupViewModel.ProductMenuPanelContentDTO == null)
                {
                    log.LogMethodExit(null, "SelectedProductMenuPanelContentSetupViewModel is empty");
                    return;
                }
                if (SelectedProductMenuPanelContentSetupViewModel.ProductMenuPanelContentDTO.ObjectType != ProductMenuObjectTypes.PRODUCT_MENU_PANEL ||
                   string.IsNullOrWhiteSpace(SelectedProductMenuPanelContentSetupViewModel.ProductMenuPanelContentDTO.ObjectGuid))
                {
                    log.LogMethodExit(null, "ObjectType is not panel or ObjectGuid is empty");
                    return;
                }
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(ExecutionContext);
                List<ProductMenuPanelDTO> productMenuPanelDTOList = await productMenuUseCases.GetProductMenuPanelDTOList(guid: SelectedProductMenuPanelContentSetupViewModel.ProductMenuPanelContentDTO.ObjectGuid);
                if (productMenuPanelDTOList == null || productMenuPanelDTOList.Any() == false)
                {
                    log.LogMethodExit(null, "productMenuPanelDTOList is empty");
                    return;
                }
                ProductMenuPanelSetupView productMenuPanelSetupView = new ProductMenuPanelSetupView();
                ProductMenuPanelSetupViewModel productMenuPanelSetupViewModel = new ProductMenuPanelSetupViewModel(ExecutionContext, productMenuPanelDTOList[0], barcodeReader);
                productMenuPanelSetupView.DataContext = productMenuPanelSetupViewModel;
                productMenuPanelSetupView.ShowDialog();
                SelectedProductMenuPanelContentSetupViewModel.ObjectType = "PRODUCT_MENU_PANEL";
                SelectedProductMenuPanelContentSetupViewModel.ObjectGuid = productMenuPanelDTOList[0].Guid;
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }
            
            log.LogMethodExit();
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

        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                productMenuPanelSetupView = parameter as ProductMenuPanelSetupView;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// choosePanelClickCommand
        /// </summary>
        public ICommand ChooseProductClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(choosePanelClickCommand);
                return chooseProductClickCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                chooseProductClickCommand = value;
            }
        }

        private void ChooseProductClick(object param)
        {
            log.LogMethodEntry(param);
            ChooseProductView chooseProductView = new ChooseProductView();
            ChooseProductVM chooseProductVM = new ChooseProductVM(ExecutionContext, barcodeReader);
            chooseProductView.DataContext = chooseProductVM;
            chooseProductView.Owner = productMenuPanelSetupView;
            chooseProductView.ShowDialog();
            if(SelectedProductMenuPanelContentSetupViewModel != null &&
                chooseProductVM.SelectedProductsContainerDTO != null)
            {
                SelectedProductMenuPanelContentSetupViewModel.ObjectType = "PRODUCTS";
                SelectedProductMenuPanelContentSetupViewModel.ObjectGuid = chooseProductVM.SelectedProductsContainerDTO.Guid;
                SelectedProductMenuPanelContentSetupViewModel.ProductMenuPanelContentDTO.IsActive = true;
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// choosePanelClickCommand
        /// </summary>
        public ICommand ClearClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(clearClickCommand);
                return clearClickCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                clearClickCommand = value;
            }
        }

        private void ClearClick(object param)
        {
            log.LogMethodEntry(param);
            if(SelectedProductMenuPanelContentSetupViewModel != null)
            {
                SelectedProductMenuPanelContentSetupViewModel.ObjectGuid = string.Empty;
                SelectedProductMenuPanelContentSetupViewModel.ObjectType = string.Empty;
                SelectedProductMenuPanelContentSetupViewModel.ProductMenuPanelContentDTO.IsActive = false;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// choosePanelClickCommand
        /// </summary>
        public ICommand SaveClickCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(saveClickCommand);
                return saveClickCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                saveClickCommand = value;
            }
        }

        private async void SaveClick(object param)
        {
            log.LogMethodEntry(param);
            if(SelectedProductMenuPanelContentSetupViewModel != null)
            {
                if (await panelContentFileExplorerVM.Save())
                {
                    SelectedProductMenuPanelContentSetupViewModel.ProductMenuPanelContentDTO.ImageURL = panelContentFileExplorerVM.FileName;
                }
            }
            if(await panelFileExplorerVM.Save())
            {
                productMenuPanelDTO.ImageURL = panelFileExplorerVM.FileName;
            }
            List<ProductMenuPanelContentDTO> productMenuPanelContentDTOList = new List<ProductMenuPanelContentDTO>();
            if (menuPanelContentSetupViewModelList != null)
            {
                foreach (var menuPanelContentSetupViewModel in menuPanelContentSetupViewModelList)
                {
                    if (menuPanelContentSetupViewModel.ProductMenuPanelContentDTO.Id == -1 &&
                      (string.IsNullOrWhiteSpace(menuPanelContentSetupViewModel.ProductMenuPanelContentDTO.ObjectType) ||
                       string.IsNullOrWhiteSpace(menuPanelContentSetupViewModel.ProductMenuPanelContentDTO.ObjectGuid)))
                    {
                        continue;
                    }
                    else
                    {
                        productMenuPanelContentDTOList.Add(menuPanelContentSetupViewModel.ProductMenuPanelContentDTO);
                        if (productMenuPanelDTO.ProductMenuPanelContentDTOList.Contains(menuPanelContentSetupViewModel.ProductMenuPanelContentDTO) == false)
                        {
                            productMenuPanelDTO.ProductMenuPanelContentDTOList.Add(menuPanelContentSetupViewModel.ProductMenuPanelContentDTO);
                        }
                    }
                    
                }
            }
            if(productMenuPanelDTO.ProductMenuPanelContentDTOList != null)
            {
                foreach (var productMenuPanelContentDTO in productMenuPanelDTO.ProductMenuPanelContentDTOList)
                {
                    if(productMenuPanelContentDTOList.Contains(productMenuPanelContentDTO) == false)
                    {
                        productMenuPanelContentDTOList.Add(productMenuPanelContentDTO);
                    }
                }
            }
            
            productMenuPanelDTO.ProductMenuPanelContentDTOList = productMenuPanelContentDTOList;
            try
            {
                FooterVM.Message = MessageViewContainerList.GetMessage(ExecutionContext, 684);
                FooterVM.MessageType = MessageType.None;
                IProductMenuUseCases productMenuUseCases = POSUseCaseFactory.GetProductMenuUseCases(ExecutionContext);
                List<ProductMenuPanelDTO> productMenuPanelDTOList = await productMenuUseCases.SaveProductMenuPanelDTOList(new List<ProductMenuPanelDTO>() { productMenuPanelDTO });
                productMenuPanelDTO = productMenuPanelDTOList[0];
                DisplayProductMenuPanel();
                FooterVM.Message = string.Empty;
                FooterVM.MessageType = MessageType.None;
            }
            catch(ValidationException ex)
            {
                if(ex.GetFirstValidationError().EntityName == "ProductMenuPanel")
                {
                    ClearProductMenuPanelContentSetupViewModelSelection();
                }
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }
            catch (Exception ex)
            {
                FooterVM.Message = ex.Message;
                FooterVM.MessageType = MessageType.Error;
            }
            
            log.LogMethodExit();
        }

        private void ClearProductMenuPanelContentSetupViewModelSelection()
        {
            log.LogMethodEntry();
            if(SelectedProductMenuPanelContentSetupViewModel == null)
            {
                log.LogMethodExit(null, "Nothing is selected");
                return;
            }
            foreach (var item in menuPanelContentSetupViewModelList)
            {
                if (item.Selected)
                {
                    item.Selected = false;
                }
            }
            OnPropertyChanged("SelectedProductMenuPanelContentSetupViewModel");
            OnPropertyChanged("SelectedMenuPanelContentSetupViewModelButtonType");
            log.LogMethodExit();
        }

        /// <summary>
        /// choosePanelClickCommand
        /// </summary>
        public ICommand SelectPanelContentCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(choosePanelClickCommand);
                return selectPanelContentCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                selectPanelContentCommand = value;
            }
        }

        private void SelectPanelContentClick(object param)
        {
            log.LogMethodEntry(param);
            ProductMenuPanelContentSetupView productMenuPanelContentSetupView = param as ProductMenuPanelContentSetupView;
            if(productMenuPanelContentSetupView == null)
            {
                log.LogMethodExit(null, "productMenuPanelContentSetupView == null");
                return;
            }
            ProductMenuPanelContentSetupViewModel menuPanelContentSetupViewModel = productMenuPanelContentSetupView.DataContext as ProductMenuPanelContentSetupViewModel;
            if(menuPanelContentSetupViewModel == null)
            {
                log.LogMethodExit(null, "menuPanelContentSetupViewModel == null");
                return;
            }
            menuPanelContentSetupViewModel.Selected = !menuPanelContentSetupViewModel.Selected;
            if(menuPanelContentSetupViewModel.Selected)
            {
                panelContentFileExplorerVM.FileName = menuPanelContentSetupViewModel.ProductMenuPanelContentDTO.ImageURL;
            }
            foreach (var item in menuPanelContentSetupViewModelList)
            {
                if (item == menuPanelContentSetupViewModel)
                {
                    continue;
                }
                if (item.Selected)
                {
                    item.Selected = false;
                }
            }
            OnPropertyChanged("SelectedProductMenuPanelContentSetupViewModel");
            OnPropertyChanged("SelectedMenuPanelContentSetupViewModelButtonType");
            log.LogMethodExit();
        }
    }
}

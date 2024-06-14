/********************************************************************************************
 * Project Name - POSUI
 * Description  - View model class of product menu user control
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
using Semnox.Parafait.Product;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    public class ProductMenuUserControlVM : ViewModelBase
    {
        #region Members 
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductMenuPanelContainerDTO> productMenuPanelContainerDTOList;
        private ProductMenuPanelVM mainProductMenuPanelVM;
        private ProductMenuPanelVM currentProductMenuPanelVM;
        private readonly List<ProductMenuPanelVM> productMenuPanelVMList = new List<ProductMenuPanelVM>();
        private readonly Dictionary<int, ProductMenuPanelVM> panelIdProductMenuPanelVMDictionary = new Dictionary<int, ProductMenuPanelVM>();
        private ProductMenuUserControl productMenuUserControl;

        private bool showMainPanel = true;
        private Brush defaultBackgroundColor;
        private FontFamily defaultFont;
        private Brush defaultTextColor;
        private double defaultFontSize;
        private string productMenuType;
        private readonly NavigationTagsVM navigationTagsVM = new NavigationTagsVM(){ NavigationTags = new ObservableCollection<NavigationTag>()};
        #endregion

        #region Commands 
        private ICommand navigationTagClickCommand;
        private ICommand panelContentClickCommand;
        private ICommand panelContentInfoClickCommand;
        private ICommand loadedCommand;
        private void IntializeCommands()
        {
            log.LogMethodEntry();
            loadedCommand = new DelegateCommand(OnLoaded);
            navigationTagClickCommand = new DelegateCommand(OnNavigationTagClick);
            panelContentClickCommand = new DelegateCommand(OnPanelContentClick);
            panelContentInfoClickCommand = new DelegateCommand(OnPanelContentInfoClick);
            log.LogMethodExit();
        }
        public ICommand NavigationTagClickCommand
        {
            get
            {
                return navigationTagClickCommand;
            }
            set
            {
                SetProperty(ref navigationTagClickCommand, value);
            }
        }

        public ICommand PanelContentClickCommand
        {
            get
            {
                return panelContentClickCommand;
            }
            set
            {
                SetProperty(ref panelContentClickCommand, value);
            }
        }

        public ICommand PanelContentInfoClickCommand
        {
            get
            {
                return panelContentInfoClickCommand;
            }
            set
            {
                SetProperty(ref panelContentInfoClickCommand, value);
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
                SetProperty(ref loadedCommand, value);
            }
        }

        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                productMenuUserControl = parameter as ProductMenuUserControl;
            }
            log.LogMethodExit();
        }
        private void OnNavigationTagClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            NavigationTag navigationTag = NavigationTagsVM.SelectedNavigationTag;
            try
            {
                int panelId = Convert.ToInt32(navigationTag.Key);
                if (panelIdProductMenuPanelVMDictionary.ContainsKey(panelId) == false)
                {
                    log.LogMethodExit(null, "panelIdProductMenuPanelVMDictionary.ContainsKey(panelId) == false");
                    return;
                }
                CurrentProductMenuPanelVM = panelIdProductMenuPanelVMDictionary[panelId];
            }
            catch (Exception)
            {

                throw;
            }
            log.LogMethodExit();
        }

        private void OnPanelContentClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            ProductMenuPanelContentContainerDTO productMenuPanelContentContainerDTO = parameter as ProductMenuPanelContentContainerDTO;
            if(productMenuPanelContentContainerDTO == null)
            {
                log.LogMethodExit(null, "productMenuPanelContentContainerDTO == null");
                return;
            }
            
            if(productMenuPanelContentContainerDTO.ChildPanelId > -1)
            {
                if (panelIdProductMenuPanelVMDictionary.ContainsKey(productMenuPanelContentContainerDTO.ChildPanelId) == false)
                {
                    log.LogVariableState("ChildPanelId", productMenuPanelContentContainerDTO.ChildPanelId);
                    log.LogMethodExit(null, "panelIdProductMenuPanelVMDictionary.ContainsKey(productMenuPanelContentContainerDTO.ChildPanelId) == false");
                    return;
                }
                CurrentProductMenuPanelVM = panelIdProductMenuPanelVMDictionary[productMenuPanelContentContainerDTO.ChildPanelId];
                NavigationTagsVM.NavigationTags.Add(new NavigationTag() { Key = CurrentProductMenuPanelVM.ProductMenuPanelContainerDTO.PanelId.ToString(), Text = CurrentProductMenuPanelVM.ProductMenuPanelContainerDTO.Name });
                log.LogMethodExit(null, "productMenuPanelContentContainerDTO.ChildPanelId == " + productMenuPanelContentContainerDTO.ChildPanelId);
                return;
            }
            else if(productMenuPanelContentContainerDTO.ProductId > -1)
            {
                productMenuUserControl.RaisePanelContentClickCommand(productMenuPanelContentContainerDTO);
                log.LogMethodExit(null, "productMenuPanelContentContainerDTO.ProductId == " + productMenuPanelContentContainerDTO.ProductId);
                return;
            }
            log.LogMethodExit();
        }

        private void OnPanelContentInfoClick(object parameter)
        {
            log.LogMethodEntry(parameter);
            log.LogMethodExit();
        }
        #endregion

        #region Constructors 
        public ProductMenuUserControlVM(ExecutionContext executionContext, string productMenuType)
        {
            log.LogMethodEntry(executionContext);
            ExecutionContext = executionContext;
            this.productMenuType = productMenuType;
            IntializeCommands();
            CreateDefaultFontAndColors();
            RefreshDisplay();
            log.LogMethodExit();
        }

        private void RefreshDisplay()
        {
            log.LogMethodEntry();
            mainProductMenuPanelVM = null;
            currentProductMenuPanelVM = null;
            NavigationTagsVM.NavigationTags = new ObservableCollection<NavigationTag>();
            this.productMenuPanelContainerDTOList = ProductMenuViewContainerList.GetProductMenuPanelContainerDTOList(ExecutionContext, productMenuType);
            if(productMenuPanelContainerDTOList.Any() == false)
            {
                log.LogMethodExit(null, "productMenuPanelContainerDTOList is empty");
                return;
            }
            foreach (var productMenuPanelContainerDTO in productMenuPanelContainerDTOList)
            {
                if (panelIdProductMenuPanelVMDictionary.ContainsKey(productMenuPanelContainerDTO.PanelId))
                {
                    continue;
                }
                ProductMenuPanelVM productMenuPanelVM = new ProductMenuPanelVM(ExecutionContext, productMenuPanelContainerDTO, defaultBackgroundColor, defaultFont, defaultTextColor, defaultFontSize);
                productMenuPanelVMList.Add(productMenuPanelVM);
                panelIdProductMenuPanelVMDictionary.Add(productMenuPanelContainerDTO.PanelId, productMenuPanelVM);
            }
            mainProductMenuPanelVM = GetMainProductMenuPanelVM();
            currentProductMenuPanelVM = mainProductMenuPanelVM;
            NavigationTagsVM.NavigationTags.Add(new NavigationTag() { Key = CurrentProductMenuPanelVM.ProductMenuPanelContainerDTO.PanelId.ToString(), Text = CurrentProductMenuPanelVM.ProductMenuPanelContainerDTO.Name });
            log.LogMethodExit();
        }

        private ProductMenuPanelVM GetMainProductMenuPanelVM()
        {
            log.LogMethodEntry();
            ProductMenuPanelContainerDTO productMenuPanelContainerDTO = GetHighLevelProductMenuPanelContainerDTO();
            ProductMenuPanelVM result;
            if (panelIdProductMenuPanelVMDictionary.ContainsKey(productMenuPanelContainerDTO.PanelId))
            {
                result = panelIdProductMenuPanelVMDictionary[productMenuPanelContainerDTO.PanelId];
            }
            else
            {
                result = new ProductMenuPanelVM(ExecutionContext, productMenuPanelContainerDTO, defaultBackgroundColor, defaultFont, defaultTextColor, defaultFontSize);
                panelIdProductMenuPanelVMDictionary.Add(productMenuPanelContainerDTO.PanelId, result);
                productMenuPanelVMList.Add(result);
            }
            log.LogMethodExit();
            return result;
        }

        private ProductMenuPanelContainerDTO GetHighLevelProductMenuPanelContainerDTO()
        {
            log.LogMethodEntry();
            ProductMenuPanelContainerDTO result;
            int mainPanelCount = productMenuPanelContainerDTOList.Count(x => x.IsMainPanel);
            if (mainPanelCount == 1)
            {
                result = productMenuPanelContainerDTOList.First(x => x.IsMainPanel);
            }
            else
            {
                ProductMenuPanelContentButtonType buttonType = ProductMenuPanelContentButtonType.NORMAL;
                int columnCount = productMenuPanelContainerDTOList.Select(x => x.ColumnCount).Min();
                int rowCount = mainPanelCount / (columnCount / buttonType.HorizontalCellCount);
                if(mainPanelCount % (columnCount / buttonType.HorizontalCellCount) > 0)
                {
                    rowCount ++;
                }
                int cellMarginLeft = productMenuPanelContainerDTOList.Select(x => x.CellMarginLeft).Min();
                int cellMarginRight = productMenuPanelContainerDTOList.Select(x => x.CellMarginRight).Min();
                int cellMarginTop = productMenuPanelContainerDTOList.Select(x => x.CellMarginTop).Min();
                int cellMarginBottom = productMenuPanelContainerDTOList.Select(x => x.CellMarginBottom).Min();
                result = new ProductMenuPanelContainerDTO(-1, true,  0, MessageViewContainerList.GetMessage(ExecutionContext, "Home"), cellMarginLeft, cellMarginRight, cellMarginTop, cellMarginBottom, rowCount, columnCount, string.Empty, string.Empty);
                int index = 0;
                foreach (var productMenuPanelContainerDTO in productMenuPanelContainerDTOList.Where(x=> x.IsMainPanel).OrderBy(x => x.DisplayOrder))
                {
                    int columnIndex = (index % (columnCount / buttonType.HorizontalCellCount)) * (buttonType.HorizontalCellCount);
                    int rowIndex = index / (columnCount / buttonType.HorizontalCellCount) * buttonType.VerticalCellCount;
                    ProductMenuPanelContentContainerDTO productMenuPanelContentContainerDTO = new ProductMenuPanelContentContainerDTO(-1, -1, -1, 
                                                                                                                                      productMenuPanelContainerDTO.PanelId, 
                                                                                                                                      productMenuPanelContainerDTO.Name, 
                                                                                                                                      productMenuPanelContainerDTO.DisplayOrder,
                                                                                                                                      productMenuPanelContainerDTO.ImageURL,
                                                                                                                                      string.Empty, string.Empty, string.Empty, columnIndex, rowIndex, buttonType.ToButtonTypeString(), false);
                    result.ProductMenuPanelContentContainerDTOList.Add(productMenuPanelContentContainerDTO);
                    index ++;
                }
            }
            log.LogMethodExit(result);
            return result;
        }
        #endregion

        #region Methods
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
        #endregion

        #region Properties
        public ProductMenuPanelVM CurrentProductMenuPanelVM
        {
            get
            {
                return currentProductMenuPanelVM;
            }
            set
            {
                SetProperty(ref currentProductMenuPanelVM, value);
            }
        }

        public NavigationTagsVM NavigationTagsVM
        {
            get
            {
                return navigationTagsVM;
            }
        }

        public bool ShowMainPanel
        {
            get
            {
                return showMainPanel;
            }

            set
            {
                SetProperty(ref showMainPanel, value);
            }
        }
        #endregion
    }
}

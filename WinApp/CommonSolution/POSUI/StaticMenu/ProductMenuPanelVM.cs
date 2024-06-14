/********************************************************************************************
 * Project Name - POSUI
 * Description  - View model class of product menu panel
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
using System.Collections.Generic;
using System.Windows.Media;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    public class ProductMenuPanelVM : BaseWindowViewModel
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<ProductMenuPanelContentVM> productMenuPanelContentVMList = new List<ProductMenuPanelContentVM>();
        private readonly ProductMenuPanelContainerDTO productMenuPanelContainerDTO;
        private readonly PanelMargin panelMargin;

        public ProductMenuPanelVM(ExecutionContext executionContext, 
                                  ProductMenuPanelContainerDTO productMenuPanelContainerDTO, 
                                  Brush defaultBackgroundColor,
                                  FontFamily defaultFont,
                                  Brush defaultTextColor,
                                  double defaultFontSize)
        {
            log.LogMethodEntry(executionContext, productMenuPanelContainerDTO);
            ExecutionContext = executionContext;
            this.productMenuPanelContainerDTO = productMenuPanelContainerDTO;
            panelMargin = new PanelMargin(productMenuPanelContainerDTO.CellMarginLeft, productMenuPanelContainerDTO.CellMarginRight, productMenuPanelContainerDTO.CellMarginTop, productMenuPanelContainerDTO.CellMarginBottom);
            foreach (var productMenuPanelContentContainerDTO in productMenuPanelContainerDTO.ProductMenuPanelContentContainerDTOList)
            {
                ProductMenuPanelContentVM productMenuPanelContentVM = new ProductMenuPanelContentVM(ExecutionContext, 
                                                                                                    productMenuPanelContentContainerDTO, 
                                                                                                    panelMargin,
                                                                                                    defaultBackgroundColor,
                                                                                                    defaultFont,
                                                                                                    defaultTextColor,
                                                                                                    defaultFontSize);
                productMenuPanelContentVMList.Add(productMenuPanelContentVM);
            }
            log.LogMethodExit();
        }

        public List<ProductMenuPanelContentVM> ProductMenuPanelContentVMList
        {
            get
            {
                return productMenuPanelContentVMList;
            }
        }

        public double PanelWidth
        {
            get
            {
                return productMenuPanelContainerDTO.ColumnCount * ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_WIDTH", 144) + (productMenuPanelContainerDTO.ColumnCount - 1) * (panelMargin.Left + panelMargin.Right) + 20;
            }
        }

        public double PanelHeight
        {
            get
            {
                return productMenuPanelContainerDTO.RowCount * ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "PRODUCT_MENU_BUTTON_HEIGHT", 96) + (productMenuPanelContainerDTO.RowCount - 1) * (panelMargin.Top + panelMargin.Bottom) + 20;
            }
        }

        public ProductMenuPanelContainerDTO ProductMenuPanelContainerDTO
        {
            get
            {
                return productMenuPanelContainerDTO;
            }
        }
    }
}

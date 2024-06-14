/********************************************************************************************
 * Project Name - POSUI
 * Description  - View model class of product menu panel content setup
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.130.0     8-June-2021      Lakshminarayana      Created
 ********************************************************************************************/
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    /// <summary>
    /// Interaction logic for ProductMenuPanelContentSetupView.xaml
    /// </summary>
    public partial class ProductMenuPanelContentSetupView : UserControl
    {
        public static readonly DependencyProperty SelectPanelContentProperty =
        DependencyProperty.Register(
            "SelectPanelContent",
            typeof(ICommand),
            typeof(ProductMenuPanelContentSetupView),
            new UIPropertyMetadata(null));
        
        public ICommand SelectPanelContent
        {
            get { return (ICommand)GetValue(SelectPanelContentProperty); }
            set { SetValue(SelectPanelContentProperty, value); }
        }

        public ProductMenuPanelContentSetupView()
        {
            InitializeComponent();
        }

    }
}

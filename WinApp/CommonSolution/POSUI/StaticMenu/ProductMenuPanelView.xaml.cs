using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    /// <summary>
    /// Interaction logic for ProductMenuPanelView.xaml
    /// </summary>
    public partial class ProductMenuPanelView : UserControl
    {
        public static readonly DependencyProperty PanelContentClickProperty =
        DependencyProperty.Register(
            "PanelContentClick",
            typeof(ICommand),
            typeof(ProductMenuPanelView),
            new UIPropertyMetadata(null));

        public ICommand PanelContentClick
        {
            get { return (ICommand)GetValue(PanelContentClickProperty); }
            set { SetValue(PanelContentClickProperty, value); }
        }

        public static readonly DependencyProperty PanelContentInfoClickProperty =
        DependencyProperty.Register(
            "PanelContentInfoClick",
            typeof(ICommand),
            typeof(ProductMenuPanelView),
            new UIPropertyMetadata(null));

        public ICommand PanelContentInfoClick
        {
            get { return (ICommand)GetValue(PanelContentInfoClickProperty); }
            set { SetValue(PanelContentInfoClickProperty, value); }
        }

        public ProductMenuPanelView()
        {
            InitializeComponent();
        }
    }
}

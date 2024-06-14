using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.ViewContainer;
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
using System.Windows.Shapes;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    /// <summary>
    /// Interaction logic for ChoosePanelView.xaml
    /// </summary>
    public partial class ChoosePanelView : Window
    {
        private KeyboardHelper keyboardHelper;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ChoosePanelView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            MainGrid.Height = SystemParameters.PrimaryScreenHeight - 20;
            MainGrid.Width = SystemParameters.PrimaryScreenWidth - 20;

            this.ContentRendered += OnContentRendered;
            log.LogMethodExit();
        }

        private void OnContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            if (DataContext != null)
            {
                ChoosePanelVM choosePanelVM = this.DataContext as ChoosePanelVM;
                if (choosePanelVM != null)
                {
                    ExecutionContext executionContext = choosePanelVM.ExecutionContext;
                    if (executionContext != null)
                    {
                        keyboardHelper = new KeyboardHelper();
                        keyboardHelper.ShowKeyBoard(this, new List<Control>() { btnKeyBoard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}

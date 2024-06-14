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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Semnox.Parafait.POSUI.StaticMenu
{
    /// <summary>
    /// Interaction logic for ProductMenuPanelSetupView.xaml
    /// </summary>
    public partial class ProductMenuPanelSetupView : Window
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KeyboardHelper keyboardHelper;
        public ProductMenuPanelSetupView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            ContentRendered += OnContentRendered;
            log.LogMethodExit();
        }


        private void OnContentRendered(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);


            if (this.DataContext != null)
            {
                ProductMenuPanelSetupViewModel productMenuPanelSetupViewModel = this.DataContext as ProductMenuPanelSetupViewModel;

                if (productMenuPanelSetupViewModel != null)
                {
                    ExecutionContext executioncontext = productMenuPanelSetupViewModel.GetExecutionContext();

                    if (executioncontext != null)
                    {
                        TranslateHelper.Translate(executioncontext, this);
                        FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                        if (footerView != null)
                        {
                            keyboardHelper = new KeyboardHelper();
                            keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(executioncontext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}

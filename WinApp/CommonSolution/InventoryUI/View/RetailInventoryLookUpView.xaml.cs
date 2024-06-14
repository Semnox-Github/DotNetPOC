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

namespace Semnox.Parafait.InventoryUI
{
    /// <summary>
    /// Interaction logic for RetailInventoryLookUpView.xaml
    /// </summary>
    public partial class RetailInventoryLookUpView : Window
    {
        #region Members
        private KeyboardHelper keyboardHelper;
        #endregion

        #region Properties
        public KeyboardHelper KeyboardHelper { get { return keyboardHelper; } }
        #endregion
        public RetailInventoryLookUpView()
        {
            InitializeComponent();
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            keyboardHelper = new KeyboardHelper();
            this.ContentRendered += RetailInventoryLookUpView_ContentRendered;
        }

        private void RetailInventoryLookUpView_ContentRendered(object sender, EventArgs e)
        {
            if (this.DataContext != null)
            {
                RetailInventoryLookUpVM retailInventoryLookUpVM = this.DataContext as RetailInventoryLookUpVM;
                if (retailInventoryLookUpVM != null)
                {
                    TranslateHelper.Translate(retailInventoryLookUpVM.ExecutionContext, this);
                    FooterUserControl footerView = this.Template.FindName("FooterUserControl", this) as FooterUserControl;
                    if (footerView != null)
                    {
                        keyboardHelper.ShowKeyBoard(this, new List<Control>() { footerView.btnKeyboard }, ParafaitDefaultViewContainerList.GetParafaitDefault(retailInventoryLookUpVM.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD") == "Y" ? true : false, new List<Control>());
                    }
                }
            }
        }
    }
}
